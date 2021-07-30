// This program displays the results of a query on a database
import java.sql.*;
import javax.swing.*;
import javax.swing.table.*;
import java.awt.*;
import java.awt.event.*;
import java.util.*;
import java.lang.Integer;
import java.io.*;
import javax.imageio.*;
import java.net.*;


public class DataFrame extends JDialog
{
   private JButton queryButton1;
   private JButton queryButton2;
   private JButton queryButton3;
   private JButton queryButton4;
   private JButton queryButton5;

   private JButton transactionButton1;
   private JButton transactionButton2;
   private JButton transactionButton3;
   private JButton transactionButton4;
   private JButton transactionButton5;

   private JPanel queryPanel;
   private JPanel transactionPanel;

   private JTable table;
   private DefaultTableCellRenderer centerRenderer;
   JScrollPane scroller;

   private Connection connection;

   private Statement statement = null;
   private PreparedStatement pstatement = null;
   private ResultSet resultSet;

   Image[] images;
   String[] urls;

   private ClickHandler clickHandler;
   private ClickHandler2 clickHandler2;

   private int correctSID;
   private int correctSID2;
   private int correctCID;
   private int correctLID;
   private int correctStock_Num;
   private String correctDate_Time;
   private String correctDate;
   private Object selectedValue;
   private int transactionNum;
   TransactionFrame tFrame;
   private int maxCid;
   private int maxSid;
   private String transaction;
   private String query;
   private String wildcard = "%";

   private int tableType;
   // Integer to indicate 3 different table types
   // 0: no mouse listener, Query3TableCellRenderer used
   // 1: ClickHandler used
   // 2: ClickHandler2 used

   public DataFrame(Connection connection, boolean isManager)
   {
      this.connection = connection;

      setLayout(new FlowLayout());

      // create GUI componenets for queries
      queryPanel = new JPanel();
      queryButton1 = new JButton("Customer Visit Info");
      queryButton2 = new JButton("Vehicle Test Drives");
      queryButton3 = new JButton("Top 5 Vehicles Sold");
      queryButton4 = new JButton("Monthly Location Sales");
      queryButton5 = new JButton("Salespeople Summary");
      queryPanel.setLayout(new GridLayout(1, 5, 5, 5));
      queryPanel.setBorder(BorderFactory.createTitledBorder("Queries"));
      queryPanel.add(queryButton1);
      queryPanel.add(queryButton2);
      queryPanel.add(queryButton3);
      queryPanel.add(queryButton4);
      queryPanel.add(queryButton5);
      add(queryPanel);

	  transactionPanel = new JPanel();
	  transactionButton1 = new JButton("Add Customers/Update Info");
	  transactionButton2 = new JButton("Update Visit Info");
	  transactionButton3 = new JButton("Reassign Customers");
	  transactionButton4 = new JButton("Add/Remove Salespeople");
	  transactionButton5 = new JButton("Update Inventory");
	  transactionPanel.setLayout(new GridLayout(1, 5, 5, 5));
	  transactionPanel.setBorder(BorderFactory.createTitledBorder("Transactions"));
	  transactionPanel.add(transactionButton1);
	  transactionPanel.add(transactionButton2);
	  transactionPanel.add(transactionButton3);
	  transactionPanel.add(transactionButton4);
	  transactionPanel.add(transactionButton5);
      add(transactionPanel);

      QueryHandler qhandler = new QueryHandler(this);
      queryButton1.addActionListener(qhandler);
      queryButton2.addActionListener(qhandler);
      queryButton3.addActionListener(qhandler);
      queryButton4.addActionListener(qhandler);
      queryButton5.addActionListener(qhandler);

      TransactionHandler tHandler = new TransactionHandler(this);
      transactionButton1.addActionListener(tHandler);
      transactionButton2.addActionListener(tHandler);
      transactionButton3.addActionListener(tHandler);
      transactionButton4.addActionListener(tHandler);
      transactionButton5.addActionListener(tHandler);

      if (!isManager)
      {
         queryButton4.setEnabled(false);
         queryButton5.setEnabled(false);

         transactionButton3.setEnabled(false);
         transactionButton4.setEnabled(false);
         transactionButton5.setEnabled(false);
         setTitle("Salesperson");
      }

      else
      {
         setTitle("Manager");
      }

      setSize(1075, 700);
      setResizable(false);
      setLocationRelativeTo(null);
      setVisible(true);
   }

   // inner class for handling query
   private class QueryHandler implements ActionListener
   {
	   DataFrame dFrame;

	   public QueryHandler(DataFrame dFrame)
	   {
		   this.dFrame = dFrame;
	   }

      public void actionPerformed(ActionEvent e)
      {
         statement = null;
         pstatement = null;
         resultSet = null;

         tableType = 0;

         try
         {
            statement = connection.createStatement();

            if (e.getSource() == queryButton1) // CustomerVisitInfoQuery
            {
               String customer = "%" + JOptionPane.showInputDialog(null, null, "Enter customer name", JOptionPane.QUESTION_MESSAGE) + "%";
               customer = wildcard.concat(customer.trim()).concat(wildcard);

			   query = "SELECT VI.time_date AS 'Date & Time', CONCAT_WS(', ', A.street, A.city, A.state) AS Location, V.year AS Year, V.make AS Make, V.model AS Model FROM contacts C, vehicles V, visit_info VI, customers_engaged_with CEW, addresses A, locations L, staff_works_in SWA WHERE VI.stock_num = V.stock_num AND C.id = VI.cid AND CEW.cid = C.id AND SWA.sid = CEW.sid AND SWA.lid = L.lid AND L.street = A.street and L.zipcode = A.zipcode AND C.name LIKE ?";

			   pstatement = connection.prepareStatement(query);

			   // instantiate parameters
			   pstatement.clearParameters();
			   pstatement.setString(1, customer);

			   resultSet = pstatement.executeQuery();
            }

            else if (e.getSource() == queryButton2) // VehicleTestDrivesQuery
            {
               JOptionPane pane;
               JTextField makeField = new JTextField(15);
               JTextField modelField = new JTextField(15);

               JPanel myPanel = new JPanel();
               myPanel.add(new JLabel("Make:"));
               myPanel.add(makeField);
               myPanel.add(Box.createVerticalStrut(15)); // a spacer
               myPanel.add(new JLabel("Model:"));
               myPanel.add(modelField);

               JOptionPane.showConfirmDialog(null, myPanel, "Please Enter Vehicle Info", JOptionPane.OK_CANCEL_OPTION, JOptionPane.QUESTION_MESSAGE);

               makeField.requestFocusInWindow();

               String make = wildcard.concat(makeField.getText().trim()).concat(wildcard);
               String model = wildcard.concat(modelField.getText().trim()).concat(wildcard);

			   query = "SELECT DISTINCT C.name AS Customer FROM contacts C WHERE EXISTS (SELECT VI.cid FROM visit_info VI, vehicles V WHERE C.id = VI.cid AND VI.stock_num = V.stock_num AND V.make = ? AND V.model = ?)";

			   pstatement = connection.prepareStatement(query);

			   // instantiate parameters
			   pstatement.clearParameters();
			   pstatement.setString(1, make);
			   pstatement.setString(2, model);

			   resultSet = pstatement.executeQuery();
            }

            else if (e.getSource() == queryButton3) // Top5VehiclesSold
            {
				tableType = 1;

                query = "SELECT V.make AS Make, V.model AS Model, COUNT(V.commission) AS 'Number Sold', V.image AS Image, V.hyperlink AS 'Car & Driver Report' FROM vehicles V GROUP BY V.make, V.model HAVING 'V.commission' IS NOT NULL ORDER BY COUNT(V.commission) DESC LIMIT 5";

                resultSet = statement.executeQuery(query);

                images = new Image[5];
                urls = new String[5];
            }

            else if (e.getSource() == queryButton4) // MonthlyLocationSalesQuery
            {
                query = "(SELECT * FROM MonthlyVehicleSalesView MVSV ORDER BY 'MTD Sales' DESC) UNION (SELECT * FROM NoMonthlyVehicleSalesView NMVSV ORDER BY 'MTD Sales' DESC)";

                resultSet = statement.executeQuery(query);
            }

            else // (e.getSource() == queryButton5) AnnualSalepeopleSummaryQuery
            {
                query = "SELECT C.name AS Name, ANTDV.TestDrives AS 'Number Test Drives', ANSV.NumSales AS 'Number of Sales', ATCV.TotCommission AS 'YTD Commission' FROM contacts C, AnnualNumberOfTestDrivesView ANTDV, AnnualNumberOfSalesView ANSV, AnnualTotalCommissionView ATCV WHERE C.id = ANTDV.StaffID AND ANTDV.StaffID = ANSV.StaffID AND ANSV.StaffID = ATCV.StaffID";

                resultSet = statement.executeQuery(query);
            }

            // If no records returned, display message
            if(!resultSet.next())
            {
                JOptionPane.showMessageDialog(null, "No Results!", "No Results!", JOptionPane.ERROR_MESSAGE);
                return;
            } // end if (no results)

            displayResults(dFrame);
         }

         catch (SQLException sse)
         {
            JOptionPane.showMessageDialog(null, sse.getMessage(), "Query error!", JOptionPane.ERROR_MESSAGE);
            return;
         }

      }
   }

   // inner class for handling transactions
   public class TransactionHandler implements ActionListener
   {
	   DataFrame dFrame;

	   public TransactionHandler(DataFrame dFrame)
	   {
		   this.dFrame = dFrame;
	   }

       public void actionPerformed(ActionEvent e)
       {
           statement = null;
           pstatement = null;
           transaction = null;
           transactionNum = 0;

           tableType = 2;

           try
           {
               statement = connection.createStatement();

               if (e.getSource() == transactionButton1) // Add Customer/Update Personal Info
               {
				   Object[] possibleValues = {"Add Customer","Update Personal Info"};
            	   selectedValue = JOptionPane.showInputDialog(null,"Choose one","Input",JOptionPane.INFORMATION_MESSAGE,null,possibleValues,possibleValues[0]);

            	   if(selectedValue == "Add Customer")
            	   {
					   transactionNum = 1;

					   query = "SELECT MAX(CEW.cid) FROM customers_engaged_with CEW";

					   resultSet = statement.executeQuery(query);

					   resultSet.next();
					   maxCid = resultSet.getInt(1);

					   String salesperson = "%" + JOptionPane.showInputDialog(null, null, "Enter saleperson name", JOptionPane.QUESTION_MESSAGE) + "%";

					   if (salesperson != "%%")
					   {
						   query = "SELECT C.name, C.id FROM contacts C, staff_works_in SWI WHERE C.name LIKE ? AND C.id = SWI.sid";

						   pstatement = connection.prepareStatement(query);

						   // instantiate parameters
						   pstatement.clearParameters();
						   pstatement.setString(1, salesperson);

						   resultSet = pstatement.executeQuery();

						   displayResults(dFrame);
					   }

					   else
					   {
						   return;
					   }
				   }

				   else
				   {
					   transactionNum = 2;

					   String customer = "%" + JOptionPane.showInputDialog(null, null, "Enter customer name", JOptionPane.QUESTION_MESSAGE) + "%";

					   if (customer != "%%")
					   {
						   query = "SELECT C.name, C.id FROM contacts C, customers_engaged_with CEW WHERE C.name LIKE ? AND C.id = CEW.cid";

						   pstatement = connection.prepareStatement(query);

						   // instantiate parameters
						   pstatement.clearParameters();
						   pstatement.setString(1, customer);

						   resultSet = pstatement.executeQuery();

						   displayResults(dFrame);
					   }

					   else
					   {
						   return;
					   }
				   }
               }

               else if (e.getSource() == transactionButton2) // Update Visit History
               {
				   transactionNum = 3;

				   String customer = "%" + JOptionPane.showInputDialog(null, null, "Enter customer name", JOptionPane.QUESTION_MESSAGE) + "%";

				   if (customer != "%%")
				   {
					   query = "SELECT CURRENT_TIMESTAMP()";

					   resultSet = statement.executeQuery(query);

					   resultSet.next();
					   correctDate_Time = resultSet.getString(1);

				 	   query = "SELECT C.name, C.id FROM contacts C, customers_engaged_with CEW WHERE C.name LIKE ? AND C.id = CEW.cid";

					   pstatement = connection.prepareStatement(query);

					   // instantiate parameters
					   pstatement.clearParameters();
					   pstatement.setString(1, customer);

					   resultSet = pstatement.executeQuery();

					   displayResults(dFrame);
				   }

				   else
				   {
					   return;
				   }
               }

               else if (e.getSource() == transactionButton3) // Reassign Customer
               {
			       transactionNum = 6;

			       String customer = "%" + JOptionPane.showInputDialog(null, null, "Enter customer name", JOptionPane.QUESTION_MESSAGE) + "%";

			       if (customer != "%%")
			       {
				       query = "SELECT C.name, C.id FROM contacts C, customers_engaged_with CEW WHERE C.name LIKE ? AND C.id = CEW.cid";

				       pstatement = connection.prepareStatement(query);

				       // instantiate parameters
				       pstatement.clearParameters();
				       pstatement.setString(1, customer);

				       resultSet = pstatement.executeQuery();

				       displayResults(dFrame);
			       }

			       else
			       {
				       return;
			       }
               }

               else if (e.getSource() == transactionButton4) // Add Salesperson/Remove Salesperson
               {
				   Object[] possibleValues = {"Add Salesperson","Remove Salesperson"};
            	   selectedValue = JOptionPane.showInputDialog(null,"Choose one","Input",JOptionPane.INFORMATION_MESSAGE,null,possibleValues,possibleValues[0]);

            	   if(selectedValue == "Add Salesperson")
            	   {
					   transactionNum = 8;

					   query = "SELECT MAX(C.id) FROM contacts C";

					   resultSet = statement.executeQuery(query);

					   resultSet.next();
					   maxSid = resultSet.getInt(1);

					   query = "SELECT CURDATE()";

					   resultSet = statement.executeQuery(query);

					   resultSet.next();
					   correctDate = resultSet.getString(1);

					   String location = "%" + JOptionPane.showInputDialog(null, null, "Enter location name", JOptionPane.QUESTION_MESSAGE) + "%";

					   if (location != "%%")
					   {
						   query = "SELECT L.name, L.lid FROM locations L WHERE L.name LIKE ?";

						   pstatement = connection.prepareStatement(query);

						   // instantiate parameters
						   pstatement.clearParameters();
						   pstatement.setString(1, location);

						   resultSet = pstatement.executeQuery();

						   displayResults(dFrame);
				       }

				       else
				       {
					       return;
				       }
			       }

			       else
			       {
				       transactionNum = 9;

				       String salesperson = "%" + JOptionPane.showInputDialog(null, null, "Enter saleperson name", JOptionPane.QUESTION_MESSAGE) + "%";

				       if (salesperson != "%%")
				       {
					       query = "SELECT C.name, C.id FROM contacts C, staff_works_in SWI WHERE C.name LIKE ? AND C.id = SWI.sid";

					       pstatement = connection.prepareStatement(query);

					       // instantiate parameters
					       pstatement.clearParameters();
					       pstatement.setString(1, salesperson);

					       resultSet = pstatement.executeQuery();

					       displayResults(dFrame);
				       }

				       else
				       {
				 	       return;
				       }
			       }
               }

               else // (e.getSource() == transactionButton5) Add Vehicle/Sell Vehicle
               {
				   Object[] possibleValues = {"Add Vehicle","Sell Vehicle"};
            	   selectedValue = JOptionPane.showInputDialog(null,"Choose one","Input",JOptionPane.INFORMATION_MESSAGE,null,possibleValues,possibleValues[0]);

            	   if(selectedValue == "Add Vehicle")
            	   {
					   transactionNum = 11;

					   query = "SELECT MAX(V.stock_num) FROM vehicles V";

					   pstatement = connection.prepareStatement(query);

					   resultSet = pstatement.executeQuery();

					   resultSet.next();
					   correctStock_Num = resultSet.getInt(1) + 1;

					   query = "SELECT CURDATE()";

					   resultSet = statement.executeQuery(query);

					   resultSet.next();
					   correctDate = resultSet.getString(1);

					   String location = "%" + JOptionPane.showInputDialog(null, null, "Enter location name", JOptionPane.QUESTION_MESSAGE) + "%";

					   if (location != "%%")
					   {
						   query = "SELECT L.name, L.lid FROM locations L WHERE L.name LIKE ?";

						   pstatement = connection.prepareStatement(query);

						   // instantiate parameters
						   pstatement.clearParameters();
						   pstatement.setString(1, location);

						   resultSet = pstatement.executeQuery();

						   displayResults(dFrame);
					   }

					   else
					   {
						   return;
					   }
				   }

				   else
				   {
					   transactionNum = 12;

					   query = "SELECT CURDATE()";

					   resultSet = statement.executeQuery(query);

					   resultSet.next();
					   correctDate = resultSet.getString(1);

					   String location = "%" + JOptionPane.showInputDialog(null, null, "Enter location name", JOptionPane.QUESTION_MESSAGE) + "%";

					   if (location != "%%")
					   {
						   query = "SELECT L.name, L.lid FROM locations L WHERE L.name LIKE ?";

						   pstatement = connection.prepareStatement(query);

						   // instantiate parameters
						   pstatement.clearParameters();
						   pstatement.setString(1, location);

						   resultSet = pstatement.executeQuery();

						   displayResults(dFrame);
					   }

					   else
					   {
						   return;
					   }
				   }
               }
	       }

           catch (SQLException sse)
           {
               JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
               return;
           }
       }
   }

   private void displayResults(DataFrame dFrame)
   {
	   int width = 0;

       try
       {
           // columnNames holds the column names of the query result
           Vector<Object> columnNames = new Vector<Object>();

           // rows is a vector of vectors, each vector is a vector of
           // values representing a certain row of the query result
           Vector<Object> rows = new Vector<Object>();

           // get column headers
           ResultSetMetaData metaData = resultSet.getMetaData();

           for(int i = 1; i <= metaData.getColumnCount(); ++i)
               columnNames.addElement(metaData.getColumnLabel(i));

           // get row data
           int inc = 0;
           if(tableType == 2)
           {
	           resultSet.next(); //can't do for queries, but need for transactions
		   }

           do
           {
               Vector<Object> currentRow = new Vector<Object>();
               for(int i = 1; i <= metaData.getColumnCount(); ++i)
               {
                   if (tableType == 1 && i == 4)
                   {
                       images[inc] = ImageIO.read(resultSet.getBinaryStream(i));
                       currentRow.addElement("image");
                   }

                   else if (tableType == 1 && i == 5)
                   {
                       urls[inc] = resultSet.getString(i);
                       currentRow.addElement(urls[inc]);
                       inc++;
                   }

                   else
                   {
                       currentRow.addElement(resultSet.getString(i));
                   }
               }
               rows.addElement(currentRow);

           } while(resultSet.next()); //moves cursor to next record

           if (pstatement != null)
           {
               pstatement.close();
           }

           else
           {
               statement.close();
           }

           if (scroller != null)
               getContentPane().remove(scroller);

           // display table with ResultSet contents
           table = new JTable(rows, columnNames);
           table.setDefaultEditor(Object.class, null);

           if (tableType == 1)
           {
               clickHandler = new ClickHandler();
               table.addMouseListener(clickHandler);
               table.getColumnModel().getColumn(3).setCellRenderer(new Query3TableCellRenderer());
               table.getColumnModel().getColumn(4).setCellRenderer(new Query3TableCellRenderer());
           }

           else if(tableType == 2)
           {
			   clickHandler2 = new ClickHandler2(dFrame);
			   table.addMouseListener(clickHandler2);
		   }

           for(int x = 0; x < metaData.getColumnCount(); x++)
           {
			   table.getColumnModel().getColumn(x).setMaxWidth(38*5);
		   }

		   for(int x = 0; x < metaData.getColumnCount(); x++)
		   {
			   width += table.getColumnModel().getColumn(x).getWidth();
		   }

           table.setPreferredScrollableViewportSize(new Dimension(width * 2, 325));
           table.setAutoResizeMode(JTable.AUTO_RESIZE_ALL_COLUMNS);

           scroller = new JScrollPane(table);
           getContentPane().add(scroller);
           validate();
           repaint();
       }

       catch(SQLException ex)
       {
           JOptionPane.showMessageDialog(null, ex.getMessage(), "Query error!", JOptionPane.ERROR_MESSAGE);
       }

       catch(IOException ex)
       {
           JOptionPane.showMessageDialog(null, ex.getMessage(), "Image error!", JOptionPane.ERROR_MESSAGE);
       }
   }

   // private class for handling images and hyperlinks for query 3
   private class ClickHandler extends MouseAdapter
   {
       @Override
       public void mouseClicked( MouseEvent e )
       {
           if ( e.getClickCount() == 2 )
           {
               if (table.columnAtPoint(e.getPoint()) == 3)
               {
                   getImage(table.rowAtPoint(e.getPoint()));
               }

               else if (table.columnAtPoint(e.getPoint()) == 4)
               {
                   openUrl(table.rowAtPoint(e.getPoint()));
               }
           }
       }
   }

   private class ClickHandler2 extends MouseAdapter
   {
	   DataFrame dFrame;

	   public ClickHandler2(DataFrame dFrame)
	   {
		   this.dFrame = dFrame;
	   }

       @Override
       public void mouseClicked( MouseEvent e )
       {
		   int row;

           if ( e.getClickCount() == 2 )
           {
			   if(transactionNum == 1)
			   {
				   row = table.rowAtPoint(e.getPoint());
            	   correctSID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

				   tFrame = new TransactionFrame(transactionNum, connection, dFrame);
				   tFrame.addCustomer(maxCid + 1, correctSID);
			   }

		  	   else if(transactionNum == 2)
		  	   {
				   row = table.rowAtPoint(e.getPoint());
            	   correctCID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

            	   tFrame = new TransactionFrame(transactionNum, connection, dFrame);
            	   tFrame.updatePersonalInfo(correctCID);
			   }

			   else if(transactionNum == 3)
			   {
				   row = table.rowAtPoint(e.getPoint());
            	   correctCID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

            	   getDBLocation(dFrame);
			   }

			   else if(transactionNum == 4)
			   {
				   row = table.rowAtPoint(e.getPoint());
            	   correctLID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

            	   getStock_Num(dFrame);
			   }

			   else if(transactionNum == 5)
			   {
				   row = table.rowAtPoint(e.getPoint());
				   correctStock_Num = Integer.parseInt(table.getModel().getValueAt(row,0).toString());

				   tFrame = new TransactionFrame(transactionNum, connection, dFrame);
				   tFrame.updateVisitInfo(correctCID, correctDate_Time, correctStock_Num, correctLID);
			   }

			   else if(transactionNum == 6)
			   {
				   try
				   {
					   row = table.rowAtPoint(e.getPoint());
					   correctCID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

					   transactionNum = 7;

					   String salesperson = "%" + JOptionPane.showInputDialog(null, null, "Enter saleperson name", JOptionPane.QUESTION_MESSAGE) + "%";

					   if (salesperson != "%%")
					   {
						   query = "SELECT C.name, C.id FROM contacts C, staff_works_in SWI WHERE C.name LIKE ? AND C.id = SWI.sid";

						   pstatement = connection.prepareStatement(query);

						   // instantiate parameters
						   pstatement.clearParameters();
						   pstatement.setString(1, salesperson);

						   resultSet = pstatement.executeQuery();

						   displayResults(dFrame);
					   }

					   else
					   {
						   return;
					   }
				   }

				   catch (SQLException sse)
				   {
				 	   JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					   return;
				   }
			   }

			   else if(transactionNum == 7)
			   {
				   try
				   {
				  	   row = table.rowAtPoint(e.getPoint());
				  	   correctSID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

					   transaction = "UPDATE customers_engaged_with CEW SET CEW.sid = ? WHERE CEW.cid = ?";

					   pstatement = connection.prepareStatement(transaction);

					   // instantiate parameters
					   pstatement.clearParameters();
					   pstatement.setInt(1, correctSID);
					   pstatement.setInt(2, correctCID);

					   pstatement.executeUpdate();

					   dFrame.getContentPane().remove(dFrame.scroller);
					   dFrame.validate();
					   dFrame.repaint();
				   }

				   catch (SQLException sse)
				   {
				 	   JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					   return;
				   }
			   }

			   else if(transactionNum == 8)
			   {
				   row = table.rowAtPoint(e.getPoint());
				   correctLID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

				   tFrame = new TransactionFrame(transactionNum, connection, dFrame);
				   tFrame.addSalesperson(maxSid + 1, correctLID, correctDate);
			   }

			   else if(transactionNum == 9)
			   {
				   try
				   {
					   transactionNum = 10;

					   row = table.rowAtPoint(e.getPoint());
					   correctSID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

					   query = "SELECT CEW.cid FROM customers_engaged_with CEW WHERE CEW.sid = ?";

					   pstatement = connection.prepareStatement(query);

					   // instantiate parameters
					   pstatement.clearParameters();
					   pstatement.setInt(1, correctSID);

					   resultSet = pstatement.executeQuery();

					   if(resultSet.next())
					   {
						   correctCID = resultSet.getInt(1);

						   String salesperson = "%" + JOptionPane.showInputDialog(null, null, "Enter saleperson name", JOptionPane.QUESTION_MESSAGE) + "%";

						   if (salesperson != "%%")
						   {
							   query = "SELECT C.name, C.id FROM contacts C, staff_works_in SWI WHERE C.name LIKE ? AND C.id = SWI.sid";

							   pstatement = connection.prepareStatement(query);

							   // instantiate parameters
							   pstatement.clearParameters();
							   pstatement.setString(1, salesperson);

							   resultSet = pstatement.executeQuery();

							   displayResults(dFrame);
						   }

						   else
						   {
							   return;
						   }
					   }

					   else
					   {
						   transaction = "DELETE FROM contacts C WHERE C.id = ?";

						   pstatement = connection.prepareStatement(transaction);

						   // instantiate parameters
						   pstatement.clearParameters();
						   pstatement.setInt(1, correctSID);

						   pstatement.executeUpdate();

					   	   dFrame.getContentPane().remove(dFrame.scroller);
					       dFrame.validate();
					       dFrame.repaint();
					   }
				   }

				   catch (SQLException sse)
				   {
				 	   JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					   return;
				   }
			   }

			   else if(transactionNum == 10)
			   {
				   try
				   {
					   row = table.rowAtPoint(e.getPoint());
					   correctSID2 = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

					   transaction = "UPDATE customers_engaged_with CEW SET CEW.sid = ? WHERE CEW.cid = ?";

					   pstatement = connection.prepareStatement(transaction);

					   // instantiate parameters
					   pstatement.clearParameters();
					   pstatement.setInt(1, correctSID2);
					   pstatement.setInt(2, correctCID);

					   pstatement.executeUpdate();

					   transaction = "DELETE FROM contacts C WHERE C.id = ?";

					   pstatement = connection.prepareStatement(transaction);

					   // instantiate parameters
					   pstatement.clearParameters();
					   pstatement.setInt(1, correctSID);

					   pstatement.executeUpdate();

					   dFrame.getContentPane().remove(dFrame.scroller);
					   dFrame.validate();
					   dFrame.repaint();
				   }

				   catch (SQLException sse)
				   {
				 	   JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					   return;
				   }
			   }

			   else if(transactionNum == 11)
			   {
				   row = table.rowAtPoint(e.getPoint());
				   correctLID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

				   tFrame = new TransactionFrame(transactionNum, connection, dFrame);
				   tFrame.addVehicle(correctStock_Num, correctLID, correctDate);
			   }

			   else if(transactionNum == 12)
			   {
				   try
				   {
					   transactionNum = 13;

					   row = table.rowAtPoint(e.getPoint());
					   correctLID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

					   query = "SELECT V.stock_num, V.make, V.model, V.year FROM vehicles V WHERE V.lid = ? AND V.status = ?";

					   pstatement = connection.prepareStatement(query);

					   // instantiate parameters
					   pstatement.clearParameters();
					   pstatement.setInt(1, correctLID);
					   pstatement.setString(2, "available");

					   resultSet = pstatement.executeQuery();

				   	   displayResults(dFrame);
				   }

				   catch (SQLException sse)
				   {
				 	   JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					   return;
				   }
			   }

			   else if(transactionNum == 13)
			   {
				   try
				   {
					   transactionNum = 14;

					   row = table.rowAtPoint(e.getPoint());
					   correctStock_Num = Integer.parseInt(table.getModel().getValueAt(row,0).toString());

					   String customer = "%" + JOptionPane.showInputDialog(null, null, "Enter customer name", JOptionPane.QUESTION_MESSAGE) + "%";

					   if (customer != "%%")
					   {
						   query = "SELECT C.name, C.id FROM contacts C, customers_engaged_with CEW WHERE C.name LIKE ? AND C.id = CEW.cid";

						   pstatement = connection.prepareStatement(query);

						   // instantiate parameters
						   pstatement.clearParameters();
						   pstatement.setString(1, customer);

						   resultSet = pstatement.executeQuery();

						   displayResults(dFrame);
					   }

					   else
					   {
						   return;
					   }
				   }

				   catch (SQLException sse)
				   {
				 	   JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					   return;
				   }
			   }

			   else if(transactionNum == 14)
			   {
				   try
				   {
					   row = table.rowAtPoint(e.getPoint());
					   correctCID = Integer.parseInt(table.getModel().getValueAt(row,1).toString());

					   query = "SELECT CEW.sid FROM customers_engaged_with CEW WHERE CEW.cid = ?";

					   pstatement = connection.prepareStatement(query);

					   // instantiate parameters
					   pstatement.clearParameters();
					   pstatement.setInt(1, correctCID);

					   resultSet = pstatement.executeQuery();

					   resultSet.next();
					   correctSID = resultSet.getInt(1);

					   transaction = "UPDATE vehicles V SET V.cid = ?, V.sid = ?, V.sale_date = ? WHERE V.stock_num = ? AND V.lid = ?";

					   pstatement = connection.prepareStatement(transaction);

					   // instantiate parameters
					   pstatement.clearParameters();
					   pstatement.setInt(1, correctCID);
					   pstatement.setInt(2, correctSID);
					   pstatement.setString(3, correctDate);
					   pstatement.setInt(4, correctStock_Num);
					   pstatement.setInt(5, correctLID);

					   pstatement.executeUpdate();

					   dFrame.getContentPane().remove(dFrame.scroller);
					   dFrame.validate();
					   dFrame.repaint();
				   }

				   catch (SQLException sse)
				   {
				 	   JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					   return;
				   }
			   }
           }
       }
   }

   private void getDBLocation(DataFrame dFrame)
   {
	   try
	   {
		   transactionNum = 4;

		   String location = "%" + JOptionPane.showInputDialog(null, null, "Enter location name", JOptionPane.QUESTION_MESSAGE) + "%";

		   if (location != "%%")
		   {
			   query = "SELECT L.name, L.lid FROM locations L WHERE L.name LIKE ?";

			   pstatement = connection.prepareStatement(query);

			   // instantiate parameters
			   pstatement.clearParameters();
			   pstatement.setString(1, location);

			   resultSet = pstatement.executeQuery();

			   displayResults(dFrame);
		   }

		   else
		   {
			   return;
		   }
	   }

       catch (SQLException sse)
       {
           JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
           return;
       }
   }

   private void getStock_Num(DataFrame dFrame)
   {
	   try
	   {
		   transactionNum = 5;

		   query = "SELECT V.stock_num, V.make, V.model, V.year FROM vehicles V WHERE V.lid = ? AND V.status = ?";

		   pstatement = connection.prepareStatement(query);

		   // instantiate parameters
		   pstatement.clearParameters();
		   pstatement.setInt(1, correctLID);
		   pstatement.setString(2, "available");

		   resultSet = pstatement.executeQuery();

		   displayResults(dFrame);
	   }

       catch (SQLException sse)
       {
           JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
           return;
       }
   }

   private void getImage(int index)
   {
       Image image;
       ImagePanel imagePanel;
       Dimension d;
       JDialog myDialog;

       d = new Dimension(650, 500);
       image = images[index].getScaledInstance( (int)d.getWidth(), (int)d.getHeight(), Image.SCALE_SMOOTH);
       myDialog = new JDialog();
       imagePanel = new ImagePanel( image );
       myDialog.add( imagePanel );
       myDialog.setDefaultCloseOperation( DISPOSE_ON_CLOSE );
       myDialog.setSize( new Dimension( (int)d.getWidth(), (int)d.getHeight()) );
       myDialog.setTitle( table.getModel().getValueAt(index, 0) + " " + table.getModel().getValueAt(index, 1) );
       myDialog.setModal(true);
       myDialog.setLocationRelativeTo(null);
       myDialog.setVisible( true );
   }


   private void openUrl(int index)
   {
       String url = urls[index];

       if(Desktop.isDesktopSupported())
       {
           Desktop desktop = Desktop.getDesktop();
           try
           {
               desktop.browse(new URI(url));
           }
           catch (IOException | URISyntaxException e)
           {
               // TODO Auto-generated catch block
               e.printStackTrace();
           }
       }
       else
       {
           Runtime runtime = Runtime.getRuntime();
           try
           {
               runtime.exec("xdg-open " + url);
           }
           catch (IOException e)
           {
               // TODO Auto-generated catch block
               e.printStackTrace();
           }
       }
   }
}