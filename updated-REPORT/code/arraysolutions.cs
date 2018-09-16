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