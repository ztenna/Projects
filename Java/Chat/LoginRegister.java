// Zachary Tennant

import javax.swing.*;
import java.awt.*;
import java.awt.event.*;
import java.io.*;

class LoginRegister extends JDialog
						implements ActionListener
{
	JTextField usernameTF;
	JPasswordField passwordPF;

	JButton registerButton;
	JButton loginButton;

	JPanel buttonPanel;
	JPanel credentialPanel;
	JPanel usernamePanel;
	JPanel passwordPanel;

	JLabel usernameLabel;
	JLabel passwordLabel;

	CTS cts;

//===============================================

	public LoginRegister(CTS cts)
	{
		this.cts = cts;

		credentialPanel = new JPanel(new GridLayout(2,1));
		usernamePanel = new JPanel();
		passwordPanel = new JPanel();
		buttonPanel = new JPanel(new GridLayout(1,2));

		usernameLabel = new JLabel("Username");
		passwordLabel = new JLabel("Password");

		usernameTF = new JTextField(25);
		passwordPF = new JPasswordField(25);

		registerButton = new JButton("Register");
		registerButton.setActionCommand("REGISTER");
		registerButton.setToolTipText("Register a new user");
		registerButton.addActionListener(this);

		loginButton = new JButton("Login");
		loginButton.setActionCommand("LOGIN");
		loginButton.setToolTipText("Login as a returning user");
		loginButton.addActionListener(this);

		usernamePanel.add(usernameLabel);
		usernamePanel.add(usernameTF);

		passwordPanel.add(passwordLabel);
		passwordPanel.add(passwordPF);

		credentialPanel.add(usernamePanel);
		credentialPanel.add(passwordPanel);

		buttonPanel.add(registerButton);
		buttonPanel.add(loginButton);

		add(credentialPanel, BorderLayout.CENTER);
		add(buttonPanel, BorderLayout.SOUTH);

		setupDialog();
	} // end constructor

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

		setTitle("Register/Login");

		setModalityType(Dialog.ModalityType.APPLICATION_MODAL);
		setVisible(true);
	} // end setupDialog

//===============================================

	public void actionPerformed(ActionEvent e)
	{
		String cmd = e.getActionCommand();
		String username = usernameTF.getText().trim();
		String password = new String(passwordPF.getPassword()).trim();

		if(cmd.equals("REGISTER"))
		{
			if(!username.equals("") && !password.equals(""))
			{
				if(!username.contains(" ") && !password.contains(" "))
				{
					try
					{
						cts.talker.send(cmd + " " + username + " " + password);
						dispose();
					}
					catch(IOException ioe)
					{
						JOptionPane.showMessageDialog(null, "Can't send username/password to server.");
					}
				}
				else
					JOptionPane.showMessageDialog(null, "Username and password can't have spaces in them");
			}
			else
				JOptionPane.showMessageDialog(null, "Username or password are blank");
		}

		else if(cmd.equals("LOGIN"))
		{
			if(!username.equals("") && !password.equals(""))
			{
				if(!username.contains(" ") && !password.contains(" "))
				{
					try
					{
						cts.talker.send(cmd + " " + username + " " + password);
						dispose();
					}
					catch(IOException ioe)
					{
						JOptionPane.showMessageDialog(null, "Can't send username/password to server.");
					}
				}
				else
					JOptionPane.showMessageDialog(null, "Username and password can't have spaces in them");
			}
			else
				JOptionPane.showMessageDialog(null, "Username or password are blank");
		}
	} // end actionPerformed
} // end class LoginRegister