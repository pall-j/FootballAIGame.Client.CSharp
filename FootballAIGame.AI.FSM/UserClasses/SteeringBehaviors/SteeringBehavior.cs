using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors
{
    abstract class SteeringBehavior
    {
        public abstract Vector CalculateAccelerationVector(Player player);

        public int Priority { get; set; }

        public double Weight { get; set; }

        protected SteeringBehavior(int priority, double weight)
        {
            Priority = priority;
            Weight = weight;
        }
    }
}
