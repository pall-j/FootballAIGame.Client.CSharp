using System;
using FootballAIGame.AI.FSM.CustomDataTypes;

namespace FootballAIGame.AI.FSM.SimulationEntities
{
    class FootballBall : MovableEntity
    {
        public const double MinDistanceForKick = 2; // [m]

        /// <summary>
        /// Gets the ball's deceleration in meters per simulation step squared.
        /// </summary>
        /// <value>
        /// The ball's deceleration in meters per simulation step squared.
        /// </value>
        public static double BallDeceleration
        {
            get { return 1.5 * GameClient.StepInterval / 1000; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FootballBall"/> class.
        /// </summary>
        public FootballBall()
        {
            Position = new Vector();
            Movement = new Vector();
        }

        public double TimeToCoverDistance(double distance, double kickPower)
        {
            var v0 = kickPower;
            var a = BallDeceleration;
            var s = distance;

            var v1Squared = 2*a*s + v0*v0;
            if (v1Squared < 0)
                return double.PositiveInfinity;;
            var v1 = Math.Sqrt(v1Squared);

            var t = (v1 - v0)/a;

            return t;
        }

        public override Vector PredictedPositionInTime(double time)
        {
            var finalSpeed = CurrentSpeed - BallDeceleration*time;

            if (finalSpeed < 0)
                time = CurrentSpeed / BallDeceleration; // time to stop

            var diff = Vector.Sum(Movement.Multiplied(time),
                Movement.Resized(-1/2.0*BallDeceleration*time*time));

            return diff;
        }
    }
}
