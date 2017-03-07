using System;
using System.Runtime.CompilerServices;

namespace FootballAIGame.AI.FSM.CustomDataTypes
{
    /// <summary>
    /// The vector class used for representing two-dimensional vectors or points.
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
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public Vector(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector(Vector from, Vector to, double size)
        {
            X = to.X - from.X;
            Y = to.Y - from.Y;
            Resize(size);
        }

        public Vector(double x, double y, double size)
        {
            X = x;
            Y = y;
            Resize(size);
        }

        public Vector(Vector from, Vector to)
        {
            X = to.X - from.X;
            Y = to.Y - from.Y;
        }

        /// <summary>
        /// Gets the vector length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// Gets the length squared.
        /// </summary>
        /// <value>
        /// The length squared.
        /// </value>
        public double LengthSquared
        {
            get { return X*X + Y*Y; }
        }

        /// <summary>
        /// Gets the normalized vector of the current instance.
        /// </summary>
        public Vector Normalized
        {
            get
            {
                var result = new Vector(X, Y);
                result.Normalize();
                return result;
            }
        }

        public Vector Multiplied(double scalar)
        {
            var result = new Vector(X, Y);
            result.Multiply(scalar);

            return result;
        }

        public Vector Resized(double newSize)
        {
            var res = Normalized;
            res.Multiply(newSize);
            return res;
        }

        public Vector Truncated(double maxLength)
        {
            var res = new Vector(X, Y);
            res.Truncate(maxLength);
            return res;
        }

        /// <summary>
        /// Normalizes this instance.
        /// </summary>
        public void Normalize()
        {
            var length = Length;
            X /= length;
            Y /= length;
        }

        /// <summary>
        /// Resizes the current instance to the specified new length.
        /// </summary>
        /// <param name="newLength">The new length.</param>
        public void Resize(double newLength)
        {
            if (Math.Abs(Length) > 0.00001)
                Normalize();
            X *= newLength;
            Y *= newLength;
        }

        public void Truncate(double maxLength)
        {
            if(Length > maxLength)
                Resize(maxLength);
        }

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
        public static double DistanceBetween(Vector firstVector, Vector secondVector)
        {
            return Math.Sqrt(Math.Pow(firstVector.X - secondVector.X, 2) + Math.Pow(firstVector.Y - secondVector.Y, 2));
        }

        /// <summary>
        /// Returns the squared distance between the given vectors.
        /// </summary>
        /// <param name="firstVector">The first vector.</param>
        /// <param name="secondVector">The second vector.</param>
        /// <returns></returns>
        public static double DistanceBetweenSquared(Vector firstVector, Vector secondVector)
        {
            return Math.Pow(firstVector.X - secondVector.X, 2) + Math.Pow(firstVector.Y - secondVector.Y, 2);
        }

        /// <summary>
        /// Gets the dot product of the given vectors.
        /// </summary>
        /// <param name="firstVector">The first vector.</param>
        /// <param name="secondVector">The second vector.</param>
        /// <returns>The dot product between the given vectors.</returns>
        public static double DotProduct(Vector firstVector, Vector secondVector)
        {
            return firstVector.X * secondVector.X + firstVector.Y * secondVector.Y;
        }

        /// <summary>
        /// Differences the to-from vector.
        /// </summary>
        /// <param name="to">To.</param>
        /// <param name="from">From.</param>
        /// <returns></returns>
        public static Vector Difference(Vector to, Vector from)
        {
            return new Vector(to.X - from.X, to.Y - from.Y);
        }

        /// <summary>
        /// Returns the sum of the given vectors.
        /// </summary>
        /// <param name="first">The first vector.</param>
        /// <param name="second">The second vector.</param>
        /// <returns>The sum of the given vectors.</returns>
        public static Vector Sum(Vector first, Vector second)
        {
            return new Vector(first.X + second.X, first.Y + second.Y);
        }

    }
}
