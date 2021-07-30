// Zachary Tennant

import java.awt.*;
import javax.swing.*;
import javax.swing.GroupLayout.*;
import java.awt.event.*;
import javax.swing.JComponent;
import java.text.*;
import java.util.Date;

class TripDialog extends JDialog
						implements ActionListener
{
	TripManager tripManager;
	int selectedIndex;
	JPanel buttonPanel;

	JButton saveOpenButton;
	JButton saveCloseButton;
	JButton cancelButton;
	JButton submitButton;

	JTextField dateOfTripTF;
	JTextField patientNameTF;
	JTextField initialMileTF;
	JTextField retMileTF;
	JTextField billRateTF;
	JTextField commentTF;

	String popDateOfTrip;
	String popPatientName;
	String popInitialMile;
	String popRetMile;
	String popBillRate;
	String popComment;

	JComboBox<String> servCodeList;

	MyFrame myFrame;
	//TripRecordTableModel trTableModel;


// add constructor
	public TripDialog(TripManager tripManager, MyFrame myFrame)
	{
		this.tripManager = tripManager;
		this.myFrame = myFrame;
		buildBasicGUI();

		buttonPanel = new JPanel(new GridLayout(1,3));

	// save and stay open button
		saveOpenButton = new JButton("Save and Stay Open");
		saveOpenButton.setActionCommand("ADD_STAY_OPEN");
		saveOpenButton.setToolTipText("Save the record and stay open");
		saveOpenButton.addActionListener(this);
    	buttonPanel.add(saveOpenButton);

	// save and close button
    	saveCloseButton = new JButton("Save and Close");
    	saveCloseButton.setActionCommand("ADD_CLOSE");
    	saveCloseButton.setToolTipText("Save the record and close");
    	saveCloseButton.addActionListener(this);
    	buttonPanel.add(saveCloseButton);

	// cancel button
    	cancelButton = new JButton("Cancel");
    	cancelButton.setActionCommand("ADD_CANCEL");
    	cancelButton.setToolTipText("Cancel");
    	cancelButton.addActionListener(this);
    	cancelButton.setVerifyInputWhenFocusTarget(false);
    	buttonPanel.add(cancelButton);

    	add(buttonPanel, BorderLayout.SOUTH);

    	dateOfTripTF.setInputVerifier(new DateVerifier());
    	initialMileTF.setInputVerifier(new MileageVerifier());
    	retMileTF.setInputVerifier(new MileageVerifier());
    	billRateTF.setInputVerifier(new BillRateVerifier());

		setupDialog();
	}
//===============================================

// edit constructor
	public TripDialog(TripManager tripManager, int selectedIndex, TripRecord tripToEdit)
	{
		this.tripManager = tripManager;
		buildBasicGUI();

		this.selectedIndex = selectedIndex;

	// populating fields
		DateFormat df = new SimpleDateFormat("MM-dd-yyyy");

		dateOfTripTF.setText(df.format(tripToEdit.dateOfTrip));
		patientNameTF.setText(tripToEdit.patientName);
		servCodeList.setSelectedItem(tripToEdit.servCode);
		initialMileTF.setText(new Integer(tripToEdit.initialMile).toString());
		retMileTF.setText(new Integer(tripToEdit.retMile).toString());
		billRateTF.setText(new Float(tripToEdit.billRate).toString());
		commentTF.setText(tripToEdit.comment);

		popDateOfTrip = dateOfTripTF.getText();
		popPatientName = patientNameTF.getText();
		popInitialMile = initialMileTF.getText();
		popRetMile = retMileTF.getText();
		popBillRate = billRateTF.getText();
		popComment = commentTF.getText();

		buttonPanel = new JPanel(new GridLayout(1,2));

	// submit button
		submitButton = new JButton("Submit");
		submitButton.setActionCommand("SAVE_EDITED_TRIP");
		submitButton.setToolTipText("Save edited trip");
		submitButton.addActionListener(this);
    	buttonPanel.add(submitButton);

	// cancel button
		cancelButton = new JButton("Cancel");
		cancelButton.setActionCommand("EDIT_CANCEL");
		cancelButton.setToolTipText("Cancel");
		cancelButton.addActionListener(this);
		cancelButton.setVerifyInputWhenFocusTarget(false);
    	buttonPanel.add(cancelButton);

    	add(buttonPanel, BorderLayout.SOUTH);

    // validation
    	dateOfTripTF.setInputVerifier(new DateVerifier());
    	initialMileTF.setInputVerifier(new MileageVerifier());
    	retMileTF.setInputVerifier(new MileageVerifier());
    	billRateTF.setInputVerifier(new BillRateVerifier());

		setupDialog();
	}
//===============================================
	void setupDialog()
	{
		Toolkit tk;
		Dimension d;

		tk = Toolkit.getDefaultToolkit();
		d = tk.getScreenSize();
		setSize(d.width/4, d.height/4);
		setLocation(d.width/4, d.height/4);

		setDefaultCloseOperation(JDialog.DISPOSE_ON_CLOSE);

		setTitle("Trip Dialog");

		setModalityType(Dialog.ModalityType.APPLICATION_MODAL);
		setVisible(true);
	} // end of setupDialog()
//===============================================
	void buildBasicGUI()
	{
		JLabel dateLabel;
		JLabel nameLabel;
		JLabel codeLabel;
		JLabel inMileLabel;
		JLabel retMileLabel;
		JLabel billRateLabel;
		JLabel comLabel;

		dateLabel = new JLabel("Date (MM-dd-yyyy)");
		nameLabel = new JLabel("Patient Name");
		codeLabel = new JLabel("Service Code");
		inMileLabel = new JLabel("Initial Miles");
		retMileLabel = new JLabel("Return Miles");
		billRateLabel = new JLabel("Bill Rate");
		comLabel = new JLabel("Comment");

		dateOfTripTF = new JTextField("");
		patientNameTF = new JTextField("");
		initialMileTF = new JTextField("");
		retMileTF = new JTextField("");
		billRateTF = new JTextField("");
		commentTF = new JTextField("");

		String[] array = new String[]{"A0428 Non-emergency transport",
										"A0429 Emergency transport",
										"A0427 Advanced life support",
										"A0434 Specialty care transport"};

		servCodeList = new JComboBox(array);

		JPanel panel = new JPanel();
		GroupLayout layout = new GroupLayout(panel);
		panel.setLayout(layout);

		layout.setAutoCreateGaps(true);

		layout.setAutoCreateContainerGaps(true);

		GroupLayout.SequentialGroup hGroup = layout.createSequentialGroup();

		hGroup.addGroup(layout.createParallelGroup().addComponent(dateLabel).
				addComponent(nameLabel).addComponent(codeLabel).
				addComponent(inMileLabel).addComponent(retMileLabel).
				addComponent(billRateLabel).addComponent(comLabel));

		hGroup.addGroup(layout.createParallelGroup().addComponent(dateOfTripTF).
				addComponent(patientNameTF).addComponent(servCodeList).
				addComponent(initialMileTF).addComponent(retMileTF).addComponent(billRateTF).
				addComponent(commentTF));

		layout.setHorizontalGroup(hGroup);

		GroupLayout.SequentialGroup vGroup = layout.createSequentialGroup();

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(dateLabel).addComponent(dateOfTripTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(nameLabel).addComponent(patientNameTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(codeLabel).addComponent(servCodeList));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(inMileLabel).addComponent(initialMileTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(retMileLabel).addComponent(retMileTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(billRateLabel).addComponent(billRateTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(comLabel).addComponent(commentTF));

		layout.setVerticalGroup(vGroup);

		add(panel, BorderLayout.CENTER);

		//setupDialog();
	}
//===============================================
	public void actionPerformed(ActionEvent e)
	{
		if(e.getActionCommand().equals("ADD_STAY_OPEN"))
			doAddOpen();
		else if(e.getActionCommand().equals("ADD_CLOSE"))
			doAddClose();
		else if(e.getActionCommand().equals("ADD_CANCEL"))
			doAddCancel();
		else if(e.getActionCommand().equals("SAVE_EDITED_TRIP"))
			doSubmit();
		else // if(e.getActionCommand().equals("EDIT_CANCEL"))
			doEditCancel();
	} // end of actionPerformed(...)
//===============================================
	public boolean verify(JTextField initialMileTF, JTextField retMileTF)
	{
		try
		{
			int initialMile = new Integer(Integer.parseInt(initialMileTF.getText().trim()));
			int retMile = new Integer(Integer.parseInt(retMileTF.getText().trim()));

			if(retMile >= initialMile)
				return true;
			else
			{
				JOptionPane.showMessageDialog(this,"Return mileage is less than initial mileage");
				return false;
			}
		}
		catch(NumberFormatException nfe)
		{
			System.out.println("NumberFormatException in verify");
			JOptionPane.showMessageDialog(this,"Try an actual number like (2)");
			return false;
		}
	}
//===============================================
	void doAddOpen()
	{
		if(hasRequiredData(dateOfTripTF, patientNameTF, initialMileTF,
				retMileTF, billRateTF) == true)
		{
			if(verify(initialMileTF, retMileTF) == true)
			{
				try
				{
					TripRecord tr = new TripRecord();

					DateFormat df = new SimpleDateFormat("MM-dd-yyyy");

					tr.dateOfTrip = df.parse(dateOfTripTF.getText().trim());
					tr.patientName = patientNameTF.getText().trim();
					tr.servCode = servCodeList.getSelectedItem().toString();
					tr.initialMile = new Integer(Integer.parseInt(initialMileTF.getText().trim()));
					tr.retMile = new Integer(Integer.parseInt(retMileTF.getText().trim()));
					tr.billRate = new Float(Float.parseFloat(billRateTF.getText().trim()));
					tr.comment = commentTF.getText().trim();

					myFrame.trTableModel.fireTableDataChanged();
					tripManager.addTrip(tr);

					dateOfTripTF.setText("");
					patientNameTF.setText("");
					initialMileTF.setText("");
					retMileTF.setText("");
					billRateTF.setText("");
					commentTF.setText("");

					dateOfTripTF.requestFocus();
				}
				catch(ParseException pe)
				{
					System.out.println("ParseException in doAddClose");
				}
			}
		}
		else // if(hasRequiredData() == false)
			JOptionPane.showMessageDialog(this,"Fill in all the required data");
	}
//===============================================
	void doAddClose()
	{
		if(hasRequiredData(dateOfTripTF, patientNameTF, initialMileTF,
				retMileTF, billRateTF) == true)
		{
			if(verify(initialMileTF, retMileTF) == true)
			{
				try
				{
					TripRecord tr = new TripRecord();

					DateFormat df = new SimpleDateFormat("MM-dd-yyyy");

					tr.dateOfTrip = df.parse(dateOfTripTF.getText().trim());
					tr.patientName = patientNameTF.getText().trim();
					tr.servCode = servCodeList.getSelectedItem().toString();
					tr.initialMile = new Integer(Integer.parseInt(initialMileTF.getText().trim()));
					tr.retMile = new Integer(Integer.parseInt(retMileTF.getText().trim()));
					tr.billRate = new Float(Float.parseFloat(billRateTF.getText().trim()));
					tr.comment = commentTF.getText().trim();

					tripManager.addTrip(tr);

					dispose();
				}
				catch(ParseException pe)
				{
					System.out.println("ParseException in doAddClose");
				}
			}
		}
		else // if(hasRequiredData() == false)
			JOptionPane.showMessageDialog(this,"Fill in all the required data");
	}
//===============================================
	boolean hasRequiredData(JTextField dateOfTripTF, JTextField patientNameTF,
							JTextField initialMileTF, JTextField retMileTF,
							JTextField billRateTF)
	{
		if(!dateOfTripTF.getText().trim().equals(""))
		{
			if(!patientNameTF.getText().trim().equals(""))
			{
				if(!initialMileTF.getText().trim().equals(""))
				{
					if(!retMileTF.getText().trim().equals(""))
					{
						if(!billRateTF.getText().trim().equals(""))
							return true;
					}
				}
			}
		}
		return false;
	}
//===============================================
	void doAddCancel()
	{
		if(unsavedAddChanges(dateOfTripTF, patientNameTF, initialMileTF,
				retMileTF, billRateTF, commentTF) == false)
			dispose();
		else
		{
			int response = JOptionPane.showConfirmDialog(this, "Unsaved changes will be lost! Do you want to still cancel?");
			if(response == JOptionPane.YES_OPTION)
				dispose();
		}
	}
//===============================================
	void doEditCancel()
	{
		if(unsavedEditChanges(popDateOfTrip, popPatientName, popInitialMile,
				popRetMile, popBillRate, popComment, dateOfTripTF,
				patientNameTF, initialMileTF, retMileTF, billRateTF,
				commentTF) == false)
			dispose();
		else
		{
			int response = JOptionPane.showConfirmDialog(this, "Unsaved changes will be lost");
			if(response == JOptionPane.YES_OPTION)
				dispose();
		}
	}
//===============================================
	boolean unsavedAddChanges(JTextField dateOfTripTF, JTextField patientNameTF,
							JTextField initialMileTF, JTextField retMileTF,
							JTextField billRateTF, JTextField commentTF)
	{
		if(dateOfTripTF.getText().trim().equals(""))
		{
			if(patientNameTF.getText().trim().equals(""))
			{
				if(initialMileTF.getText().trim().equals(""))
				{
					if(retMileTF.getText().trim().equals(""))
					{
						if(billRateTF.getText().trim().equals(""))
						{
							if(commentTF.getText().trim().equals(""))
								return false;
						}
					}
				}
			}
		}
		return true;
	}
//===============================================
	boolean unsavedEditChanges(String popDateOfTrip, String popPatientName, String popInitialMile,
							String popRetMile, String popBillRate, String popComment,
							JTextField dateOfTripTF, JTextField patientNameTF,
							JTextField initialMileTF, JTextField retMileTF,
							JTextField billRateTF, JTextField commentTF)
	{
		if(popDateOfTrip.equals(dateOfTripTF.getText().trim()))
		{
			if(popPatientName.equals(patientNameTF.getText().trim()))
			{
				if(popInitialMile.equals(initialMileTF.getText().trim()))
				{
					if(popRetMile.equals(retMileTF.getText().trim()))
					{
						if(popBillRate.equals(billRateTF.getText().trim()))
						{
							if(popComment.equals(commentTF.getText().trim()))
								return false;
						}
					}
				}
			}
		}
		return true;
	}
//===============================================
	void doSubmit()
	{
		if(hasRequiredData(dateOfTripTF, patientNameTF, initialMileTF,
				retMileTF, billRateTF) == true)
		{
			if(verify(initialMileTF, retMileTF) == true)
			{
				try
				{
					TripRecord tr = new TripRecord();

					DateFormat df = new SimpleDateFormat("MM-dd-yyyy");

					tr.dateOfTrip = df.parse(dateOfTripTF.getText().trim());
					tr.patientName = patientNameTF.getText().trim();
					tr.servCode = servCodeList.getSelectedItem().toString();
					tr.initialMile = new Integer(Integer.parseInt(initialMileTF.getText().trim()));
					tr.retMile = new Integer(Integer.parseInt(retMileTF.getText().trim()));
					tr.billRate = new Float(Float.parseFloat(billRateTF.getText().trim()));
					tr.comment = commentTF.getText().trim();

					tripManager.replaceTrip(selectedIndex, tr);

					dispose();
				}
				catch(ParseException pe)
				{
					System.out.println("ParseException in doAddClose");
				}
			}
		}
		else // if(hasRequiredData() == false)
			JOptionPane.showMessageDialog(this,"Fill in all the required data");
	}

} // end of class TripDialog