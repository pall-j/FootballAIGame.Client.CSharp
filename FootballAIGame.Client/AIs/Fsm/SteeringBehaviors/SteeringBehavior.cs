using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    /// <summary>
    /// Provides the base class from which the classes that represent steering behaviors are derived.
    /// </summary>
    abstract class SteeringBehavior
    {
        /// <summary>
        /// Gets or sets the priority. Affects the combination of behaviors in <see cref="SteeringBehaviorsManager"/>.
        /// The behaviors with higher priority are preferred.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets the weight. Affects the combination of behaviors in <see cref="SteeringBehaviorsManager"/>.
        /// The acceleration vector of this behavior is multiplied by the weight in the combination.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the player to whom this instance belongs.
        /// </summary>
        /// <value>
        /// The player to whom this instance belongs.
        /// </value>
        protected Player Player { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteeringBehavior"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="weight">The weight.</param>
        protected SteeringBehavior(Player player, int priority, double weight)
        {
            Player = player;
            Priority = priority;
            Weight = weight;
        }

        /// <summary>
        /// Gets the current acceleration vector of the behavior.
        /// </summary>
        /// <returns>The acceleration <see cref="Vector"/>.</returns>
        public abstract Vector GetAccelerationVector();
    }
}
