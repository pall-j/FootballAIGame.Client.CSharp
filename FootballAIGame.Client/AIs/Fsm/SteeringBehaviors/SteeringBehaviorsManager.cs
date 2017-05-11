using System.Collections.Generic;
using System.Linq;
using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.SteeringBehaviors
{
    /// <summary>
    /// Responsible for managing player's steering behaviors.
    /// Provides methods for adding and removing steering behavior of the player. 
    /// Combines the behaviors in accordance with their priority and weight.
    /// </summary>
    class SteeringBehaviorsManager
    {
        /// <summary>
        /// Gets or sets the player to whom this instance belongs.
        /// </summary>
        /// <value>
        /// The player to whom this instance belongs..
        /// </value>
        private Player Player { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="SortedDictionary{TKey,TValue}"/> that holds the active steering behaviors with keys
        /// equal to the behaviors' priorities.
        /// </summary>
        /// <value>
        /// The <see cref="SortedDictionary{TKey,TValue}"/> with keys equal to the behavior's priorities and
        /// values containing the <see cref="SteeringBehavior"/>s.
        /// </value>
        private SortedDictionary<int, List<SteeringBehavior>> SteeringBehaviors { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SteeringBehaviorsManager"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        public SteeringBehaviorsManager(Player player)
        {
            SteeringBehaviors = new SortedDictionary<int, List<SteeringBehavior>>();
            Player = player;
        }

        /// <summary>
        /// Adds the specified behavior.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        public void AddBehavior(SteeringBehavior behavior)
        {
            List<SteeringBehavior> list;
            SteeringBehaviors.TryGetValue(behavior.Priority, out list);

            if (list != null)
                list.Add(behavior);
            else
                SteeringBehaviors.Add(behavior.Priority, new List<SteeringBehavior>() { behavior });
        }

        /// <summary>
        /// Removes the specified behavior.
        /// </summary>
        /// <param name="behavior">The behavior.</param>
        public void RemoveBehavior(SteeringBehavior behavior)
        {
            List<SteeringBehavior> list;
            if (SteeringBehaviors.TryGetValue(behavior.Priority, out list))
                list.Remove(behavior);
        }

        /// <summary>
        /// Removes all behaviors of the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        public void RemoveAllBehaviorsOfType<T>()
        {
            foreach (var keyValuePfootballAIr in SteeringBehaviors)
            {
                var list = keyValuePfootballAIr.Value;
                list.RemoveAll(sb => sb.GetType() == typeof(T));
            }
        }

        /// <summary>
        /// Gets all behaviors of the specified type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns><see cref="List{T}"/> of all active <see cref="SteeringBehavior"/>s of the specified type.</returns>
        public List<SteeringBehavior> GetAllBehaviorsOfType<T>()
        {
            var result = new List<SteeringBehavior>();

            foreach (var keyValuePfootballAIr in SteeringBehaviors)
            {
                var list = keyValuePfootballAIr.Value;
                result.AddRange(list.Where(sb => sb.GetType() == typeof(T)));
            }

            return result;
        }

        /// <summary>
        /// Gets the current acceleration vector by combining the active behaviors accordingly.
        /// </summary>
        /// <returns>The acceleration <see cref="Vector"/>.</returns>
        public Vector GetAccelerationVector()
        {
            // Weighted Prioritized Truncated Sum method used

            var acceleration = new Vector(0, 0);
            var accelerationRemfootballAIning = Player.MaxAcceleration;

            foreach (var keyValuePfootballAIr in SteeringBehaviors)
            {
                var list = keyValuePfootballAIr.Value;

                foreach (var steeringbehavior in list)
                {
                    var behaviorAccel = steeringbehavior.GetAccelerationVector();
                    behaviorAccel.Multiply(steeringbehavior.Weight);

                    if (accelerationRemfootballAIning - behaviorAccel.Length < 0)
                        behaviorAccel.Resize(accelerationRemfootballAIning);
                    accelerationRemfootballAIning -= behaviorAccel.Length;

                    acceleration = Vector.GetSum(acceleration, behaviorAccel);

                    if (accelerationRemfootballAIning <= 0)
                        break;
                }

                if (accelerationRemfootballAIning <= 0)
                    break;
            }

            var nextMovement = Vector.GetSum(Player.Movement, acceleration);
            nextMovement.Truncate(Player.MaxSpeed);
            acceleration = Vector.GetDifference(nextMovement, Player.Movement);

            return acceleration;
        }

        /// <summary>
        /// Removes all active behaviors.
        /// </summary>
        public void Reset()
        {
            SteeringBehaviors = new SortedDictionary<int, List<SteeringBehavior>>();
        }
    }
}
