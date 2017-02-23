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
            var acceleration = new Vector();

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
                var acceleration = new Vector();

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
                var seek = Seek;
                if (desiredMovement.Length > Player.MaxSpeed)
                {
                    desiredMovement.Resize(Player.MaxSpeed);
                }

                if (distance > 1/2.0 * Math.Pow(Player.CurrentSpeed, 2)/Player.MaxAcceleration)
                    return seek;

                if (desiredMovement.Length > 0)
                {
                    var minusMovement = new Vector(new Vector(), desiredMovement,
                        -Player.MaxAcceleration);

                    desiredMovement.Resize(Player.CurrentSpeed);
                    desiredMovement = Vector.Sum(desiredMovement, minusMovement);

                    if (desiredMovement.Length < 0)
                        desiredMovement.Resize(0);
                }

                var acceleration = Vector.Difference(desiredMovement, Player.Movement);
                if (acceleration.Length > Player.MaxAcceleration)
                    acceleration.Resize(Player.MaxAcceleration);

                return acceleration;

                /*
                var accel = Seek;
                var accelTangent = accel.Y / accel.X;

                var s = Player.CurrentSpeed + 1/2.0*accelTangent + 1/2.0*
                        Math.Pow(accelTangent + Player.CurrentSpeed, 2)/Player.MaxAcceleration;

                if (s <= 1.01*Vector.DistanceBetween(Player.Position, Target) || accelTangent < 0)
                    return accel;

                // quadratic equation ax^2+bx+c=0
                var a = Math.Pow(1/Player.MaxAcceleration, 2);
                var b = 1/2.0 + 2*Player.CurrentSpeed/-Player.MaxAcceleration;
                var c = Player.CurrentSpeed
                        + Math.Pow(Player.CurrentSpeed, 2)/-Player.MaxAcceleration
                        - Vector.DistanceBetween(Player.Position, Target);
                var discrimant = b*b - 4*a*c;
                if (discrimant < 0)
                    Console.WriteLine("ERROR : Discriminant less than zero!");
                var result = (-b - Math.Sqrt(discrimant))/(2*a);

                return new Vector(1, result, Player.MaxAcceleration);
                */
            }
        }
    }
}
