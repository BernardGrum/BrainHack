﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSL;
using System.IO;
using System.Diagnostics;

namespace NIKBCI.ConsoleRecorder
{
    class NBTrainingData
    {
        public string What { get; set; }
        public float[] Data { get; set; }
    }

    class Program
    {
        static void RecordMain(string[] args)
        {
            // wait until an EEG stream shows up
            liblsl.StreamInfo[] results = liblsl.resolve_stream("type", "EEG");

            // open an inlet and print some interesting info about the stream (meta-data, etc.)
            liblsl.StreamInlet inlet = new liblsl.StreamInlet(results[0]);
            System.Console.Write(inlet.info().as_xml());

            string now = DateTime.Now.ToLongTimeString().Replace(":", "");
            StreamWriter SW = new StreamWriter("dump" + now + ".txt");
            // read samples
            float[] sample = new float[8];
            string action = "NONE";
            string[] actionsToTrain = { "LEFT", "RIGHT" };
            int stepsToRecord = 1000;
            int i = stepsToRecord;
            int num = 0;
            Random R = new Random();
            Stopwatch stw = new Stopwatch();
            stw.Start();
            while (!Console.KeyAvailable)
            {
                i--;
                num++;
                if (i <= 0)
                {
                    Console.Clear();
                    if (action == "NONE")
                    {
                        action = actionsToTrain[R.Next(actionsToTrain.Length)];
                    }
                    else
                    {
                        action = "NONE";
                    }
                    Console.WriteLine(action);
                    i = stepsToRecord + R.Next(0, 100);
                }
                inlet.pull_sample(sample);
                SW.Write(action);
                foreach (float f in sample)
                    SW.Write("\t{0}", f);
                SW.WriteLine();
            }
            stw.Stop();
            SW.Close();
            Console.WriteLine("YOOOO");
            Console.WriteLine("{0} samples in {1} milliseconds, {2} samples/sec", num, stw.ElapsedMilliseconds, 1000 * num / stw.ElapsedMilliseconds);
            System.Console.ReadLine();

        }

        static void SendMain()
        {
            System.Globalization.NumberFormatInfo NF = System.Globalization.CultureInfo.InvariantCulture.NumberFormat;

            liblsl.StreamInfo eegInfo = new liblsl.StreamInfo("NB_EEG", "EEG", 8, 100, liblsl.channel_format_t.cf_float32, "NB_ConsoleSender");
            liblsl.StreamOutlet eegOutlet = new liblsl.StreamOutlet(eegInfo);
            float[] eegArray = new float[8];

            liblsl.StreamInfo markerInfo = new liblsl.StreamInfo("NB_MARKER", "Markers", 1, 0, liblsl.channel_format_t.cf_string, "NB_ConsoleSender");
            liblsl.StreamOutlet markerOutlet = new liblsl.StreamOutlet(markerInfo);
            string[] markerArray = new string[1];

            // calib-begin , calib-end, left, right
            string fname = @"c:\Users\anon\Desktop\dump35959 AM.txt";
            bool calibStarted = false;
            StreamReader SR = new StreamReader(fname);
            Console.WriteLine("CALIB STARTED");
            while (!SR.EndOfStream)
            {
                string line = SR.ReadLine();
                string[] fields = line.Split('\t');
                if (fields[0] != "NONE" && !calibStarted)
                {
                    markerArray[0] = "calib-begin";
                    markerOutlet.push_sample(markerArray);
                    calibStarted = true;
                }
                for (int i = 0; i < eegArray.Length; i++)
                {
                    eegArray[i] = float.Parse(fields[i + 1], NF);
                }
                eegOutlet.push_sample(eegArray);
                if (fields[0] != "NONE")
                {
                    markerArray[0] = fields[0];
                    markerOutlet.push_sample(markerArray);
                }
                //System.Threading.Thread.Sleep(2);
            }
            markerArray[0] = "calib-end";
            markerOutlet.push_sample(markerArray);
            SR.Close();
            Console.WriteLine("CALIB ENDED");
            SR = new StreamReader(fname);
            List<NBTrainingData> lista = new List<NBTrainingData>();
            while (!SR.EndOfStream)
            {
                float[] eegArray2 = new float[8];
                string line = SR.ReadLine();
                string[] fields = line.Split('\t');
                for (int i = 0; i < eegArray.Length; i++)
                {
                    eegArray2[i] = float.Parse(fields[i + 1], NF);
                }
                eegOutlet.push_sample(eegArray);
                lista.Add(new NBTrainingData() { What = fields[0], Data = eegArray2 });
                //System.Threading.Thread.Sleep(2);
            }
            Console.WriteLine("TOTALS {0}, LEFT {1} RIGHT {2}", lista.Count, lista.Count(x => x.What == "LEFT"), lista.Count(x => x.What == "RIGHT"));

            float[] leftAvg = new float[8];
            float[] rightAvg = new float[8];
            for (int i = 0; i < 8; i++)
            {
                leftAvg[i] = lista.Where(x => x.What == "LEFT").Average(x => x.Data[i]);
                rightAvg[i] = lista.Where(x => x.What == "RIGHT").Average(x => x.Data[i]);
            }

            List<List<float[]>> leftSeries = new List<List<float[]>>();
            int skipNums=50;
            for (int i = 0; i < lista.Count; i++)
            {
                List<float[]> newSeries = new List<float[]>();

                // search first left
                for ( ; i < lista.Count && lista[i].What != "LEFT"; i++) ;
                if (i == lista.Count) break;

                //skip
                i += skipNums;
                for (; i < lista.Count && lista[i].What == "LEFT"; i++)
                {
                    newSeries.Add(lista[i].Data);
                }
                Console.WriteLine("NEW SERIES");
                leftSeries.Add(newSeries.Take(newSeries.Count - skipNums).ToList());
            }

            for (int i = 0; i < 8; i++)
            {
                foreach (var aktSeries in leftSeries)
                {
                    foreach (var aktData in  aktSeries)
                    {
                        File.AppendAllText("c:\\dump.tsv", "\t"+aktData[i].ToString(NF));
                    }
                    File.AppendAllText("c:\\dump.tsv", "\t-500");
                }
                File.AppendAllText("c:\\dump.tsv", "\r\n");
            }
            Console.WriteLine("FILE OK");
            int good_left = 0, bad_left = 0, good_right=0, bad_right=0;
            for (int i = 0; i < lista.Count; i++)
            {
                if (lista[i].What != "NONE")
                {
                    double sqDiff_left = 0, sqDiff_right = 0;
                    for (int j = 0; j < 8; j++)
                    {
                        sqDiff_left += Math.Sqrt((lista[i].Data[j] - leftAvg[j]) * (lista[i].Data[j] - leftAvg[j]));
                        sqDiff_right += Math.Sqrt((lista[i].Data[j] - rightAvg[j]) * (lista[i].Data[j] - rightAvg[j]));
                    }
                    string decision = sqDiff_left > sqDiff_right ? "RIGHT" : "LEFT";
                    if (lista[i].What == "LEFT")
                    {
                        if (decision == lista[i].What)
                        {
                            good_left++;
                        }
                        else
                        {
                            bad_left++;
                        }
                    }
                    else if (lista[i].What == "RIGHT")
                    {
                        if (decision == lista[i].What)
                        {
                            good_right++;
                        }
                        else
                        {
                            bad_right++;
                        }
                    }
                }
            }
            Console.WriteLine("LEFT: GOOD {0}, BAD {1}, RIGHT: GOOD {2}, BAD {3}, ", good_left, bad_left, good_right, bad_right);
            
        }

        static void Main()
        {
            SendMain();
            Console.WriteLine("OK");
            Console.ReadLine();
        }

    }
}
