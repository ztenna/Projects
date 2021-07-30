// Zachary Tennant

import javax.swing.InputVerifier;
import java.awt.*;
import javax.swing.*;

class TimeVerifier extends InputVerifier
{
	public TimeVerifier()
	{
	} // end of constructor
//===============================================
	public boolean verify(JComponent c)
	{
		JTextField tf;
		int scrapInt;
		String inString;

		tf = (JTextField)c;
		inString = tf.getText().trim();

		if(inString.equals(""))
			return true;
		try
		{
			scrapInt = Integer.parseInt(inString);
			if(scrapInt > 3600 || scrapInt < 20)
				JOptionPane.showMessageDialog(tf,"Try a time between 20 and 3600");
			return 20 <= scrapInt && scrapInt <= 3600;
		}
		catch(NumberFormatException nfe)
		{
			JOptionPane.showMessageDialog(tf,"Try a time between 20 and 3600");
			return false;
		}
	} // end of verify(...)
} // end of TimeVerifier class