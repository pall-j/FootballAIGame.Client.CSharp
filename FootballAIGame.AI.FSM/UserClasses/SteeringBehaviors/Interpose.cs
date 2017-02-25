using System;
using System.Collections.Generic;
using System.Linq;
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

        public Interpose(Player player, int priority, double weight, 
            MovableEntity first, MovableEntity second) : base(player, priority, weight)
        {
            First = first;
            Second = second;
            Arrive = new Arrive(player, priority, weight, player.Position);
        }

        public override Vector CalculateAccelerationVector()
        {
            var midpoint = Vector.Sum(First.Position, Second.Position).Multiplied(1/2.0);
            var time = Player.TimeToGetToTarget(midpoint);
            var firstPredictedPos = First.PredictedPositionInTime(time);
            var secondPredictedPos = Second.PredictedPositionInTime(time);

            midpoint = Vector.Sum(firstPredictedPos, secondPredictedPos).Multiplied(1/2.0);
            Arrive.Target = midpoint;

            return Arrive.CalculateAccelerationVector();
        }
    }
}
