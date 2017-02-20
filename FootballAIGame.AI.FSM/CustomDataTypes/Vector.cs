using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAIGameClient.CustomDataTypes
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
            Normalize();
            X *= newLength;
            Y *= newLength;
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
        /// Returns the distance between the given vectors squared.
        /// </summary>
        /// <param name="firstVector">The first vector.</param>
        /// <param name="secondVector">The second vector.</param>
        /// <returns></returns>
        public static double DistanceBetweenSquared(Vector firstVector, Vector secondVector)
        {
            return Math.Pow(firstVector.X - secondVector.X, 2) + Math.Pow(firstVector.Y - secondVector.Y, 2);
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
