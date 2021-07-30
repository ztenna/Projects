import javax.swing.*;
import java.awt.*;
import javax.swing.GroupLayout.*;
import java.awt.event.*;
import java.sql.*;
import javax.swing.filechooser.*;
import java.io.*;

public class TransactionFrame extends JDialog
								implements ActionListener
{
    private JTextField streetTF;
    private JTextField cityTF;
    private JTextField zipcodeTF;
    private JTextField stateTF;
    private JTextField phoneTF;
    private JTextField emailTF;
    private JTextField nameTF;
    private JTextField notesTF;
    private JTextField passwordTF;
    private JTextField makeTF;
    private JTextField modelTF;
    private JTextField yearTF;
    private JTextField priceTF;

    private JLabel streetLabel;
    private JLabel cityLabel;
    private JLabel zipcodeLabel;
    private JLabel stateLabel;
    private JLabel phoneLabel;
    private JLabel emailLabel;
    private JLabel nameLabel;
    private JLabel notesLabel;
    private JLabel passwordLabel;
    private JLabel makeLabel;
    private JLabel modelLabel;
    private JLabel yearLabel;
    private JLabel priceLabel;

    private JPanel panel;

    private int transactionNum;
    private int maxCid;
    private int maxSid;
    private Connection connection;
    private PreparedStatement pstatement;
    private ResultSet resultSet;
    private int correctSID;
    private int correctCID;
    private int correctLID;
    private String correctDate;
    private int correctStock_Num;
    private String correctDate_Time;
    private Object selectedValue;

    DataFrame dFrame;

    private JButton execute;
    private String transaction;
    private String query;

    private String street;
    private String city;
    private String zipcode;
    private String state;
    private String phone;
    private String email;
    private String name;
    private String notes;
    private String password;
    private String make;
    private String model;
    private String year;
    private String price;

	private JFileChooser chooser;
	private String filename;
	private FileNameExtensionFilter filter;
	private int response;
	private String link;
	private File file;
	private FileInputStream input;

    public TransactionFrame(int transactionNum, Connection connection, DataFrame dFrame)
    {
	    this.transactionNum = transactionNum;
	    this.connection = connection;
	    this.dFrame = dFrame;

	    execute = new JButton("Execute");
	    execute.addActionListener(this);
    }

    void addCustomer(int maxCid, int correctSID)
    {
	    this.maxCid = maxCid;
	    this.correctSID = correctSID;

	    streetTF = new JTextField();
	    cityTF = new JTextField();
	    zipcodeTF = new JTextField();
	    stateTF = new JTextField();
	    phoneTF = new JTextField();
	    emailTF = new JTextField();
	    nameTF = new JTextField();
	    notesTF = new JTextField();

	    doLayout1();
	    setTitle("Add Customer");
	    setup();
	}

    void updatePersonalInfo(int correctCID)
    {
	    this.correctCID = correctCID;

	    try
	    {
			query = "SELECT A.street, A.city, A.zipcode, A.state, C.phone, C.email, C.name, CEW.notes FROM addresses A, contacts C, customers_engaged_with CEW WHERE C.id = ? AND C.street = A.street AND C.zipcode = A.zipcode AND C.id = CEW.cid";

			pstatement = connection.prepareStatement(query);

			// instantiate parameters
			pstatement.clearParameters();
			pstatement.setInt(1, correctCID);

			resultSet = pstatement.executeQuery();
			resultSet.next();

			streetTF = new JTextField(resultSet.getString(1));
			cityTF = new JTextField(resultSet.getString(2));
			zipcodeTF = new JTextField(resultSet.getString(3));
			stateTF = new JTextField(resultSet.getString(4));
			phoneTF = new JTextField(resultSet.getString(5));
			emailTF = new JTextField(resultSet.getString(6));
			nameTF = new JTextField(resultSet.getString(7));
			notesTF = new JTextField(resultSet.getString(8));
		}

		catch (SQLException sse)
		{
			JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
			return;
		}

	    doLayout1();
	    setTitle("Update Customer Info");
	    setup();
	}

	void updateVisitInfo(int correctCID, String correctDate_Time, int correctStock_Num, int correctLID)
	{
		this.correctCID = correctCID;
		this.correctDate_Time = correctDate_Time;
		this.correctStock_Num = correctStock_Num;
		this.correctLID = correctLID;

		try
		{
			query = "SELECT CEW.notes FROM customers_engaged_with CEW WHERE CEW.cid = ?";

			pstatement = connection.prepareStatement(query);

			// instantiate parameters
			pstatement.clearParameters();
			pstatement.setInt(1, correctCID);

			resultSet = pstatement.executeQuery();
			resultSet.next();

			notesTF = new JTextField(resultSet.getString(1));
		}

		catch (SQLException sse)
		{
			JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
			return;
		}

		doLayout2();
		setTitle("Update Visit Info");
		setup();
	}

	void addSalesperson(int maxSid, int correctLID, String correctDate)
	{
		this.maxSid = maxSid;
		this.correctLID = correctLID;
		this.correctDate = correctDate;

		Object[] possibleValues = {"Add Floor Salesperson","Add Internet Salesperson", "Add Sales Manager"};
		selectedValue = JOptionPane.showInputDialog(null,"Choose one","Input",JOptionPane.INFORMATION_MESSAGE,null,possibleValues,possibleValues[0]);

	    streetTF = new JTextField();
	    cityTF = new JTextField();
	    zipcodeTF = new JTextField();
	    stateTF = new JTextField();
	    phoneTF = new JTextField();
	    emailTF = new JTextField();
	    nameTF = new JTextField();
	    passwordTF = new JTextField();

	    doLayout3();
	    setTitle("Add Salesperson");
	    setup();
    }

    void addVehicle(int correctStock_Num, int correctLID, String correctDate)
    {
		this.correctStock_Num = correctStock_Num;
		this.correctLID = correctLID;
		this.correctDate = correctDate;

	    makeTF = new JTextField();
	    modelTF = new JTextField();
	    yearTF = new JTextField();
	    priceTF = new JTextField();

	    doLayout4();
	    setTitle("Add Vehicle");
	    setup();
	}

    void setup()
    {
	    setSize(700, 700);
	    //setResizable(true);
	    setLocationRelativeTo(null);
	    setVisible(true);
    }

    void doLayout1()
    {
	    streetLabel = new JLabel("Street");
	    cityLabel = new JLabel("City");
	    zipcodeLabel = new JLabel("Zipcode");
	    stateLabel = new JLabel("State");
	    phoneLabel = new JLabel("Phone");
	    emailLabel = new JLabel("Email");
	    nameLabel = new JLabel("Name");
	    notesLabel = new JLabel("Notes");

	    panel = new JPanel();

	    GroupLayout layout = new GroupLayout(panel);
	    panel.setLayout(layout);

	    layout.setAutoCreateGaps(true);

	    layout.setAutoCreateContainerGaps(true);

	    GroupLayout.SequentialGroup hGroup = layout.createSequentialGroup();

	    hGroup.addGroup(layout.createParallelGroup().addComponent(nameLabel).addComponent(streetLabel).addComponent(cityLabel).
				addComponent(stateLabel).addComponent(zipcodeLabel).addComponent(phoneLabel).addComponent(emailLabel).addComponent(notesLabel));

	    hGroup.addGroup(layout.createParallelGroup().addComponent(nameTF).addComponent(streetTF).addComponent(cityTF).
				addComponent(stateTF).addComponent(zipcodeTF).addComponent(phoneTF).addComponent(emailTF).addComponent(notesTF));

	    layout.setHorizontalGroup(hGroup);

	    GroupLayout.SequentialGroup vGroup = layout.createSequentialGroup();

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(nameLabel).addComponent(nameTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(streetLabel).addComponent(streetTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(cityLabel).addComponent(cityTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(stateLabel).addComponent(stateTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(zipcodeLabel).addComponent(zipcodeTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(phoneLabel).addComponent(phoneTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(emailLabel).addComponent(emailTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(notesLabel).addComponent(notesTF));

	    layout.setVerticalGroup(vGroup);

	    add(panel, BorderLayout.CENTER);
	    add(execute, BorderLayout.SOUTH);
    }

    void doLayout2()
    {
	    notesLabel = new JLabel("Notes");

	    panel = new JPanel();

	    GroupLayout layout = new GroupLayout(panel);
	    panel.setLayout(layout);

	    layout.setAutoCreateGaps(true);

	    layout.setAutoCreateContainerGaps(true);

	    GroupLayout.SequentialGroup hGroup = layout.createSequentialGroup();

	    hGroup.addGroup(layout.createParallelGroup().addComponent(notesLabel));

	    hGroup.addGroup(layout.createParallelGroup().addComponent(notesTF));

	    layout.setHorizontalGroup(hGroup);

	    GroupLayout.SequentialGroup vGroup = layout.createSequentialGroup();

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(notesLabel).addComponent(notesTF));

	    layout.setVerticalGroup(vGroup);

	    add(panel, BorderLayout.CENTER);
	    add(execute, BorderLayout.SOUTH);
    }

    void doLayout3()
    {
	    streetLabel = new JLabel("Street");
	    cityLabel = new JLabel("City");
	    zipcodeLabel = new JLabel("Zipcode");
	    stateLabel = new JLabel("State");
	    phoneLabel = new JLabel("Phone");
	    emailLabel = new JLabel("Email");
	    nameLabel = new JLabel("Name");
	    passwordLabel = new JLabel("Password");

	    panel = new JPanel();

	    GroupLayout layout = new GroupLayout(panel);
	    panel.setLayout(layout);

	    layout.setAutoCreateGaps(true);

	    layout.setAutoCreateContainerGaps(true);

	    GroupLayout.SequentialGroup hGroup = layout.createSequentialGroup();

	    hGroup.addGroup(layout.createParallelGroup().addComponent(nameLabel).addComponent(streetLabel).addComponent(cityLabel).
				addComponent(stateLabel).addComponent(zipcodeLabel).addComponent(phoneLabel).addComponent(emailLabel).addComponent(passwordLabel));

	    hGroup.addGroup(layout.createParallelGroup().addComponent(nameTF).addComponent(streetTF).addComponent(cityTF).
				addComponent(stateTF).addComponent(zipcodeTF).addComponent(phoneTF).addComponent(emailTF).addComponent(passwordTF));

	    layout.setHorizontalGroup(hGroup);

	    GroupLayout.SequentialGroup vGroup = layout.createSequentialGroup();

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(nameLabel).addComponent(nameTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(streetLabel).addComponent(streetTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(cityLabel).addComponent(cityTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(stateLabel).addComponent(stateTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(zipcodeLabel).addComponent(zipcodeTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(phoneLabel).addComponent(phoneTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(emailLabel).addComponent(emailTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(passwordLabel).addComponent(passwordTF));

	    layout.setVerticalGroup(vGroup);

	    add(panel, BorderLayout.CENTER);
	    add(execute, BorderLayout.SOUTH);
	}

    void doLayout4()
    {
	    makeLabel = new JLabel("Make");
	    modelLabel = new JLabel("Model");
	    yearLabel = new JLabel("Year");
	    priceLabel = new JLabel("Price");

	    panel = new JPanel();

	    GroupLayout layout = new GroupLayout(panel);
	    panel.setLayout(layout);

	    layout.setAutoCreateGaps(true);

	    layout.setAutoCreateContainerGaps(true);

	    GroupLayout.SequentialGroup hGroup = layout.createSequentialGroup();

	    hGroup.addGroup(layout.createParallelGroup().addComponent(makeLabel).addComponent(modelLabel).addComponent(yearLabel).
				addComponent(priceLabel));

	    hGroup.addGroup(layout.createParallelGroup().addComponent(makeTF).addComponent(modelTF).addComponent(yearTF).
				addComponent(priceTF));

	    layout.setHorizontalGroup(hGroup);

	    GroupLayout.SequentialGroup vGroup = layout.createSequentialGroup();

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(makeLabel).addComponent(makeTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(modelLabel).addComponent(modelTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(yearLabel).addComponent(yearTF));

	    vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).addComponent(priceLabel).addComponent(priceTF));

	    layout.setVerticalGroup(vGroup);

	    add(panel, BorderLayout.CENTER);
	    add(execute, BorderLayout.SOUTH);
	}

	public void actionPerformed(ActionEvent e)
	{
		String transaction;
		String query;

		if(e.getSource() == execute)
		{
			if(transactionNum == 1)
			{
				try
				{
					street = streetTF.getText().trim();
					city = cityTF.getText().trim();
					zipcode = zipcodeTF.getText().trim();
					state = stateTF.getText().trim();
					phone = phoneTF.getText().trim();
					email = emailTF.getText().trim();
					name = nameTF.getText().trim();
					notes = notesTF.getText().trim();

					query = "SELECT COUNT(*) FROM addresses A WHERE A.street = ? AND A.zipcode = ?";

					pstatement = connection.prepareStatement(query);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setString(1, street);
					pstatement.setString(2, zipcode);

					resultSet = pstatement.executeQuery();
					resultSet.next();

					if(resultSet.getInt(1) == 0)
					{
						transaction = "INSERT INTO addresses (street,city,state,zipcode) VALUES(?,?,?,?)";

						pstatement = connection.prepareStatement(transaction);

						// instantiate parameters
						pstatement.clearParameters();
						pstatement.setString(1, street);
						pstatement.setString(2, city);
						pstatement.setString(3, state);
						pstatement.setString(4, zipcode);

						pstatement.executeUpdate();
					}

					transaction = "INSERT INTO contacts (id,name,street,zipcode,phone,email) VALUES(?,?,?,?,?,?)";

					pstatement = connection.prepareStatement(transaction);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setInt(1, maxCid);
					pstatement.setString(2, name);
					pstatement.setString(3, street);
					pstatement.setString(4, zipcode);
					pstatement.setString(5, phone);
					pstatement.setString(6, email);

					pstatement.executeUpdate();

					transaction = "SELECT * FROM floor_salespeople FS WHERE FS.sid = ?";

					pstatement = connection.prepareStatement(transaction);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setInt(1, correctSID);

					resultSet = pstatement.executeQuery();

					if(!resultSet.next())
					{
						transaction = "INSERT INTO customers_engaged_with (cid,sid,notes) VALUES(?,?,?)";

						pstatement = connection.prepareStatement(transaction);

						// instantiate parameters
						pstatement.clearParameters();
						pstatement.setInt(1, maxCid);
						pstatement.setString(2, null);
						pstatement.setString(3, notes);

						pstatement.executeUpdate();
					}
					else
					{
						transaction = "INSERT INTO customers_engaged_with (cid,sid,notes) VALUES(?,?,?)";

						pstatement = connection.prepareStatement(transaction);

						// instantiate parameters
						pstatement.clearParameters();
						pstatement.setInt(1, maxCid);
						pstatement.setInt(2, correctSID);
						pstatement.setString(3, notes);

						pstatement.executeUpdate();
					}
				}
				catch (SQLException sse)
				{
					JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					return;
				}
			}

			else if(transactionNum == 2)
			{
				try
				{
					street = streetTF.getText().trim();
					city = cityTF.getText().trim();
					zipcode = zipcodeTF.getText().trim();
					state = stateTF.getText().trim();
					phone = phoneTF.getText().trim();
					email = emailTF.getText().trim();
					name = nameTF.getText().trim();
					notes = notesTF.getText().trim();

					query = "SELECT COUNT(*) FROM addresses A WHERE A.street = ? AND A.zipcode = ?";

					pstatement = connection.prepareStatement(query);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setString(1, street);
					pstatement.setString(2, zipcode);

					resultSet = pstatement.executeQuery();
					resultSet.next();

					if(resultSet.getInt(1) == 0)
					{
						transaction = "INSERT INTO addresses (street,city,state,zipcode) VALUES(?,?,?,?)";

						pstatement = connection.prepareStatement(transaction);

						// instantiate parameters
						pstatement.clearParameters();
						pstatement.setString(1, street);
						pstatement.setString(2, city);
						pstatement.setString(3, state);
						pstatement.setString(4, zipcode);

						pstatement.executeUpdate();
					}

					transaction = "UPDATE contacts C SET C.name = ?, C.street = ?, C.zipcode = ?, C.phone = ?, C.email = ? WHERE C.id = ?";

					pstatement = connection.prepareStatement(transaction);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setString(1, name);
					pstatement.setString(2, street);
					pstatement.setString(3, zipcode);
					pstatement.setString(4, phone);
					pstatement.setString(5, email);
					pstatement.setInt(6, correctCID);

					pstatement.executeUpdate();

					transaction = "UPDATE customers_engaged_with CEW SET CEW.notes = ? WHERE CEW.cid = ?";

					pstatement = connection.prepareStatement(transaction);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setString(1, notes);
					pstatement.setInt(2, correctCID);

					pstatement.executeUpdate();
				}
				catch (SQLException sse)
				{
					JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					return;
				}
			}

			else if(transactionNum == 5)
			{
				try
				{
					notes = notesTF.getText().trim();

					transaction = "INSERT INTO timestamps VALUES(?)";

					pstatement = connection.prepareStatement(transaction);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setString(1, correctDate_Time);

					pstatement.executeUpdate();

					transaction = "INSERT INTO visit_info VALUES(?,?,?,?)";

					pstatement = connection.prepareStatement(transaction);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setInt(1, correctCID);
					pstatement.setInt(2, correctStock_Num);
					pstatement.setInt(3, correctLID);
					pstatement.setString(4, correctDate_Time);

					pstatement.executeUpdate();

					transaction = "UPDATE customers_engaged_with CEW SET CEW.notes = ? WHERE CEW.cid = ?";

					pstatement = connection.prepareStatement(transaction);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setString(1, notes);
					pstatement.setInt(2, correctCID);

					pstatement.executeUpdate();
				}

				catch (SQLException sse)
				{
					JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					return;
				}
			}

			else if(transactionNum == 8)
			{
				try
				{
					street = streetTF.getText().trim();
					city = cityTF.getText().trim();
					zipcode = zipcodeTF.getText().trim();
					state = stateTF.getText().trim();
					phone = phoneTF.getText().trim();
					email = emailTF.getText().trim();
					name = nameTF.getText().trim();
					password = passwordTF.getText().trim();

					query = "SELECT COUNT(*) FROM addresses A WHERE A.street = ? AND A.zipcode = ?";

					pstatement = connection.prepareStatement(query);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setString(1, street);
					pstatement.setString(2, zipcode);

					resultSet = pstatement.executeQuery();
					resultSet.next();

					if(resultSet.getInt(1) == 0)
					{
						transaction = "INSERT INTO addresses (street,city,state,zipcode) VALUES(?,?,?,?)";

						pstatement = connection.prepareStatement(transaction);

						// instantiate parameters
						pstatement.clearParameters();
						pstatement.setString(1, street);
						pstatement.setString(2, city);
						pstatement.setString(3, state);
						pstatement.setString(4, zipcode);

						pstatement.executeUpdate();
					}

					transaction = "INSERT INTO contacts (id,name,street,zipcode,phone,email) VALUES(?,?,?,?,?,?)";

					pstatement = connection.prepareStatement(transaction);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setInt(1, maxSid);
					pstatement.setString(2, name);
					pstatement.setString(3, street);
					pstatement.setString(4, zipcode);
					pstatement.setString(5, phone);
					pstatement.setString(6, email);

					pstatement.executeUpdate();

					transaction = "INSERT INTO staff_works_in (sid,since,lid,password) VALUES(?,?,?,?)";

					pstatement = connection.prepareStatement(transaction);

					// instantiate parameters
					pstatement.clearParameters();
					pstatement.setInt(1, maxSid);
					pstatement.setString(2, correctDate);
					pstatement.setInt(3, correctLID);
					pstatement.setString(4, password);

					pstatement.executeUpdate();

					if(selectedValue == "Add Floor Salesperson")
					{
						transaction = "INSERT INTO floor_salespeople (sid) VALUES(?)";

						pstatement = connection.prepareStatement(transaction);

						// instantiate parameters
						pstatement.clearParameters();
						pstatement.setInt(1, maxSid);

						pstatement.executeUpdate();
					}

					else if(selectedValue == "Add Internet Salesperson")
					{
						transaction = "INSERT INTO internet_salespeople (sid) VALUES(?)";

						pstatement = connection.prepareStatement(transaction);

						// instantiate parameters
						pstatement.clearParameters();
						pstatement.setInt(1, maxSid);

						pstatement.executeUpdate();
					}

					else
					{
						transaction = "INSERT INTO sales_manager (sid) VALUES(?)";

						pstatement = connection.prepareStatement(transaction);

						// instantiate parameters
						pstatement.clearParameters();
						pstatement.setInt(1, maxSid);

						pstatement.executeUpdate();
					}
				}
				catch (SQLException sse)
				{
					JOptionPane.showMessageDialog(null, sse.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					return;
				}
			}

			else if(transactionNum == 11)
			{
				try
				{
					make = makeTF.getText().trim();
					model = modelTF.getText().trim();
					year = yearTF.getText().trim();
					price = priceTF.getText().trim();

					link = "https://www.caranddriver.com/" + make + "/" + model;
					link = link.toLowerCase().replace(' ','-');

					chooser = new JFileChooser();
					chooser.setCurrentDirectory(new java.io.File("."));
					chooser.setDialogTitle("Choose Vehicle Image");
					filter = new FileNameExtensionFilter("JPG, GIF, and PNG Images", "jpg", "gif", "png");
					chooser.setFileFilter(filter);

					response = chooser.showOpenDialog(this);

					if(response == JFileChooser.APPROVE_OPTION)
					{
						filename = chooser.getCurrentDirectory() + "\\" + chooser.getSelectedFile().getName();
						file = new File(filename);
						input = new FileInputStream(file);

						transaction = "INSERT INTO vehicles (stock_num,lid,cid,sid,delivery_date,sale_date,commission,make,model,year,price,status,image,hyperlink) VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

						pstatement = connection.prepareStatement(transaction);

						// instantiate parameters
						pstatement.clearParameters();
						pstatement.setInt(1, correctStock_Num);
						pstatement.setInt(2, correctLID);
						pstatement.setString(3, null);
						pstatement.setString(4, null);
						pstatement.setString(5, correctDate);
						pstatement.setString(6, null);
						pstatement.setString(7, null);
						pstatement.setString(8, make);
						pstatement.setString(9, model);
						pstatement.setString(10, year);
						pstatement.setString(11, price);
						pstatement.setString(12, "available");
						pstatement.setBinaryStream(13, input);
						pstatement.setString(14, link);

						pstatement.executeUpdate();
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

				catch (FileNotFoundException fnfe)
				{
					JOptionPane.showMessageDialog(null, fnfe.getMessage(), "Transaction error!", JOptionPane.ERROR_MESSAGE);
					return;
				}
			}
		}

		dFrame.getContentPane().remove(dFrame.scroller);
		dFrame.validate();
		dFrame.repaint();
		dispose();
	}
}