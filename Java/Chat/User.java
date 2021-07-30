// Zachary Tennant

import java.io.*;
import java.util.*;

class User
{
	String username;
	String password;
	Vector<String> listOfBuddies;
	int buddyLength;
	CTC ctc;
	Vector<String> offlineCommands;
	int cmdLength;

	public User(String username, String password, CTC ctc)
	{
		this.username = username;
		this.password = password;
		this.ctc = ctc;

		listOfBuddies = new Vector<String>();
		offlineCommands = new Vector<String>();
	} // end constructor

//===============================================

	void store(DataOutputStream dos)
	{
		try
		{
			buddyLength = listOfBuddies.size();

			dos.writeUTF(username);
			dos.writeUTF(password);

			dos.writeInt(buddyLength);

			for(int n = 0; n < buddyLength; n++)
				dos.writeUTF(listOfBuddies.elementAt(n));

			cmdLength = offlineCommands.size();

			dos.writeInt(cmdLength);

			for(int n = 0; n < cmdLength; n++)
				dos.writeUTF(offlineCommands.elementAt(n));
		}
		catch(IOException ioe)
		{
			System.out.println("Can't store username/password");
		}
	} // end store

//===============================================

	public User(DataInputStream dis)
	{
		String buddyToAdd;
		String cmdToAdd;

		listOfBuddies = new Vector<String>();
		offlineCommands = new Vector<String>();

		try
		{
			username = dis.readUTF();
			password = dis.readUTF();
			buddyLength = dis.readInt();

			for(int n = 0; n < buddyLength; n++)
			{
				buddyToAdd = dis.readUTF();
				listOfBuddies.add(buddyToAdd);
			}

			cmdLength = dis.readInt();

			for(int n = 0; n < cmdLength; n++)
			{
				cmdToAdd = dis.readUTF();
				offlineCommands.add(cmdToAdd);
			}
		}
		catch(IOException ioe)
		{
			System.out.println("Can't load username/password");
		}
	} // end load
} // end class User