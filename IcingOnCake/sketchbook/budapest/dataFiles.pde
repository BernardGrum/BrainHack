import java.util.*;
import oscP5.*;

////////////////////////////////////////////////////////////
// Class: OutputFile_rawtxt
// Purpose: handle file creation and writing for the text log file
// Created: Chip Audette  May 2, 2014
//
//write data to a text file
public class OutputFile_rawtxt {
  PrintWriter output;
  String fname;
  private int rowsWritten;

  OutputFile_rawtxt(float fs_Hz) {
    //build up the file name
    fname = "SavedData"+System.getProperty("file.separator")+"OpenBCI-RAW-";

    //add year month day to the file name
    fname = fname + year() + "-";
    if (month() < 10) fname=fname+"0";
    fname = fname + month() + "-";
    if (day() < 10) fname = fname + "0";
    fname = fname + day(); 

    //add hour minute sec to the file name
    fname = fname + "_";
    if (hour() < 10) fname = fname + "0";
    fname = fname + hour() + "-";
    if (minute() < 10) fname = fname + "0";
    fname = fname + minute() + "-";
    if (second() < 10) fname = fname + "0";
    fname = fname + second();

    //add the extension
    fname = fname + ".txt";

    //open the file
    output = createWriter(fname);

    //add the header
    writeHeader(fs_Hz);
    
    //init the counter
    rowsWritten = 0;
  }

  //variation on constructor to have custom name
  OutputFile_rawtxt(float fs_Hz, String _fileName) {
    fname = _fileName;
    output = createWriter(fname);        //open the file
    writeHeader(fs_Hz);    //add the header
    rowsWritten = 0;    //init the counter
  }

  public void writeHeader(float fs_Hz) {
    synchronized (output) {
      output.println("%OpenBCI Raw EEG Data");
      output.println("%Sample Rate = " + fs_Hz + " Hz");
      output.println("%First Column = unix timestamp");
      output.println("%Other Columns = EEG data in microvolts followed by Accel Data (in G) interleaved with Aux Data");
      output.flush();
    }
  }

  public void writeRawData_dataPacket(DataPacket_ADS1299 data, float scale_to_uV, float scale_for_aux) {
    if (output == null) return;
    
    synchronized (output) {
      output.print(System.currentTimeMillis());
      writeValues(data.values,scale_to_uV);
      writeValues(data.auxValues,scale_for_aux);
      output.println(); rowsWritten++;
      output.flush();
    }
  }
  
  public void writeRawData_OSC(OscMessage data) {
    if (output == null) return;
    /*
    println("### received an osc message.");
    println("### addrpattern\t"+data.addrPattern());
    println("### typetag\t"+data.typetag());
    println("### address\t"+data.address());
    println("### netAddress\t"+data.netAddress());
    /**/
    
    List<String> values = new ArrayList<String>();
    // data.typetag() outputs a list of args as a string "sf" means one string + one float
    if (data.addrPattern().startsWith("/videostart/")) {
      if (!data.typetag().isEmpty()) {
        println("OutputFile_rawtxt: writeRawData_OSC received videostart argument with strange args, ignoring ...");
        return;
      }
      String[] addrPatternArr = data.addrPattern().split("/", 3);
      
      values.add(addrPatternArr[1]);
      values.add(addrPatternArr[2]);
    } else if (data.addrPattern().startsWith("/eyetrackr")) {
      if (data.typetag().isEmpty()) {
        println("OutputFile_rawtxt: writeRawData_OSC received eyetrackr argument without args, ignoring ...");
        return;
      }
      values.add(data.addrPattern().split("/")[1]);
      
      char[] typeTagArr = data.typetag().toCharArray();
      char curType = typeTagArr[0];
      for (int i = 0; i < typeTagArr.length; i++, curType = typeTagArr[i]) {
        OscArgument curPayload = data.get(i);
        switch (curType) {
          case 's':
            values.add(curPayload.stringValue());
            break;
          case 'i':
            values.add(Integer.valueOf(curPayload.intValue()).toString());
            break;
          case 'f':
            values.add(String.format("%.3f", curPayload.floatValue()));
            break;
          case 'd':
            values.add(String.format("%.6f", curPayload.doubleValue()));
            break;
        }
      }
    }
    
    synchronized (output) {
      output.print("%!");
      output.print(System.currentTimeMillis());
      writeValues(values);
      output.println(); rowsWritten++;
      output.flush();
    }
  }
  
  private void writeValues(Collection<String> values) {
    for (String value : values) {
      output.print(", ");
      output.print(value.replaceAll("," , ""));
    }
  }
  
  private void writeValues(int[] values, float scale_fac) {          
    int nVal = values.length;
    for (int Ival = 0; Ival < nVal; Ival++) {
      output.print(", ");
      output.print(String.format("%.2f", scale_fac * float(values[Ival])));
    }
  }



  public void closeFile() {
    synchronized (output) {
      output.flush();
      output.close();
    }
  }

  public int getRowsWritten() {
    return rowsWritten;
  }
}


///////////////////////////////////////////////////////////////
// Class: Table_CSV
// Purpose: Extend the Table class to handle data files with comment lines
// Created: Chip Audette  May 2, 2014
//
// Usage: Only invoke this object when you want to read in a data
//    file in CSV format.  Read it in at the time of creation via
//    
//    String fname = "myfile.csv";
//    TableCSV myTable = new TableCSV(fname);
//
//import java.io.*; 
//import processing.core.PApplet;
class Table_CSV extends Table {
  Table_CSV(String fname) throws IOException {
    init();
    readCSV(PApplet.createReader(createInput(fname)));
  }

  //this function is nearly completely copied from parseBasic from Table.java
  void readCSV(BufferedReader reader) throws IOException {
    boolean header=false;  //added by Chip, May 2, 2014;
    boolean tsv = false;  //added by Chip, May 2, 2014;

    String line = null;
    int row = 0;
    if (rowCount == 0) {
      setRowCount(10);
    }
    //int prev = 0;  //-1;
    try {
      while ( (line = reader.readLine ()) != null) {
        //added by Chip, May 2, 2014 to ignore lines that are comments
        if (line.charAt(0) == '%') {
          //println("Table_CSV: readCSV: ignoring commented line...");
          continue;
        }

        if (row == getRowCount()) {
          setRowCount(row << 1);
        }
        if (row == 0 && header) {
          setColumnTitles(tsv ? PApplet.split(line, '\t') : splitLineCSV(line, reader));
          header = false;
        } 
        else {
          setRow(row, tsv ? PApplet.split(line, '\t') : splitLineCSV(line, reader));
          row++;
        }
      }
    } 
    catch (Exception e) {
      throw new RuntimeException("Error reading table on line " + row, e);
    }
    // shorten or lengthen based on what's left
    if (row != getRowCount()) {
      setRowCount(row);
    }
  }
}