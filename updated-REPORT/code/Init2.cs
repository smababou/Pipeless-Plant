
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

            for (int j = 0; j < 8; j++)//converting UID to the specific decimal numbers in lookup table
            {
                counter = 0;
                for (int i = 0; i < messungen; i++)
                {
                    try
                    {
                        temp_ID = Signals[i, j].Substring(Signals[i, j].Length - 6, 4);   //seperatiion of UID in the string
                        if (temp_ID == "2788")
                        {
                            temp_ID = "1";
                        }
                        if (temp_ID == "1414")
                        {
                            temp_ID = "2";
                        }
                        if (temp_ID == "3060")
                        {
                            temp_ID = "3";
                        }
                        if (temp_ID == "1673")
                        {
                            temp_ID = "4";
                        }
                        if (temp_ID == "1925" || temp_ID == "1025")
                        {
                            temp_ID = "5";
                        }
                        if (temp_ID == "1681")
                        {
                            temp_ID = "6";
                        }
                        if (temp_ID == "1146")
                        {
                            temp_ID = "7";
                        }
                        if (temp_ID == "2780")
                        {
                            temp_ID = "8";
                        }
                        if (temp_ID == "1933")
                        {
                            temp_ID = "9";
                        }
                    }
                    catch (AggregateException e)
                    {
                        Console.WriteLine("Array incomplete");
                    }
                    
                    temp_RSSI = Signals[i, j].Substring(Signals[i, j].Length - 1, 1);    //seperation of RSSI in the string
                    if (temp_ID != Init_array[j, 0] && temp_ID != Init_array[j, 1] && temp_ID != Init_array[j, 2] && temp_ID != Init_array[j, 3] && temp_ID != Init_array[j, 4] && temp_ID != Init_array[j, 5] && temp_ID != Init_array[j, 6] && temp_ID != Init_array[j, 7])  //check if the UID already exists in the Init_array
                    {
                        //Filling Init_Array
                        Init_array[j, counter] = temp_ID;
                        Init_array[j, counter + 7] = temp_RSSI;
                        counter++;
                    }
                }
            }

                int rowLength = Init_array.GetLength(0);
                int colLength = Init_array.GetLength(1);
                string str;
                string headline = "|" + "ID 1" + "|" + "ID 2" + "|" + "ID 3" + "|" + "ID 4" + "|" + "ID 5" + "| " + "ID 6" + "|" + "ID 7" + "|" + "ST 1" + "|" + "ST 2" + "|" + "ST 3" + "|" + "ST 4" + "|" + "ST 5" + "|" + "ST 6" + "|" + "ST 7" + "|";
                System.Console.WriteLine(headline);

                for (int k = 0; k < rowLength; k++)
                {
                    str = "|" + Init_array[k, 0] + "   |" + Init_array[k, 1] + "   |" + Init_array[k, 2] + "   |" + Init_array[k, 3] + "   |" + Init_array[k, 4] + "   |" + Init_array[k, 5] + "   |" + Init_array[k, 6] + "   |" + Init_array[k, 7] + "   |" + Init_array[k, 8] + "   |" + Init_array[k, 9] + "   |" + Init_array[k, 10] + "   |" + Init_array[k, 11] + "   |" + Init_array[k, 12] + "   |" + Init_array[k, 13] + "   |";
                    System.Console.WriteLine(str);
                }  

                // Solver
                // Different Positions
                Position Starting = new Position();
                Position Antenna1 = new Position();
                Position Antenna2 = new Position();
                Position Antenna3 = new Position();
                Position Antenna4 = new Position();

                //Initialization for Position estimation
                float m1 = 0.000f;
                float m2 = 0.000f;

                float RobStartx_fl = 0.000f;
                float RobStarty_fl = 0.000f;

                double angle;
                double angleTemp;

                int null_counter = 0;
                int[] check_row = new int[8];
                for (int m = 0; m < rowLength; m++)
                {
                    null_counter = 0;
                    for (int n = 0; n < 7; n++)
                    {
                        if (Init_array[m, n] == null)
                        {
                            null_counter++;
                        }
                    }
                    check_row[m] = 7 - null_counter;    //Array of elements with the number empty places of each init_array row
                    System.Console.WriteLine("The number of elements at " + m * 45 + "° is: \t" + check_row[m]);
                }
                bool solution_found = false;    //true if initialization process is solvable
                bool solution1_found = false;   //true if one possible point is found
                bool solution2_found = false;   //true if two possible points are found
                int count = 0;
                int[] solution1 = new int[2];   //Array with the both degree numbers of solution 1      
                int[] solution2 = new int[2];   //Array with the both degree numbers of solution 2
                while (solution_found == false)
                {
                    while (solution1_found == false)
                    {
                        if (check_row[count] >= 3)
                        {
                            if (check_row[count + 4] >= 3)
                            {
                                solution1[0] = count;
                                solution1[1] = count + 4;
                                break;
                            }
                            if (count >= 2)    //if we reach the 180 degree there will be no solution for this initialization turn
                            {
                                System.Console.WriteLine("NO SOLUTION FOUND!!!");
                                break;
                            }
                        }
                        count++;
                    }
                    System.Console.WriteLine(count);
                    count = count + 1;
                    while (solution2_found == false)
                    {
                        if (check_row[count] >= 3)
                        {
                            if (count >= 8)
                            {
                                Console.WriteLine("Out of Range Exception caused in Array: count");
                            }
                            if (check_row[count + 4] >= 3)
                            {
                                solution2[0] = count;
                                solution2[1] = count + 4;
                                solution_found = true;
                                break;
                            }
                            if (count >= 3)    //if we reach the 180 degree there will be no solution for this initialization turn
                            {
                                System.Console.WriteLine("NO SOLUTION FOUND!!!");
                                break;
                            }
                            else
                            {
                                //count = count - 1;
                                break;
                            }
                        }
                    }
                    System.Console.WriteLine("Solution No. 1 found at: " + solution1[0] * 45 + " degree -- " + solution1[1] * 45 + " degree");
                    System.Console.WriteLine("Solution No. 2 found at: " + solution2[0] * 45 + " degree -- " + solution2[1] * 45 + " degree");
                }

                // Providing the distance with the highest probability
                // Input:   # of tags, all IDs of the tags 
                // Output:  best fitting IDs (e.g.[3 4 5] if 3rd, 4th and 5th are best ones)
                //          the correct distance <-> RSSI signal (e.g.[2 1 3] for middle, max and min)
                int[,] best_arr1 = new int[2, 3];
                int[,] best_arr2 = new int[2, 3];
                int[,] best_arr3 = new int[2, 3];
                int[,] best_arr4 = new int[2, 3];

                int[] temp_input1 = new int[7];
                int[] temp_input2 = new int[7];
                int[] temp_input3 = new int[7];
                int[] temp_input4 = new int[7];

                int[] temp_inputRSSI1 = new int[7];
                int[] temp_inputRSSI2 = new int[7];
                int[] temp_inputRSSI3 = new int[7];
                int[] temp_inputRSSI4 = new int[7];

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 14; j++)
                    {
                        if (Init_array[i, j] == null)
                        {
                            Init_array[i, j] = "0";
                        }
                    }
                }

                for (int m = 0; m < 7; m++)
                {
                    temp_input1[m] = Int32.Parse(Init_array[solution1[0], m]);
                    temp_input2[m] = Int32.Parse(Init_array[solution1[1], m]);
                    temp_input3[m] = Int32.Parse(Init_array[solution2[0], m]);
                    temp_input4[m] = Int32.Parse(Init_array[solution2[1], m]);

                    temp_inputRSSI1[m] = Int32.Parse(Init_array[solution1[0], m + 7]);
                    temp_inputRSSI2[m] = Int32.Parse(Init_array[solution1[1], m + 7]);
                    temp_inputRSSI3[m] = Int32.Parse(Init_array[solution2[0], m + 7]);
                    temp_inputRSSI4[m] = Int32.Parse(Init_array[solution2[1], m + 7]);
                }

                best_arr1 = CorrectID_Distance(temp_input1, temp_inputRSSI1, check_row[solution1[0]]);
                best_arr2 = CorrectID_Distance(temp_input2, temp_inputRSSI2, check_row[solution1[1]]);
                best_arr3 = CorrectID_Distance(temp_input3, temp_inputRSSI3, check_row[solution2[0]]);
                best_arr4 = CorrectID_Distance(temp_input4, temp_inputRSSI4, check_row[solution2[1]]);

                // Position of the antennae 
                Antenna1 = Trilateration(IDtoPOS(Int32.Parse(Init_array[solution1[0], best_arr1[0, 0]])), IDtoPOS(Int32.Parse(Init_array[solution1[0], best_arr1[0, 1]])),
                                        IDtoPOS(Int32.Parse(Init_array[solution1[0], best_arr1[0, 2]])), Int32.Parse(Init_array[solution1[0], best_arr1[0, 0] + 7]),
                                        Int32.Parse(Init_array[solution1[0], best_arr1[0, 1] + 7]), Int32.Parse(Init_array[solution1[0], best_arr1[0, 2] + 7]),
                                        best_arr1[1, 0], best_arr1[1, 1], best_arr1[1, 2]);

                Antenna2 = Trilateration(IDtoPOS(Int32.Parse(Init_array[solution1[1], best_arr2[0, 0]])), IDtoPOS(Int32.Parse(Init_array[solution1[1], best_arr2[0, 1]])),
                                        IDtoPOS(Int32.Parse(Init_array[solution1[1], best_arr2[0, 2]])), Int32.Parse(Init_array[solution1[1], best_arr2[0, 0] + 7]),
                                        Int32.Parse(Init_array[solution1[1], best_arr2[0, 1] + 7]), Int32.Parse(Init_array[solution1[1], best_arr2[0, 2] + 7]),
                                        best_arr2[1, 0], best_arr2[1, 1], best_arr2[1, 2]);

                Antenna3 = Trilateration(IDtoPOS(Int32.Parse(Init_array[solution2[0], best_arr3[0, 0]])), IDtoPOS(Int32.Parse(Init_array[solution2[0], best_arr3[0, 1]])),
                                        IDtoPOS(Int32.Parse(Init_array[solution2[0], best_arr3[0, 2]])), Int32.Parse(Init_array[solution2[0], best_arr3[0, 0] + 7]),
                                        Int32.Parse(Init_array[solution2[0], best_arr3[0, 1] + 7]), Int32.Parse(Init_array[solution2[0], best_arr3[0, 2] + 7]),
                                        best_arr3[1, 0], best_arr3[1, 1], best_arr3[1, 2]);

                Antenna4 = Trilateration(IDtoPOS(Int32.Parse(Init_array[solution2[1], best_arr4[0, 0]])), IDtoPOS(Int32.Parse(Init_array[solution2[1], best_arr4[0, 1]])),
                                        IDtoPOS(Int32.Parse(Init_array[solution2[1], best_arr4[0, 2]])), Int32.Parse(Init_array[solution2[1], best_arr4[0, 0] + 7]),
                                        Int32.Parse(Init_array[solution2[1], best_arr4[0, 1] + 7]), Int32.Parse(Init_array[solution2[1], best_arr4[0, 2] + 7]),
                                        best_arr4[1, 0], best_arr4[1, 1], best_arr4[1, 2]);

                Console.WriteLine("1st Antenna " + Antenna1.X + " and " + Antenna1.Y);
                Console.WriteLine("2nd Antenna " + Antenna2.X + " and " + Antenna2.Y);
                Console.WriteLine("3rd Antenna " + Antenna3.X + " and " + Antenna3.Y);
                Console.WriteLine("4th Antenna " + Antenna4.X + " and " + Antenna4.Y);

                //Console.ReadKey();
                // Alternative estimation of the centre of the robot + position
                //m1 = ((float)Antenna2.Y - (float)Antenna1.Y) / ((float)Antenna2.X - (float)Antenna1.X);
                //m2 = ((float)Antenna4.Y - (float)Antenna3.Y) / ((float)Antenna4.X - (float)Antenna3.X);
                //RobStartx_fl = (1 / (m1 - m2)) * (m1 * (float)Antenna1.X - m2 * (float)Antenna3.X - (float)Antenna1.Y + (float)Antenna3.Y);
                //RobStarty_fl = m1 * (RobStartx_fl - (float)Antenna1.X) + (float)Antenna1.Y;

                //Starting.X = (int)RobStartx_fl;
                //Starting.Y = (int)RobStarty_fl;

                Starting.X = (((Antenna4.X-Antenna3.X)*(Antenna2.X*Antenna1.Y-Antenna1.X*Antenna2.Y)-(Antenna2.X-Antenna1.X)*(Antenna4.X*Antenna3.Y-Antenna3.X*Antenna4.Y)) /
                                    ((Antenna4.Y - Antenna3.Y) * (Antenna2.X - Antenna1.X) - (Antenna2.Y - Antenna1.Y) * (Antenna4.X - Antenna3.X)));
                Starting.Y = (((Antenna1.Y - Antenna2.Y) * (Antenna4.X * Antenna3.Y - Antenna3.X * Antenna4.Y) - (Antenna3.Y - Antenna4.Y) * (Antenna2.X * Antenna1.Y - Antenna1.X * Antenna2.Y)) /
                                    ((Antenna4.Y - Antenna3.Y) * (Antenna2.X - Antenna1.X) - (Antenna2.Y - Antenna1.Y) * (Antenna4.X - Antenna3.X)));

                Console.WriteLine("Robotstarting Position at:" + Starting.X + "mm, " + Starting.Y + "mm");

                // Computing the orientation of the Robot
                //angle = (Math.Atan2(y, x)) * (180 / Math.PI);
                angleTemp = (Math.Atan2((Antenna1.Y - Starting.Y), (Antenna1.X - Starting.X))) * (180 / Math.PI);
                Console.WriteLine("Angle temp " + angleTemp);
                angle = angleTemp - (double)(solution1[0]*45.0);  // in deg
                Console.WriteLine("Angle wrong direction " + angle);
                if (angle <= 0.0)
                {
                    angle = angle + 180;
                }
                else
                {
                    angle = angle - 180;
                }

               Console.WriteLine("Robotangle: " + angle + "°");
                
            }



       

        

        