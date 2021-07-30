// Zachary Tennant

import javax.swing.*;
import java.io.*;

class TripRecordList extends DefaultListModel<TripRecord>
								implements TripManager
{
	public TripRecordList(DataInputStream dis)
	{
		try
		{
			int numRecords;
			numRecords = dis.readInt();
			for(int i = 0; i < numRecords; i++)
				addElement(new TripRecord(dis));
		}
		catch(IOException e)
		{
			System.out.println("List in error");
		}
	}
//===============================================
	public TripRecordList()
	{
	}
//===============================================
	void store(DataOutputStream dos)
	{
		try
		{
			dos.writeInt(size());
			for(int i = 0; i < size(); i++)
				elementAt(i).store(dos);
		}
		catch(IOException e)
		{
			System.out.println("List out error");
		}
	}
//===============================================
	void dump()
	{
		for(int n = 0; n < size(); n++)
			elementAt(n).dumpToConsole();
	}
//===============================================
	public void addTrip(TripRecord trip)
	{
		addElement(trip);
	}
//===============================================
	public void replaceTrip(int selectedIndex, TripRecord trip)
	{
		set(selectedIndex, trip);
	}
} // end of class