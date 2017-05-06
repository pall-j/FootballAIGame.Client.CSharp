using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    abstract class SteeringBehavior
    {
        protected Player Player { get; set; }

        public int Priority { get; set; }

        public double Weight { get; set; }

        public abstract Vector GetAccelerationVector();

        protected SteeringBehavior(Player player, int priority, double weight)
        {
            Player = player;
            Priority = priority;
            Weight = weight;
        }
    }
}
