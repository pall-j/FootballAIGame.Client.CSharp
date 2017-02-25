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

        public Seek(Player player, int priority, double weight, Vector target) : 
            base(player, priority, weight)
        {
            Target = target;
        }

        public override Vector CalculateAccelerationVector()
        {
            var acceleration = new Vector(0, 0);

            if (Target == null) return acceleration;

            var desiredMovement = Vector.Difference(Target, Player.Position);
            desiredMovement.Truncate(Player.MaxSpeed);

            acceleration = Vector.Difference(desiredMovement, Player.Movement);
            acceleration.Truncate(Player.MaxAcceleration);

            return acceleration;
        }

    }
}
