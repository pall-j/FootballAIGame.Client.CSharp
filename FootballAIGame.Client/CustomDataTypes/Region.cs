using System;
using System.Diagnostics;

namespace FootballAIGame.Client.CustomDataTypes
{
    /// <summary>
    /// Represents the region of the football field.
    /// </summary>
    class Region
    {
        /// <summary>
        /// The number of rows into which the field is divided.
        /// </summary>
        public const int NumberOfRows = 9;

        /// <summary>
        /// The number of columns into which the field is divided.
        /// </summary>
        public const int NumberOfColumns = 8;

        /// <summary>
        /// Gets the number of regions.
        /// </summary>
        /// <value>
        /// The number of field regions.
        /// </value>
        public static int NumberOfRegions
        {
            get { return NumberOfRows*NumberOfColumns; }
        }

        /// <summary>
        /// Gets the width of one region.
        /// </summary>
        /// <value>
        /// The width of one region.
        /// </value>
        public static double RegionWidth
        {
            get { return GameClient.FieldWidth/NumberOfColumns; }
        }

        /// <summary>
        /// Gets the height of one region.
        /// </summary>
        /// <value>
        /// The height of one region.
        /// </value>
        public static double RegionHeight
        {
            get { return GameClient.FieldHeight/NumberOfRows; }
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; private set; }

        /// <summary>
        /// Gets the x coordinate in the regions' grid.
        /// </summary>
        /// <value>
        /// The x coordinate.
        /// </value>
        public int X { get; private set; }

        /// <summary>
        /// Gets the y coordinate in the regions' grid.
        /// </summary>
        /// <value>
        /// The y coordinate.
        /// </value>
        public int Y { get; private set; }

        /// <summary>
        /// Gets the center position of the region.
        /// </summary>
        /// <value>
        /// The center position <see cref="Vector"/>.
        /// </value>
        public Vector Center { get; private set; }

        /// <summary>
        /// Gets the top left position of the region.
        /// </summary>
        /// <value>
        /// The top left position <see cref="Vector"/>.
        /// </value>
        public Vector TopLeft { get; private set; }

        /// <summary>
        /// Gets the bottom right position of the region.
        /// </summary>
        /// <value>
        /// The bottom right position <see cref="Vector"/>.
        /// </value>
        public Vector BottomRight { get; private set; }

        /// <summary>
        /// Gets or sets the array that should contain all regions.
        /// </summary>
        /// <value>
        /// The array of regions.
        /// </value>
        private static Region[] Regions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="x">The x coordinate in the regions' grid.</param>
        /// <param name="y">The y coordinate in the regions' grid.</param>
        /// <param name="topLeft">The top left position of the region.</param>
        /// <param name="bottomRight">The bottom right position of the region.</param>
        private Region(int id, int x, int y, Vector topLeft, Vector bottomRight)
        {
            Id = id;
            X = x;
            Y = y;
            TopLeft = topLeft;
            BottomRight = bottomRight;
            Center = new Vector((topLeft.X + bottomRight.X) / 2, (topLeft.Y + bottomRight.Y) / 2);
        }

        /// <summary>
        /// Gets the region with the specified identifier.
        /// </summary>
        /// <param name="id">The region's identifier.</param>
        /// <returns>The corresponding <see cref="Region"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if an invalid identifier was specified.</exception>
        public static Region GetRegion(int id)
        {
            if (Regions == null)
                CreateRegions();

            if (id >= NumberOfRegions || id < 0)
                throw new IndexOutOfRangeException();

            Debug.Assert(Regions != null, "Regions != null");
            return Regions[id];
        }

        /// <summary>
        /// Gets the region with the specified grid coordinates.
        /// </summary>
        /// <param name="x">The x coordinate in the regions' grid.</param>
        /// <param name="y">The y coordinate in the regions' grid.</param>
        /// <returns>The corresponding <see cref="Region"/>.</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown if invalid coordinates were specified.</exception>
        public static Region GetRegion(int x, int y)
        {
            if (Regions == null)
                CreateRegions();

            if (x >= NumberOfColumns || x < 0 || y >= NumberOfRows || y < 0)
                throw new IndexOutOfRangeException();

            Debug.Assert(Regions != null, "Regions != null");

            return Regions[x*NumberOfRows + y];
        }

        /// <summary>
        /// Creates the regions in accordance with the <see cref="NumberOfColumns"/> and <see cref="NumberOfRows"/>
        /// and adds them into <see cref="Regions"/>.
        /// </summary>
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
