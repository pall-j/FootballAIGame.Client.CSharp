using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    /// <summary>
    /// Represents the behavior where the player is pursuing the specified movable entity and
    /// slows down smoothly as he approaches the entity.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.SteeringBehaviors.SteeringBehavior" />
    class Pursuit : SteeringBehavior
    {
        /// <summary>
        /// Gets or sets the arrive to the target behavior.
        /// </summary>
        /// <value>
        /// The arrive to the target behavior.
        /// </value>
        private Arrive TargetArrive { get; set; }

        /// <summary>
        /// Gets or sets the pursued target.
        /// </summary>
        /// <value>
        /// The pursued target.
        /// </value>
        public MovableEntity Target { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pursuit"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="target">The target.</param>
        public Pursuit(Player player, int priority, double weight, MovableEntity target) : 
            base(player, priority, weight)
        {
            Target = target;
            TargetArrive = new Arrive(Player, priority, weight, target.Position);
        }

        /// <summary>
        /// Gets the current acceleration vector of the behavior.
        /// </summary>
        /// <returns>
        /// The acceleration <see cref="Vector" />.
        /// </returns>
        public override Vector GetAccelerationVector()
        {
            var distance = Vector.GetDistanceBetween(Player.Position, Target.Position);

            double lookAheadTime = 0;
            if (Player.CurrentSpeed + Target.CurrentSpeed > 0)
                lookAheadTime = distance /(Player.CurrentSpeed + Target.CurrentSpeed);

            TargetArrive.Target = Target.PredictPositionInTime(lookAheadTime);

            return TargetArrive.GetAccelerationVector();
        }
    }
}
