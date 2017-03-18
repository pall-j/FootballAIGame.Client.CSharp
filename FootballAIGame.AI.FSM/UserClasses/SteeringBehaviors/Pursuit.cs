using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors
{
    class Pursuit : SteeringBehavior
    {
        private Arrive TargetArrive { get; set; }

        public MovableEntity Target { get; set; }

        public Pursuit(Player player, int priority, double weight, MovableEntity target) : 
            base(player, priority, weight)
        {
            Target = target;
            TargetArrive = new Arrive(Player, priority, weight, target.Position);
        }

        public override Vector CalculateAccelerationVector()
        {
            var distance = Vector.DistanceBetween(Player.Position, Target.Position);

            double lookAheadTime = 0;
            if (Player.CurrentSpeed + Target.CurrentSpeed > 0)
                lookAheadTime = distance /(Player.CurrentSpeed + Target.CurrentSpeed);

            TargetArrive.Target = Target.PredictedPositionInTime(lookAheadTime);

            return TargetArrive.CalculateAccelerationVector();
        }
    }
}
