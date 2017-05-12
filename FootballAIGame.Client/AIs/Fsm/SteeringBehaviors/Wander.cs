using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    /// <summary>
    /// Represents the behavior where the player is wandering around the specified position.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.SteeringBehaviors.SteeringBehavior" />
    class Wander : SteeringBehavior
    {
        /// <summary>
        /// Gets or sets the wander radius.
        /// </summary>
        /// <value>
        /// The wander radius.
        /// </value>
        public double WanderRadius { get; set; }

        /// <summary>
        /// Gets or sets the wander distance.
        /// </summary>
        /// <value>
        /// The wander distance.
        /// </value>
        public double WanderDistance { get; set; }

        /// <summary>
        /// Gets or sets the wander jitter.
        /// </summary>
        /// <value>
        /// The wander jitter.
        /// </value>
        public double WanderJitter { get; set; }

        /// <summary>
        /// Gets or sets the wander target.
        /// </summary>
        /// <value>
        /// The wander target.
        /// </value>
        private Vector WanderTarget { get; set; }

        /// <summary>
        /// Gets or sets the seek to the wander target.
        /// </summary>
        /// <value>
        /// The seek to the wander target.
        /// </value>
        private Seek Seek { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Wander"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="wanderDistance">The wander distance.</param>
        /// <param name="wanderRadius">The wander radius.</param>
        /// <param name="wanderJitter">The wander jitter.</param>
        public Wander(Player player, int priority, double weight, double wanderDistance, double wanderRadius,
            double wanderJitter) : base(player, priority, weight)
        {
            WanderDistance = wanderDistance;
            WanderRadius = wanderRadius;
            WanderJitter = wanderJitter;
            
            // initial wander target (in local space)
            WanderTarget = new Vector(WanderDistance + WanderRadius, 0);
            Seek = new Seek(player, priority, weight, player.Position);

        }

        /// <summary>
        /// Gets the current acceleration vector of the behavior.
        /// </summary>
        /// <returns>
        /// The acceleration <see cref="Vector" />.
        /// </returns>
        public override Vector GetAccelerationVector()
        {
            // we are working in local space (Player heading = x-coordinate)

            var diff = new Vector((FsmAI.Random.NextDouble() - 0.5), (FsmAI.Random.NextDouble() - 0.5), WanderJitter);

            WanderTarget = Vector.GetSum(WanderTarget, diff);
            WanderTarget.Resize(WanderRadius);
            WanderTarget = Vector.GetSum(WanderTarget, new Vector(WanderDistance, 0));

            // change to world space
            var target = new Vector(WanderTarget.X, WanderTarget.Y);

            if (Player.CurrentSpeed > 0.001)
            {
                var m = Player.Movement.Normalized;

                target.X = WanderTarget.X*m.X - WanderTarget.Y*m.Y;
                target.Y = WanderTarget.X*m.Y + WanderTarget.Y*m.X;
            }

            target = Vector.GetSum(Player.Position, target);

            Seek.Target = target;

            return Seek.GetAccelerationVector();
        }
    }
}
