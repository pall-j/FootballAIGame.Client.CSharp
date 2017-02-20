using System;
using System.Collections.Generic;
using System.Text;
using FootballAIGameClient.CustomDataTypes;

namespace FootballAIGameClient.SimulationEntities
{
    class Ball
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
        /// Initializes a new instance of the <see cref="Ball"/> class.
        /// </summary>
        public Ball()
        {
            Position = new Vector();
            Movement = new Vector();
        }

    }
}
