// Zachary Tennant

import java.util.*;
import java.io.*;
import java.text.*;

class TripRecord
{
    static Random r = new Random();

     Date dateOfTrip;
     String patientName;
     String servCode;
     int initialMile;
     int retMile;
     float billRate;
     String comment;
     String date;
     static final int NUM_FIELDS = 7;
//===============================================
    public TripRecord()
    {
    }
//===============================================
TripRecord(Date dateOfTrip, String patientName, String servCode, int initialMile, int retMile, float billRate, String comment)
{
    //r.setSeed(seed);

    this.dateOfTrip = dateOfTrip;
    this.patientName = patientName;
    this.servCode = servCode;
    this.initialMile = initialMile;
    this.retMile = retMile;
    this.billRate = billRate;
    this.comment = comment;
}
//===============================================
static TripRecord getRandom()
{
    TripRecord tr = new TripRecord(); //dateOfTrip, patientName, servCode, initialMile, retMile, billRate, comment);
    long cTime;
    long ranTime;
    long newTime;
    int n;
    int s;
    int c;

    tr.dateOfTrip = new Date();
    cTime = tr.dateOfTrip.getTime();
    ranTime = r.nextInt() % 315400000;
    newTime = cTime - ranTime;
    tr.dateOfTrip.setTime(newTime);
    tr.date = DateFormat.getDateInstance().format(tr.dateOfTrip);

    n = r.nextInt(5);
    if(n == 0)
        tr.patientName = "John Smith";
    else if(n == 1)
        tr.patientName = "Daniel Ross";
    else if(n == 2)
        tr.patientName = "Robert Turner";
    else if(n == 3)
        tr.patientName = "Kathy Stein";
    else if(n == 4)
        tr.patientName = "Sam Estel";

    s = r.nextInt(4);
    if(s == 0)
        tr.servCode = "A0428 Non-emergency transport";
    else if(s == 1)
        tr.servCode = "A0429 Emergency transport";
    else if(s == 2)
        tr.servCode = "A0427 Advanced life support";
    else if(s == 3)
        tr.servCode = "A0434 Specialty care transport";

    tr.initialMile = 120000 + r.nextInt(10001);

    tr.retMile = tr.initialMile + 20 + r.nextInt(11);

    tr.billRate = 100 + 1000 * r.nextFloat();

    c = r.nextInt(5);
    if(c == 0)
        tr.comment = "That dog was mean.";
    else if(c == 1)
        tr.comment = "We were sent to the wrong address.";
    else if(c == 2)
        tr.comment = "Got there just in time.";
    else if(c == 3)
        tr.comment = "The neighbor offered us cookies.";
    else if(c == 4)
        tr.comment = "That place was in the middle of nowhere.";

    return tr;
}
//===============================================
void dumpToConsole()
{
    System.out.printf("%11s %11s %31s %8d %8d %5.2f %35s %n",
                      date, patientName, servCode, initialMile, retMile, billRate, comment);
}
//===============================================
void store(DataOutputStream dos)
{
    try
    {
        dos.writeLong(dateOfTrip.getTime());
        dos.writeUTF(patientName);
        dos.writeUTF(servCode);
        dos.writeInt(initialMile);
        dos.writeInt(retMile);
        dos.writeFloat(billRate);
        dos.writeUTF(comment);
    }
    catch(IOException e)
    {
        System.out.println("record out error");
    }
}
//===============================================
public TripRecord(DataInputStream dis)
{
	long dateOfTripTime;
    try
    {
        dateOfTripTime = dis.readLong();
        dateOfTrip = new Date();
        dateOfTrip.setTime(dateOfTripTime);

        patientName = dis.readUTF();
        servCode = dis.readUTF();
        initialMile = dis.readInt();
        retMile = dis.readInt();
        billRate = dis.readFloat();
        comment = dis.readUTF();
    }
    catch(IOException e)
    {
        System.out.println("record in error");
    }
}
} // end of class