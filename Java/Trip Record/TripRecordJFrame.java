// Zachary Tennant

import java.awt.*;
import java.awt.event.*;
import java.awt.dnd.*;
import java.awt.datatransfer.*;
import java.awt.Dimension.*;
import java.io.*;
import java.text.*;
import javax.swing.*;
import javax.swing.event.*;
import javax.swing.table.*;
import javax.swing.filechooser.FileNameExtensionFilter;
import java.lang.Exception;
import java.util.Arrays;
import java.util.*;


public class TripRecordJFrame
{
//===============================================
	public static void main(String[] args)
	{
		new MyFrame();
	} // end of main

} // end of TripRecordJFrame class

//###############################################
class MyFrame extends JFrame
					implements ActionListener,
								ListSelectionListener,
								DropTargetListener,
								MouseListener
{
// menu
    JLabel msgLabel;
    Container cp;
    JPanel buttonPanel;

// buttons
    JButton loadButton;
    JButton saveButton;
    JButton saveAsButton;
    JButton addButton;
    JButton editButton;
    JButton deleteButton;
    JButton exitButton;

    DropTarget dropTarget;

    String fileName;
    JMenuItem del;
    JMenuItem ed;
    int selectedPopupIndex;

    TripRecordTableModel trTableModel;
    JTable myJTable;
    JScrollPane scroller;

    JFileChooser chooser;
    int response;

    boolean isSaved;
//===============================================

// constructor
    MyFrame()
    {
		isSaved = true;
// construct a TableModel
		trTableModel = new TripRecordTableModel();

// construct a JTable
		myJTable = new JTable(trTableModel);

// look at this
		myJTable.setMinimumSize(new Dimension(160,160));
		myJTable.setColumnModel(trTableModel.colModel);

		scroller = new JScrollPane(myJTable);
		myJTable.setPreferredScrollableViewportSize(new Dimension(10,10));

		dropTarget = new DropTarget(scroller, this);

// sorting
		myJTable.setRowSorter(new TableRowSorter(trTableModel));

// list selection listener
		myJTable.getSelectionModel().addListSelectionListener(this);

		myJTable.addMouseListener(this);

		chooser = new JFileChooser(".");

// filter
		FileNameExtensionFilter filter;
		filter = new FileNameExtensionFilter("text file", "txt");
		chooser.setFileFilter(filter);
		filter = new FileNameExtensionFilter("data file", "dat");
		chooser.setFileFilter(filter);

// button panel
		buttonPanel = new JPanel(new GridLayout(7,1));

// load button
    	loadButton = new JButton("Load");
    	loadButton.setActionCommand("LOAD");
    	loadButton.setToolTipText("Load a list from a file");
    	loadButton.addActionListener(this);
    	buttonPanel.add(loadButton);

// save button
    	saveButton = new JButton("Save");
    	saveButton.setActionCommand("SAVE");
    	saveButton.setToolTipText("Save to an already existing file");
    	saveButton.addActionListener(this);
    	buttonPanel.add(saveButton);

// save as button
    	saveAsButton = new JButton("save As");
    	saveAsButton.setActionCommand("SAVE AS");
    	saveAsButton.setToolTipText("Save to a new file");
    	saveAsButton.addActionListener(this);
    	buttonPanel.add(saveAsButton);

// add button
    	addButton = new JButton("Add");
    	addButton.setActionCommand("ADD");
    	addButton.setToolTipText("Add new trip record to list");
    	addButton.addActionListener(this);
    	buttonPanel.add(addButton);
    	getRootPane().setDefaultButton(addButton);

// edit button
    	editButton = new JButton("Edit");
    	editButton.setActionCommand("EDIT");
    	editButton.setEnabled(false);
    	editButton.setToolTipText("Edit selected trip record");
    	editButton.addActionListener(this);
    	buttonPanel.add(editButton);

// delete button
    	deleteButton = new JButton("Delete");
    	deleteButton.setActionCommand("DELETE");
    	deleteButton.setEnabled(false);
    	deleteButton.setToolTipText("Delete selected item(s)");
    	deleteButton.addActionListener(this);
    	buttonPanel.add(deleteButton);

// exit button
    	exitButton = new JButton("Exit");
    	exitButton.setActionCommand("EXIT");
    	exitButton.setToolTipText("Quit the program");
    	exitButton.addActionListener(this);
    	buttonPanel.add(exitButton);

// content pane
    	cp = getContentPane();
    	cp.add(scroller, BorderLayout.CENTER);
    	cp.add(buttonPanel, BorderLayout.EAST);

// creating the menu
    	setJMenuBar(newMenuBar());

    	setupMainFrame();
	} // end of constructor
//###############################################
	private JMenuBar newMenuBar() // builds and returns a JMenuBar
	{
		JMenuBar menuBar;
		JMenu subMenu;

		menuBar = new JMenuBar();
//===============================================
		subMenu = new JMenu("File"); // file menu

		subMenu.getAccessibleContext().setAccessibleDescription("File operations");

		subMenu.add(newItem("Save...","SAVE",this,KeyEvent.VK_S,KeyEvent.VK_S,"Save the list."));
		subMenu.add(newItem("Load...","LOAD",this,KeyEvent.VK_L,KeyEvent.VK_L,"Load a list."));

		menuBar.add(subMenu);
//===============================================
		subMenu = new JMenu("Edit"); //edit menu

		subMenu.getAccessibleContext().setAccessibleDescription("Edit operations");

		subMenu.add(newItem("Add...","ADD",this,KeyEvent.VK_A,KeyEvent.VK_A,"Add record."));

		del = newItem("Delete...","DELETE",this,KeyEvent.VK_D,KeyEvent.VK_D,"Delete selected record(s).");
		subMenu.add(del);
		del.setEnabled(false);

		ed = newItem("Edit...","EDIT",this,KeyEvent.VK_E,KeyEvent.VK_E,"Edit selected record.");
		subMenu.add(ed);
		ed.setEnabled(false);

		menuBar.add(subMenu);
//===============================================
		return menuBar;
	} // end of JMenuBar

//###############################################
	private JMenuItem newItem(String label, String actionCommand, ActionListener menuListener, int mnemonic, int keyCode, String toolTipText)
	{
		JMenuItem m;

		m = new JMenuItem(label, mnemonic);
		m.setAccelerator(KeyStroke.getKeyStroke(keyCode, ActionEvent.ALT_MASK));
		m.getAccessibleContext().setAccessibleDescription(toolTipText);
		m.setToolTipText(toolTipText);
		m.setActionCommand(actionCommand);
		m.addActionListener(menuListener);

		return m;
	} // end of JMenuItem

//###############################################
	void setupMainFrame()
	{
		Toolkit tk;
		Dimension d;

		tk = Toolkit.getDefaultToolkit();
		d = tk.getScreenSize();
		setSize(d.width/2, d.height/4);
		setLocation(d.width/4, d.height/4);

		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		setTitle("TripRecordList");

		setVisible(true);
	} // end of setupMainFrame()

//############# Action Performed ################
	public void actionPerformed(ActionEvent e)
	{
	    if(e.getActionCommand().equals("EXIT"))
	    {
			if(isSaved == true)
	        	System.exit(0);
	        else
	        {
				response = JOptionPane.showConfirmDialog(this,"Unsaved data will be lost. Do you want to continue?");
				if(response == JOptionPane.YES_OPTION)
					System.exit(0);
			}
		}
	    else if(e.getActionCommand().equals("LOAD"))
	        doLoad();
	    else if(e.getActionCommand().equals("SAVE"))
	        doSave();
	    else if(e.getActionCommand().equals("SAVE AS"))
	        doSaveAs();
	    else if(e.getActionCommand().equals("ADD"))
	        doAdd();
	    else if(e.getActionCommand().equals("DELETE"))
	        doDelete();
	    else if(e.getActionCommand().equals("POP_DELETE"))
	    	doPopupDelete();
	    else //if(e.getActionCommand().equals("EDIT"))
	        doEdit();
	} // end of actionPerformed(...)
//===============================================
	void doLoad()
	{
		DataInputStream dis;

		//response = chooser.showOpenDialog(this);

		try
		{
			if(trTableModel.getRowCount() == 0)
			{
				response = chooser.showOpenDialog(this);

				fileName = chooser.getSelectedFile().getPath();

				dis = new DataInputStream(new FileInputStream(fileName));
				trTableModel.tripRecordList = new TripRecordList(dis);
				if(trTableModel.tripRecordList.isEmpty() == false)
					trTableModel.fireTableDataChanged();
				else
					JOptionPane.showMessageDialog(this,"Try another file");
			}
			else
			{
				response = JOptionPane.showConfirmDialog(this,"Any unsaved changes will be lost. Do you want to continue?");
				if(response == JOptionPane.YES_OPTION)
				{
					response = chooser.showOpenDialog(this);

					fileName = chooser.getSelectedFile().getPath();

					dis = new DataInputStream(new FileInputStream(fileName));
					trTableModel.tripRecordList = new TripRecordList(dis);
					if(trTableModel.tripRecordList.isEmpty() == false)
						trTableModel.fireTableDataChanged();
					else
						JOptionPane.showMessageDialog(this,"Try another file");
				}
			}
		}
		catch(FileNotFoundException e)
		{
			JOptionPane.showMessageDialog(this,"Error loading");
		}
		catch(NullPointerException npe)
		{
			System.out.println("NullPointerException in load");
			JOptionPane.showMessageDialog(this,"Try another file");
		}
	}
//===============================================
	void doSave()
	{
		DataOutputStream dos;
		if(chooser.getSelectedFile() == null)
			doSaveAs();
		else
		{
			try
			{
				fileName = chooser.getSelectedFile().getPath();

				dos = new DataOutputStream(new FileOutputStream(fileName));
				trTableModel.store(dos);
				isSaved = true;
			}
			catch(FileNotFoundException fnfe)
			{
				JOptionPane.showMessageDialog(this,"The file you are trying to save to was not found");
			}
		}
	}
//===============================================
	void doSaveAs()
	{
		if(trTableModel.tripRecordList.isEmpty() == false)
		{
			DataOutputStream dos;

			response = chooser.showSaveDialog(this);

			if(response == JFileChooser.APPROVE_OPTION)
			{
				fileName = chooser.getSelectedFile().getPath();

				try
				{
					dos = new DataOutputStream(new FileOutputStream(fileName));
					trTableModel.store(dos);
					isSaved = true;
				}
				catch(FileNotFoundException fnfe)
				{
					JOptionPane.showMessageDialog(this,"File not found in save as");
				}
			}
		}
	}
//===============================================
	void doAdd()
	{
		TripManager tripManager;
		//TripRecordJFrame trJFrame;

		new TripDialog(trTableModel.tripRecordList, this);
		trTableModel.fireTableDataChanged();
		isSaved = false;

	}
//===============================================
	void doEdit()
	{
		TripManager tripManager;
		TripRecord tripToEdit;

		tripToEdit = trTableModel.tripRecordList.elementAt(selectedPopupIndex);//Indices[0]);

		new TripDialog(trTableModel.tripRecordList, selectedPopupIndex, tripToEdit);
		trTableModel.fireTableDataChanged();
		isSaved = false;
	}
//===============================================
	void doDelete()
	{
	   	int[] indicesToDelete = myJTable.getSelectedRows();
	   	for(int n = 0; n < indicesToDelete.length; n++)
	   	{
		   indicesToDelete[n] = myJTable.convertRowIndexToModel(indicesToDelete[n]);
	   	}
	   	Arrays.sort(indicesToDelete);
	   	for(int n = indicesToDelete.length - 1; n >= 0; n--)
	   	{
		   trTableModel.tripRecordList.removeElementAt(indicesToDelete[n]);
		   trTableModel.fireTableDataChanged();
	   	}
	   	isSaved = false;
	}
//===============================================
	void doPopupDelete()
	{
		trTableModel.tripRecordList.removeElementAt(selectedPopupIndex);
		trTableModel.fireTableDataChanged();
		isSaved = false;
	}
//############### Drag and Drop #################
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
		int response;

		transferableData = dtde.getTransferable();

		try
		{
			if(transferableData.isDataFlavorSupported(DataFlavor.javaFileListFlavor))
			{
				dtde.acceptDrop(DnDConstants.ACTION_COPY);

				fileList = (java.util.List<File>)(transferableData.getTransferData(DataFlavor.javaFileListFlavor));

				if(fileList.size() > 1)
					JOptionPane.showMessageDialog(this, "Can only drag and drop one file");
				else
				{
					if(chooser.getSelectedFile() == null && trTableModel.getRowCount() > 0)
					{
						response = JOptionPane.showConfirmDialog(this,"Any unsaved changes will be lost, do you want to continue?");
						if(response == JOptionPane.YES_OPTION)
						{
							dis = new DataInputStream(new FileInputStream(fileList.get(0)));
							trTableModel.tripRecordList = new TripRecordList(dis);
							if(trTableModel.tripRecordList.isEmpty() == false)
							{
								chooser.setSelectedFile(fileList.get(0));
								trTableModel.fireTableDataChanged();
								isSaved = false;
							}
							else
								JOptionPane.showMessageDialog(this,"Try another file");
						}
					}
					else
					{
						dis = new DataInputStream(new FileInputStream(fileList.get(0)));
						trTableModel.tripRecordList = new TripRecordList(dis);
						if(trTableModel.tripRecordList.isEmpty() == false)
						{
							chooser.setSelectedFile(fileList.get(0));
							trTableModel.fireTableDataChanged();
							isSaved = false;
						}
						else
							JOptionPane.showMessageDialog(this,"Try another file");
					}
				}
			}
			else
				System.out.println("File list flavor not supported.");
		}
		catch(UnsupportedFlavorException ufe)
		{
			System.out.println("Unsupported flavor found!");
			ufe.printStackTrace();
		}
		catch(IOException ioe)
		{
			System.out.println("IOException found getting transferable data!");
			ioe.printStackTrace();
		}
	}
//===============================================
	public void dropActionChanged(DropTargetDragEvent dtde)
	{
	}
//################## Mouse ######################
	public void mouseClicked(MouseEvent e)
	{
		int selectedIndex;
		if(SwingUtilities.isRightMouseButton(e) == true)
		{
			selectedIndex = myJTable.rowAtPoint(e.getPoint());

			if(selectedIndex > -1)
			{
				selectedPopupIndex = selectedIndex;
				selectedPopupIndex = myJTable.convertRowIndexToModel(selectedPopupIndex);
				myJTable.getSelectionModel().setSelectionInterval(selectedIndex, selectedIndex);

				JMenuItem menuItem;

				JPopupMenu popupMenu = new JPopupMenu("Edit or Delete?");

				menuItem = new JMenuItem("Edit");
				menuItem.setActionCommand("EDIT");
				menuItem.addActionListener(this);
				popupMenu.add(menuItem);

				menuItem = new JMenuItem("Delete");
				menuItem.setActionCommand("POP_DELETE");
				menuItem.addActionListener(this);
				popupMenu.add(menuItem);

				popupMenu.show(this,100,100);
				//popupMenu.setSize(100,100);
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
//############## List Selection #################
	public void valueChanged(ListSelectionEvent e)
	{
	    del.setEnabled(myJTable.getSelectedRow() >= 0);
	    deleteButton.setEnabled(myJTable.getSelectedRow() >= 0);

	    if(myJTable.getSelectedRowCount() == 1)
	    {
	    	ed.setEnabled(true);
	    	editButton.setEnabled(true);
		}
		else
		{
			ed.setEnabled(false);
			editButton.setEnabled(false);
		}
	} // end of valueChanged(...)
} // end of class TripRecordJFrame
				//dos = new DataOutputStrh t t p s : / / i m g - p r o d - c m s - r t - m i c r o s o f t - c o m . a k a m a i z e d . n e t / c m s / a p i / a m / i m a g e F i l e D a t a / R W f 4 6 o ? v e r = f e 5 5   l a s t - m o d i f i e d :   M o n ,   1 6   O c t   2 0 1 7   0 9 : 0 0 : 0 4   G M T   s e r v e r :   M i c r o s o f t - I I S / 8 . 5   a c c e s s - c o n t r o l - a l l o w - o r i g i n :   *   x - d a t a c e n t e r :   E a s t U s   x - a c t i v i t y i d :   6 2 f 9 1 2 5 2 - 9 c a f - 4 b 5 a - 9 b 1 c - 3 4 0 9 4 2 e 0 e 4 e 3   x - i n s t a n c e :   R e s i z e r . W e b _ I N _ 1   x - d e p l o y m e n t :   b 5 c 9 3 3 d e 7 8 3 0 4 8 0 6 a b e 7 f f d 8 0 c 6 2 b b 0 e   t i m i n g - a l l o w - o r i g i n :   *   x - a s p n e t - v e r s i o n :   4 . 0 . 3 0 3 1 9   x - p o w e r e d - b y :   A S P . N E T   c o n t e n t - t y p e :   i m a g e / j p e g   c o n t e n t - l o c a t i o n :   h t t p s : / / i m g - p r o d - c m s - r t - m i c r o s o f t - c o m . a k a m a i z e d . n e t / c m s / a p i / a m / i m a g e F i l e D a t a / R W f 4 6 o ? v e r = f e 5 5   x - s o u r c e - l e n g t h :   4 0 0 9 5 0   x - c m s - c d n i n v a l k e y :   a m : R W f 4 6 o   c o n t e n t - l e n g t h :   4 0 0 9 5 0   c a c h e - c o n t r o l :   p u b l i c ,   m a x - a g e = 1 5 6 2 0 9   e x p i r e s :   S a t ,   2 1   O c t   2 0 1 7   0 9 : 0 0 : 4 5   G M T   d a t e :   T h u ,   1 9   O c t   2 0 1 7   1 3 : 3 7 : 1 6   G M T                                                                                                                                                                                                                                                                                                                                                                                        