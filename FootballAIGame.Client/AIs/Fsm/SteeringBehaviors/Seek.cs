using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    /// <summary>
    /// Represents the behavior where the player is running to the specified target with the
    /// maximum possible speed. When he reaches the target he might run across it because of
    /// the maximum deceleration limit (<see cref="Arrive"/> stops exactly at the target).
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.SteeringBehaviors.SteeringBehavior" />
    class Seek : SteeringBehavior
    {
        /// <summary>
        /// Gets or sets the target.
        /// </summary>
        /// <value>
        /// The target.
        /// </value>
        public Vector Target { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Seek"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="target">The target.</param>
        public Seek(Player player, int priority, double weight, Vector target) : 
            base(player, priority, weight)
        {
            Target = target;
        }

        /// <summary>
        /// Gets the current acceleration vector of the behavior.
        /// </summary>
        /// <returns>
        /// The acceleration <see cref="Vector" />.
        /// </returns>
        public override Vector GetAccelerationVector()
        {
            var acceleration = new Vector(0, 0);

            if (Target == null) return acceleration;

            var desiredMovement = Vector.GetDifference(Target, Player.Position);
            desiredMovement.Truncate(Player.MaxSpeed);

            acceleration = Vector.GetDifference(desiredMovement, Player.Movement);
            acceleration.Truncate(Player.MaxAcceleration);

            return acceleration;
        }

    }
}
