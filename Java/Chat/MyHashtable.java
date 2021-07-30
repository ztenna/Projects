// Zachary Tennant

import java.util.*;
import java.io.*;

class MyHashtable extends Hashtable<String, User>
{
	void store()
	{
		DataOutputStream dos;
		Enumeration<User> e;

		try
		{
			dos = new DataOutputStream(new FileOutputStream("RegUserList"));
			dos.writeInt(size());

			e = this.elements();

			while(e.hasMoreElements())
				e.nextElement().store(dos);
		}
		catch(FileNotFoundException fnfe)
		{
			System.out.println("File was not found when storing list");
		}
		catch(IOException ioe)
		{
			System.out.println("Can't write size into file.");
		}
	}

//===============================================

	public MyHashtable(File file)
	{
		DataInputStream dis;
		User user;
		int size;

		try
		{
			dis = new DataInputStream(new FileInputStream(file));
			size = dis.readInt();

			for(int i = 0; i < size; i++)
			{
				user = new User(dis);
				put(user.username, user);
			}
		}
		catch(FileNotFoundException fnfe)
		{
			System.out.println("File was not found when loadinging list");
		}
		catch(IOException ioe)
		{
			System.out.println("Can't get size of file when loading");
		}
	}

//===============================================

	public MyHashtable()
	{
	}
} // end class MyHashtable