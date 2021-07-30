// This program displays the results of a query on a database
import java.sql.*;
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.util.*;
import java.lang.Integer;

public class DatabaseGui extends JFrame
{
   private JFrame frame;

   private JLabel idLabel;
   private JLabel pwdLabel;
   private JTextField idField;
   private JPasswordField pwdField;
   private JTextField queryField;

   private JButton loginButton;
   private JButton queryButton;

   private JPanel loginPanel;
   private JPanel queryPanel;

   private JTable table;
   private JScrollPane scroller;

   Dimension screenSize;

   private Connection connection;

   private boolean isManager;

   public DatabaseGui()
   {
      connectToDatabase();

      setLayout(new FlowLayout());

      // create GUI componenets for login
      loginPanel = new JPanel();
      idLabel = new JLabel("Staff ID");
      pwdLabel = new JLabel("Password");
      idField = new JTextField(10);
      pwdField = new JPasswordField(15);

      loginButton = new JButton("Login");
      getRootPane().setDefaultButton(loginButton);
      loginPanel.setLayout(new GridLayout(2,2,0,5));
      loginPanel.add(idLabel);
      loginPanel.add(idField);
      loginPanel.add(pwdLabel);
      loginPanel.add(pwdField);
      add(loginPanel);
      add(loginButton);

      LoginHandler lhandler = new LoginHandler();
      loginButton.addActionListener(lhandler);

      // create GUI componenets for query
      queryPanel = new JPanel();
      queryField = new JTextField(30);
      queryField.setText("select * from vehicles");
      queryButton = new JButton("Submit");
      queryButton.setEnabled(false);
//      queryButton.setEnabled(true);
//      getRootPane().setDefaultButton(queryButton);
      queryPanel.setLayout(new BoxLayout(queryPanel,BoxLayout.X_AXIS));
      queryPanel.setBorder(BorderFactory.createTitledBorder("Query"));
      queryPanel.add(queryField);
      queryPanel.add(queryButton);
      add(queryPanel);

      QueryHandler qhandler = new QueryHandler();
      queryButton.addActionListener(qhandler);

      WindowHandler window = new WindowHandler();
      this.addWindowListener(window);

      frame = this;
   }

   // method for handling connection to database
   private void connectToDatabase()
   {
      JOptionPane.showMessageDialog(null, "Connecting to database");

      String id = "admin";
      String pwd = "Database-Group4";

      // JDBC driver name and database URL - for MySQL
      String driver = "com.mysql.cj.jdbc.Driver";
      String url = "jdbc:mysql://db-fall2019-group4.cj37opin8pnk.us-east-1.rds.amazonaws.com:3306/DB_Fall2019_Group4";

      try
      {
         // Load the JDBC driver to allow connection to the database
         Class.forName( driver );

         // establish connection to database
         connection = DriverManager.getConnection(url, id, pwd);
      }
      catch(ClassNotFoundException ex)
      {
         JOptionPane.showMessageDialog(null, "Failed to load JDBC driver!");
         System.exit(1);
      }
      catch(SQLException ex)
      {
         JOptionPane.showMessageDialog(null, "Access denied!");
         System.exit(1);
      }
   }

   // inner class for handling login
   private class LoginHandler implements ActionListener
   {
      public void actionPerformed(ActionEvent e)
      {
         PreparedStatement pstatement = null;

         String id = idField.getText();

         char[] p = pwdField.getPassword();
         String pwd = new String(p);

JOptionPane.showMessageDialog(null, "ID given = " + id + "/end", "Query error!", JOptionPane.ERROR_MESSAGE);

JOptionPane.showMessageDialog(null, "ID as integer given = " + Integer.parseInt(id) + "/end", "Query error!", JOptionPane.ERROR_MESSAGE);Integer.parseInt(id);

JOptionPane.showMessageDialog(null, "Password given = " + pwd + "/end", "Query error!", JOptionPane.ERROR_MESSAGE);

         try
         {
            // validate staff id and password
            pstatement = connection.prepareStatement("SELECT S.sid FROM staff_works_in S WHERE S.sid = ? and S.password = ?");

            // instantiate parameters
            //pstatement.clearParameters();

            //try
            //{
               pstatement.clearParameters();
               pstatement.setInt(1, 120);
               pstatement.setString(2, pwd);
            //}

            // query database
            ResultSet resultSet = pstatement.executeQuery();

            // If no records returned, display message
            if(!resultSet.next())
            {
               JOptionPane.showMessageDialog(null, "Invalid credentials!");
               return;
            } // end if (no results)

            // Check if manager
            pstatement = connection.prepareStatement( "SELECT * FROM sales_manager S WHERE S.sid = ?" );

            // Query database
            resultSet = pstatement.executeQuery();

            // If there are no records, then sales person
            if(!resultSet.next())
            {
               isManager = false; // not a manager
            }

            pstatement.close();
         }

         catch (NumberFormatException nfe)
         {
            JOptionPane.showMessageDialog(null, "Parse integer exception = " + nfe.getMessage(), "Query error!", JOptionPane.ERROR_MESSAGE);
            return;
         }

         catch (SQLException sse)
         {
            JOptionPane.showMessageDialog(null, "SQL setString() exception = " + sse.getMessage(), "Query error!", JOptionPane.ERROR_MESSAGE);
            return;
         }

      }
   }

   // inner class for handling query
   private class QueryHandler implements ActionListener
   {
      public void actionPerformed(ActionEvent e)
      {
         String query = queryField.getText();
         Statement statement;
         ResultSet resultSet;
         try {
            statement = connection.createStatement();
            resultSet = statement.executeQuery(query);

            //If there are no records, display a message
            if(!resultSet.next()) {
               JOptionPane.showMessageDialog(null,"No records found!");
               return;
            }
            else {
               // columnNames holds the column names of the query result
               Vector<Object> columnNames = new Vector<Object>();

               // rows is a vector of vectors, each vector is a vector of
               // values representing a certain row of the query result
               Vector<Object> rows = new Vector<Object>();

               // get column headers
               ResultSetMetaData metaData = resultSet.getMetaData();

               for(int i = 1; i <= metaData.getColumnCount(); ++i)
                  columnNames.addElement(metaData.getColumnName(i));

               // get row data
               do {
                  Vector<Object> currentRow = new Vector<Object>();
                  for(int i = 1; i <= metaData.getColumnCount(); ++i)
                     currentRow.addElement(resultSet.getObject(i));
                  rows.addElement(currentRow);
               } while(resultSet.next()); //moves cursor to next record

               if(scroller!=null)
                  getContentPane().remove(scroller);

               // display table with ResultSet contents
               table = new JTable(rows, columnNames);
               table.setPreferredScrollableViewportSize(new Dimension((int)(frame.getWidth() * 0.95), (int)(frame.getHeight() * 0.8)));
               //table.setAutoResizeMode(JTable.AUTO_RESIZE_ALL_COLUMNS);
               //table.doLayout();

               scroller = new JScrollPane(table);
               getContentPane().add(scroller);
               validate();
            }

            statement.close();
         }

         catch(SQLException ex) {
            JOptionPane.showMessageDialog(null, ex.getMessage(), "Query error!", JOptionPane.ERROR_MESSAGE);
         }
      }
   }

   // inner class for handling window event
   private class WindowHandler extends WindowAdapter
   {
      public void windowClosing(WindowEvent e)
      {
         try {
            if(connection!=null)
               connection.close();
         }
         catch(SQLException ex) {
            JOptionPane.showMessageDialog(null, "Unable to disconnect!");
         }
         System.exit(0);
      }
   }

   public static void main(String args[])
   {
      DatabaseGui db = new DatabaseGui();
      //db.setSize(500,500);
      //db.setExtendedState(JFrame.MAXIMIZED_BOTH);
      db.screenSize = Toolkit.getDefaultToolkit().getScreenSize();
      db.setSize( (int)(db.screenSize.getWidth()), (int)(db.screenSize.getHeight() * 0.95) );

      db.setResizable(true);
      db.setTitle("Accessing a Database using a GUI");
      db.setVisible(true);
   }

}