// Zachary Tennant

import javax.swing.JPanel;
import java.util.Vector;
import java.awt.Graphics;
import java.awt.image.BufferedImage;
import java.awt.Graphics2D;
import java.awt.Color;
import javax.swing.JCheckBox;
import java.awt.*;

class MyDrawingPanel extends JPanel
{
	Vector<LivingThings> listOfLivingThings;
	BufferedImage im;
	Graphics2D imGraphics;
	JCheckBox traceBox;

	public MyDrawingPanel(JCheckBox traceBox)
	{
		listOfLivingThings = new Vector();
		this.traceBox = traceBox;
		imGraphics = null;
	}
//===============================================
	public void paintComponent(Graphics g1)
	{
		Graphics2D g;
		g = (Graphics2D)g1;

		Color savedColor;
		Toolkit tk;
		Dimension d;

		tk = Toolkit.getDefaultToolkit();
		d = tk.getScreenSize();

		try
		{
			super.paintComponent(g1);

			if(imGraphics == null)
			{
				im = new BufferedImage(d.width, d.height, BufferedImage.TYPE_INT_RGB);
				imGraphics = im.createGraphics();
			}

			//for(LivingThings thing : listOfLivingThings)
				if(!traceBox.isSelected())
				{
					savedColor = imGraphics.getColor();
					imGraphics.setColor(Color.BLACK);
					imGraphics.fillRect(0,0,im.getWidth(),im.getHeight());
					imGraphics.setColor(savedColor);
				}

			for(int n = 0; n < listOfLivingThings.size(); n++)
				listOfLivingThings.elementAt(n).draw(imGraphics);

			g.drawImage(im, 0,0,this);
		}
		catch(NullPointerException npe)
		{
			System.out.println("paint null");
			npe.printStackTrace();
			System.exit(1);
		}
	}
}