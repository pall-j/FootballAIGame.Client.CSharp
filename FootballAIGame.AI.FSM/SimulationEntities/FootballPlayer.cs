using FootballAIGame.AI.FSM.CustomDataTypes;

namespace FootballAIGame.AI.FSM.SimulationEntities
{
    class FootballPlayer
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
        /// Gets or sets the position of the player.
        /// </summary>
        /// <value>
        /// The player's position.
        /// </value>
        public Vector Position { get; set; }

        /// <summary>
        /// Gets or sets the movement vector of the player. It describes how player position will
        /// change in one simulation step.
        /// </summary>
        /// <value>
        /// The player's movement vector.
        /// </value>
        public Vector Movement { get; set; }

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
        /// Gets the current speed in meters per simulation step.
        /// </summary>
        /// <value>
        /// The current speed in meters per second.
        /// </value>
        public double CurrentSpeed
        {
            get { return Movement.Length; }
        }

        /// <summary>
        /// Gets the maximum allowed speed of the player in meters per simulation step.
        /// </summary>
        /// <value>
        /// The maximum speed in meters per simulation step.
        /// </value>
        public double MaxSpeed
        {
            get { return (5 + Speed*2.5/0.4) * GameClient.StepInterval / 1000; }
        }

        /// <summary>
        /// Gets the maximum allowed acceleration in meters per simulation step squared of football player.
        /// </summary>
        /// <value>
        /// The maximum allowed acceleration in meters per simulation step squared of football player.
        /// </value>
        public double MaxAcceleration
        {
            get { return 3 * GameClient.StepInterval / 1000.0; }
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
    }
}
