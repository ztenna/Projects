// Zachary Tennant

import javax.swing.*;
import java.io.*;

class Handler implements Runnable
{
	Talker t;

	public Handler(Talker t)
	{
		this.t = t;
	}

//###############################################

	public void run()
	{
		String msg;
		ExtractFiles thread;

		while(true)
		{
			try
			{
				msg = t.receive();

				if(msg.equals("TERMINATE"))
					System.exit(1);

				else if(msg.equals("SEND_FILE_NAMES"))
				{
					System.out.println("About to create MyThread");
					thread = new ExtractFiles(t);
					thread.start();
				}

				else
					SwingUtilities.invokeLater(new Runnable()
					{
						public void run()
						{
							JOptionPane.showMessageDialog(null, "Having fun?");
						} // end run
					} // end SwingUtilities
					);
			} // end try
			catch(IOException ioe)
			{
				JOptionPane.showMessageDialog(null, "No message was recieved");
			}
		} // end while
	} // end run
} // end Handler class