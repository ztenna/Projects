// Zachary Tennant

import java.io.*;
import java.net.*;
import javax.swing.*;

class CTC extends Thread
{
	Talker talker;
	Acceptor server;
	User user;
	String currentUsername;
	int length;
	String[] parts;
	String potentialBuddy;
	String requester;
	String buddyString = "";
	User buddyUser;
	String actBuddy;

	public CTC(Socket socket, Acceptor server)
	{
		try
		{
			this.server = server;
			talker = new Talker(socket);
			new Thread(this).start();
		}
		catch(IOException ioe)
		{
			System.out.println("Trouble constructing server talker");
		}
	}

//===============================================

	public void run()
	{
		String cmd;

		try
		{
			cmd = talker.receive();

			// Try to register
			if(cmd.startsWith("REGISTER"))
			{
				parts = cmd.split(" ");

				// Check to see if user already exists
				if(server.userList.containsKey(parts[1]))
					talker.send("ALREADY_EXISTS");

				// Create new user
				else
				{
					user = new User(parts[1], parts[2], this);
					server.userList.put(user.username, user);
					server.userList.store();
					talker.send("ADDED_NEW_USER" + user.username);
				}
			}

			// Try to login
			else if(cmd.startsWith("LOGIN"))
			{
				parts = cmd.split(" ");

				// Verify is a user
				if(server.userList.containsKey(parts[1]))
				{
					user = server.userList.get(parts[1]);

					// Not logged in more than once
					if(user.ctc == null)
					{
						// Check password
						if(parts[2].equals(user.password))
						{
							// Get the users "buddies"/friends list
							if(user.listOfBuddies.size() > 0)
							{
								buddyString = user.listOfBuddies.elementAt(0);
								if(user.listOfBuddies.size() > 1)
								{
									for(int n = 1; n < user.listOfBuddies.size(); n++)
										buddyString = buddyString + " " + user.listOfBuddies.elementAt(n);
								}
							}

							// Send response to client that user is logged in and establish connection between server and client
							talker.send("LOGGED_ON" + " " + user.username + " " + buddyString);
							user.ctc = this;

							// Send offline commands
							for(int n = 0; n < this.user.offlineCommands.size(); n++)
							{
								handleCommands(this.user.offlineCommands.elementAt(n));
							}
							this.user.offlineCommands.clear();
							server.userList.store();

							// Set buddy status
							for(int n = 0; n < this.user.listOfBuddies.size(); n++)
							{
								actBuddy = this.user.listOfBuddies.elementAt(n);
								buddyUser = server.userList.get(actBuddy);
								if(buddyUser.ctc == null)
									talker.send("STATUS" + " " + buddyUser.username + " " + "offline");
								else
								{
									System.out.println("Buddy ctc != null");
									talker.send("STATUS" + " " + buddyUser.username + " " + "online");
								}
							}
						}

						// Passwords don't match so alert the client
						else
						{
							System.out.println("Passwords don't match");
							talker.send("INVALID_LOGIN");
						}
					}

					// A server client connection alread exists so alert the client
					else
					{
						System.out.println("CTC is not null");
						talker.send("INVALID_LOGIN");
					}
				}

				// Person logging in is not a user
				else
				{
					SwingUtilities.invokeLater(new Runnable()
					{
						public void run()
						{
							JOptionPane.showMessageDialog(null, "Not a user");
						}
					}
					);
					talker.send("INVALID_LOGIN");
				}
			}

			// Client is not registering or logging in
			else
				talker.send("INVALID_COMMAND");
		}
		catch(IOException ioe)
		{
			System.out.println("Trouble recieveing register/login command");
		}

		// Get commands from the client
		try
		{
			while(true)
			{
				cmd = talker.receive();

				handleCommands(cmd);
			}
		}

		// Update the current users friend list the current user is offline
		catch(IOException ioe)
		{
			try
			{
				currentUsername = this.user.username;
				for(int n = 0; n < this.user.listOfBuddies.size(); n++)
				{
					actBuddy = this.user.listOfBuddies.elementAt(n);
					buddyUser = server.userList.get(actBuddy);

					if(buddyUser.ctc != null)
						buddyUser.ctc.talker.send("UPDATE_STATUS" + " " + currentUsername + " " + "offline");
				}
				user = server.userList.get(currentUsername);
				user.ctc = null;

				System.out.println("Trouble receiveing execute command");
			}
			catch(IOException ioe2)
			{
				System.out.println("Can't update status");
			}
		}
	}

//===============================================

	void handleCommands(String cmd) throws IOException
	{
		int port;
		int fileLength;
		String fileName;
		String domain;

		// Send friend request
		if(cmd.startsWith("SEND_REQ"))
		{
			parts = cmd.split(" ");
			potentialBuddy = parts[1];

			// See if potential friend is a user
			if(server.userList.containsKey(potentialBuddy))
			{
				requester = parts[2];
				user = server.userList.get(potentialBuddy);

				// See if potential friend is online
				if(user.ctc != null)
					user.ctc.talker.send("BUD_REQ" + " " + requester + " " + potentialBuddy);

				// Potential friend is not online so store the friend request
				else
				{
					System.out.println("CTC is null");
					user.offlineCommands.add(cmd);
					server.userList.store();
				}
			}

			// Alert client that potential friend is not a user
			else
				talker.send("NOT_A_USER" + potentialBuddy);
		}

		// Accepted friend request
		else if(cmd.startsWith("ADDED_BUD"))
		{
			parts = cmd.split(" ");
			requester = parts[1];
			currentUsername = parts[2];
			System.out.println("currentUsername: " + currentUsername);

			// Potential friend added requester as a friend
			this.user.listOfBuddies.add(requester);

			// Requester add potential friend as a friend
			user = server.userList.get(requester);
			user.listOfBuddies.add(currentUsername);
			server.userList.store();

			// Alert requester that potential friend accepted friend request
			if(user.ctc != null)
				user.ctc.talker.send("BUD_SAID_YES" + currentUsername);

			// Requester is offline so store message that potential friend accepted friend request
			else
			{
				user.offlineCommands.add(cmd);
				server.userList.store();
			}
		}

		// Send message to friend
		else if(cmd.startsWith("SEND_TO"))
		{
			parts = cmd.split("`");
			buddyUser = server.userList.get(parts[1]);
			currentUsername = parts[2];

			if(buddyUser.ctc != null)
				buddyUser.ctc.talker.send("INCOME_MSG" + "`" + currentUsername + "`" + parts[3]);
			else
			{
				buddyUser.offlineCommands.add(cmd);
				server.userList.store();
			}
		}

		// Alert all friends that your online
		else if(cmd.equals("UPDATE_STATUS"))
		{
			for(int n = 0; n < this.user.listOfBuddies.size(); n++)
			{
				actBuddy = this.user.listOfBuddies.elementAt(n);
				buddyUser = server.userList.get(actBuddy);
				currentUsername = this.user.username;

				if(buddyUser.ctc != null)
					buddyUser.ctc.talker.send("UPDATE_STATUS" + " " + currentUsername + " " + "online");
			}
		}

		// Logged out and inform friends your offline
		else if(cmd.equals("LOGGED_OUT"))
		{
			currentUsername = this.user.username;
			for(int n = 0; n < this.user.listOfBuddies.size(); n++)
			{
				actBuddy = this.user.listOfBuddies.elementAt(n);
				buddyUser = server.userList.get(actBuddy);

				if(buddyUser.ctc != null)
					buddyUser.ctc.talker.send("UPDATE_STATUS" + " " + currentUsername + " " + "offline");
			}
			user = server.userList.get(currentUsername);
			user.ctc = null;
		}

		// Send file to friend
		else if(cmd.startsWith("SEND_FILE"))
		{
			parts = cmd.split(":");
			actBuddy = parts[3];
			fileLength = Integer.parseInt(parts[1]);
			fileName = parts[2];
			buddyUser = server.userList.get(actBuddy);

			if(buddyUser.ctc != null)
				buddyUser.ctc.talker.send("WANT_THIS_FILE" + ":" + fileLength + ":" + fileName + ":" + this.user.username);
			else
			{
				buddyUser.offlineCommands.add(cmd);
				server.userList.store();
			}
		}

		// Accept file from friend via P2P
		else if(cmd.startsWith("ACCEPT_FILE"))
		{
			parts = cmd.split(":");
			actBuddy = parts[4];
			fileName = parts[1];
			port = Integer.parseInt(parts[2]);
			fileLength = Integer.parseInt(parts[5]);
			domain = parts[3];
			currentUsername = this.user.username;
			buddyUser = server.userList.get(actBuddy);

			if(buddyUser.ctc != null)
				buddyUser.ctc.talker.send("FORWARD_FILE" + ":" + fileName + ":" + port + ":" + domain + ":" + fileLength);
			else
			{
				buddyUser.offlineCommands.add(cmd);
				server.userList.store();
			}
		}
	}
}