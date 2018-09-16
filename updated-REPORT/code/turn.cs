
        public static void initialize(Int32 time)
        {
                int messungen = 100;
                //Gateway.ConnectionModule.ConnectionCTRLModule.getInstance().setCTRLForRobot(0, 0.0, 100.0, 0.0, 8.0, 0.0, 0.0, 3.0);
                receive initial = new receive();    //Create a new instance of class Receive
                var tokenSource = new CancellationTokenSource();
                var token = tokenSource.Token;
                Init compare = new Init();
                string[] rfid_signals = new string[messungen];
                int currentRobot = 0;
                int[] RobotAssingment = new int[] { 0, 1, 3 };
                Gateway.CTRLModule.getInstance().camCtrl.processFrameAndUpdateGUI();
                RobotDiscription[] RobotArray = new RobotDiscription[] { Gateway.CTRLModule.getInstance().camCtrl.RobotA, Gateway.CTRLModule.getInstance().camCtrl.RobotB, 
                Gateway.CTRLModule.getInstance().camCtrl.RobotC };
                double[][] velocity1 = new double[RobotArray.Length][];
                string[,] Signals = new string[messungen,8];
                velocity1[currentRobot] = new double[] { 0, 0 }; //Starts the Robot
                Gateway.CTRLModule.getInstance().getRobotRemoteCTRL(RobotAssingment[currentRobot]).forward(velocity1[currentRobot], 0, 0, 0); //Sends velocity to Robot
                //Opening a new Task which works in the background to read data from RFID Antenna
                Task t = Task.Factory.StartNew(() => initial.reading(token));
                Thread.Sleep(1000);
                for (int i = 0; i < 8; i++)     //9 Because of 8 measurements every 45 degree
                {                   
                    for (int j = 0; j < messungen; j++)    //in this for loop we find all the reachable TAGs 
                    {
                        Signals[j, i] = initial.availablearray[0];
                        Thread.Sleep(100);
                    }
                    velocity1[currentRobot] = new double[] { 100, -100 }; //Starts the Robot
                    Gateway.CTRLModule.getInstance().getRobotRemoteCTRL(RobotAssingment[currentRobot]).forward(velocity1[currentRobot], 0, 0, 0); //Sends velocity to Robot
                    Thread.Sleep(time); //time the robot needs for a 45 degree turn
                    velocity1[currentRobot] = new double[] { 0, 0 };  //Stops the Robot
                    Gateway.CTRLModule.getInstance().getRobotRemoteCTRL(RobotAssingment[currentRobot]).forward(velocity1[currentRobot], 0, 0, 0); //Sends velocity to Robot
                }

                tokenSource.Cancel();   //close the reading Thread
                try
                {
                    Task.WaitAll(t);
                }
                catch (AggregateException e)
                {
                }
                finally
                {
                    tokenSource.Dispose();
                }
                    Console.WriteLine("END\r\r");
        
            Array[] Liste = new Array[8];   //List of arrays each array in the array contains the data of a special position (45°, 90°,...)
            string[,] Init_array = new string[8, 14];   //Array filled with signal strengthes and ID of every degree position
            string temp_ID="begin", temp_RSSI;  //Substrings of Data
            int counter;    //Counter for the row in the Init_Array
            Console.WriteLine("");