using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors
{
    class Flee : SteeringBehavior
    {
        public Vector From { get; set; }

        public double SafeDistance { get; set; }

        public Flee(int priority, double weight, Vector from, double safeDistance) : base(priority, weight)
        {
            From = from;
            SafeDistance = safeDistance;
        }

        public override Vector CalculateAccelerationVector(Player player)
        {
            if (Vector.DistanceBetween(player.Position, From) >= SafeDistance)
                return new Vector(0, 0);

            var desiredMovemnet = Vector.Difference(player.Movement, From);

            if (Math.Abs(desiredMovemnet.LengthSquared) < 0.01)
                desiredMovemnet = new Vector(1, 0);

            desiredMovemnet.Resize(player.MaxSpeed);

            var acceleration = Vector.Difference(desiredMovemnet, player.Movement);
            if (acceleration.Length > player.MaxAcceleration)
                acceleration.Resize(player.MaxAcceleration);

            return acceleration; ;
        }
    }
}
