using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;

namespace FootballAIGame.AI.FSM.SimulationEntities
{
    abstract class MovableEntity
    {
        /// <summary>
        /// Gets or sets the movement vector of the entity. It describes how entity's position will
        /// change in one simulation step.
        /// </summary>
        /// <value>
        /// The entity's movement vector.
        /// </value>
        public Vector Movement { get; set; }

        /// <summary>
        /// Gets or sets the position of the entity.
        /// </summary>
        /// <value>
        /// The entity's position.
        /// </value>
        public Vector Position { get; set; }

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

        public virtual Vector PredictedPositionInTime(double time)
        {
            return Vector.Sum(Position, Movement.Multiplied(time));
        }
    }
}
