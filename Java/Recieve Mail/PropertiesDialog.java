// Zachary Tennant

import javax.swing.*;
import java.awt.event.*;
import java.awt.*;
import java.util.*;

class PropertiesDialog extends JDialog
							implements ActionListener
{
//	text fields
	JTextField serverNameTF;
	JTextField usernameTF;
	//JTextField passwordTF;
	JPasswordField passwordTF;
	JTextField timeBetCheckTF;

// labels
	JLabel serverNameLabel;
	JLabel usernameLabel;
	JLabel passwordLabel;
	JLabel timeLabel;

// panels
	JPanel serverNamePanel;
	JPanel usernamePanel;
	JPanel passwordPanel;
	JPanel timePanel;
	JPanel textFieldPanel;
	JPanel buttonPanel;
	Container cp;

// buttons
	JButton saveButton;
	JButton cancelButton;

// check box
	JCheckBox soundDiaCheckBox;

	Properties props;
	Boolean isSaved;

//===============================================

	public PropertiesDialog(Properties props)
	{
// server name
		serverNamePanel = new JPanel(new GridLayout(1,2));
		serverNameLabel = new JLabel("IMAP Server Name");
		serverNameTF = new JTextField("");

		serverNamePanel.add(serverNameLabel);
		serverNamePanel.add(serverNameTF);

		serverNameTF.requestFocus();

// username
		usernamePanel = new JPanel(new GridLayout(1,2));
		usernameLabel = new JLabel("Username");
		usernameTF = new JTextField("");

		usernamePanel.add(usernameLabel);
		usernamePanel.add(usernameTF);

// password
		passwordPanel = new JPanel(new GridLayout(1,2));
		passwordLabel = new JLabel("Password");
		//passwordTF = new JTextField("");
		passwordTF = new JPasswordField("");

		passwordPanel.add(passwordLabel);
		passwordPanel.add(passwordTF);

// time between checks
		timePanel = new JPanel(new GridLayout(1,2));
		timeLabel = new JLabel("Seconds Between New Email Checks");
		timeBetCheckTF = new JTextField("");
		timeBetCheckTF.setInputVerifier(new TimeVerifier());

		timePanel.add(timeLabel);
		timePanel.add(timeBetCheckTF);

// text field panel
		textFieldPanel = new JPanel(new GridLayout(4,1));
		textFieldPanel.add(serverNamePanel);
		textFieldPanel.add(usernamePanel);
		textFieldPanel.add(passwordPanel);
		textFieldPanel.add(timePanel);

// sound checkbox
		soundDiaCheckBox = new JCheckBox("Sound");
		soundDiaCheckBox.setToolTipText("Select if want sound with new mail");

// save button
		saveButton = new JButton("Save");
		saveButton.setActionCommand("SAVE");
		saveButton.setToolTipText("Press to save properties");
		saveButton.addActionListener(this);
		getRootPane().setDefaultButton(saveButton);

// cancel button
		cancelButton = new JButton("Cancel");
		cancelButton.setActionCommand("CANCEL");
		cancelButton.setToolTipText("Press to cancel property changes");
		cancelButton.addActionListener(this);

// button panel
		buttonPanel = new JPanel(new GridLayout(1,3));
		buttonPanel.add(soundDiaCheckBox);
		buttonPanel.add(saveButton);
		buttonPanel.add(cancelButton);

		this.props = props;
		isSaved = false;
		if(props.isEmpty() != true)
		{
			System.out.println("not empty");
			serverNameTF.setText(props.getProperty("SERVERNAME"));
			usernameTF.setText(props.getProperty("USERNAME"));
			passwordTF.setText(props.getProperty("PASSWORD"));
			timeBetCheckTF.setText(props.getProperty("TIMEBETCHECK"));
			if(props.getProperty("SOUND").equals("ON"))
				soundDiaCheckBox.setSelected(true);
			else
				soundDiaCheckBox.setSelected(false);
		}

// content pane
		cp = getContentPane();
		cp.add(textFieldPanel, BorderLayout.CENTER);
		cp.add(buttonPanel, BorderLayout.SOUTH);

		setupDialog();
	} // end of constructor

//###############################################

	void setupDialog()
	{
		Toolkit tk;
		Dimension d;

		tk = Toolkit.getDefaultToolkit();
		d = tk.getScreenSize();
		setSize(d.width/4, d.height/4);
		setLocation(d.width/4, d.height/4);

		setDefaultCloseOperation(JDialog.DISPOSE_ON_CLOSE);

		setTitle("Properties Dialog");

		setModalityType(Dialog.ModalityType.APPLICATION_MODAL);
		setVisible(true);
	} // end of setupDialog()

//###############################################

	public void actionPerformed(ActionEvent e)
	{
		if(e.getActionCommand().equals("SAVE"))
			doSave();
		else if(e.getActionCommand().equals("CANCEL"))
			dispose();
	} // end of actionPerformed()

//===============================================

	void doSave()
	{
		if(hasRequiredData(serverNameTF, usernameTF, passwordTF, timeBetCheckTF) == true)
		{
			//check for connection

			props.setProperty("SERVERNAME", serverNameTF.getText().trim());
			props.setProperty("USERNAME", usernameTF.getText().trim());
			props.setProperty("PASSWORD", passwordTF.getText().trim());
			props.setProperty("TIMEBETCHECK", timeBetCheckTF.getText().trim());
			if(soundDiaCheckBox.isSelected() == true)
				props.setProperty("SOUND", "ON");
			else
				props.setProperty("SOUND", "OFF");

			isSaved = true;

			dispose();
		}
	} // end of doSave

//===============================================

	boolean hasRequiredData(JTextField serverNameTF, JTextField usernameTF,
								JTextField passwordTF, JTextField timeBetCheckTF)
	{
		if(!serverNameTF.getText().trim().equals(""))
		{
			if(!usernameTF.getText().trim().equals(""))
			{
				if(!passwordTF.getText().trim().equals(""))
				{
					if(!timeBetCheckTF.getText().trim().equals(""))
					{
						return true;
					}
				}
			}
		}
		return false;
	} // end of hasRequiredData
} // end of dialog