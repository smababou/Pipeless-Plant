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