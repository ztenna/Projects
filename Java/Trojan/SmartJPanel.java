// Zachary Tennant

import javax.swing.*;
import java.awt.event.*;
import java.util.*;
import java.awt.*;

class SmartJPanel extends JPanel
					implements MouseListener
{
	static Random r = new Random();

	int red;
	int green;
	int blue;

	public SmartJPanel()
	{
	}

//###############################################

	public void mouseClicked(MouseEvent e)
	{
	}

//===============================================

	public void mouseEntered(MouseEvent e)
	{
		red = r.nextInt(256);
		green = r.nextInt(256);
		blue = r.nextInt(256);

		setBackground(new Color(red, green, blue));
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
} // end SmartJPanel class