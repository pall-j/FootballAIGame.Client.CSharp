using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.SimulationEntities
{
    /// <summary>
    /// Provides the base class from which all moving game entities are derived.
    /// </summary>
    abstract class MovableEntity
    {
        /// <summary>
        /// Gets or sets the movement vector of the entity. It describes how entity's position
        /// changes in one simulation step.
        /// </summary>
        /// <value>
        /// The entity's movement vector.
        /// </value>
        public Vector Movement { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position <see cref="Vector"/>.
        /// </value>
        public Vector Position { get; set; }

        /// <summary>
        /// Gets the current speed in meters per simulation step.
        /// </summary>
        /// <value>
        /// The current speed in meters per simulation step.
        /// </value>
        public double CurrentSpeed
        {
            get { return Movement.Length; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MovableEntity"/> class.
        /// </summary>
        protected MovableEntity()
        {
            Movement = new Vector();
            Position = new Vector();
        }

        /// <summary>
        /// Predicts the position in time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>The predicted position <see cref="Vector"/>.</returns>
        public virtual Vector PredictPositionInTime(double time)
        {
            return Vector.GetSum(Position, Movement.GetMultiplied(time));
        }

    }
}
