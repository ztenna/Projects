// Zachary Tennant

import javax.swing.*;
import java.awt.event.*;
import java.awt.*;
import javax.swing.text.html.*;
import javax.swing.text.*;
import java.io.*;
import java.awt.dnd.*;
import java.awt.datatransfer.*;

class ChatBox extends JDialog
					implements ActionListener, DropTargetListener
{
	//JPanel contentPanel;

	JPanel msgPanel;
	JLabel msgLabel;
	JTextArea msgTA;

	JPanel chatPanel;
	JLabel chatLabel;
	JEditorPane edPane;
	JScrollPane scroller;

	JPanel buttonPanel;
	JButton sendButton;

	CTS cts;
	String buddyUsername;
	String myUsername;

	DropTarget dropTarget;
	File file;

//===============================================

	public ChatBox(CTS cts, String buddyUsername, String myUsername)
	{
		this.cts = cts;
		this.buddyUsername = buddyUsername;
		this.myUsername = myUsername;

		//contentPanel = new JPanel(new GridLayout(2,1));

		msgPanel = new JPanel();

		msgLabel = new JLabel("Message");
		msgTA = new JTextArea(5, 20);
		msgPanel.add(msgLabel);
		msgPanel.add(msgTA);
		//contentPanel.add(msgPanel);

		edPane = new JEditorPane();
		edPane.setContentType("text/html");
		edPane.setEditable(false);
		edPane.setPreferredSize(new Dimension(400, 200));

		chatPanel = new JPanel();

		chatLabel = new JLabel("Conversation");
		scroller = new JScrollPane(edPane);
		chatPanel.add(chatLabel);
		chatPanel.add(scroller);
		//contentPanel.add(chatPanel);

		buttonPanel = new JPanel();

		sendButton = new JButton("Send");
		sendButton.setActionCommand("SEND");
		sendButton.setToolTipText("Send a message to your buddy.");
		sendButton.addActionListener(this);
		buttonPanel.add(sendButton);

		dropTarget = new DropTarget(chatPanel, this);

		add(msgPanel, BorderLayout.NORTH);
		add(chatPanel, BorderLayout.CENTER);
		add(buttonPanel, BorderLayout.SOUTH);

		setupDialog();
	} // end constructor

//===============================================

	void setupDialog()
	{
		Toolkit tk;
		Dimension d;

		tk = Toolkit.getDefaultToolkit();
		d = tk.getScreenSize();
		setSize(d.width/3, d.height/3);
		setLocation(d.width/4, d.height/4);

		setDefaultCloseOperation(JDialog.DISPOSE_ON_CLOSE);

		setTitle(myUsername + " Chat Window");

		//setModalityType(Dialog.ModalityType.APPLICATION_MODAL);
		setVisible(true);
	} // end setupDialog

//===============================================

	public void actionPerformed(ActionEvent e)
	{
		String cmd = e.getActionCommand();
		String msg;
		String sendMsg;
		String myName;

		if(cmd.equals("SEND"))
		{
			msg = msgTA.getText().trim();
			if(!msg.equals(""))
			{
				if(!msg.contains("`"))
				{
					try
					{
						myName = "Me";
						sendMsg = msg.replace('\n', '~');
						cts.talker.send("SEND_TO" + "`" + buddyUsername + "`" + myUsername + "`" + sendMsg);
						addMsg(sendMsg, myName);
						msgTA.setText("");
						msgTA.requestFocus();
					}
					catch(IOException ioe)
					{
						JOptionPane.showMessageDialog(null, "Can't send message.");
						ioe.printStackTrace();
					}
				}
				else
					JOptionPane.showMessageDialog(null, "Can't have '|' in message.");
			}
			else
				JOptionPane.showMessageDialog(null, "Can't send empty message.");
		}
	}

//===============================================

	void addMsg(String msg, String myName)
	{
		HTMLDocument doc;
		Element rootElement;
		Element bodyElement;
		String text;
		String[] parts;
		String color;

		try
		{
			doc = (HTMLDocument)edPane.getDocument();
			rootElement = doc.getRootElements()[0];
			bodyElement = rootElement.getElement(0);

			parts = msg.split("~");

			if(myName.equals("Me"))
				color = "BLUE";
			else
				color = "RED";

			text = "<DIV alignment=left><FONT Color=" + color + ">" + myName + ": ";
			for(int n = 0; n < parts.length; n++)
				text = text + parts[n] + "<br>";
			doc.insertBeforeEnd(bodyElement, text + "</FONT></DIV>");

			edPane.repaint();
			edPane.setCaretPosition(doc.getLength());
		}
		catch(BadLocationException ble)
		{
			JOptionPane.showMessageDialog(null, "Bad message location");
		}
		catch(IOException ioe)
		{
			JOptionPane.showMessageDialog(null, "Can't add message");
		}
	}

//###############################################

	public void dragEnter(DropTargetDragEvent dtde)
	{
	}

//===============================================

	public void dragExit(DropTargetEvent dte)
	{
	}

//===============================================

	public void dragOver(DropTargetDragEvent dtde)
	{
	}

//===============================================

	public void drop(DropTargetDropEvent dtde)
	{
		java.util.List<File> fileList;
		Transferable transferableData;
		DataInputStream dis;

		transferableData = dtde.getTransferable();

		try
		{
			if(transferableData.isDataFlavorSupported(DataFlavor.javaFileListFlavor))
			{
				dtde.acceptDrop(DnDConstants.ACTION_COPY);

				fileList = (java.util.List<File>)(transferableData.getTransferData(DataFlavor.javaFileListFlavor));
				file = fileList.get(0);
				if(!file.isDirectory())
				{
					dis = new DataInputStream(new FileInputStream(file));
					cts.setFile(file);
					cts.talker.send("SEND_FILE" + ":" + file.length() + ":" + file.getName() + ":" + buddyUsername);
				}
				else
					JOptionPane.showMessageDialog(null, "The file is a directory");
			}
			else
				JOptionPane.showMessageDialog(null, "File is not flavor supported");
		}
		catch(UnsupportedFlavorException ufe)
		{
			System.out.println("Unsupported flavor found");
		}
		catch(IOException ioe)
		{
			System.out.println("Can't send file");
		}
	}

//===============================================

	public void dropActionChanged(DropTargetDragEvent dtde)
	{
	}
} // end class ChatBox