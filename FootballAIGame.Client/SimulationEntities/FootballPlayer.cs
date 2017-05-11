using System;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.SimulationEntities
{
    /// <summary>
    /// Represents the football player in the simulation.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.SimulationEntities.MovableEntity" />
    class FootballPlayer : MovableEntity
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the speed parameter of the player. <para />
        /// The max value is 0.4.
        /// </summary>
        /// <value>
        /// The speed parameter.
        /// </value>
        public float Speed { get; set; }

        /// <summary>
        /// Gets or sets the possession parameter of the player. <para />
        /// The maximum value should be 0.4.
        /// </summary>
        /// <value>
        /// The player's possession parameter.
        /// </value>
        public float Possession { get; set; }

        /// <summary>
        /// Gets or sets the precision parameter of the player. <para />
        /// The maximum value should be 0.4.
        /// </summary>
        /// <value>
        /// The player's precision parameter.
        /// </value>
        public float Precision { get; set; }

        /// <summary>
        /// Gets or sets the kick power parameter of the player. <para />
        /// The maximum value should be 0.4.
        /// </summary>
        /// <value>
        /// The player's kick power parameter.
        /// </value>
        public float KickPower { get; set; }

        /// <summary>
        /// Gets or sets the kick vector of the player. If describes movement vector
        /// that ball would get if the kick was done with 100% precision.
        /// </summary>
        /// <value>
        /// The <see cref="Vector"/> representing a kick.
        /// </value>
        public Vector KickVector { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FootballPlayer"/> class.
        /// </summary>
        /// <param name="id">The player's identifier.</param>
        public FootballPlayer(int id)
        {
            KickVector = new Vector();
            Id = id;
        }

        /// <summary>
        /// Gets the maximum allowed speed of the player in meters per simulation step.
        /// </summary>
        /// <value>
        /// The maximum speed in meters per simulation step.
        /// </value>
        public double MaxSpeed
        {
            get { return (4 + Speed*2/0.4) * GameClient.StepInterval / 1000.0; }
        }

        /// <summary>
        /// Gets the maximum allowed acceleration in meters per simulation step squared of football player.
        /// </summary>
        /// <value>
        /// The maximum allowed acceleration in meters per simulation step squared of football player.
        /// </value>
        public double MaxAcceleration
        {
            get { return 5 * Math.Pow(GameClient.StepInterval / 1000.0, 2); }
        }

        /// <summary>
        /// Gets the maximum allowed kick speed in meters per simulation step of football player.
        /// </summary>
        /// <value>
        /// The maximum allowed kick speed in meters per simulation step of football player.
        /// </value>
        public double MaxKickSpeed
        {
            get { return (15 + KickPower*5) * GameClient.StepInterval / 1000.0; }
        }

        /// <summary>
        /// Determines whether the player can kick the specified ball.
        /// </summary>
        /// <param name="ball">The ball.</param>
        /// <returns>
        ///   <c>true</c> if the player can kick the specified ball; otherwise, <c>false</c>.
        /// </returns>
        public bool CanKickBall(FootballBall ball)
        {
            return Vector.GetDistanceBetween(Position, ball.Position) <= FootballBall.MaxDistanceForKick;
        }

        /// <summary>
        /// Kicks the ball to the specified target with the maximum kick speed. Sets the <see cref="KickVector"/> accordingly.
        /// </summary>
        /// <param name="ball">The ball.</param>
        /// <param name="target">The kick target.</param>
        public void KickBall(FootballBall ball, Vector target)
        {
            KickBall(ball, target, MaxKickSpeed);
        }

        /// <summary>
        /// Kicks the ball to the specified target with the specified kick speed. Sets the kick vector accordingly.
        /// </summary>
        /// <param name="ball">The ball.</param>
        /// <param name="target">The kick target.</param>
        /// <param name="kickSpeed">The kick speed.</param>
        public void KickBall(FootballBall ball, Vector target, double kickSpeed)
        {
            if (kickSpeed > MaxKickSpeed)
                kickSpeed = MaxKickSpeed;
            KickVector = new Vector(ball.Position, target, kickSpeed);
        }

        /// <summary>
        /// Passes the specified ball to the specified player. Sets the kick vector accordingly.
        /// </summary>
        /// <param name="ball">The ball.</param>
        /// <param name="passTarget">The pass target player.</param>
        /// <returns>The kick target.</returns>
        public Vector PassBall(FootballBall ball, FootballPlayer passTarget)
        {
            var time = ball.GetTimeToCoverDistance(Vector.GetDistanceBetween(ball.Position, passTarget.Position), MaxKickSpeed);
            var nextPos = passTarget.PredictPositionInTime(time);
            KickBall(ball, nextPos);
            return nextPos;
        }

        /// <summary>
        /// Gets the approximated time to get to the specified position.
        /// </summary>
        /// <param name="target">The target position.</param>
        /// <returns>The approximated time to get to target.</returns>
        public double GetTimeToGetToTarget(Vector target)
        {
            // this is only approx. (continuous acceleration)

            var toTarget = Vector.GetDifference(target, Position);
            if (toTarget.Length < 0.001)
                return 0;

            var v0 = Vector.GetDotProduct(toTarget, Movement) / toTarget.Length;
            var v1 = MaxSpeed;
            var a = MaxAcceleration;
            var t1 = (v1 - v0) / a;
            var s = Vector.GetDistanceBetween(Position, target);

            var s1 = v0*t1 + 1/2.0*a*t1*t1; // distance traveled during acceleration
            if (s1 >= s) // target reached during acceleration
            {
                var discriminant = 4*v0*v0 + 8*a*s;
                return (-2*v0 + Math.Sqrt(discriminant))/(2*a);
            }

            var s2 = s - s1; // distance traveled during max speed
            var t2 = s2/v1;

            return t1 + t2;
        }

    }
}
