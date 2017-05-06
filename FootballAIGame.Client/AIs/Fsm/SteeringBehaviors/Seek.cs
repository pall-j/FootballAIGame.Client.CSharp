using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    class Seek : SteeringBehavior
    {
        public Vector Target { get; set; }

        public Seek(Player player, int priority, double weight, Vector target) : 
            base(player, priority, weight)
        {
            Target = target;
        }

        public override Vector GetAccelerationVector()
        {
            var acceleration = new Vector(0, 0);

            if (Target == null) return acceleration;

            var desiredMovement = Vector.GetDifference(Target, Player.Position);
            desiredMovement.Truncate(Player.MaxSpeed);

            acceleration = Vector.GetDifference(desiredMovement, Player.Movement);
            acceleration.Truncate(Player.MaxAcceleration);

            return acceleration;
        }

    }
}
