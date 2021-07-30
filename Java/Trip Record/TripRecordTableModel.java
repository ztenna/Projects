// Zachary Tennant

import javax.swing.table.*;
import java.util.*;
import java.io.*;
import java.text.*;

class TripRecordTableModel extends AbstractTableModel
{
	TripRecordList tripRecordList;
	DefaultTableColumnModel colModel;
	TableColumn col;

	Date dateOfTrip;
	String patientName;
	String servCode;
	int initialMile;
	int retMile;
	float billRate;
	String comment;

	TripRecordTableModel()
	{
		tripRecordList = new TripRecordList();

		colModel = new DefaultTableColumnModel();

		col = new TableColumn(0);
		col.setPreferredWidth(14);
		col.setMinWidth(11);
		col.setHeaderValue("Date");
		colModel.addColumn(col);

		col = new TableColumn(1);
		col.setPreferredWidth(15);
		col.setMinWidth(10);
		col.setHeaderValue("Patient Name");
		colModel.addColumn(col);

		col = new TableColumn(2);
		col.setPreferredWidth(30);
		col.setMinWidth(25);
		col.setHeaderValue("Service Code");
		colModel.addColumn(col);

		col = new TableColumn(3);
		col.setPreferredWidth(15);
		col.setMinWidth(6);
		col.setHeaderValue("Initial Mileage");
		colModel.addColumn(col);

		col = new TableColumn(4);
		col.setPreferredWidth(15);
		col.setMinWidth(6);
		col.setHeaderValue("Return Mileage");
		colModel.addColumn(col);

		col = new TableColumn(5);
		col.setPreferredWidth(9);
		col.setMinWidth(6);
		col.setHeaderValue("Bill Rate");
		colModel.addColumn(col);

		col = new TableColumn(6);
		col.setPreferredWidth(30);
		col.setMinWidth(10);
		col.setHeaderValue("Comment");
		colModel.addColumn(col);
	}
//===============================================

// convenience method (wrapper)
	TripRecord elementAt(int index)
	{
		return tripRecordList.elementAt(index);
	}
//===============================================
	void store(DataOutputStream dos)
	{
		tripRecordList.store(dos);
	}
//===============================================
	public int getColumnCount()
	{
		return TripRecord.NUM_FIELDS;
	}
//===============================================
	public int getRowCount()
	{
		return tripRecordList.size();
	}
//===============================================
	public Object getValueAt(int row, int col)
	{
		if(col == 0)
		{
			DateFormat df = new SimpleDateFormat("MM-dd-yyyy");
			if(tripRecordList.elementAt(row).dateOfTrip == null)
				System.out.println("tripRecordList in getValueAt is null");
			return df.format(tripRecordList.elementAt(row).dateOfTrip);
		}
		else if(col == 1)
			return tripRecordList.elementAt(row).patientName;
		else if(col == 2)
			return tripRecordList.elementAt(row).servCode;
		else if(col == 3)
			return new Integer(tripRecordList.elementAt(row).initialMile);
		else if(col == 4)
			return new Integer(tripRecordList.elementAt(row).retMile);
		else if(col == 5)
			return new Float(tripRecordList.elementAt(row).billRate);
		else
			return tripRecordList.elementAt(row).comment;
	}
}