using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors
{
    class Wander : SteeringBehavior
    {
        private Random Random { get; set; }

        private Vector WanderTarget { get; set; }

        private Seek Seek { get; set; }

        public double WanderRadius { get; set; }

        public double WanderDistance { get; set; }

        public double WanderJitter { get; set; }

        public Wander(Player player, int priority, double weight, double wanderDistance, double wanderRadius,
            double wanderJitter) : base(player, priority, weight)
        {
            WanderDistance = wanderDistance;
            WanderRadius = wanderRadius;
            WanderJitter = wanderJitter;
            
            // initial wander target (in local space)
            WanderTarget = new Vector(WanderDistance + WanderRadius, 0);
            Seek = new Seek(player, priority, weight, player.Position);

        }

        public override Vector CalculateAccelerationVector()
        {
            // we are working in local space (Player heading = x-coordinate)

            var diff = new Vector((Ai.Random.NextDouble() - 0.5), (Ai.Random.NextDouble() - 0.5), WanderJitter);

            WanderTarget = Vector.Sum(WanderTarget, diff);
            WanderTarget.Resize(WanderRadius);
            WanderTarget = Vector.Sum(WanderTarget, new Vector(WanderDistance, 0));

            // change to world space
            var target = new Vector(WanderTarget.X, WanderTarget.Y);

            if (Player.CurrentSpeed > 0.001)
            {
                var alpha = -Math.Acos(Vector.DotProduct(Player.Movement.Normalized, new Vector(1, 0)));
                target.X = WanderTarget.X*Math.Cos(alpha) + WanderTarget.Y*Math.Sin(alpha);
                target.Y = -WanderTarget.X*Math.Sin(alpha) + WanderTarget.Y*Math.Cos(alpha);
            }

            target = Vector.Sum(Player.Position, target);

            Seek.Target = target;

            return Seek.CalculateAccelerationVector();
        }
    }
}
