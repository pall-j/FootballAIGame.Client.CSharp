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

        public Flee(Player player, int priority, double weight, Vector from, 
            double safeDistance) : base(player, priority, weight)
        {
            From = from;
            SafeDistance = safeDistance;
        }

        public override Vector CalculateAccelerationVector()
        {
            if (Vector.DistanceBetween(Player.Position, From) >= SafeDistance)
                return new Vector(0, 0);

            var desiredMovemnet = Vector.Difference(Player.Movement, From);

            if (Math.Abs(desiredMovemnet.LengthSquared) < 0.01)
                desiredMovemnet = new Vector(1, 0);

            desiredMovemnet.Resize(Player.MaxSpeed);

            var acceleration = Vector.Difference(desiredMovemnet, Player.Movement);
            acceleration.Truncate(Player.MaxAcceleration);

            return acceleration; ;
        }
    }
}
