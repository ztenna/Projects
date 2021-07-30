// Zachary Tennant

import java.awt.*;
import javax.swing.*;
import java.awt.event.*;
import java.io.*;
import java.text.*;
import javax.swing.GroupLayout.*;
import java.util.Properties;
import javax.mail.Message;
import javax.mail.MessagingException;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;

public class SendMailMain
{
	public static void main(String[] args)
	{
		new MailFrame();
	} // end of main

} // end of SendMailMain

//###############################################

class MailFrame extends JFrame
					implements ActionListener
{
// default values
	public static String domain = "smtp.gmx.com";
	public static String port = "465";
	public static String username = "atenn11@gmx.us";//"larue@gmx.us";
	public static String password = "larue321";
	public static String from = "atenn11@gmx.us";//"larue@gmx.us";
	public static String date = "01-01-2010";

// labels
	JLabel domainLabel;
	JLabel portLabel;
	JLabel usernameLabel;
	JLabel passwordLabel;
	JLabel fromLabel;
	JLabel dateLabel;
	JLabel subjectLabel;
	JLabel messageLabel;

// text fields
	JTextField domainTF;
	JTextField portTF;
	JTextField usernameTF;
	JPasswordField passwordTF;
	JTextField fromTF;
	JTextField dateTF;
	JTextField subjectTF;

// text area
	JTextArea messageTA;

// button
	JButton sendButton;

// grouping
	//JPanel textPanel;
	//JPanel domainPanel;
	//JPanel portPanel;
	//JPanel usernamePanel;
	//JPanel passwordPanel;
	//JPanel fromPanel;
	//JPanel datePanel;
	//JPanel subjectPanel;
	//JPanel messagePanel;
	Container cp;

	JPanel panel;
	GroupLayout layout;

	DefaultListModel<String> list;

//===============================================

// constructor
	MailFrame()
	{
// text panel
		//textPanel = new JPanel(new GridLayout(7,1));

// domain
		//domainPanel = new JPanel(new GridLayout(1,2));
		domainLabel = new JLabel("Domain");
		domainTF = new JTextField(domain);
		//domainPanel.add(domainLabel);
		//domainPanel.add(domainTF);
		//textPanel.add(domainPanel);

// port
		//portPanel = new JPanel(new GridLayout(1,2));
		portLabel = new JLabel("Port");
		portTF = new JTextField(port);
		portTF.setInputVerifier(new PortVerifier());
		//portPanel.add(portLabel);
		//portPanel.add(portTF);
		//textPanel.add(portPanel);

// username
		//usernamePanel = new JPanel(new GridLayout(1,2));
		usernameLabel = new JLabel("Username");
		usernameTF = new JTextField(username);
		//usernamePanel.add(usernameLabel);
		//usernamePanel.add(usernameTF);
		//textPanel.add(usernamePanel);

// password
		//passwordPanel = new JPanel(new GridLayout(1,2));
		passwordLabel = new JLabel("Password");
		passwordTF = new JPasswordField(password);
		//passwordPanel.add(passwordLabel);
		//passwordPanel.add(passwordTF);
		//textPanel.add(passwordPanel);

// from
		//fromPanel = new JPanel(new GridLayout(1,2));
		fromLabel = new JLabel("Sent From");
		fromTF = new JTextField(from);
		//fromPanel.add(fromLabel);
		//fromPanel.add(fromTF);
		//textPanel.add(fromPanel);

// date
		//datePanel = new JPanel(new GridLayout(1,2));
		dateLabel = new JLabel("Sent Date");
		dateTF = new JTextField(date);
		dateTF.setInputVerifier(new DateVerifier());
		//datePanel.add(dateLabel);
		//datePanel.add(dateTF);
		//textPanel.add(datePanel);

// subject
		//subjectPanel = new JPanel(new GridLayout(1,2));
		subjectLabel = new JLabel("Subject");
		subjectTF = new JTextField("");
		//subjectPanel.add(subjectLabel);
		//subjectPanel.add(subjectTF);
		//textPanel.add(subjectPanel);

// message
		//messagePanel = new JPanel(new GridLayout(2,1));
		messageLabel = new JLabel("Message", messageLabel.CENTER);
		messageTA = new JTextArea();
		//messagePanel.add(messageLabel);
		//messagePanel.add(messageTA);
		//textPanel.add(messagePanel);

// send button
		sendButton = new JButton("Send");
		sendButton.setActionCommand("SEND");
		sendButton.setToolTipText("Send the message");
		sendButton.addActionListener(this);

		panel = new JPanel();
		layout = new GroupLayout(panel);
		panel.setLayout(layout);

		layout.setAutoCreateGaps(true);
		layout.setAutoCreateContainerGaps(true);

		GroupLayout.SequentialGroup hGroup = layout.createSequentialGroup();

		hGroup.addGroup(layout.createParallelGroup().addComponent(domainLabel).
				addComponent(portLabel).addComponent(usernameLabel).addComponent(passwordLabel).
				addComponent(fromLabel).addComponent(dateLabel).addComponent(subjectLabel).
				addComponent(messageLabel));

		hGroup.addGroup(layout.createParallelGroup().addComponent(domainTF).
				addComponent(portTF).addComponent(usernameTF).addComponent(passwordTF).
				addComponent(fromTF).addComponent(dateTF).addComponent(subjectTF).
				addComponent(messageTA));

		layout.setHorizontalGroup(hGroup);

		GroupLayout.SequentialGroup vGroup = layout.createSequentialGroup();

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(domainLabel).addComponent(domainTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(portLabel).addComponent(portTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(usernameLabel).addComponent(usernameTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(passwordLabel).addComponent(passwordTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(fromLabel).addComponent(fromTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(dateLabel).addComponent(dateTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(subjectLabel).addComponent(subjectTF));

		vGroup.addGroup(layout.createParallelGroup(Alignment.BASELINE).
				addComponent(messageLabel).addComponent(messageTA));

		layout.setVerticalGroup(vGroup);

// content pane
		cp = getContentPane();
		//cp.add(textPanel, BorderLayout.WEST);
		cp.add(panel, BorderLayout.CENTER);
		cp.add(sendButton, BorderLayout.SOUTH);

		list = new DefaultListModel<String>();

// main frame
		setupMainFrame();
	} // end of constructor

//###############################################

	void setupMainFrame()
	{
		Toolkit tk;
		Dimension d;

		tk = Toolkit.getDefaultToolkit();
		d = tk.getScreenSize();
		setSize(d.width/3, d.height/3);
		setLocation(d.width/4, d.height/4);

		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		setTitle("Send Mail");

		setVisible(true);
	} // end of setupMainFrame

//###############################################

	public void actionPerformed(ActionEvent e)
	{
		if(e.getActionCommand().equals("SEND"))
			doSend();
	} // end of actionPerformed

//===============================================

	void doSend()
	{
		System.out.println("Send");

		MyMailAuthenticator authenticator;
		Properties props;
		Session session;
		Message message;
		PrintWriter goodPW;
		PrintWriter badPW;

		DateFormat df = new SimpleDateFormat("MM-dd-yyyy");

		getEmailList();

		authenticator = new MyMailAuthenticator(username, password);
		System.out.println("got an authenticator");

		props = new Properties();
		props.setProperty("mail.smtp.host", domainTF.getText().trim());
		props.setProperty("mail.smtp.socketFactory.port", portTF.getText().trim());
		props.setProperty("mail.smtp.socketFactory.class", "javax.net.ssl.SSLSocketFactory");
		props.setProperty("mail.smtp.auth", "true");
		props.setProperty("mail.smtp.port", portTF.getText().trim());
		props.setProperty("mail.smtp.ssl.enable", "true");
		props.setProperty("mail.transport.protocol", "smtps");

		session = Session.getInstance(props, authenticator);
		//session.setDebug(true);
		System.out.println("got a session");

		try
		{
			goodPW = new PrintWriter(new FileWriter("GoodEmails.txt"));
			badPW = new PrintWriter(new FileWriter("BadEmails.txt"));

			for(int index = 0; index < list.getSize(); index++)
			{
				try
				{
					message = new MimeMessage(session);
					message.setFrom(new InternetAddress(fromTF.getText().trim()));
					message.setRecipients(Message.RecipientType.TO, InternetAddress.parse(list.elementAt(index)));
					message.setSubject(subjectTF.getText().trim());
					message.setText(messageTA.getText().trim());
					message.setSentDate(df.parse(dateTF.getText().trim()));

					System.out.println("hello");
					Transport.send(message);
					System.out.println("message sent");

					goodPW.println(list.elementAt(index));
				}
				catch(MessagingException e)
				{
					JOptionPane.showMessageDialog(this, "domain/port/username/password combo invalid");
					badPW.println(list.elementAt(index));
				}
				catch(ParseException pe)
				{
				//	JOptionPane.showMessageDialog(this, "Bad date");
				}
			}
			goodPW.close();
			badPW.close();
		}
		catch(IOException ioe)
		{
			JOptionPane.showMessageDialog(this, "Bad file in send");
		}
	} // end of doSend

//===============================================

	void getEmailList()
	{
		BufferedReader br;
    	String addTo;

		try
		{
			br = new BufferedReader(new FileReader("emails.txt"));
			while(br.ready())
			{
				addTo = br.readLine();
				list.addElement(addTo);
			}
			br.close();
		}
		catch(Exception e)
		{
			JOptionPane.showMessageDialog(this,"Error");
        }
	} // end of getEmailList
} // end of class MailFrame