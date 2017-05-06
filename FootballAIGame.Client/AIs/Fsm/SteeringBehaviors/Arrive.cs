using System;
using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    class Arrive : SteeringBehavior
    {
        public Vector Target { get; set; }

        public Arrive(Player player, int priority, double weight, Vector target) : 
            base(player, priority, weight)
        {
            Target = target;
        }

        public override Vector GetAccelerationVector()
        {
            var distance = Vector.GetDistanceBetween(Player.Position, Target);

            var desiredMovement = Vector.GetDifference(Target, Player.Position);
            desiredMovement.Truncate(Player.MaxSpeed);

            var acceleration = Vector.GetDifference(desiredMovement, Player.Movement);
            acceleration.Truncate(Player.MaxAcceleration);

            desiredMovement = Vector.GetSum(Player.Movement, acceleration);
            double speed = desiredMovement.Length;

            // calculation (k == 0 -> next step will be stop)
            var v0 = desiredMovement.Length;
            var v1 = 0;
            var a = -Player.MaxAcceleration;
            var k = Math.Floor((v1 - v0)/a);
            if (v0 > 0 && v0 > -a && distance - desiredMovement.Length < k*v0 + a*k/2.0*(1 + k))
                speed = Math.Max(0, Player.CurrentSpeed + a);

            speed = Math.Min(speed, Player.MaxSpeed);

            if (desiredMovement.Length > 0.001)
                desiredMovement.Resize(speed);
            else
                desiredMovement = new Vector(0, 0);

            acceleration = Vector.GetDifference(desiredMovement, Player.Movement);
            if (acceleration.Length > Player.MaxAcceleration)
                acceleration.Resize(Player.MaxAcceleration);

            return acceleration;
        }
    }
}
