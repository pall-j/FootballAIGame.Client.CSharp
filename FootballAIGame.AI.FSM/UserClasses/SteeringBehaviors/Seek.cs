using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors
{
    class Seek : SteeringBehavior
    {
        public Vector Target { get; set; }

        public override Vector CalculateAccelerationVector(Player player)
        {
            var acceleration = new Vector(0, 0);

            if (Target == null) return acceleration;

            var desiredMovement = Vector.Difference(Target, player.Position);
            if (desiredMovement.Length > player.MaxSpeed)
                desiredMovement.Resize(player.MaxSpeed);

            acceleration = Vector.Difference(desiredMovement, player.Movement);
            if (acceleration.Length > player.MaxAcceleration)
                acceleration.Resize(player.MaxAcceleration);

            return acceleration;
        }

        public Seek(int priority, double weight, Vector target) : base(priority, weight)
        {
            Target = target;
        }
    }
}
