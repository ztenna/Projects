// Zachary Tennant

import java.io.*;
import javax.swing.*;
import java.net.*;

class CTS extends Thread
{
	Talker talker;
	Chat client;
	String domain;
	int port;
	String id;
	File file;

	public CTS(Chat client)
	{
		this.client = client;
		domain = "127.0.0.1";
		port = 1234;
		id = "Client";

		try
		{
			talker = new Talker(domain, port, id);
			new LoginRegister(this);
			new Thread(this).start();
		}
		catch(IOException ioe)
		{
			SwingUtilities.invokeLater(new Runnable()
			{
				public void run()
				{
					JOptionPane.showMessageDialog(null, "Can't connect to server.");
				}
			}
			);
			client.sendRequestButton.setEnabled(false);
			client.logoutButton.setEnabled(false);
			client.connectButton.setEnabled(true);
		}
	} // end constructor

//===============================================

	public void run()
	{
		String msg;
		String[] parts;

		String requester;
		String newBuddyName;
		Buddy buddy;
		ChatBox chatBox;
		String sender;

		Socket socket;
		OutputStream os;
		FileInputStream fis;
		byte[] buff;
		int numBytesRead;
		int totNumBytesRead;
		String fileName;
		int fileLength;

		try
		{
			while(true)
			{
				msg = talker.receive();
// Register exists
				if(msg.equals("ALREADY_EXISTS"))
				{
					final CTS cts = this;
					SwingUtilities.invokeLater(new Runnable()
					{
						public void run()
						{
							JOptionPane.showMessageDialog(null, "Username already exists.");
						}
					}
					);
					client.sendRequestButton.setEnabled(false);
					client.logoutButton.setEnabled(false);
					client.connectButton.setEnabled(true);
					return;
				}
// Added new register
				else if(msg.startsWith("ADDED_NEW_USER"))
				{
					talker.setID(msg.substring(14));
					client.setTitle(talker.id);
					client.list.clear();
				}
// Logged on
				else if(msg.startsWith("LOGGED_ON"))
				{
					parts = msg.split(" ");
					talker.setID(parts[1]);
					client.setTitle(talker.id);
					if(!client.previousUsername.equals(talker.id))
					{
						for(int n = 2; n < parts.length; n++)
						{
							chatBox = null;
							buddy = new Buddy(parts[n], "offline", chatBox);
							client.list.addElement(buddy);
						}
					}
					talker.send("UPDATE_STATUS");
				}
// Invalid login
				else if(msg.equals("INVALID_LOGIN"))
				{
					final CTS cts = this;
					SwingUtilities.invokeLater(new Runnable()
					{
						public void run()
						{
							JOptionPane.showMessageDialog(null, "Username/password combo incorrect.");
						}
					}
					);
					client.sendRequestButton.setEnabled(false);
					client.logoutButton.setEnabled(false);
					client.connectButton.setEnabled(true);
					return;

				}
// Set buddy status
				else if(msg.startsWith("STATUS"))
				{
					parts = msg.split(" ");
					for(int n = 0; n < client.list.size(); n++)
					{
						buddy = client.list.get(parts[1]);
						buddy.status = parts[2];
					}
					SwingUtilities.invokeLater(new Runnable()
					{
						public void run()
						{
							client.listBox.repaint();
						}
					}
					);
				}
// Update status
				else if(msg.startsWith("UPDATE_STATUS"))
				{
					parts = msg.split(" ");
					buddy = client.list.get(parts[1]);
					if(buddy != null)
					{
						buddy.status = parts[2];
						SwingUtilities.invokeLater(new Runnable()
						{
							public void run()
							{
								client.listBox.repaint();
							}
						}
						);
					}
					else
						System.out.println("Buddy does not exist");
				}
// Accept/Deny request
				else if(msg.startsWith("BUD_REQ"))
				{
					final String tempMsg = msg;
					SwingUtilities.invokeLater(new Runnable()
					{
						int response;
						String[] parts = tempMsg.split(" ");
						String requester = parts[1];
						Buddy buddy;
						ChatBox chatBox;
						public void run()
						{
							String potentialBuddy = parts[2];
							response = JOptionPane.showConfirmDialog(null, "Would you like to be buddies with " + requester + "?", talker.id, JOptionPane.YES_NO_OPTION);
							if(response == JOptionPane.YES_OPTION)
							{
								try
								{
									chatBox = null;
									buddy = new Buddy(requester, "offline", chatBox);
									client.list.addElement(buddy);
									talker.send("ADDED_BUD" + " " + requester + " " + potentialBuddy);
									talker.send("UPDATE_STATUS");
								}
								catch(IOException ioe)
								{
									JOptionPane.showMessageDialog(null, "Can't send message to server.");
								}
							}
						}
					}
					);
				}
// Not a user
				else if(msg.startsWith("NOT_A_USER"))
				{
					final String tempMsg = msg;
					SwingUtilities.invokeLater(new Runnable()
					{
						String potentialBuddy = tempMsg.substring(10);
						public void run()
						{
							JOptionPane.showMessageDialog(null, potentialBuddy + " is not a user.");
						}
					}
					);
				}
// Add accepted buddy
				else if(msg.startsWith("BUD_SAID_YES"))
				{
					newBuddyName = msg.substring(12);

					chatBox = null;
					buddy = new Buddy(newBuddyName, "offline", chatBox);
					client.list.addElement(buddy);
					talker.send("UPDATE_STATUS");
				}
// Incoming message
				else if(msg.startsWith("INCOME_MSG"))
				{
					parts = msg.split("`");
					sender = parts[1];
					System.out.println("sender: " + sender);
					buddy = client.list.get(sender);
					System.out.println("Incoming message");
					if(buddy == null)
						System.out.println("buddy is null");

					if(buddy.chatBox == null)
					{
						final CTS cts = this;
						final Buddy tempBuddy = buddy;
						final String tempMsg = parts[2];
						final String tempSender = sender;
						SwingUtilities.invokeLater(new Runnable()
						{
							public void run()
							{
								tempBuddy.chatBox = new ChatBox(cts, tempSender, cts.talker.id);
								tempBuddy.chatBox.requestFocus();
								tempBuddy.chatBox.addMsg(tempMsg, tempSender);
							}
						}
						);
					}
					else
					{
						buddy.chatBox.requestFocus();
						buddy.chatBox.addMsg(parts[2], sender);
					}
				}
// Want this file
				else if(msg.startsWith("WANT_THIS_FILE"))
				{
					final String tempMsg = msg;
					SwingUtilities.invokeLater(new Runnable()
					{
						String[] parts = tempMsg.split(":");
						int response;
						FileReceiver fileReceiver;
						int fileLength = Integer.parseInt(parts[1]);
						String fileName = parts[2];
						String tempSender = parts[3];
						int port = 4321;
						public void run()
						{
							response = JOptionPane.showConfirmDialog(null, "Would you like to get this file: " + fileName +
										", which is " + fileLength + " long, from " + tempSender + "?", talker.id, JOptionPane.YES_NO_OPTION);
							if(response == JOptionPane.YES_OPTION)
							{
								try
								{
									fileReceiver = new FileReceiver(fileLength, fileName);
									talker.send("ACCEPT_FILE" + ":" + fileName + ":" + port + ":" + "127.0.0.1" + ":" + tempSender + ":" + fileLength);
								}
								catch(IOException ioe)
								{
									JOptionPane.showMessageDialog(null, "Can't accept file");
								}
							}
							else
								JOptionPane.showMessageDialog(null, "Said no to file");
						}
					}
					);
				}
// Forward file
				else if(msg.startsWith("FORWARD_FILE"))
				{
					try
					{
						parts = msg.split(":");
						port = Integer.parseInt(parts[2]);
						domain = parts[3];
						fileName = parts[1];
						fileLength = Integer.parseInt(parts[4]);

						socket = new Socket(domain, port);
						os = socket.getOutputStream();
						fis = new FileInputStream(file);

						totNumBytesRead = 0;
						while(totNumBytesRead != fileLength)
						{
							buff = new byte[256];
							numBytesRead = fis.read(buff);
							totNumBytesRead = totNumBytesRead + numBytesRead;
							os.write(buff, 0, numBytesRead);
						}
					}
					catch(IOException ioe)
					{
						JOptionPane.showMessageDialog(null, "Can't read/write file on sender");
					}
				}
			} // end while
		} // end try
		catch(IOException ioe)
		{
			SwingUtilities.invokeLater(new Runnable()
			{
				public void run()
				{
					JOptionPane.showMessageDialog(null, "Server is not working");
				}
			}
			);
			client.cts = null;
			client.sendRequestButton.setEnabled(false);
			client.logoutButton.setEnabled(false);
			client.connectButton.setEnabled(true);
		}
	} // end run
	void setFile(File file)
	{
		this.file = file;
	}
} // end class CTS