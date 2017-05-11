using System;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.SimulationEntities
{
    /// <summary>
    /// Represents the football ball in the simulation.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.SimulationEntities.MovableEntity" />
    class FootballBall : MovableEntity
    {
        /// <summary>
        /// The maximum allowed distance for kick to be successful.
        /// </summary>
        public const double MaxDistanceForKick = 2; // [m]

        /// <summary>
        /// Gets the ball's deceleration in meters per simulation step squared.
        /// </summary>
        /// <value>
        /// The ball's deceleration in meters per simulation step squared.
        /// </value>
        public static double BallDeceleration
        {
            get { return 1.5 * Math.Pow(GameClient.StepInterval / 1000.0, 2); }
        }

        /// <summary>
        /// Gets the time that is needed to cover the specified distance.
        /// </summary>
        /// <param name="distance">The distance.</param>
        /// <param name="kickPower">The kick power that would be applied.</param>
        /// <returns>The time that is need to cover the specified distance.</returns>
        public double GetTimeToCoverDistance(double distance, double kickPower)
        {
            var v0 = kickPower;
            var a = BallDeceleration;
            var s = distance;

            // s = v0*t - 1/2 at^2   --> at^2 - 2v0t + 2s = 0  (quadratic equation) 
            // smaller solution only - larger solution corresponds to the backwards movement
            var discriminant = 4*v0*v0 - 8*s*a;
            if (discriminant < 0)
                return double.PositiveInfinity; // ball will stop before target

            var t = (2*v0 - Math.Sqrt(discriminant))/(2*a);

            return t;
        }

        /// <summary>
        /// Predicts the position in time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>The predicted position <see cref="Vector"/>.</returns>
        public override Vector PredictPositionInTime(double time)
        {
            return PredictPositionInTimeAfterKick(time, Movement);
        }

        /// <summary>
        /// Predicts the position in time after kick.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <param name="kick">The kick.</param>
        /// <returns>The predicted position <see cref="Vector"/>.</returns>
        public Vector PredictPositionInTimeAfterKick(double time, Vector kick)
        {
            var kickSpeed = kick.Length;

            var finalSpeed = kickSpeed - BallDeceleration * time;

            if (Math.Abs(kickSpeed) < 0.001)
                return Position;

            if (finalSpeed < 0 || double.IsInfinity(time))
                time = kickSpeed / BallDeceleration; // time to stop

            var diff = Vector.GetSum(kick.GetResized(time * kickSpeed),
                kick.GetResized(-1 / 2.0 * BallDeceleration * time * time));

            return Vector.GetSum(Position, diff);
        }

    }
}
