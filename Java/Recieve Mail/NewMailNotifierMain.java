// Zachary Tennant

import javax.swing.*;
import java.awt.event.*;
import java.awt.*;
import java.io.*;
import java.util.*;
import javax.mail.*;
import javax.swing.*;

public class NewMailNotifierMain
{
	public static void main(String[] args)
	{
		new MyMailNotifier();
	} // end of main
} // end of NewMailNotifierMain class

//###############################################

class MyMailNotifier
			implements ActionListener, WindowListener
{
// popup menu
	PopupMenu popupMenu;
	MenuItem menuItem;
	CheckboxMenuItem soundCheckBox;

// tray
	SystemTray tray;
	TrayIcon trayIcon;

// file
	String fileName;
	File myFile;
	FileInputStream fileInStream;
	FileOutputStream fileOutStream;

// properties
	Properties props;
	PropertiesDialog propDialog;
	boolean isOpen;

// mail
	Session session;
	Store storeVar;
	Folder inboxFolder;

// timer
	javax.swing.Timer timer;
	int time;

//===============================================

	MyMailNotifier()
	{
		popupMenu = new PopupMenu();

		menuItem = new MenuItem("Exit Program");
		menuItem.setActionCommand("EXIT");
		menuItem.addActionListener(this);
		popupMenu.add(menuItem);

		menuItem = new MenuItem("Edit Properties");
		menuItem.setActionCommand("EDIT");
		menuItem.addActionListener(this);
		popupMenu.add(menuItem);

		soundCheckBox = new CheckboxMenuItem("Sound");
		soundCheckBox.setActionCommand("SOUND");
		soundCheckBox.addActionListener(this);
		popupMenu.add(soundCheckBox);

		isOpen = true;

		if(SystemTray.isSupported())
		{
			setupTray();
		}
		else
			System.out.println("SystemTray not supported");

		fileName = "ZMT-MailProperties";
		myFile = new File(fileName);

		try
		{
			props = new Properties();
			if(myFile.exists())
			{
				//System.out.println("load it");

				fileInStream = new FileInputStream(myFile);
				props.load(fileInStream);
				fileInStream.close();
			}
			else
			{
				propDialog = new PropertiesDialog(props);

				if(propDialog.isSaved == false)
					System.exit(1);

				fileOutStream = new FileOutputStream(myFile);
				props.store(fileOutStream, "Properties");
				fileOutStream.close();
			}
			time = Integer.parseInt(props.getProperty("TIMEBETCHECK")) * 1000;
			timer = new javax.swing.Timer(time, this);
			timer.setActionCommand("UPDATE");
			timer.setRepeats(true);
			timer.setCoalesce(true);

			checkMail();
		}
		catch(FileNotFoundException fnfe)
		{
			System.out.println("myFile not found");
			fnfe.printStackTrace();
		}
		catch(IOException ioe)
		{
			System.out.println("Can't load, store, or close stream");
			ioe.printStackTrace();
		}
		timer.start();
	} // end of constructor

//###############################################

	void setupTray()
	{
		tray = SystemTray.getSystemTray();

		trayIcon = new TrayIcon(Toolkit.getDefaultToolkit().
					getImage("mail.png"), "Email");
		trayIcon.setImageAutoSize(true);
		trayIcon.setPopupMenu(popupMenu);

		try
		{
			tray.add(trayIcon);
		}
		catch(AWTException e)
		{
			System.out.println("TrayIcon could not be added");
			return;
		}
	} // end of setupTray

//###############################################

	public void actionPerformed(ActionEvent e)
	{
		int response;
		String cmd = e.getActionCommand();

// exit
		if(cmd.equals("EXIT"))
		{
			timer.stop();
			response = JOptionPane.showConfirmDialog(null,"Do you want to exit the program?");
			if(response == JOptionPane.YES_OPTION)
				System.exit(1);
			else
				timer.start();
		}
// edit
		else if(cmd.equals("EDIT"))
		{
			timer.stop();

			try
			{
				if(isOpen == true)
				{
					System.out.println(isOpen);
					fileInStream = new FileInputStream(myFile);
					props.load(fileInStream);
					isOpen = false;
					propDialog = new PropertiesDialog(props);

					fileOutStream = new FileOutputStream(myFile);
					props.store(fileOutStream, "Properties");
					fileOutStream.close();

					propDialog.addWindowListener(this);
					fileInStream.close();
				}
				else
					JOptionPane.showMessageDialog(null,"You already have Properties open");
			}
			catch(FileNotFoundException fnfe)
			{
				System.out.println("myFile not found");
				fnfe.printStackTrace();
			}
			catch(IOException ioe)
			{
				System.out.println("Can't load or close stream");
				ioe.printStackTrace();
			}

			timer.setDelay(Integer.parseInt(props.getProperty("TIMEBETCHECK")) * 1000);
			timer.start();
		}
// sound
		else if(cmd.equals("SOUND"))
		{
			if(soundCheckBox.getState() == true)
				props.setProperty("SOUND", "ON");
			else
				props.setProperty("SOUND", "OFF");
		}
// update
		else
		{
			try
			{
				if(props.getProperty("SOUND").equals("ON"))
					soundCheckBox.setState(true);
				else
					soundCheckBox.setState(false);

				fileOutStream = new FileOutputStream(myFile);
				props.store(fileOutStream, "Properties");
				fileOutStream.close();

				checkMail();
			}
			catch(FileNotFoundException fnfe)
			{
				System.out.println("myFile not found");
				fnfe.printStackTrace();
			}
			catch(IOException ioe)
			{
				System.out.println("Can't load or close stream");
				ioe.printStackTrace();
			}
		}
	} // end of actionPerformed

//===============================================

	void checkMail() //throws MessagingException
	{
		System.out.println("Checking mail");

		String protocolProvider = "imaps";
		String host = props.getProperty("SERVERNAME");
		String username = props.getProperty("USERNAME");
		String password = props.getProperty("PASSWORD");

		try
		{
			session = Session.getDefaultInstance(props, null);
			//session.setDebug(true);

			storeVar = session.getStore(protocolProvider);
			storeVar.connect(host, username, password);

			inboxFolder = storeVar.getFolder("INBOX");
			inboxFolder.open(Folder.READ_WRITE);

			if(inboxFolder.hasNewMessages())
			{
				System.out.println("has new mail");
				if(props.getProperty("SOUND").equals("ON"))
					new Sound().makeASound();
				JOptionPane.showMessageDialog(null, "You have " + inboxFolder.getNewMessageCount() + " new messages!");
			}

			inboxFolder.close(false);
			storeVar.close();
		}
		catch(Exception e)
		{
			timer.stop();
			System.out.println("exception in checkMail");
			JOptionPane.showMessageDialog(null, "Check properties for errors");
			e.printStackTrace();
			//System.out.println("No such email protocol provider: " + protocolProvider);
		}
	} // end of checkMail

//###############################################

	public void windowActivated(WindowEvent we)
	{
	}

//===============================================

	public void windowClosed(WindowEvent we)
	{
		isOpen = true;
		//System.out.println("boolean is: " + isOpen + " in windowClosed");
	}

//===============================================

	public void windowClosing(WindowEvent we)
	{
		isOpen = true;
		//System.out.println("boolean is: " + isOpen + " in windowClosed");
	}

//===============================================

	public void windowDeactivated(WindowEvent we)
	{
	}

//===============================================

	public void windowDeiconified(WindowEvent we)
	{
	}

//===============================================

	public void windowIconified(WindowEvent we)
	{
	}

//===============================================

	public void windowOpened(WindowEvent we)
	{
	}
} // end of class