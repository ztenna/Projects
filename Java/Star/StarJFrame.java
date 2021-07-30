// Zachary Tennant

import java.awt.event.*;
import java.util.Vector;
import java.awt.*;
import javax.swing.*;
import javax.swing.JCheckBox;

public class StarJFrame
{
//===============================================
	public static void main(String[] args)
	{
		new MyMainFrame();

	} // end of main

} // end of Star class

//###############################################
class MyMainFrame extends JFrame
					implements ActionListener, MortalityListener
{
	Vector<LivingThings> listOfLivingThings;

	MyDrawingPanel myDrawingPanel;
	JPanel controlPanel;
	JPanel buttonPanel;
	JPanel gravityPanel;
	JPanel bouncePanel;
	JPanel animSpeedPanel;
	JPanel lifetimePanel;

	JLabel bounceLabel;
	JLabel animSpeedLabel;
	JLabel lifetimeLabel;

	JButton addButton;
	JButton addMultButton;
	JButton clearButton;
	Graphics g1;

	JCheckBox gravityBox;
	JCheckBox traceBox;
	JCheckBox deathSpiralBox;

	JSlider gravitySlider;
	JSlider bounceSlider;
	JSlider animSpeedSlider;
	JSlider lifetimeSlider;

	Container cp;

	long currTime;
	long lastUpdate;
	int deltaTime;
	static int deltaScaledMillis;

	long starBirth;

	int panelWidth;
	int panelHeight;

	Timer timer;
//===============================================
	MyMainFrame()
	{
// panels
		controlPanel = new JPanel(new GridLayout(4,1));
		//controlPanelTwo = new JPanel(new GridLayout(3,1));
		buttonPanel = new JPanel(new GridLayout(1,3));
		gravityPanel = new JPanel(new GridLayout(3,1));
		bouncePanel = new JPanel(new GridLayout(2,1));
		animSpeedPanel = new JPanel(new GridLayout(2,1));
		lifetimePanel = new JPanel(new GridLayout(3,1));

// addButton
		addButton = new JButton("Add");
		addButton.setActionCommand("ADD");
		addButton.setToolTipText("Add a new star");
		addButton.addActionListener(this);
		buttonPanel.add(addButton);

// addMultiButton
		addMultButton = new JButton("Add Multiple");
		addMultButton.setActionCommand("ADD_MULT");
		addMultButton.setToolTipText("Add multiple stars");
		addMultButton.addActionListener(this);
		buttonPanel.add(addMultButton);

// clearButton
		clearButton = new JButton("Clear");
		clearButton.setActionCommand("CLEAR");
		clearButton.setToolTipText("Clear the panel");
		clearButton.addActionListener(this);
		buttonPanel.add(clearButton);

// gravityBox
		gravityBox = new JCheckBox("Gravity");
		gravityBox.setActionCommand("GRAVITY");
		gravityBox.setToolTipText("Produce gravity");
		gravityBox.addActionListener(this);
		gravityPanel.add(gravityBox);

// gravitySlider
		gravitySlider = new JSlider(-10,10);
		gravitySlider.setMajorTickSpacing(5);
		gravitySlider.setPaintTicks(true);
		gravitySlider.setPaintLabels(true);
		gravityPanel.add(gravitySlider);

// traceBox
		traceBox = new JCheckBox("Trace");
		traceBox.setToolTipText("Leaves a trail from star");
		gravityPanel.add(traceBox);

// myDrawingPanel
		myDrawingPanel = new MyDrawingPanel(traceBox);

// bounceLabel
		bounceLabel = new JLabel("Bounce");
		bouncePanel.add(bounceLabel);

// bounceSlider
		bounceSlider = new JSlider(0,10,0);
		bounceSlider.setMajorTickSpacing(2);
		bounceSlider.setPaintTicks(true);
		bounceSlider.setPaintLabels(true);
		bouncePanel.add(bounceSlider);

// animSpeedLabel
		animSpeedLabel = new JLabel("Animation Speed");
		animSpeedPanel.add(animSpeedLabel);

// animSpeedSlider
		animSpeedSlider = new JSlider(0,10,1);
		animSpeedSlider.setMajorTickSpacing(2);
		animSpeedSlider.setPaintTicks(true);
		animSpeedSlider.setPaintLabels(true);
		animSpeedPanel.add(animSpeedSlider);

// lifetimeLabel
		lifetimeLabel = new JLabel("Lifetime of Star");
		lifetimePanel.add(lifetimeLabel);

// lifetimeSlider
		lifetimeSlider = new JSlider(0, 30000);
		lifetimeSlider.setMajorTickSpacing(10000);
		lifetimeSlider.setMinorTickSpacing(5000);
		lifetimeSlider.setPaintTicks(true);
		lifetimeSlider.setPaintLabels(true);
		lifetimePanel.add(lifetimeSlider);

		deathSpiralBox = new JCheckBox("Death Spiral");
		deathSpiralBox.setToolTipText("Before star dies it goes into spiral");
		lifetimePanel.add(deathSpiralBox);

// controlPanel
		//controlPanelTwo.add(buttonPanel);
		controlPanel.add(gravityPanel);
		controlPanel.add(bouncePanel);
		controlPanel.add(animSpeedPanel);
		controlPanel.add(lifetimePanel);

// timer
		timer = new Timer(20, this);
		timer.setActionCommand("TIMER");

		cp = getContentPane();
		cp.add(buttonPanel, BorderLayout.NORTH);
		cp.add(controlPanel, BorderLayout.WEST);
		cp.add(myDrawingPanel, BorderLayout.CENTER);

		timer.start();

		setupMainFrame();
	}
//===============================================
	public void actionPerformed(ActionEvent e)
	{
		StarThing star;
// add a star
		if(e.getActionCommand().equals("ADD"))
		{
			starBirth = System.currentTimeMillis();
			star = StarThing.getRandomInstance(myDrawingPanel.getWidth(), myDrawingPanel.getHeight());
			star.addMortalityListener(this);
			myDrawingPanel.listOfLivingThings.addElement(star);
		}
// add multiple stars
		else if(e.getActionCommand().equals("ADD_MULT"))
		{
			for(int n = 0; n < 15; n++)
			{
				starBirth = System.currentTimeMillis();
				star = StarThing.getRandomInstance(myDrawingPanel.getWidth(), myDrawingPanel.getHeight());
				star.addMortalityListener(this);
				myDrawingPanel.listOfLivingThings.addElement(star);
			}
		}
// clear panel
		else if(e.getActionCommand().equals("CLEAR"))
		{
			myDrawingPanel.listOfLivingThings.clear();
			myDrawingPanel.repaint();
		}
// gravity
		else if(e.getActionCommand().equals("GRAVITY"))
		{
			if(gravityBox.isSelected() == true)
			{
				for(int n = 0; n < myDrawingPanel.listOfLivingThings.size(); n++)
					myDrawingPanel.listOfLivingThings.elementAt(n).yAcceleration = gravitySlider.getValue() * 0.5;
			}
			else
			{
				for(int n = 0; n < myDrawingPanel.listOfLivingThings.size(); n++)
					myDrawingPanel.listOfLivingThings.elementAt(n).yAcceleration = 0.0;
			}
		}
// timer
		else if(e.getActionCommand().equals("TIMER"))
		{
			currTime = System.currentTimeMillis();
			deltaTime = (int)(currTime - lastUpdate);
			deltaScaledMillis = (int)(animSpeedSlider.getValue() / 10.0 * deltaTime);

			for(int n = 0; n < myDrawingPanel.listOfLivingThings.size(); n++)
			{
				myDrawingPanel.listOfLivingThings.elementAt(n).
					update(deltaScaledMillis, starBirth, lifetimeSlider.getValue(), deathSpiralBox);
			}
			for(int n = 0; n < myDrawingPanel.listOfLivingThings.size(); n++)
			{
		// reflect
				if(bounceSlider.getValue() == 0)
					myDrawingPanel.listOfLivingThings.elementAt(n).
						reflect(myDrawingPanel.getWidth(), myDrawingPanel.getHeight());
		// bounce
				else
					myDrawingPanel.listOfLivingThings.elementAt(n).
						bounce(myDrawingPanel.getWidth(), myDrawingPanel.getHeight(), bounceSlider.getValue() * 0.25);
			}

			myDrawingPanel.repaint();

			lastUpdate = System.currentTimeMillis();
		}
	}
//===============================================
	public void mortalityChanged(MortalityEvent me)
	{
		int indexToDelete;
		indexToDelete = myDrawingPanel.listOfLivingThings.indexOf(me.mortal);
		myDrawingPanel.listOfLivingThings.removeElementAt(indexToDelete);
	}
//===============================================
	void setupMainFrame()
		{
			Toolkit tk;
			Dimension d;

			tk = Toolkit.getDefaultToolkit();
			d = tk.getScreenSize();
			setSize(d.width/2, d.height/2);
			setLocation(d.width/2, d.height/2);

			setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

			setTitle("Star");

			setVisible(true);
	} // end of setupMainFrame()
}