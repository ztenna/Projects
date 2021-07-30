// Zachary Tennant

import java.io.*;
import java.net.*;
import javax.swing.*;

class FileReceiver
		implements Runnable
{
	ServerSocket serverSocket;
	Socket socket;
	int port;
	int fileLength;
	String fileName;

	public FileReceiver(int fileLength, String fileName)
	{
		port = 4321;
		this.fileLength = fileLength;
		this.fileName = fileName;

		new Thread(this).start();
	}

//===============================================

	public void run()
	{
		InputStream is;
		FileOutputStream fos;
		byte[] buff;
		int numBytesRead;
		int totNumBytesRead;

		try
		{
			serverSocket = new ServerSocket(port);
			socket = serverSocket.accept();
			is = socket.getInputStream();
			fos = new FileOutputStream(fileName);

			totNumBytesRead = 0;
			while(totNumBytesRead != fileLength)
			{
				buff = new byte[256];
				numBytesRead = is.read(buff);
				totNumBytesRead = totNumBytesRead + numBytesRead;
				fos.write(buff, 0, numBytesRead);
			}
		}
		catch(IOException ioe)
		{
			JOptionPane.showMessageDialog(null, "Trouble reading/writing file on receiver");
		}
	} // end run
} // end class FileReceiver