// Zachary Tennant

import java.io.*;
import java.net.*;
import java.util.*;

public class ChatServerMain
{
	public static void main(String[] x)
	{
		new Acceptor().startAcceptingConnections();
	}
}

//===============================================

class Acceptor
{
	ServerSocket serverSocket;
	Socket socket;
	int port;
	File file;
	MyHashtable userList;

	public Acceptor()
	{
		port = 1234;

		file = new File("RegUserList");
		if(file.exists())
			userList = new MyHashtable(file);
		else
			userList = new MyHashtable();
	}

//===============================================

	void startAcceptingConnections()
	{
		try
		{
			serverSocket = new ServerSocket(port);
			while(true)
			{
				try
				{
					socket = serverSocket.accept();
					new CTC(socket, this);
				}
				catch(IOException ioe)
				{
					System.out.println("Server can't connect.");
				}
			}
		}
		catch(IOException ioe)
		{
			System.out.println("Bad port.");
		}
	}

//===============================================
}