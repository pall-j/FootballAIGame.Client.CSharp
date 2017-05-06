using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    class Evade : SteeringBehavior
    {
        private Flee FleeFromTarget { get; set; }

        public MovableEntity Target { get; set; }

        public double SafeDistance
        {
            get { return FleeFromTarget.SafeDistance; }
            set { FleeFromTarget.SafeDistance = value; }
        }

        public Evade(Player player, int priority, double weight, MovableEntity target, 
            double safeDistance) : base(player, priority, weight)
        {
            Target = target;
            FleeFromTarget = new Flee(player, priority, weight, Target.Position, safeDistance);
        }

        public override Vector GetAccelerationVector()
        {
            var distance = Vector.GetDistanceBetween(Player.Position, Target.Position);

            double lookAheadTime = 0;
            if (Player.CurrentSpeed + Target.CurrentSpeed > 0)
                lookAheadTime = distance / (Player.CurrentSpeed + Target.CurrentSpeed);

            var predictedPosition = Vector.GetSum(Target.Position,
                Target.Movement.GetMultiplied(lookAheadTime));

            FleeFromTarget.From = predictedPosition;

            return FleeFromTarget.GetAccelerationVector();
        }
    }
}
