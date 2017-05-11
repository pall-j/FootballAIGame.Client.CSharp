using System;

namespace FootballAIGame.Client.CustomDataTypes
{
    /// <summary>
    /// Represents the two-dimensional vector or point.
    /// </summary>
    public class Vector
    {
        /// <summary>
        /// Gets or sets the x value.
        /// </summary>
        /// <value>
        /// The x.
        /// </value>
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the y value.
        /// </summary>
        /// <value>
        /// The y.
        /// </value>
        public double Y { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        public Vector() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// The vector is specified by the parameters and is equal to (<see cref="to"/> - <see cref="from"/>).
        /// </summary>
        /// <param name="from">From which position.</param>
        /// <param name="to">To which position.</param>
        public Vector(Vector from, Vector to)
        {
            X = to.X - from.X;
            Y = to.Y - from.Y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// The vector is specified by the parameters and is equal to (<see cref="to"/> - <see cref="from"/>).
        /// </summary>
        /// <param name="from">From which position.</param>
        /// <param name="to">To which position.</param>
        /// <param name="length">The length of the vector.</param>
        public Vector(Vector from, Vector to, double length)
        {
            X = to.X - from.X;
            Y = to.Y - from.Y;
            Resize(length);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="length">The length of the vector.</param>
        public Vector(double x, double y, double length)
        {
            X = x;
            Y = y;
            Resize(length);
        }

        /// <summary>
        /// Gets the vector's length.
        /// </summary>
        /// <value>
        /// The length of the vector.
        /// </value>
        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// Gets the vector's squared length.
        /// </summary>
        /// <value>
        /// The squared length of the vector.
        /// </value>
        public double LengthSquared
        {
            get { return X*X + Y*Y; }
        }

        /// <summary>
        /// Gets the normalized vector of the current vector.
        /// </summary>
        /// <value>
        /// The normalized vector of the current vector.
        /// </value>
        public Vector Normalized
        {
            get
            {
                var result = new Vector(X, Y);
                result.Normalize();
                return result;
            }
        }

        /// <summary>
        /// Gets the multiplied vector of the current vector.
        /// </summary>
        /// <param name="scalar">The scalar value for multiplication.</param>
        /// <returns>The multiplied vector of the current vector.</returns>
        public Vector GetMultiplied(double scalar)
        {
            var result = new Vector(X, Y);
            result.Multiply(scalar);

            return result;
        }

        /// <summary>
        /// Gets the resized vector of the current vector.
        /// </summary>
        /// <param name="newLength">The length of the resized vector.</param>
        /// <returns>The resized vector of the current vector.</returns>
        public Vector GetResized(double newLength)
        {
            var res = Normalized;
            res.Multiply(newLength);
            return res;
        }

        /// <summary>
        /// Gets the truncated vector to the specified maximum length if the vector had longer length.
        /// </summary>
        /// <param name="maxLength">The maximum length.</param>
        /// <returns>The truncated vector to the specified length if the vector had longer length.</returns>
        public Vector GetTruncated(double maxLength)
        {
            var res = new Vector(X, Y);
            res.Truncate(maxLength);
            return res;
        }

        /// <summary>
        /// Normalizes the vector.
        /// </summary>
        public void Normalize()
        {
            var length = Length;
            X /= length;
            Y /= length;
        }

        /// <summary>
        /// Resizes the vector to the specified new length.
        /// </summary>
        /// <param name="newLength">The new length.</param>
        public void Resize(double newLength)
        {
            if (Math.Abs(Length) > 0.00001)
                Normalize();
            X *= newLength;
            Y *= newLength;
        }

        /// <summary>
        /// Truncates the vector to the specified maximum length if the vector had longer length.
        /// </summary>
        /// <param name="maxLength">The maximum length.</param>
        public void Truncate(double maxLength)
        {
            if(Length > maxLength)
                Resize(maxLength);
        }

        /// <summary>
        /// Multiplies the vector by the specified scalar value.
        /// </summary>
        /// <param name="scalar">The scalar.</param>
        public void Multiply(double scalar)
        {
            X *= scalar;
            Y *= scalar;
        }

        /// <summary>
        /// Gets the distances between the given vectors.
        /// </summary>
        /// <param name="firstVector">The first vector.</param>
        /// <param name="secondVector">The second vector.</param>
        /// <returns>The distance between the given vectors.</returns>
        public static double GetDistanceBetween(Vector firstVector, Vector secondVector)
        {
            return Math.Sqrt(Math.Pow(firstVector.X - secondVector.X, 2) + Math.Pow(firstVector.Y - secondVector.Y, 2));
        }

        /// <summary>
        /// Gets the squared distance between the given vectors.
        /// </summary>
        /// <param name="firstVector">The first vector.</param>
        /// <param name="secondVector">The second vector.</param>
        /// <returns>The squared distance between the given vectors.</returns>
        public static double GetDistanceBetweenSquared(Vector firstVector, Vector secondVector)
        {
            return Math.Pow(firstVector.X - secondVector.X, 2) + Math.Pow(firstVector.Y - secondVector.Y, 2);
        }

        /// <summary>
        /// Gets the dot product of the given vectors.
        /// </summary>
        /// <param name="firstVector">The first vector.</param>
        /// <param name="secondVector">The second vector.</param>
        /// <returns>The dot product between the given vectors.</returns>
        public static double GetDotProduct(Vector firstVector, Vector secondVector)
        {
            return firstVector.X * secondVector.X + firstVector.Y * secondVector.Y;
        }

        /// <summary>
        /// Gets the difference between the specified vectors.
        /// The difference <see cref="Vector"/> is equal to <see cref="to"/> - <see cref="from"/>.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <returns>The <see cref="Vector"/> that is equal to <see cref="to"/> - <see cref="from"/>.</returns>
        public static Vector GetDifference(Vector to, Vector from)
        {
            return new Vector(to.X - from.X, to.Y - from.Y);
        }

        /// <summary>
        /// Gets the sum of the given vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The sum <see cref="Vector"/> of the given vectors.</returns>
        public static Vector GetSum(Vector first, Vector second)
        {
            return new Vector(first.X + second.X, first.Y + second.Y);
        }

    }
}
