using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    /// <summary>
    /// Represents the behavior where the player is moving to a
    /// position between the specified entities.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.SteeringBehaviors.SteeringBehavior" />
    class Interpose : SteeringBehavior
    {
        /// <summary>
        /// Gets or sets the first entity.
        /// </summary>
        /// <value>
        /// The first <see cref="MovableEntity"/>.
        /// </value>
        public MovableEntity First { get; set; }

        /// <summary>
        /// Gets or sets the second entity.
        /// </summary>
        /// <value>
        /// The second <see cref="MovableEntity"/>.
        /// </value>
        public MovableEntity Second { get; set; }

        /// <summary>
        /// Gets or sets the preferred distance from the second entity.
        /// </summary>
        /// <value>
        /// The preferred distance from second.
        /// </value>
        public double PreferredDistanceFromSecond { get; set; }

        /// <summary>
        /// Gets or sets the arrive behavior, that is used to move to a position
        /// between the first and the second entity.
        /// </summary>
        /// <value>
        /// The arrive behavior.
        /// </value>
        private Arrive Arrive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpose"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="first">The first entity.</param>
        /// <param name="second">The second entity.</param>
        public Interpose(Player player, int priority, double weight, 
            MovableEntity first, MovableEntity second) : base(player, priority, weight)
        {
            First = first;
            Second = second;
            Arrive = new Arrive(player, priority, weight, player.Position);
            PreferredDistanceFromSecond = Vector.GetDistanceBetween(Second.Position, First.Position)/2.0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Interpose"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="priority">The priority.</param>
        /// <param name="weight">The weight.</param>
        /// <param name="first">The first entity.</param>
        /// <param name="secondPosition">The second position that is used instead of entity.
        /// The artificial second entity is created at this position.</param>
        public Interpose(Player player, int priority, double weight,
            MovableEntity first, Vector secondPosition) : base(player, priority, weight)
        {
            First = first;
            Second = new Ball(new FootballBall()) // artificial movable entity for representing second
            {
                Position = secondPosition
            };

            Arrive = new Arrive(player, priority, weight, player.Position);
            PreferredDistanceFromSecond = Vector.GetDistanceBetween(Second.Position, First.Position) / 2.0;
        }

        /// <summary>
        /// Gets the current acceleration vector of the behavior.
        /// </summary>
        /// <returns>
        /// The acceleration <see cref="Vector" />.
        /// </returns>
        public override Vector GetAccelerationVector()
        {

            var firstToSecond = new Vector(First.Position, Second.Position);
            var firstToPlayer = new Vector(First.Position, Player.Position);

            var firstToTargetDistance = Vector.GetDotProduct(firstToPlayer, firstToSecond)/firstToSecond.Length;

            if (firstToTargetDistance < 0 || firstToTargetDistance > firstToSecond.Length)
            {
                Arrive.Target = Vector.GetSum(First.Position, firstToSecond.GetMultiplied(1/2.0)); // go to midpoint
                return Arrive.GetAccelerationVector();
            }

            Arrive.Target = Vector.GetSum(First.Position, firstToSecond.GetResized(firstToTargetDistance));

            var playerToTargetDistance = Vector.GetDistanceBetween(Arrive.Target, Player.Position);

            if (playerToTargetDistance < 0.01 && firstToSecond.Length > PreferredDistanceFromSecond)
            {
                // move player to meet DistanceFromSecond condition
                Arrive.Target = Vector.GetSum(First.Position, firstToSecond.GetResized(firstToSecond.Length - PreferredDistanceFromSecond));
            }


            return Arrive.GetAccelerationVector();
        }
    }
}
