// Zachary Tennant

import java.awt.*;
import javax.swing.*;
import java.awt.event.*;
import java.io.*;
import java.net.*;
import javax.swing.text.html.parser.*;

public class ImageSearchMain
{
	public static void main(String[] args)
	{
		new MyFrame();
	} // end of main

} // end of ImageSearchMain class

//###############################################

class MyFrame extends JFrame
					implements ActionListener,
								MouseListener
{
// menu
	JPanel buttonPanel;
	JPanel textPanel;
	Container cp;

// buttons
	JButton goButton;
	JButton printButton;

// list
	DefaultListModel<String> list;
	JList listBox;

	JScrollPane scroller;

// text field
	JLabel urlLabel;
	JTextField urlTextField;

// go
	URL url;
	URLConnection urlConnection;
	InputStreamReader isr;
	MyParserCallbackTagHandler tagHandler;
	String domain;

// double click
	Image image;
	JDialog imageDialog;

	TextFilePrinter tfp;

//===============================================

// constructor
	MyFrame()
	{
// button panel
		buttonPanel = new JPanel(new GridLayout(1,2));

// go button
		goButton = new JButton("Go");
		goButton.setActionCommand("GO");
		goButton.setToolTipText("Retrieve the SRC for each tag and display in list");
		goButton.addActionListener(this);
		buttonPanel.add(goButton);
		getRootPane().setDefaultButton(goButton);

// print button
		printButton = new JButton("Print");
		printButton.setActionCommand("PRINT");
		printButton.setToolTipText("Print the list of image url's");
		printButton.addActionListener(this);
		buttonPanel.add(printButton);

// list
		list = new DefaultListModel<String>();
		listBox = new JList(list);
		listBox.addMouseListener(this);
		scroller = new JScrollPane(listBox);

// text panel
		textPanel = new JPanel(new GridLayout(1,2));

// url text field
		urlLabel = new JLabel("URL");
		urlTextField = new JTextField();
		textPanel.add(urlLabel);
		textPanel.add(urlTextField);

// content pane
		cp = getContentPane();
		cp.add(textPanel, BorderLayout.NORTH);
		cp.add(scroller, BorderLayout.CENTER);
		cp.add(buttonPanel, BorderLayout.SOUTH);

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
		setSize(d.width/2, d.height/2);
		setLocation(d.width/4, d.height/4);

		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		setTitle("Image Search");

		setVisible(true);
	} // end of setupMainFrame()

//###############################################

	public void actionPerformed(ActionEvent e)
	{
		if(e.getActionCommand().equals("GO"))
			doGo();
		else if(e.getActionCommand().equals("PRINT"))
			doPrint();
	} // end of actionPerformed(...)

//===============================================

	void doGo()
	{
		//System.out.println("Go has been pressed");

		domain = urlTextField.getText();

		try
		{
			url = new URL(domain);

			urlConnection = url.openConnection();

			isr = new InputStreamReader(urlConnection.getInputStream());

			tagHandler = new MyParserCallbackTagHandler(domain, list);
			new ParserDelegator().parse(isr, tagHandler, true);
		}
		catch(MalformedURLException mue)
		{
			System.out.println("Bad URL");
			//mue.printStackTrace();
			JOptionPane.showMessageDialog(this, "Bad URL");
		}
		catch(IOException ioe)
		{
			System.out.println("IO Exception");
			//ioe.printStackTrace();
			JOptionPane.showMessageDialog(this, "Bad Connection to Site");
		}
	} // end of doGo

//===============================================

	void doPrint()
	{
		//System.out.println("Print has been pressed");

		tfp = new TextFilePrinter(list);
		tfp.printIt(tfp);
	}

//###############################################

	public void mouseClicked(MouseEvent e)
	{
		Toolkit tk;
		Dimension d;
		int selectedIndex;
		String attribute;
		URL url2;

		ImagePanel imagePanel;

		tk = Toolkit.getDefaultToolkit();
		d = tk.getScreenSize();

		if(e.getClickCount() == 2)
		{
			//System.out.println("Double click");

			selectedIndex = listBox.locationToIndex(e.getPoint());
			attribute = list.elementAt(selectedIndex);

			//System.out.println("URL: " + attribute);

			try
			{
				url2 = new URL(attribute);
				image = tk.getImage(url2);
				imageDialog = new JDialog();

				imagePanel = new ImagePanel(image);
				imageDialog.add(imagePanel);

				imageDialog.setSize(d.width/8, d.height/4);
				imageDialog.setVisible(true);
			}
			catch(MalformedURLException mue)
			{
				System.out.println("No image");
				//mue.printStackTrace();
				JOptionPane.showMessageDialog(this, "No image found");
			}
		}
	}

//===============================================

	public void mouseEntered(MouseEvent e)
	{
	}

//===============================================

	public void mouseExited(MouseEvent e)
	{
	}

//===============================================

	public void mousePressed(MouseEvent e)
	{
	}

//===============================================

	public void mouseReleased(MouseEvent e)
	{
	}
} // end of class MyFrame