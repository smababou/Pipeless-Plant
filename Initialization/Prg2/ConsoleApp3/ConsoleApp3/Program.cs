using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections;
using System.Drawing;

/* Version no. 1
 *  Program to read txt file from the same type as the reader provides
 *  Computation of best fitting angles 
 *  Computng the position of the antennae and estimation of the position of the robot + orientation
 *  01.06.2018
 */

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            //Read data from .txt file and writes it into the string-list "Worte"
            string Dateiinhalt = System.IO.File.ReadAllText(@"C:\Users\Stephan\Dropbox\Project_PP\C#_Files\Initialisierung_STtefan\Meas_StartingProc_like_reader1.txt");
            // string Dateiinhalt = System.IO.File.ReadAllText(@"C:\Users\Administrator\Documents\Visual Studio 2017\Projects\TEST\ConsoleApp1\bin\Debug\netcoreapp2.0\Meas_StartingProc.txt");


            List<string> Worte = Dateiinhalt.Split(new string[] { "OK", "<\\r>", "\n", " ", "SCAN:+UID=", ",+RSSI=", "/" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            //Initialization of Variables
            //Console.ReadKey();

            int[,] init_array = new int[37, 8];   //Array of TAG-IDs and each Signal strengthes
            int wordcount = 0;                          //Number of Word in string-list
            int j = 0;
            int jj = 0;
            int wordcount_temp;
            bool go_on;
            int c = Worte.Count;                        //Length of string-list

            // Different Positions
            Position Starting = new Position();
            Position Tag1 = new Position();
            Position Tag2 = new Position();
            Position Tag3 = new Position();
            Position Tag4 = new Position();
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

            //For-Loop fills the Initialization Array with the equivalent values from the .txt file
            for (int i = 0; i < 37; i++)
            {
                go_on = true;
                j = 0;
                jj = 0;
                while (go_on == true)
                {
                    int x = (Int32.Parse(Worte[wordcount])) / 10;
                    init_array[x, j] = Int32.Parse(Worte[wordcount + 1], System.Globalization.NumberStyles.HexNumber);
                    jj = j + 4;
                    init_array[x, jj] = Int32.Parse(Worte[wordcount + 2]);
                    wordcount_temp = wordcount + 4;

                    if (wordcount >= c - 4)
                    {
                        break;
                    }
                    System.Console.WriteLine(wordcount_temp);
                    if (Worte[wordcount] == Worte[wordcount_temp])
                    {
                        go_on = true;
                        wordcount = wordcount_temp;
                        j++;
                    }
                    else
                    {
                        go_on = false;
                        wordcount = wordcount_temp;
                    }
                }

            }


            int rowLength = init_array.GetLength(0);
            int colLength = init_array.GetLength(1);
            string str;
            string headline = "|  " + "ID 1" + "\t|  " + "ID 2" + "\t|  " + "ID 3" + "\t|  " + "ID 4" + "\t|  " + "ST 1" + "\t|  " + "ST 2" + "\t|  " + "ST 3" + "\t|  " + "ST 4" + "\t|";
            System.Console.WriteLine(headline);

            for (int k = 0; k < rowLength; k++)
            {
                str = "|  " + init_array[k, 0] + "\t|  " + init_array[k, 1] + "\t|  " + init_array[k, 2] + "\t|  " + init_array[k, 3] + "\t|  " + init_array[k, 4] + "\t|  " + init_array[k, 5] + "\t|  " + init_array[k, 6] + "\t|  " + init_array[k, 7] + "\t|";
                System.Console.WriteLine(str);
            }

            int null_counter = 0;
            int[] check_row = new int[37];
            for (int m = 0; m < rowLength; m++)
            {
                null_counter = 0;
                for (int n = 0; n < 4; n++)
                {
                    if (init_array[m, n] == 0)
                    {
                        null_counter++;
                    }
                }
                check_row[m] = null_counter;    //Array of elements with the number empty places of each init_array row
                System.Console.WriteLine("The number of empty elements at " + m + "0° is:\t" + check_row[m]);
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
                    if (check_row[count] < 2)
                    {
                        if (check_row[count + 18] < 2)
                        {
                            solution1[0] = count;
                            solution1[1] = count + 18;
                            break;
                        }
                    }
                    count++;
                }
                System.Console.WriteLine(count);
                count = count + 9;
                while (solution2_found == false)
                {
                    if (check_row[count] < 2)
                    {
                        if (check_row[count + 18] < 2)
                        {
                            solution2[0] = count;
                            solution2[1] = count + 18;
                            solution_found = true;
                            break;
                        }
                        if (count == 19)    //if we reach the 180 degree there will be no solution for this initialization turn
                        {
                            System.Console.WriteLine("NO SOLUTIO FOUND!!!");
                            break;
                        }
                        else
                        {
                            count = count - 8;
                            break;
                        }
                    }
                    if (count == 19)    //if we reach the 180 degree there will be no solution for this initialization turn
                    {
                        System.Console.WriteLine("NO SOLUTIO FOUND!!!");
                        break;
                    }
                    else
                    {
                        count = count - 8;
                        break;
                    }



                }
                System.Console.WriteLine("Solution No. 1 found at: " + solution1[0] + "0 degree -- " + solution1[1] + "0 degree");
                System.Console.WriteLine("Solution No. 2 found at: " + solution2[0] + "0 degree -- " + solution2[1] + "0 degree");
            }

            // Position of the antennae 
            Antenna1 = Trilateration(IDtoPOS(init_array[solution1[0], 0]), IDtoPOS(init_array[solution1[0], 1]),
                                    IDtoPOS(init_array[solution1[0], 2]), init_array[solution1[0], 4],
                                    init_array[solution1[0], 5], init_array[solution1[0], 6]);

            Antenna2 = Trilateration(IDtoPOS(init_array[solution1[1], 0]), IDtoPOS(init_array[solution1[1], 1]),
                                    IDtoPOS(init_array[solution1[1], 2]), init_array[solution1[1], 4],
                                    init_array[solution1[1], 5], init_array[solution1[1], 6]);

            Antenna3 = Trilateration(IDtoPOS(init_array[solution2[0], 0]), IDtoPOS(init_array[solution2[0], 1]),
                                    IDtoPOS(init_array[solution2[0], 2]), init_array[solution2[0], 4],
                                    init_array[solution2[0], 5], init_array[solution2[0], 6]);

            Antenna4 = Trilateration(IDtoPOS(init_array[solution2[1], 0]), IDtoPOS(init_array[solution2[1], 1]),
                                    IDtoPOS(init_array[solution2[1], 2]), init_array[solution2[1], 4],
                                    init_array[solution2[1], 5], init_array[solution2[1], 6]);

           
            Console.WriteLine("1st Antenna " + Antenna1.X + " and " + Antenna1.Y);
            Console.WriteLine("2nd Antenna " + Antenna2.X + " and " + Antenna2.Y);
            Console.WriteLine("3rd Antenna " + Antenna3.X + " and " + Antenna3.Y);
            Console.WriteLine("4th Antenna " + Antenna4.X + " and " + Antenna4.Y);

            //Console.ReadKey();

            // Estimation of the centre of the robot + position
            m1 = ((float)Antenna2.Y - (float)Antenna1.Y) / ((float)Antenna2.X - (float)Antenna1.X);
            m2 = ((float)Antenna4.Y - (float)Antenna3.Y) / ((float)Antenna4.X - (float)Antenna3.X);
            RobStartx_fl = (1 / (m1 - m2)) * (m1 * (float)Antenna1.X - m2 * (float)Antenna3.X - (float)Antenna1.Y + (float)Antenna3.Y);
            RobStarty_fl = m1 * (RobStartx_fl - (float)Antenna1.X) + (float)Antenna1.Y;

            Starting.X = (int)RobStartx_fl;
            Starting.Y = (int)RobStarty_fl;

            Console.WriteLine("Robotstarting Position at:" + Starting.X + "mm, " + Starting.Y + "mm.");

            // Computing the orientation of the Robot
            //angle = (Math.Atan2(y, x)) * (180 / Math.PI);
            angleTemp = (Math.Atan2((Antenna1.Y- Starting.Y), (Antenna1.X-Starting.X))) * (180 / Math.PI);
            angle = angleTemp - (double)(solution1[0]);  // in deg
            if (angle < 0)
            {
                angle = angle + 360;
            }

            Console.WriteLine("Robotangle " + angle  + "°.");
            Console.ReadKey();  // Keeps the console open

        }

        // Method to compute the norm of a vector
        public static double Norm(PositionD p) // get the norm of a vector
        {
            return (Math.Pow(Math.Pow(p.X, 2) + Math.Pow(p.Y, 2), 0.5));
        }

        //Methode to compute the position based on the ID in [cm], Output in [mm]
        public static Position Trilateration(Position point1, Position point2, Position point3, int r1t, int r2t, int r3t)
        {
            double[] dist = new double[] { 10.5, 10.0, 9.5, 9.0, 8.0, 6.0, 5.0, 4.0 };

            Position resultPose = new Position();
            PositionD ex = new PositionD();
            PositionD ey = new PositionD();
            PositionD aux = new PositionD();
            PositionD auy = new PositionD();
            PositionD aux2 = new PositionD();
            double r1;
            double r2;
            double r3;
            r1 = dist[r1t];
            r2 = dist[r2t];
            r3 = dist[r3t];

            //unit vector in a direction from point1 to point 2
            double p2p1Distance = Math.Pow(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2), 0.5);
            ex.X = (point2.X - point1.X) / p2p1Distance;
            ex.Y =  (point2.Y - point1.Y) / p2p1Distance;
            aux.X = point3.X - point1.X;
            aux.Y = point3.Y - point1.Y;
            //signed magnitude of the x component
            double i = ex.X * aux.X + ex.Y * aux.Y;
            //the unit vector in the y direction. 
            aux2.X = point3.X - point1.X - i * ex.X;
            aux2.Y = point3.Y - point1.Y - i * ex.Y;
            ey.X = aux2.X / Norm(aux2);
            ey.Y = aux2.Y / Norm(aux2);
            //the signed magnitude of the y component
            double j = ey.X * aux.X + ey.Y * aux.Y;
            //coordinates
            double x = (Math.Pow(r1, 2) - Math.Pow(r2, 2) + Math.Pow(p2p1Distance, 2)) / (2 * p2p1Distance);
            double y = (Math.Pow(r1, 2) - Math.Pow(r3, 2) + Math.Pow(i, 2) + Math.Pow(j, 2)) / (2 * j) - i * x / j;
            //result coordinates
            double finalX = 10*(point1.X + x * ex.X + y * ey.X);
            double finalY = 10*(point1.Y + x * ex.Y + y * ey.Y);
            resultPose.X = (int)(finalX);
            resultPose.Y = (int)(finalY);


            return resultPose;

        }
        // Methode to compute the position based on the ID in [mm]
        public static Position IDtoPOS(int ID)
        {
            Position FinalPos = new Position();

            FinalPos.Y = 10 * ((ID % 11) - 1);
            FinalPos.X = 10 * ((ID - ((FinalPos.Y/10) + 1)) / 11);

            return FinalPos;
        }
    }
}
