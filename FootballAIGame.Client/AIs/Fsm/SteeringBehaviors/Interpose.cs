using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    class Interpose : SteeringBehavior
    {
        public MovableEntity First { get; set; }

        public MovableEntity Second { get; set; }

        private Arrive Arrive { get; set; }

        public double PreferredDistanceFromSecond { get; set; }

        public Interpose(Player player, int priority, double weight, 
            MovableEntity first, MovableEntity second) : base(player, priority, weight)
        {
            First = first;
            Second = second;
            Arrive = new Arrive(player, priority, weight, player.Position);
            PreferredDistanceFromSecond = Vector.GetDistanceBetween(Second.Position, First.Position)/2.0;
        }

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
