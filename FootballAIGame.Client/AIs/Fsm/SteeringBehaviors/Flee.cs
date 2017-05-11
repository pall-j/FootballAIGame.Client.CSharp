using System;
using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    /// <summary>
    /// Represents the behavior where the player if running away from the specified target.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.SteeringBehaviors.SteeringBehavior" />
    class Flee : SteeringBehavior
    {
        /// <summary>
        /// Gets or sets the target from which the player is running away.
        /// </summary>
        /// <value>
        /// The target <see cref="Vector"/>.
        /// </value>
        public Vector Target { get; set; }
        
        /// <summary>
        /// Gets or sets the safe distance. If this distance from the target is reached, then
        /// the behavior produces zero acceleration vector.
        /// </summary>
        /// <value>
        /// The safe distance.
        /// </value>
        public double SafeDistance { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Flee"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="target">The target from which the player should run away.</param>
        /// <param name="safeDistance">The safe distance. If this distance from the target is reached, then
        /// the behavior produces zero acceleration vector.</param>
        public Flee(Player player, int priority, double weight, Vector target, 
            double safeDistance) : base(player, priority, weight)
        {
            Target = target;
            SafeDistance = safeDistance;
        }

        /// <summary>
        /// Gets the current acceleration vector of the behavior.
        /// </summary>
        /// <returns>
        /// The acceleration <see cref="Vector" />.
        /// </returns>
        public override Vector GetAccelerationVector()
        {
            if (Vector.GetDistanceBetween(Player.Position, Target) >= SafeDistance)
                return new Vector(0, 0);

            var desiredMovement = Vector.GetDifference(Player.Movement, Target);

            if (Math.Abs(desiredMovement.LengthSquared) < 0.01)
                desiredMovement = new Vector(1, 0);

            desiredMovement.Resize(Player.MaxSpeed);

            var acceleration = Vector.GetDifference(desiredMovement, Player.Movement);
            acceleration.Truncate(Player.MaxAcceleration);

            return acceleration;
        }
    }
}
