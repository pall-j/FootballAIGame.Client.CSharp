using System.Collections.Generic;
using System.Linq;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors
{
    class SteeringBehaviorsManager
    {
        private Player Player { get; set; }

        private SortedDictionary<int, List<SteeringBehavior>> SteeringBehaviors { get; set; }

        public SteeringBehaviorsManager(Player player)
        {
            SteeringBehaviors = new SortedDictionary<int, List<SteeringBehavior>>();
            Player = player;
        }

        public void AddBehavior(SteeringBehavior behavior)
        {
            List<SteeringBehavior> list;
            SteeringBehaviors.TryGetValue(behavior.Priority, out list);

            if (list != null)
                list.Add(behavior);
            else
                SteeringBehaviors.Add(behavior.Priority, new List<SteeringBehavior>() { behavior });
        }

        public void RemoveBehavior(SteeringBehavior behavior)
        {
            var list = SteeringBehaviors.FirstOrDefault(kv => kv.Key == behavior.Priority);
            if (list.Value != null)
                list.Value.Remove(behavior);
        }

        public void RemoveAllbehaviorsOfType<T>()
        {
            foreach (var keyValuePair in SteeringBehaviors)
            {
                var list = keyValuePair.Value;
                list.RemoveAll(sb => sb.GetType() == typeof(T));
            }
        }

        public List<SteeringBehavior> GetAllbehaviorsOfType<T>()
        {
            var result = new List<SteeringBehavior>();

            foreach (var keyValuePair in SteeringBehaviors)
            {
                var list = keyValuePair.Value;
                result.AddRange(list.Where(sb => sb.GetType() == typeof(T)));
            }

            return result;
        }

        public Vector CalculateAccelerationVector()
        {
            var acceleration = new Vector(0, 0);

            foreach (var keyValuePair in SteeringBehaviors)
            {
                var list = keyValuePair.Value;

                foreach (var steeringbehavior in list)
                {
                    var behaviorAccel = steeringbehavior.CalculateAccelerationVector();
                    behaviorAccel.Multiply(steeringbehavior.Weight);
                    acceleration = Vector.Sum(acceleration, behaviorAccel);
                }
            }

            return acceleration;
        }

    }
}
