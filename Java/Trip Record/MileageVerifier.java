// Zachary Tennant

import javax.swing.InputVerifier;
import java.awt.*;
import javax.swing.*;

class MileageVerifier extends InputVerifier
{
	public MileageVerifier()
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
			if(scrapInt > 25000 || scrapInt < 0)
				JOptionPane.showMessageDialog(tf,"Try a mileage between 0 and 25000");
			return 0 < scrapInt && scrapInt <= 25000;
		}
		catch(NumberFormatException nfe)
		{
			JOptionPane.showMessageDialog(tf,"Try a mileage between 0 and 25000");
			return false;
		}
	} // end of verify(...)

} // end of MyGirthVerifier class