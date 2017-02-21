using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses
{
    class PlayerSteeringBehaviours
    {
        private Player Player { get; set; }

        public Vector Target { get; set; }

        public bool SeekOn { get; set; }

        public PlayerSteeringBehaviours(Player player)
        {
            this.Player = player;
        }

        public Vector CalculateAccelerationVector()
        {
            var acceleration = new Vector();

            if (SeekOn)
                acceleration = Vector.Sum(acceleration, Seek());

            return acceleration;
        }

        private Vector Seek()
        {
            var acceleration = new Vector();

            if (Target == null) return acceleration;

            var desiredMovement = new Vector(Player.Position, Target, Player.MaxSpeed);
            acceleration = Vector.Difference(desiredMovement, Player.Movement);
            acceleration.Resize(Player.MaxAcceleration);

            return acceleration;
        }
    }
}
