        
        // Method to compute the norm of a vector
        public static double Norm(PositionD p) // get the norm of a vector
        {
            return (Math.Pow(Math.Pow(p.X, 2) + Math.Pow(p.Y, 2), 0.5));
        }
		
		// Methode to compute the position based on the ID in [mm]
        public static Position IDtoPOS(int ID)
        {
            Position FinalPos = new Position();
            int[] posx = new int[9] { 10, 10, 10, 20, 20, 20, 30, 30, 30 };
            int[] posy = new int[9] { 10, 20, 30, 10, 20, 30, 10, 20, 30 };
            // For a 3x3 testing field
            FinalPos.X = posx[ID - 1];
            FinalPos.Y = posy[ID - 1];
            return FinalPos;
        }
		
		