// Zachary Tennant

import javax.swing.*;
import java.awt.*;
import java.awt.Graphics;
import java.awt.Graphics2D;
import java.awt.print.*;

class TextFilePrinter implements Printable
{
	DefaultListModel<String> list;

	public TextFilePrinter(DefaultListModel list)
	{
		this.list = list;
	}

//===============================================

	void printIt(Printable printable)
	{
		PrinterJob pj;
		PageFormat pf;
		PageFormat defPage;
		int linesPerPage;

		pj = PrinterJob.getPrinterJob();

		defPage = pj.defaultPage();
		pf = pj.pageDialog(defPage);

		pj.setPrintable(printable, pf);

		if(pf != defPage)
		{
			try
			{
				if(pj.printDialog())
					pj.print();
			}
			catch(PrinterException pe)
			{
				System.out.println("Printer Exception");
				//pe.printStackTrace();
			}
		}
	} // end of printIt(...)

//===============================================

	public int print(Graphics g1, PageFormat pf, int pageIndex)
	{
		Graphics2D g2;
		int maxLineLength;
		String lineToPrint;
		int linesPerPage;
		int size;
		int numPages;
		int yCoordinate;

		g2 = (Graphics2D)g1;
		g2.translate(pf.getImageableX(), pf.getImageableY());

		g2.setFont(new Font("Monospaced", Font.PLAIN, 14));
		g2.setPaint(Color.BLUE);

		//System.out.println("Imageable Height: " + pf.getImageableHeight());
		linesPerPage = (int)(pf.getImageableHeight() / 14);
		//System.out.println("Lines per page: " + linesPerPage);

		size = list.getSize();
		//System.out.println("Size: " + size);
		numPages = size / linesPerPage;
		//System.out.println("Pages: " + numPages);

		//System.out.println("Index: " + pageIndex);

		if(pageIndex > numPages)
			return Printable.NO_SUCH_PAGE;

		yCoordinate = 14;
		int counter = 1;

		for(int listIndex = pageIndex * linesPerPage; listIndex < size; listIndex++)
		{
			//System.out.println("listIndex: " + listIndex + " counter: " + counter);

			lineToPrint = list.getElementAt(listIndex);
			g2.drawString(counter + ": " + lineToPrint, 14, yCoordinate);
			counter++;

			yCoordinate = yCoordinate + 14;
		}

		return Printable.PAGE_EXISTS;
	} // end of print(...)
} // end of class TextFilePrinter