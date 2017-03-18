using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors
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
            PreferredDistanceFromSecond = Vector.DistanceBetween(Second.Position, First.Position)/2.0;
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
            PreferredDistanceFromSecond = Vector.DistanceBetween(Second.Position, First.Position) / 2.0;
        }

        public override Vector CalculateAccelerationVector()
        {

            var firstToSecond = new Vector(First.Position, Second.Position);
            var firstToPlayer = new Vector(First.Position, Player.Position);

            var firstToTargetDistance = Vector.DotProduct(firstToPlayer, firstToSecond)/firstToSecond.Length;

            if (firstToTargetDistance < 0 || firstToTargetDistance > firstToSecond.Length)
            {
                Arrive.Target = Vector.Sum(First.Position, firstToSecond.Multiplied(1/2.0)); // go to midpoint
                return Arrive.CalculateAccelerationVector();
            }

            Arrive.Target = Vector.Sum(First.Position, firstToSecond.Resized(firstToTargetDistance));

            var playerToTargetDistance = Vector.DistanceBetween(Arrive.Target, Player.Position);

            if (playerToTargetDistance < 0.01 && firstToSecond.Length > PreferredDistanceFromSecond)
            {
                // move player to meet DistanceFromSecond condition
                Arrive.Target = Vector.Sum(First.Position, firstToSecond.Resized(firstToSecond.Length - PreferredDistanceFromSecond));
            }


            return Arrive.CalculateAccelerationVector();
        }
    }
}
