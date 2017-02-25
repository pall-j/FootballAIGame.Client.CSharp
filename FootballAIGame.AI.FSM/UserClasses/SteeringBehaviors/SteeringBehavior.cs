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
        protected Player Player { get; set; }

        public int Priority { get; set; }

        public double Weight { get; set; }

        public abstract Vector CalculateAccelerationVector();

        protected SteeringBehavior(Player player, int priority, double weight)
        {
            Priority = priority;
            Weight = weight;
            Player = player;
        }
    }
}
