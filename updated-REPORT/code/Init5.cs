// Find neighbours of the IDs
        public static int[] FindNeig(int[] arrID, int numTags)
        {
            // Init
            int[] neighbours = new int[numTags];
            int[] tempNeig = new int[4];

            // Find the number of neighbours
            for (int m = 0; m < numTags; m++)
            {
                // Init
                neighbours[m] = 0;

                // Take actual ID and compute the possible neighbours
                tempNeig[0] = arrID[m] - 11;
                tempNeig[1] = arrID[m] - 1;
                tempNeig[2] = arrID[m] + 1;
                tempNeig[3] = arrID[m] + 11;

                for (int v = 0; v < 4; v++)
                {
                    foreach (int tempinput in arrID)
                    {
                        if (tempinput == tempNeig[v])
                        {
                            neighbours[m] += 1;
                        }
                    }
                }
            }
            return neighbours;
        }