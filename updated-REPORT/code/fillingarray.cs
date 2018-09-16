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