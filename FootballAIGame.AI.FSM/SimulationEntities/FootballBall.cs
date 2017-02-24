using FootballAIGame.AI.FSM.CustomDataTypes;

namespace FootballAIGame.AI.FSM.SimulationEntities
{
    class FootballBall
    {

        /// <summary>
        /// Gets or sets the position of the ball.
        /// </summary>
        /// <value>
        /// The ball's position.
        /// </value>
        public Vector Position { get; set; }

        /// <summary>
        /// Gets or sets the movement vector of the ball. It describes how ball position
        /// changes in one simulation step.
        /// </summary>
        /// <value>
        /// The ball's movement vector.
        /// </value>
        public Vector Movement { get; set; }

        /// <summary>
        /// Gets the ball's current speed in meters per simulation step.
        /// </summary>
        /// <value>
        /// The ball's current speed in meters per simulation step.
        /// </value>
        public double CurrentSpeed
        {
            get { return Movement.Length; }
        }

        /// <summary>
        /// Gets the ball's deceleration in meters per simulation step squared.
        /// </summary>
        /// <value>
        /// The ball's deceleration in meters per simulation step squared.
        /// </value>
        public static double BallDeceleration
        {
            get { return 1.5*GameClient.StepInterval/1000; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FootballBall"/> class.
        /// </summary>
        public FootballBall()
        {
            Position = new Vector();
            Movement = new Vector();
        }

        public double TimeToCoverDistance(double kickPower, double distance)
        {


            return 0.0;
        }

    }
}
