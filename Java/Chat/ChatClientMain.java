// Zachary Tennant

import java.awt.*;
import javax.swing.*;
import java.awt.event.*;
import java.io.*;

// setTitle(cts.dialog.username)
class ChatClientMain
{
	public static void main(String[] x)
	{
		new Chat();
	} // end main
} // end ChatClientMain

//###############################################

class Chat extends JFrame
				implements ActionListener, MouseListener, WindowListener
{
	JLabel buddiesLabel;
	JPanel listPanel;
	BuddyList list;
	JList listBox;
	JScrollPane scroller;

	JPanel buttonPanel;
	JButton connectButton;
	JButton sendRequestButton;
	JButton logoutButton;

	CTS cts;
	Buddy chatBuddy;
	String previousUsername;

	Container cp;

//===============================================

	Chat()
	{
		listPanel = new JPanel();

		buddiesLabel = new JLabel("Buddies List");
		listPanel.add(buddiesLabel);

		list = new BuddyList();
		listBox = new JList(list);
		listBox.addMouseListener(this);
		scroller = new JScrollPane(listBox);
		listPanel.add(scroller);

		buttonPanel = new JPanel();

		connectButton = new JButton("Connect");
		connectButton.setActionCommand("CONNECT");
		connectButton.setToolTipText("Login or register");
		connectButton.addActionListener(this);
		buttonPanel.add(connectButton);

		sendRequestButton = new JButton("Send Request");
		sendRequestButton.setActionCommand("SEND_REQ");
		sendRequestButton.setToolTipText("Send buddy request to potential new buddy");
		sendRequestButton.addActionListener(this);
		sendRequestButton.setEnabled(false);
		buttonPanel.add(sendRequestButton);

		logoutButton = new JButton("Logout");
		logoutButton.setActionCommand("LOGOUT");
		logoutButton.setToolTipText("Logout user account");
		logoutButton.addActionListener(this);
		logoutButton.setEnabled(false);
		buttonPanel.add(logoutButton);

		previousUsername = "";

		cp = getContentPane();
		cp.add(listPanel, BorderLayout.CENTER);
		cp.add(buttonPanel, BorderLayout.SOUTH);

		setupMainFrame();
	} // end constructor

//===============================================

	void setupMainFrame()
	{
		Toolkit tk;
		Dimension d;

		tk = Toolkit.getDefaultToolkit();
		d = tk.getScreenSize();
		setSize(d.width/4, d.height/4);
		setLocation(d.width/4, d.height/4);

		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		setVisible(true);

	} // end setupMainFrame

//###############################################

	public void actionPerformed(ActionEvent e)
	{
		String cmd = e.getActionCommand();
		String potentialBuddy;

		if(cmd.equals("CONNECT"))
		{
			cts = new CTS(this);
			sendRequestButton.setEnabled(true);
			logoutButton.setEnabled(true);
			connectButton.setEnabled(false);
		}
		else if(cmd.equals("SEND_REQ"))
		{
			try
			{
				potentialBuddy = JOptionPane.showInputDialog(null, "Who's your buddy?");
				if(potentialBuddy != null)
				{
					if(!potentialBuddy.equals(""))
					{
						if(!potentialBuddy.contains(" "))
						{
							if(!potentialBuddy.equals(cts.talker.id))
							{
								chatBuddy = list.get(potentialBuddy);
								if(chatBuddy == null)
									cts.talker.send(cmd + " " + potentialBuddy + " " + cts.talker.id);
								else
									JOptionPane.showMessageDialog(null, "Already buddies with " + potentialBuddy + ".");
							}
							else
								JOptionPane.showMessageDialog(null, "Can't be buddies with yourself.");
						}
						else
							JOptionPane.showMessageDialog(null, "Buddies don't have spaces.");
					}
					else
						JOptionPane.showMessageDialog(null, "You didn't enter a buddy.");
				}
			} // end try
			catch(IOException ioe)
			{
				JOptionPane.showMessageDialog(null, "Unable to send buddy request.");
			}
		}
		else
		{
			try
			{
				cts.talker.send("LOGGED_OUT");
				previousUsername = cts.talker.id;
				cts = null;
				sendRequestButton.setEnabled(false);
				logoutButton.setEnabled(false);
				connectButton.setEnabled(true);
			}
			catch(IOException ioe)
			{
				JOptionPane.showMessageDialog(null, "Unable to send logged out message.");
			}
		}
	} // end actionPerformed

//###############################################

	public void mouseClicked(MouseEvent e)
	{
		int selectedIndex;
		String buddyUsername;

		if(e.getClickCount() == 2)
		{
			selectedIndex = listBox.locationToIndex(e.getPoint());

			if(selectedIndex > -1)
			{
				chatBuddy = list.elementAt(selectedIndex);
				if(chatBuddy.chatBox == null)
				{
					buddyUsername = chatBuddy.username;
					chatBuddy.chatBox = new ChatBox(cts, buddyUsername, cts.talker.id);
					chatBuddy.chatBox.addWindowListener(this);
				}
				else
					chatBuddy.chatBox.toFront();
			}
		}
	}

//===============================================

	public void mouseEntered(MouseEvent e)
	{
	}

//===============================================

	public void mouseExited(MouseEvent e)
	{
	}

//===============================================

	public void mousePressed(MouseEvent e)
	{
	}

//===============================================

	public void mouseReleased(MouseEvent e)
	{
	}

//###############################################

	public void windowActivated(WindowEvent e)
	{
	}

//===============================================

	public void windowClosed(WindowEvent e)
	{
		chatBuddy.chatBox = null;
	}

//===============================================

	public void windowClosing(WindowEvent e)
	{
		chatBuddy.chatBox = null;
	}

//===============================================

	public void windowDeactivated(WindowEvent e)
	{
	}

//===============================================

	public void windowDeiconified(WindowEvent e)
	{
	}

//===============================================

	public void windowIconified(WindowEvent e)
	{
	}

//===============================================

	public void windowOpened(WindowEvent e)
	{
	}
} // end class Chat