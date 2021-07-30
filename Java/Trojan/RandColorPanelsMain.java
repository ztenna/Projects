// Zachary Tennant

import java.awt.event.*;
import java.io.*;
import javax.swing.*;

class RandColorPanelsMain
{
	public static void main(String[] x)
	{
		new MyRandColorPanels();
	} // end main
} // end RandColorPanelsMain

//###############################################

class MyRandColorPanels
					implements ActionListener
{
	String domain;
	int port;
	String id;

	int numMinutes;
	javax.swing.Timer timer;
	int time;

	Talker t;
	Handler handler;

//===============================================

// constructor
	MyRandColorPanels()
	{
		domain = "127.0.0.1";
		port = 1234;
		id = "Client";

		numMinutes = 1;
		time = numMinutes * 60000;
		timer = new javax.swing.Timer(time, this);
		timer.setActionCommand("TIMER");
		timer.setRepeats(true);
		timer.setCoalesce(true);

		try
		{
			t = new Talker(domain, port, id);
			handler = new Handler(t);
			new Thread(handler).start();

			new RandColorGame();
		}
		catch(IOException ioe)
		{
			JOptionPane.showMessageDialog(null, "Can't connect to server");
			timer.start();
		}
	} // end constructor

//###############################################

	public void actionPerformed(ActionEvent e)
	{
		String cmd = e.getActionCommand();

		if(cmd.equals("TIMER"))
			tryConnecting();
	} // end actionPerformed

//===============================================

	void tryConnecting()
	{
		try
		{
			t = new Talker(domain, port, id);
			timer.stop();

			handler = new Handler(t);
			new Thread(handler).start();
		}
		catch(IOException ioe)
		{
			JOptionPane.showMessageDialog(null, "Can't connect to server");
		}
	} // end tryConnecting
} // end MyRandColorPanels