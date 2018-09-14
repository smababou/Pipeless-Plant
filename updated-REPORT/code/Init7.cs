 //Methode to compute the position based on the ID in [cm], Output in [mm]
        public static Position Trilateration(Position point1, Position point2, Position point3, int r1t, int r2t, int r3t, int bestr1, int bestr2, int bestr3)
        {
            //double[] dist = new double[] { 10.5, 10.0, 9.5, 9.0, 8.0, 6.0, 5.0, 4.0 }; // FH paper
            double[,] dist = new double[3, 8] { { 14, 9.75, 9.0, 8.0, 7.0, 6.0, 3.5, 2.75 },
                                                { 5.0, 5.1, 5.3, 5.5, 5.8, 4.0, 3.5, 2.75 },
                                                { 5.0, 4.7, 4.5, 4.3, 4.2, 4.0, 3.5, 2.75} }; // Approximation of our measurements

			Position resultPose = new Position();
			PositionD ex = new PositionD();
			PositionD ey = new PositionD();
			PositionD aux = new PositionD();
			PositionD auy = new PositionD();
			PositionD aux2 = new PositionD();
			double r1;
			double r2;
			double r3;
			r1 = dist[bestr1, r1t];
			r2 = dist[bestr2, r2t];
			r3 = dist[bestr3, r3t];

            // For testing purpose
            //Console.WriteLine("1st radius " + r1);
            //Console.WriteLine("2nd radius " + r2);
            //Console.WriteLine("3rd radius " + r3);

            //Console.WriteLine("1st point " + point1.X + " " + point1.Y);
            //Console.WriteLine("2nd point " + point2.X + " " + point2.Y);
            //Console.WriteLine("3rd point " + point3.X + " " + point3.Y);

            //unit vector in a direction from point1 to point 2
            double p2p1Distance = Math.Pow(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2), 0.5);
            ex.X = (point2.X - point1.X) / p2p1Distance;
            ex.Y = (point2.Y - point1.Y) / p2p1Distance;
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
            double y = (Math.Pow(r1, 2) - Math.Pow(r3, 2) + Math.Pow(i, 2) + Math.Pow(j, 2)) / (2 * j) - i * (x / j);
            //result coordinates
            double finalX = 10 * (point1.X + x * ex.X + y * ey.X);
            double finalY = 10 * (point1.Y + x * ex.Y + y * ey.Y);
            resultPose.X = (int)(finalX);
            resultPose.Y = (int)(finalY);

            return resultPose;
        }

