using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors
{
    class Pursuit : SteeringBehavior
    {
        private Seek TargetSeek { get; set; }

        public FootballPlayer Target { get; set; }

        public Pursuit(int priority, double weight, FootballPlayer target) : base(priority, weight)
        {
            Target = target;
        }

        public override Vector CalculateAccelerationVector(Player player)
        {
            var distance = Vector.DistanceBetween(player.Position, Target.Position);

            double lookAheadTime = 0;
            if (player.CurrentSpeed + Target.CurrentSpeed > 0)
                lookAheadTime = distance /(player.CurrentSpeed + Target.CurrentSpeed);

            var predictedPosition = Vector.Sum(Target.Position, 
                Target.Movement.Multiplied(lookAheadTime));

            if (TargetSeek == null)
                TargetSeek = new Seek(Priority, Weight, predictedPosition);
            else
                TargetSeek.Target = predictedPosition;

            return TargetSeek.CalculateAccelerationVector(player);
        }
    }
}
