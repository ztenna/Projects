import java.sql.*;
import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.util.*;
import java.lang.Integer;

public class DatabaseApp extends JDialog
{
   private String id = "admin";
   private String pwd = "Database-Group4";

   // JDBC driver name and database URL - for MySQL
   private String driver = "com.mysql.cj.jdbc.Driver";
   private String url = "jdbc:mysql://db-fall2019-group4.cj37opin8pnk.us-east-1.rds.amazonaws.com:3306/DB_Fall2019_Group4?yearIsDateType=false";

   private JLabel idLabel;
   private JLabel pwdLabel;
   private JTextField idField;
   private JPasswordField pwdField;
   private JButton loginButton;

   private Connection connection;

   private boolean isManager;

   private DataFrame frame;
   private DatabaseApp app;
   private WindowHandler window;

   public DatabaseApp()
   {
      connectToDatabase();

      setLayout(new FlowLayout());

      // create GUI componenets for login
      idLabel = new JLabel("Staff ID:");
      pwdLabel = new JLabel("Password:");
      idField = new JTextField(5);
      pwdField = new JPasswordField(15);
      loginButton = new JButton("Login");
      getRootPane().setDefaultButton(loginButton);
      add(idLabel);
      add(idField);
      add(pwdLabel);
      add(pwdField);
      add(loginButton);

      LoginHandler lhandler = new LoginHandler();
      loginButton.addActionListener(lhandler);

      window = new WindowHandler();
      this.addWindowListener(window);

      setSize(400, 100);
      setResizable(false);
      setTitle("LOGIN");
      setLocationRelativeTo(null);
      setVisible(true);

      app = this;
   }

   // method for handling connection to database
   private void connectToDatabase()
   {
      JOptionPane.showMessageDialog(null, "Connecting to database");

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
         JOptionPane.showMessageDialog(null, ex.getMessage());
         System.exit(1);
      }
   }

   // inner class for handling login
   private class LoginHandler implements ActionListener
   {
      public void actionPerformed(ActionEvent e)
      {
         PreparedStatement pstatement = null;

         String sid = idField.getText();

         char[] p = pwdField.getPassword();
         String pwd = new String(p);

         try
         {
            // validate staff id and password
            pstatement = connection.prepareStatement("SELECT sid FROM staff_works_in WHERE sid = ? and password = ?");

            pstatement.clearParameters();
            pstatement.setInt(1, Integer.parseInt(sid));
            pstatement.setString(2, pwd);

            // query database
            ResultSet resultSet = pstatement.executeQuery();

            // If no records returned, display message
            if(!resultSet.next())
            {
               JOptionPane.showMessageDialog(null, "Invalid credentials!", "Login Error", JOptionPane.ERROR_MESSAGE);
            //   return;
            } // end if (no results)

            // Check if manager
            pstatement = connection.prepareStatement( "SELECT * FROM sales_manager S WHERE S.sid = ?" );

            pstatement.setInt(1, Integer.parseInt(sid));

            // Query database
            resultSet = pstatement.executeQuery();

            // If there are no records, then sales person
            if(!resultSet.next())
            {
               isManager = false; // not a manager
            }

            else
            {
               isManager = true;
            }

            pstatement.close();

            idField.setText("");
            pwdField.setText("");

            setVisible(false);

            frame = new DataFrame(connection, isManager);
            frame.addWindowListener(window);
         }

         catch (NumberFormatException nfe)
         {
            JOptionPane.showMessageDialog(null, "Invalid credentials!", "Login Error", JOptionPane.ERROR_MESSAGE);
            return;
         }

         catch (SQLException sse)
         {
            JOptionPane.showMessageDialog(null, sse.getMessage(), "Query error!", JOptionPane.ERROR_MESSAGE);
            return;
         }

      }
   }

   // inner class for handling window event
   private class WindowHandler extends WindowAdapter
   {
      public void windowClosing(WindowEvent e)
      {
         if (e.getSource() == app)
         {
            try
            {
               if(connection != null)
               {
                  connection.close();
               }
            }

            catch(SQLException ex)
            {
               JOptionPane.showMessageDialog(null, "Unable to disconnect!");
            }

            System.exit(0);
         }

         else if (e.getSource() == frame)
         {
            app.setVisible(true);
         }
      }
   }

   public static void main(String args[])
   {
      DatabaseApp db = new DatabaseApp();
   }

}