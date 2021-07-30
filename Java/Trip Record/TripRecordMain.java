// Zachary Tennant

import java.util.*;
import java.text.*;
import java.io.*;

public class TripRecordMain
{
	public static void main(String[] args)
	{
		Date dateOfTrip;
		String patientName;
		String servCode;
		int initialMile;
		int retMile;
		double billRate;
		String comment;

		DataInputStream dis;
		DataOutputStream dos;
		TripRecord tr;
		TripRecordList list;
		list = new TripRecordList();
		//dis = new DataInputStream(new FileInputStream("Junk.dat"));
		//list = new TripRecordList(dis);

		tr = TripRecord.getRandom();
		tr.dumpToConsole();

		try
		{
			for(int i = 0; i < 5; i++)
			{
				tr = TripRecord.getRandom();
				list.addElement(tr);
			}
			list.dump();
			dos = new DataOutputStream(new FileOutputStream("Junk.dat"));
			list.store(dos);
			dos.close();
			dis = new DataInputStream(new FileInputStream("Junk.dat"));
			list = new TripRecordList(dis);
			list.dump();
		}

		catch(FileNotFoundException e)
		{
			System.out.println("file error");
		}
		catch(IOException e)
		{
			System.out.println("io error");
		}
	}
} // end of class