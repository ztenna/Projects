// Zachary Tennant

import javax.swing.*;
import java.awt.*;

class RandColorGame extends JFrame
{

	JPanel board;
	SmartJPanel panel;

	Container cp;

//###############################################

	public RandColorGame()
	{
		board = new JPanel(new GridLayout(50,50));

		for(int n = 0; n < 2500; n++)
		{
			panel = new SmartJPanel();
			panel.addMouseListener(panel);
			board.add(panel);
		}

		cp = getContentPane();
		cp.add(board, BorderLayout.CENTER);

		setupFrame();
	} // end constructor

//###############################################

	void setupFrame()
	{
		Toolkit tk;
		Dimension d;

		tk = Toolkit.getDefaultToolkit();
		d = tk.getScreenSize();
		setSize(d.width/2, d.height/2);
		setLocation(d.width/2, d.height/2);

		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		setTitle("Random Color Game");

		setVisible(true);
	} // end setupFrame
} // end RandColorGame class