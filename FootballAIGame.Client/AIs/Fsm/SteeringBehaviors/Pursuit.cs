using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
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
