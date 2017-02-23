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
        private const double SlowingDownRadius = 10;

        private Player Player { get; set; }

        public Vector Target { get; set; }

        public bool SeekOn { get; set; }

        public bool ArriveOn { get; set; }

        public PlayerSteeringBehaviours(Player player)
        {
            this.Player = player;
        }

        public Vector CalculateAccelerationVector()
        {
            var acceleration = new Vector(0, 0);

            if (SeekOn)
                acceleration = Vector.Sum(acceleration, Seek);
            if (ArriveOn)
                acceleration = Vector.Sum(acceleration, Arrive);

            return acceleration;
        }

        private Vector Seek
        {
            get
            {
                var acceleration = new Vector(0, 0);

                if (Target == null) return acceleration;

                var desiredMovement = Vector.Difference(Target, Player.Position);
                if (desiredMovement.Length > Player.MaxSpeed)
                    desiredMovement.Resize(Player.MaxSpeed);

                acceleration = Vector.Difference(desiredMovement, Player.Movement);
                if (acceleration.Length > Player.MaxAcceleration)
                    acceleration.Resize(Player.MaxAcceleration);

                return acceleration;
            }
        }

        private Vector Arrive
        {
            get
            {
                var distance = Vector.DistanceBetween(Player.Position, Target);
                var desiredMovement = Vector.Difference(Target, Player.Position);
                double speed = Player.MaxSpeed;

                if ((distance - desiredMovement.Length) <= Math.Pow(Player.CurrentSpeed, 2) / (2 * Player.MaxAcceleration) ||
                    distance < Player.MaxAcceleration)
                    speed = Player.MaxSpeed * distance / SlowingDownRadius;

                speed = Math.Min(speed, Player.MaxSpeed);

                if (desiredMovement.Length > 0.01)
                    desiredMovement.Resize(speed);
                else
                    desiredMovement = new Vector(0, 0);

                var acceleration = Vector.Difference(desiredMovement, Player.Movement);
                if (acceleration.Length > Player.MaxAcceleration)
                    acceleration.Resize(Player.MaxAcceleration);

                return acceleration;

            }
        }
    }
}
