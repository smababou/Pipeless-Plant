using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using MULTIFORM_PCS.ControlModules.SchedulingModule;
using MULTIFORM_PCS.ControlModules.FeedForwadModule;
using MULTIFORM_PCS.ControlModules.RoutingModule.PathAndVelocityPlanning.DataTypes;
using MULTIFORM_PCS.ControlModules.CameraModule.CameraForm;
using MULTIFORM_PCS.ControlModules.CameraControl.CameraControlClass;
using System.Windows.Threading;
using System.Diagnostics; // Process
using System.Globalization;
using Emgu.CV.WPF;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MULTIFORM_PCS.ControlModules.RFID
{
    public class receive
    {
        public string[] availablearray=new string[1];
        public void connect()
        {
            try
            {
                Console.WriteLine("Connecting");
                TcpClient tcpClient = new TcpClient("192.168.0.100", 8883);
                if (tcpClient.Connected)
                {
                    Console.WriteLine("Connected to server");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Connection Failed");
            }           
        }

        public void reading(CancellationToken ct)
        {
            if (ct.IsCancellationRequested == true)
            {
                ct.ThrowIfCancellationRequested();
            }

            Console.WriteLine("Connecting");
            TcpClient tcpClient = new TcpClient("192.168.0.100", 8883);

            if (tcpClient.Connected)
            {
                Console.WriteLine("Connected to server");
            }

            using (StreamReader STR = new StreamReader(tcpClient.GetStream()))
            {
                string recieve;
                char[] trash = new char[16];
                char[] UID = new char[3];
                char[] RSSI = new char[3];
                long milliseconds, seconds, minutes;
                string UID_, RSSI_, RSSI___;               
                string[] array = new string[1];
                
                List<string> RSSI__;
                int UID_DEC=0;
                int RSSI_int = 0;
                
                    while ((recieve = STR.ReadLine()) != null && !ct.IsCancellationRequested)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            try
                            {
                                ct.ThrowIfCancellationRequested();
                            }
                            catch (AggregateException e)
                            {
                            }
                        }

                        if (recieve.Contains("+"))
                        {
                        
                        List<string> Worte = recieve.Split(new string[] { "OK", "<\\r>", "\n", "", "SCAN:+UID=", "+RSSI=" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        string Wort = string.Join("", Worte.ToArray());

                        using (StringReader sr = new StringReader(Wort))
                        {
                            sr.Read(trash, 0, 13);
                            sr.Read(UID, 0, 3);
                            UID_ = new string(UID);
                            sr.Read(trash, 0, 1);
                            sr.Read(RSSI, 0, 1);
                            RSSI_ = new string(RSSI);
                            try
                            {
                                UID_DEC = Int32.Parse(UID_, System.Globalization.NumberStyles.HexNumber);
                            }
                            catch (Exception e)
                            {
                            }
                        }

                        RSSI__ = RSSI_.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        RSSI___ = string.Join("", RSSI__.ToArray());
                        try
                        {
                            RSSI_int = Int32.Parse(RSSI___);
                        }
                        catch (Exception e)
                        {
                        }

                        milliseconds = DateTimeOffset.Now.Millisecond;
                        seconds = DateTimeOffset.Now.Second;
                        minutes = DateTimeOffset.Now.Minute;
                        array[0] = minutes + " " + seconds + " " + milliseconds + " " + UID_DEC + " " + RSSI_int;
                        //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\pythonfiles\\python_1robot\\RFID_Data.log", minutes + " " + seconds + " " + milliseconds + "\t UID: " + UID_ + " RSSI: " + RSSI___ + "\r");
                        //File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\pythonfiles\\python_1robot\\RFID_Data_original.log", hour + ":" + minutes + ":" + seconds + ":" + milliseconds + "\t" + recieve + "\r");
                        //Console.WriteLine(minutes + " " + seconds + " " + milliseconds + "\t" + " " + UID_ + "  " + RSSI___);
                        }
                            this.availablearray[0] = array[0];
                        }
                    }
                }
            
         
        
        public void disconnect()
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect("192.168.0.100", 8883);
            tcpClient.Close();

        }
}
}


    

