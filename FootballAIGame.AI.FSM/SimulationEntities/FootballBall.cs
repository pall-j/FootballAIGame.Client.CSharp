using System;
using FootballAIGame.AI.FSM.CustomDataTypes;

namespace FootballAIGame.AI.FSM.SimulationEntities
{
    class FootballBall : MovableEntity
    {
        public const double MinDistanceForKick = 1.5; // [m]

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

            // s = v0*t - 1/2 at^2   --> at^2 - 2v0t + 2s = 0  (quadratic equation) 
            // smaller solution only - larger solution corresponds to the backwards movement
            var discriminant = 4*v0*v0 - 8*s*a;
            if (discriminant < 0)
                return double.PositiveInfinity; // ball will stop before target

            var t = (2*v0 - Math.Sqrt(discriminant))/(2*a);

            return t;
        }

        public override Vector PredictedPositionInTime(double time)
        {
            var finalSpeed = CurrentSpeed - BallDeceleration*time;

            if (Math.Abs(CurrentSpeed) < 0.001)
                return Position;

            if (finalSpeed < 0)
                time = CurrentSpeed / BallDeceleration; // time to stop

            var diff = Vector.Sum(Movement.Multiplied(time),
                Movement.Resized(-1/2.0*BallDeceleration*time*time));

            return Vector.Sum(Position, diff);
        }

        public Vector PredictedPositionInTimeAfterKick(double time, double kickSpeed)
        {
            var finalSpeed = kickSpeed - BallDeceleration * time;

            if (Math.Abs(kickSpeed) < 0.001)
                return Position;

            if (finalSpeed < 0)
                time = kickSpeed / BallDeceleration; // time to stop

            var diff = Vector.Sum(Movement.Resized(time * kickSpeed),
                Movement.Resized(-1 / 2.0 * BallDeceleration * time * time));

            return Vector.Sum(Position, diff);
        }

    }
}
