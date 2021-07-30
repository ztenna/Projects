// Zachary Tennant

import javax.swing.InputVerifier;
import java.awt.*;
import javax.swing.*;

class BillRateVerifier extends InputVerifier
{
	public BillRateVerifier()
	{
	} // end of constructor
//===============================================
	public boolean verify(JComponent c)
	{
		JTextField tf;
		float scrapFloat;
		String inString;

		tf = (JTextField)c;
		inString = tf.getText().trim();

		if(inString.equals(""))
			return true;
		try
		{
			scrapFloat = Float.parseFloat(inString);
			if(scrapFloat > 1200.00 || scrapFloat < 0.00)
				JOptionPane.showMessageDialog(tf,"Try a bill rate between 0.00 and 1200.00");
			return 0.00 <= scrapFloat && scrapFloat <= 1200.00;
		}
		catch(NumberFormatException nfe)
		{
			JOptionPane.showMessageDialog(tf,"Try a bill rate between 0.00 and 1200.00");
			return false;
		}
	} // end of verify(...)

} // end of MyGirthVerifier class