// Zachary Tennant

import java.util.Vector;
import java.util.Random;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.Polygon;
import java.awt.BasicStroke;
import java.awt.Color;
import javax.swing.JCheckBox;

class StarThing extends LivingThings
{
	static Random r = new Random();

	int numPoints;
	double innerRadius;
	double outerRadius;
	int stroke;
	int red;
	int green;
	int blue;

	Vector<MortalityListener> listenerList;

	public StarThing()
	{
		listenerList = new Vector();
	}
//===============================================
	void draw(Graphics g1)
	{
		Graphics2D g;
		g = (Graphics2D)g1;

		int x;
		int y;
		double theta;

		Polygon p = new Polygon();
		theta = currAngle;

		for(int k = 0; k < numPoints; k++)
		{
			x = (int)(currXPosition + Math.cos(theta) * innerRadius);
			y = (int)(currYPosition + Math.sin(theta) * innerRadius);
			p.addPoint(x,y);
			theta = theta + Math.PI / numPoints;

			x = (int)(currXPosition + Math.cos(theta) * outerRadius);
			y = (int)(currYPosition + Math.sin(theta) * outerRadius);
			p.addPoint(x,y);
			theta = theta + Math.PI / numPoints;
		}
		g.setStroke(new BasicStroke(stroke));
		g.setColor(new Color(red,green,blue));
		g.fillPolygon(p);
		g.drawPolygon(p);
	}
//===============================================
	static StarThing getRandomInstance(int panelWidth, int panelHeight)
	{
		//System.out.println("random");
		double randXPos;
		double randYPos;

		StarThing star = new StarThing();

		star.innerRadius = 10 + 5 * r.nextDouble();
		star.outerRadius = star.innerRadius + 20 + 15 * r.nextDouble();

		randXPos = panelWidth - (2 * star.outerRadius) + 1.0;
		randYPos = panelHeight - (2 * star.outerRadius) + 1.0;

		star.currXPosition = star.outerRadius + randXPos * r.nextDouble();
		star.currYPosition = star.outerRadius + randYPos * r.nextDouble();

		star.xSpeed = 5.5 * r.nextDouble();
		star.ySpeed = 5.5 * r.nextDouble();

		star.xAcceleration = 0.0;
		star.yAcceleration = 0.0;

		star.angVel = 5 * r.nextDouble();
		star.angAcc = 0.0;
		star.currAngle = 0.0;

		star.numPoints = 5 + r.nextInt(6);

		star.stroke = r.nextInt(6);
		star.red = r.nextInt(256);
		star.green = r.nextInt(256);
		star.blue = r.nextInt(256);

		return star;
	}
//===============================================
	void reflect(int panelWidth, int panelHeight)
	{
		if(currXPosition - outerRadius < 0)
			reflectOffLeftVerticalWall();

		if(currXPosition + outerRadius > panelWidth)
			reflectOffRightVerticalWall();

		if(currYPosition - outerRadius < 0)
			reflectOffTopHorizontalWall();

		if(currYPosition + outerRadius > panelHeight)
			reflectOffBottomHorizontalWall();
	}
//===============================================
	void bounce(int panelWidth, int panelHeight, double bounceFactor)
	{
		if(currXPosition - outerRadius < 0)
			bounceOffLeftWall(bounceFactor);

		if(currXPosition + outerRadius > panelWidth)
			bounceOffRightWall(bounceFactor);

		if(currYPosition - outerRadius < 0)
			bounceOffTopWall(bounceFactor);

		if(currYPosition + outerRadius > panelHeight)
			bounceOffBottomWall(bounceFactor);
	}
//===============================================
	void updateVitality(long starBirth, int lifetime, JCheckBox deathSpiralBox)
	{
		//System.out.println("updateVitality");
		long deathTime;
		long nearDeath;
		long now;
		MortalityEvent mortalityEvent;

		now = System.currentTimeMillis();
		deathTime = starBirth + lifetime;
		nearDeath = deathTime - 2000;


		if(deathSpiralBox.isSelected() && now >= nearDeath)
		{
			angAcc = 10.0;
		}
		if(now >= deathTime)
		{
			System.out.println("dead");
			mortalityEvent = new MortalityEvent(this, MortalityEvent.DEATH);

			//System.out.println("kill");
			//System.out.println(listenerList.size());
			//System.exit(1);
			for(MortalityListener ml : listenerList)
			{
				System.out.println("kill");
				ml.mortalityChanged(mortalityEvent);
			}
		}
	}
//===============================================
	void addMortalityListener(MortalityListener ml)
	{
		listenerList.addElement(ml);
	}
}