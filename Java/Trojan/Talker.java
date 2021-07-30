// Zachary Tennant

import java.io.*;
import java.net.*;

class Talker
{
	private DataOutputStream dos;
	private BufferedReader reader;
	String id;

// server constructor
	public Talker(Socket socket) throws IOException
	{
		dos = new DataOutputStream(socket.getOutputStream());
		reader = new BufferedReader(new InputStreamReader(socket.getInputStream()));
	}

//===============================================

// client constructor
	public Talker(String domain, int port, String id) throws IOException
	{
		Socket socket = new Socket(domain, port);

		setID(id);

		reader = new BufferedReader(new InputStreamReader(socket.getInputStream()));
		dos = new DataOutputStream(socket.getOutputStream());
	}

//===============================================

// send
	void send(String msg) throws IOException
	{
		//System.out.println("Sending");

		dos.writeBytes(msg + '\n');
		System.out.println(id + " SENT >>> " + msg + " <<<");
	}

//===============================================

// receive
	String receive() throws IOException
	{
		//System.out.println("Receiving");
		String str;

		str = reader.readLine();
		System.out.println(id + " RECD <<< " + str + " >>>");

		return str;
	}

//===============================================

// set id
	void setID(String id)
	{
		this.id = id;

		if(id == null)
			id = "Server";
	}
}
