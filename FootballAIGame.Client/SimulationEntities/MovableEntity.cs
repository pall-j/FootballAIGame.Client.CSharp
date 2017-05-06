using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.SimulationEntities
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

        protected MovableEntity()
        {
            Movement = new Vector();
            Position = new Vector();
        }

        public virtual Vector PredictPositionInTime(double time)
        {
            return Vector.GetSum(Position, Movement.GetMultiplied(time));
        }

    }
}
