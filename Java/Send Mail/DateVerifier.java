// Zachary Tennant

import javax.swing.InputVerifier;
import java.awt.*;
import javax.swing.*;
import java.util.Date;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.text.ParseException;

class DateVerifier extends InputVerifier
{
	public DateVerifier()
	{
	} // end of constructor
//===============================================
	public boolean verify(JComponent c)
	{
		JTextField tf;
		long scrapDate;
		String inString;

		tf = (JTextField)c;
		inString = tf.getText().trim();

		if(inString.equals(""))
			return true;
		try
		{
			DateFormat df = new SimpleDateFormat("MM-dd-yyyy");
			scrapDate = df.parse(inString).getTime();
			return true;
		}
		catch(ParseException pe)
		{
			JOptionPane.showMessageDialog(tf,"Try a date of the form (MM-dd-yyyy)");
			return false;
		}
	} // end of verify(...)

} // end of DateVerifier class