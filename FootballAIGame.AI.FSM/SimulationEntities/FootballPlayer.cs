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
        public Vector Kick { get; set; }

        public FootballPlayer(int id)
        {
            Id = id;
            Movement = new Vector();
            Kick = new Vector();
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
            get { return (15 + KickPower*10) * GameClient.StepInterval / 1000; }
        }

        public static double DotProduct(Vector v1, Vector v2)
        {
            return v1.X*v2.X + v1.Y*v2.Y;
        }

        public double TimeToGetToTarget(Vector target)
        {
            // todo this is only approx. (continuous acceleration)

            var v0 = Vector.DotProduct(target, Movement);
            var v1 = MaxSpeed;
            var a = MaxAcceleration;
            var t1 = (v1 - v0) / a;
            var s = Vector.DistanceBetween(Position, target);

            return (s - v0 * t1 - 1 / 2.0 * t1 * t1 + v1 * t1) / v1;
        }

    }
}
