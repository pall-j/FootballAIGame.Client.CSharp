using System;
using System.Diagnostics;

namespace FootballAIGame.Client.CustomDataTypes
{
    class Region
    {
        public const int NumberOfRows = 9;
        public const int NumberOfColumns = 8;

        public static int NumberOfRegions
        {
            get { return NumberOfRows*NumberOfColumns; }
        }

        public static double RegionWidth
        {
            get { return GameClient.FieldWidth/NumberOfColumns; }
        }

        public static double RegionHeight
        {
            get { return GameClient.FieldHeight/NumberOfRows; }
        }

        public int Id { get; private set; }

        public int X { get; private set; }

        public int Y { get; private set; }

        public Vector Center { get; private set; }

        public Vector TopLeft { get; private set; }

        public Vector BottomRight { get; private set; }

        private Region(int id, int x, int y, Vector topLeft, Vector bottomRight)
        {
            Id = id;
            X = x;
            Y = y;
            TopLeft = topLeft;
            BottomRight = bottomRight;
            Center = new Vector((topLeft.X + bottomRight.X) / 2, (topLeft.Y + bottomRight.Y) / 2);
        }

        private static Region[] Regions { get; set; }

        public static Region GetRegion(int id)
        {
            if (Regions == null)
                CreateRegions();

            if (id >= NumberOfRegions || id < 0)
                throw new IndexOutOfRangeException();

            Debug.Assert(Regions != null, "Regions != null");
            return Regions[id];
        }

        public static Region GetRegion(int x, int y)
        {
            if (Regions == null)
                CreateRegions();

            if (x >= NumberOfColumns || x < 0 || y >= NumberOfRows || y < 0)
                throw new IndexOutOfRangeException();

            Debug.Assert(Regions != null, "Regions != null");

            return Regions[x*NumberOfRows + y];
        }

        private static void CreateRegions()
        {
            Regions = new Region[NumberOfRegions];
            for (int x = 0; x < NumberOfColumns; x++)
            {
                for (int y = 0; y < NumberOfRows; y++)
                {
                    Regions[x*NumberOfRows+y] = new Region(x*NumberOfRows+y, x, y,
                        new Vector(x*RegionWidth, y*RegionHeight),
                        new Vector((x+1)*RegionWidth, (y+1)*RegionHeight));
                }
            }
        }
    }
}
