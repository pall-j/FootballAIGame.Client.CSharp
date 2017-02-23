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
        public const double SlowingDownRadius = 10;

        public Vector Target { get; set; }

        public Arrive(int priority, double weight, Vector target) : base(priority, weight)
        {
            Target = target;
        }

        public override Vector CalculateAccelerationVector(Player player)
        {
            var distance = Vector.DistanceBetween(player.Position, Target);
            var desiredMovement = Vector.Difference(Target, player.Position);
            if (desiredMovement.Length > player.MaxSpeed)
                desiredMovement.Resize(player.MaxSpeed);

            double speed = desiredMovement.Length;

            if ((distance - desiredMovement.Length) <= Math.Pow(player.CurrentSpeed, 2)/(2*player.MaxAcceleration)
                || distance < player.MaxAcceleration)
            {
                speed = Math.Min(speed, player.MaxSpeed*distance/SlowingDownRadius);
            }

            speed = Math.Min(speed, player.MaxSpeed);

            if (desiredMovement.Length > 0.0001)
                desiredMovement.Resize(speed);
            else
                desiredMovement = new Vector(0, 0);

            var acceleration = Vector.Difference(desiredMovement, player.Movement);
            if (acceleration.Length > player.MaxAcceleration)
                acceleration.Resize(player.MaxAcceleration);

            return acceleration;
        }
    }
}
