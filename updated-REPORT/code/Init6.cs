// Methode to compute the best IDs and correct distances
        public static int[,] CorrectID_Distance(int[] arr, int[] arrRSSI, int numTags)
        {
            // Inputs
            /*  arr = Array of all IDs
             *  arrRSSI = Array of all RSSI
                numTags = Int with the num of tags found
            */
            // Init
            int[,] best = new int[2, 3];
            int[] dist = new int[numTags];          // Array which contain the best distance (max(0),middle(1),min(2))
            int[] neighbours = new int[numTags];
            int i = 0;
            int p = 4;

            // Compute the neighbours
            neighbours = FindNeig(arr, numTags);
			

            // Switch case for the different possible shapes
            switch (numTags)
            {
                case 3:
                    Console.WriteLine("3 Tags ----------");
                    for (int l = 0; l < neighbours.GetLength(0); l++)
                    {
                        if (neighbours[l] == 0)     // Detect outlier and boarder
                        {
                            dist[l] = 1;            // stay max
                        }
                        else if (neighbours[l] <= 3)     // Detect outlier and boarder
                        {
                            dist[l] = 0;            // stay max
                        }
                        else if (neighbours[l] == 4)     // Detect the inner, change it to min/middle
                        {
                            dist[l] = 1;            // change it to middle
                        }
                        else if (neighbours[l] > 4)     // Detect the inner, change it to min/middle
                        {
                            dist[l] = 0;            // change it to middle
                        }
                        // all other numbers are at the boarder 
                    }
                    // Select the best 3 readings   
                    i = 0;
                    p = 4;
                    while (i < 3)                   // Start for the first ID
                    {
                        for (int h = 0; h < neighbours.GetLength(0); h++)   // looks for a fitting
                        {
                            if (neighbours[h] == p && i < 3)    // hit must be same value and less then 3 hits
                            {
                                best[0, i] = h;                 // index of the best ID 
                                best[1, i] = dist[h];           // info about max, mid and min of this ID
                                i += 1;
                            }
                            else if (i >= 3)
                            {
                                break;
                            }
                        }
                        p -= 1;
                    }
                    break;
                /*--------------------------------------------------------------------------------*/
                case 4:
                    Console.WriteLine("4 Tags ----------");
                    for (int l = 0; l < neighbours.GetLength(0); l++)
                    {
                        if (neighbours[l] == 0)     // Detect outlier and boarder
                        {
                            dist[l] = 1;            // stay max
                        }
                        else if (neighbours[l] <= 3)     // Detect outlier and boarder
                        {
                            dist[l] = 0;            // stay max
                        }
                        else if (neighbours[l] == 4)     // Detect the inner, change it to min/middle
                        {
                            dist[l] = 1;            // change it to middle
                        }
                        else if (neighbours[l] > 4)     // Detect the inner, change it to min/middle
                        {
                            dist[l] = 0;            // change it to middle
                        }
                        // all other numbers are at the boarder 
                    }
                    // Select the best 3 readings   
                    i = 0;
                    p = 4;
                    while (i < 3)                   // Start for the first ID
                    {
                        for (int h = 0; h < neighbours.GetLength(0); h++)   // looks for a fitting
                        {
                            if (neighbours[h] == p && i < 3)    // hit must be same value and less then 3 hits
                            {
                                best[0, i] = h;                 // index of the best ID 
                                best[1, i] = dist[h];           // info about max, mid and min of this ID
                                i += 1;
                            }
                            else if (i >= 3)
                            {
                                break;
                            }
                        }
                        p -= 1;
                    }

                    break;
                /*--------------------------------------------------------------------------------*/
                case 5:
                    Console.WriteLine("5 Tags ----------");
                    for (int l = 0; l < neighbours.GetLength(0); l++)
                    {
                        if (neighbours[l] <= 2)     // Detect outlier and boarder
                        {
                            dist[l] = 0;            // stay max
                        }
                        else if (neighbours[l] == 3)     // Detect the inner, change it to min/middle
                        {
                            dist[l] = 1;            // change it to middle
                        }
                        // all other numbers are at the boarder 
                    }
                    // Select the best 3 readings   
                    i = 0;
                    p = 4;
                    while (i < 3)                   // Start for the first ID
                    {
                        for (int h = 0; h < neighbours.GetLength(0); h++)   // looks for a fitting
                        {
                            if (neighbours[h] == p && i < 3)    // hit must be same value and less then 3 hits
                            {
                                best[0, i] = h;                 // index of the best ID 
                                best[1, i] = dist[h];           // info about max, mid and min of this ID
                                i += 1;
                            }
                            else if (i >= 3)
                            {
                                break;
                            }
                        }
                        p -= 1;
                    }
                    break;

                /*--------------------------------------------------------------------------------*/
                case 6:
                    Console.WriteLine("6 Tags ----------");
                    switch (neighbours.Sum())
                    {
                        case 12: // Shape with 2 outliers
                            Console.WriteLine("2 Outliers");
                            for (int l = 0; l < neighbours.GetLength(0); l++)
                            {
                                if (neighbours[l] <= 2)     // Detect outlier and boarder
                                {
                                    dist[l] = 0;            // stay max
                                }
                                else if (neighbours[l] == 4)     // Detect the inner, change it to min/middle
                                {
                                    if (arrRSSI[l] <= 3)
                                    {
                                        dist[l] = 2;            // change it to min
                                    }
                                    else if (arrRSSI[l] > 3)
                                    {
                                        dist[l] = 1;            // change it to middle
                                    }
                                }
                                // all other numbers are at the boarder 
                            }
                            break;
                        case 14: // Shape like a domino 
                            Console.WriteLine("Domino");
                            for (int l = 0; l < neighbours.GetLength(0); l++)
                            {
                                if (neighbours[l] <= 2)     // Detect outlier and boarder
                                {
                                    dist[l] = 0;            // stay max
                                }
                                else if (neighbours[l] == 3)     // Detect the centre, change it to min
                                {
                                    dist[l] = 2;            // change it to min
                                }
                                // all other numbers are at the boarder 
                            }
                            break;
                        default:
                            Console.WriteLine("Default case");
                            break;
                    }
                    // Select the best 3 readings   
                    i = 0;
                    p = 4;
                    while (i < 3)                   // Start for the first ID
                    {
                        for (int h = 0; h < neighbours.GetLength(0); h++)   // looks for a fitting
                        {
                            if (neighbours[h] == p && i < 3)    // hit must be same value and less then 3 hits
                            {
                                best[0, i] = h;                 // index of the best ID 
                                best[1, i] = dist[h];           // info about max, mid and min of this ID
                                i += 1;
                            }
                            else if (i >= 3)
                            {
                                break;
                            }
                        }
                        p -= 1;
                    }

                    break;
                /*--------------------------------------------------------------------------------*/
                case 7:
                    Console.WriteLine("7 Tags ----------");
                    for (int l = 0; l < neighbours.GetLength(0); l++)
                    {
                        if (neighbours[l] <= 3)     // Detect outlier and boarder
                        {
                            dist[l] = 0;            // stay max
                        }
                        else if (neighbours[l] == 4)     // Detect the centre, change it to min
                        {
                            dist[l] = 2;            // change it to min
                        }
                        // all other numbers are at the boarder 
                    }
                    // Select the best 3 readings
                    i = 0;
                    p = 4;
                    while (i < 3)                   // Start for the first ID
                    {
                        for (int h = 0; h < neighbours.GetLength(0); h++)   // looks for a fitting
                        {
                            if (neighbours[h] == p && i < 3)    // hit must be same value and less then 3 hits
                            {
                                best[0, i] = h;                 // index of the best ID 
                                best[1, i] = dist[h];           // info about max, mid and min of this ID
                                i += 1;
                            }
                            else if (i >= 3)
                            {
                                break;
                            }
                        }
                        p -= 1;
                    }
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            foreach (int ee in best)
            {
                Console.WriteLine(ee);
            }
            return best;
        }