using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors
{
    class Arrive : SteeringBehavior
    {
        public Vector Target { get; set; }

        public Arrive(int priority, double weight, Vector target) : base(priority, weight)
        {
            Target = target;
        }

        public override Vector CalculateAccelerationVector(Player player)
        {
            var distance = Vector.DistanceBetween(player.Position, Target);

            var desiredMovement = Vector.Difference(Target, player.Position);
            desiredMovement.Truncate(player.MaxSpeed);

            var acceleration = Vector.Difference(desiredMovement, player.Movement);
            acceleration.Truncate(player.MaxAcceleration);

            desiredMovement = Vector.Sum(player.Movement, acceleration);
            double speed = desiredMovement.Length;

            // calculation (k == 0 -> next step will be stop)
            var v0 = desiredMovement.Length;
            var v1 = 0;
            var a = -player.MaxAcceleration;
            var k = Math.Floor((v1 - v0)/a);
            if (v0 > 0 && v0 > -a && distance - desiredMovement.Length < k*v0 + a*k/2.0*(1 + k))
                speed = Math.Max(0, player.CurrentSpeed + a);

            speed = Math.Min(speed, player.MaxSpeed);

            if (desiredMovement.Length > 0.001)
                desiredMovement.Resize(speed);
            else
                desiredMovement = new Vector(0, 0);

            acceleration = Vector.Difference(desiredMovement, player.Movement);
            if (acceleration.Length > player.MaxAcceleration)
                acceleration.Resize(player.MaxAcceleration);

            return acceleration;
        }
    }
}
