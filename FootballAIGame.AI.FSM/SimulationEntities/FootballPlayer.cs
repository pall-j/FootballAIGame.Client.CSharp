using System;
using FootballAIGame.AI.FSM.CustomDataTypes;

namespace FootballAIGame.AI.FSM.SimulationEntities
{
    class FootballPlayer : MovableEntity
    {
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
        /// Gets or sets the kick vector of the player. It describes movement vector
        /// that ball would get if the kick was done with 100% precision.
        /// </summary>
        /// <value>
        /// The kick vector of the player.
        /// </value>
        public Vector KickVector { get; set; }

        public FootballPlayer(int id)
        {
            Id = id;
            Movement = new Vector();
            KickVector = new Vector();
        }

        /// <summary>
        /// Gets the maximum allowed speed of the player in meters per simulation step.
        /// </summary>
        /// <value>
        /// The maximum speed in meters per simulation step.
        /// </value>
        public double MaxSpeed
        {
            get { return (5 + Speed*2.5/0.4) * GameClient.StepInterval / 1000.0; }
        }

        /// <summary>
        /// Gets the maximum allowed acceleration in meters per simulation step squared of football player.
        /// </summary>
        /// <value>
        /// The maximum allowed acceleration in meters per simulation step squared of football player.
        /// </value>
        public double MaxAcceleration
        {
            get { return 3 * Math.Pow(GameClient.StepInterval / 1000.0, 2); }
        }

        /// <summary>
        /// Gets the maximum allowed kick speed in meters per simulation step of football player.
        /// </summary>
        /// <value>
        /// The maximum allowed kick speed in meters per simulation step of football player.
        /// </value>
        public double MaxKickSpeed
        {
            get { return (15 + KickPower*10) * GameClient.StepInterval / 1000.0; }
        }

        public bool CanKickBall(FootballBall ball)
        {
            return Vector.DistanceBetween(Position, ball.Position) <= FootballBall.MinDistanceForKick;
        }

        public void KickBall(FootballBall ball, Vector target)
        {
            
        }

        public static double DotProduct(Vector v1, Vector v2)
        {
            return v1.X*v2.X + v1.Y*v2.Y;
        }

        public double TimeToGetToTarget(Vector target)
        {
            // todo this is only approx. (continuous acceleration)

            var toTarget = Vector.Difference(target, Position);

            var v0 = Vector.DotProduct(toTarget, Movement) / toTarget.Length;
            var v1 = MaxSpeed;
            var a = MaxAcceleration;
            var t1 = (v1 - v0) / a;
            var s = Vector.DistanceBetween(Position, target);

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
