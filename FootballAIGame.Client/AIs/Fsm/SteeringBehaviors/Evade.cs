using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    /// <summary>
    /// Represents the behavior where player is running away from the specified movable entity.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.SteeringBehaviors.SteeringBehavior" />
    class Evade : SteeringBehavior
    {
        /// <summary>
        /// Gets or sets the target entity from which the player is running away.
        /// </summary>
        /// <value>
        /// The target <see cref="MovableEntity"/>.
        /// </value>
        public MovableEntity Target { get; set; }

        /// <summary>
        /// Gets or sets the safe distance. If this distance from the target is reached, then
        /// the behavior produces zero acceleration vector.
        /// </summary>
        /// <value>
        /// The safe distance.
        /// </value>
        public double SafeDistance
        {
            get { return FleeFromTarget.SafeDistance; }
            set { FleeFromTarget.SafeDistance = value; }
        }

        /// <summary>
        /// Gets or sets the flee behavior to flee from the current position of the target entity.
        /// </summary>
        /// <value>
        /// The flee from target behavior.
        /// </value>
        private Flee FleeFromTarget { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Evade"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="target">The target from which the player should run away.</param>
        /// <param name="safeDistance">The safe distance. If this distance from the target is reached, then
        /// the behavior produces zero acceleration vector.</param>
        public Evade(Player player, int priority, double weight, MovableEntity target, 
            double safeDistance) : base(player, priority, weight)
        {
            Target = target;
            FleeFromTarget = new Flee(player, priority, weight, Target.Position, safeDistance);
        }

        /// <summary>
        /// Gets the current acceleration vector of the behavior.
        /// </summary>
        /// <returns>
        /// The acceleration <see cref="Vector"/>.
        /// </returns>
        public override Vector GetAccelerationVector()
        {
            var distance = Vector.GetDistanceBetween(Player.Position, Target.Position);

            double lookAheadTime = 0;
            if (Player.CurrentSpeed + Target.CurrentSpeed > 0)
                lookAheadTime = distance / (Player.CurrentSpeed + Target.CurrentSpeed);

            var predictedPosition = Vector.GetSum(Target.Position,
                Target.Movement.GetMultiplied(lookAheadTime));

            FleeFromTarget.Target = predictedPosition;

            return FleeFromTarget.GetAccelerationVector();
        }
    }
}
