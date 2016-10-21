
//////////////////////////////////////
//
// This file contains classes that are helfpul in some way.
//
// Created: Chip Audette, Oct 2013 - Dec 2014
//
/////////////////////////////////////

class DataPacket_ADS1299 {
  int sampleIndex;
  int[] values;
  int[] auxValues;

  //constructor, give it "nValues", which should match the number of values in the
  //data payload in each data packet from the Arduino.  This is likely to be at least
  //the number of EEG channels in the OpenBCI system (ie, 8 channels if a single OpenBCI
  //board) plus whatever auxiliary data the Arduino is sending. 
  DataPacket_ADS1299(int nValues, int nAuxValues) {
    values = new int[nValues];
    auxValues = new int[nAuxValues];
  }
  int printToConsole() {
    print("printToConsole: DataPacket = ");
    print(sampleIndex);
    for (int i=0; i < values.length; i++) {
      print(", " + values[i]);
    }
    for (int i=0; i < auxValues.length; i++) {
      print(", " + auxValues[i]);
    }
    println();
    return 0;
  }
  
  int copyTo(DataPacket_ADS1299 target) { return copyTo(target, 0, 0); }
  int copyTo(DataPacket_ADS1299 target, int target_startInd_values, int target_startInd_aux) {
    target.sampleIndex = sampleIndex;
    return copyValuesAndAuxTo(target, target_startInd_values, target_startInd_aux);
  }
  int copyValuesAndAuxTo(DataPacket_ADS1299 target, int target_startInd_values, int target_startInd_aux) {
    int nvalues = values.length;
    for (int i=0; i < nvalues; i++) {
      target.values[target_startInd_values + i] = values[i];
    }
    nvalues = auxValues.length;
    for (int i=0; i < nvalues; i++) {
      target.auxValues[target_startInd_aux + i] = auxValues[i];
    }
    return 0;
  }
};

class DataStatus {
  public boolean is_railed;
  private int threshold_railed;
  public boolean is_railed_warn;
  private int threshold_railed_warn;
  
  DataStatus(int thresh_railed, int thresh_railed_warn) {
    is_railed = false;
    threshold_railed = thresh_railed;
    is_railed_warn = false;
    threshold_railed_warn = thresh_railed_warn;
  }
  public void update(int data_value) {
    is_railed = false;
    if (abs(data_value) >= threshold_railed) is_railed = true;
    is_railed_warn = false;
    if (abs(data_value) >= threshold_railed_warn) is_railed_warn = true;
  }
};
    

public class FilterConstants {
  public double[] a;
  public double[] b;
  public String name;
  public String short_name;
  FilterConstants(double[] b_given, double[] a_given, String name_given, String short_name_given) {
    b = new double[b_given.length];a = new double[b_given.length];
    for (int i=0; i<b.length;i++) { b[i] = b_given[i];}
    for (int i=0; i<a.length;i++) { a[i] = a_given[i];}
    name = name_given;
    short_name = short_name_given;
  }
}

public class DetectionData_FreqDomain {
  public float inband_uV = 0.0f;
  public float inband_freq_Hz = 0.0f;
  public float guard_uV = 0.0f;
  public float thresh_uV = 0.0f;
  public boolean isDetected = false;
  
  DetectionData_FreqDomain() {
  }
};