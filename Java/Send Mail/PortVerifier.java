// Zachary Tennant

import javax.swing.InputVerifier;
import java.awt.*;
import javax.swing.*;

class PortVerifier extends InputVerifier
{
	public PortVerifier()
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
			if(scrapInt > 65535 || scrapInt < 0)
				JOptionPane.showMessageDialog(tf,"Try a port between 0 and 65535");
			return 0 <= scrapInt && scrapInt <= 65535;
		}
		catch(NumberFormatException nfe)
		{
			JOptionPane.showMessageDialog(tf,"Try a port between 0 and 65535");
			return false;
		}
	} // end of verify(...)
} // end of PortVerifier class