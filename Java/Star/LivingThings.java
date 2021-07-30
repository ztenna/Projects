// Zachary Tennant

import java.awt.Graphics;
import javax.swing.JCheckBox;

abstract class LivingThings
{
	double currXPosition;
	double currYPosition;
	double currAngle;
	double xSpeed;
	double ySpeed;
	double angVel;
	double xAcceleration;
	double yAcceleration;
	double angAcc;

//###############################################
	void update(int deltaScaledMillis, long starBirth, int lifetime, JCheckBox deathSpiralBox)
	{
		updateLinearVelocity(deltaScaledMillis);
		updateAngularVelocity(deltaScaledMillis);
		updateCurrentPosition(deltaScaledMillis);
		updateCurrentAngle(deltaScaledMillis);
		updateVitality(starBirth, lifetime, deathSpiralBox);
	}
//===============================================
	void updateLinearVelocity(int deltaScaledMillis)
	{
		xSpeed = xSpeed + xAcceleration * deltaScaledMillis;
		ySpeed = ySpeed + yAcceleration * deltaScaledMillis;
	}
//===============================================
	void updateAngularVelocity(int deltaScaledMillis)
	{
		angVel = angVel + angAcc * deltaScaledMillis;
	}
//===============================================
	void updateCurrentPosition(int deltaScaledMillis)
	{
		currXPosition = (int)(currXPosition + xSpeed * deltaScaledMillis);
		currYPosition = (int)(currYPosition + ySpeed * deltaScaledMillis);
	}
//===============================================
	void updateCurrentAngle(int deltaScaledMillis)
	{
		currAngle = currAngle + angVel * deltaScaledMillis;
	}
//###############################################
	void reflectOffRightVerticalWall()
	{
		xSpeed = -Math.abs(xSpeed);
	}
//===============================================
	void reflectOffLeftVerticalWall()
	{
		xSpeed = Math.abs(xSpeed);
	}
//===============================================
	void reflectOffBottomHorizontalWall()
	{
		ySpeed = -Math.abs(ySpeed);
	}
//===============================================
	void reflectOffTopHorizontalWall()
	{
		ySpeed = Math.abs(ySpeed);
	}
//###############################################
	void bounceOffRightWall(double bounceFactor)
	{
		xSpeed = -xSpeed * bounceFactor;
	}
//===============================================
	void bounceOffLeftWall(double bounceFactor)
	{
		xSpeed = Math.abs(xSpeed) * bounceFactor;
	}
//===============================================
	void bounceOffBottomWall(double bounceFactor)
	{
		ySpeed = -ySpeed * bounceFactor;
	}
//===============================================
	void bounceOffTopWall(double bounceFactor)
	{
		ySpeed = Math.abs(ySpeed) * bounceFactor;
	}
//###############################################
	abstract void draw(Graphics g1);

	abstract void addMortalityListener(MortalityListener ml);

	abstract void updateVitality(long starBirth, int lifetime, JCheckBox deathSpiralBox);

	abstract void reflect(int panelWidth, int panelHeight);

	abstract void bounce(int panelWidth, int panelHeight, double bounceFactor);
}