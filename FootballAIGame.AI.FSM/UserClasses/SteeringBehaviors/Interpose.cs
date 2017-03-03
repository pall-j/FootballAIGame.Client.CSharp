using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors
{
    class Interpose : SteeringBehavior
    {
        private MovableEntity First { get; set; }

        private MovableEntity Second { get; set; }

        private Arrive Arrive { get; set; }

        public double DistanceFromSecond { get; set; }

        public Interpose(Player player, int priority, double weight, 
            MovableEntity first, MovableEntity second) : base(player, priority, weight)
        {
            First = first;
            Second = second;
            Arrive = new Arrive(player, priority, weight, player.Position);
            DistanceFromSecond = Vector.DistanceBetween(Second.Position, First.Position)/2.0;
        }

        public Interpose(Player player, int priority, double weight,
            MovableEntity first, Vector secondPosition) : base(player, priority, weight)
        {
            First = first;
            Second = new Ball(new FootballBall()) // artificial movable entity for representing second
            {
                Position =
                {
                    X = secondPosition.X,
                    Y = secondPosition.Y
                }
            };
            Arrive = new Arrive(player, priority, weight, player.Position);
            DistanceFromSecond = Vector.DistanceBetween(Second.Position, First.Position) / 2.0;
        }

        public override Vector CalculateAccelerationVector()
        {
            /*
            var midpoint = Vector.Sum(First.Position, Second.Position).Multiplied(1/2.0);
            var time = Player.TimeToGetToTarget(midpoint);
            var firstPredictedPos = First.PredictedPositionInTime(time);
            var secondPredictedPos = Second.PredictedPositionInTime(time);

            midpoint = Vector.Sum(firstPredictedPos, secondPredictedPos).Multiplied(1/2.0);
            Arrive.Target = midpoint;
            */

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

            if (playerToTargetDistance < 0.01 && firstToSecond.Length > DistanceFromSecond)
            {
                // move player to meet DistanceFromSecond condition
                Arrive.Target = Vector.Sum(First.Position, firstToSecond.Resized(firstToSecond.Length - DistanceFromSecond));
            }


            return Arrive.CalculateAccelerationVector();
        }
    }
}
