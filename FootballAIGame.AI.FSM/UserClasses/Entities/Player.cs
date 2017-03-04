using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    abstract class Player : FootballPlayer
    {
        public FiniteStateMachine<Player> StateMachine { get; set; }

        public Region HomeRegion { get; set; }

        public SteeringBehaviorsManager SteeringBehaviorsManager { get; set; }

        public PlayerAction GetAction()
        {
            var action = new PlayerAction
            {
                Movement = Vector.Sum(SteeringBehaviorsManager.CalculateAccelerationVector(), Movement),
                Kick = KickVector
            };

            return action;
        }

        public abstract void ProcessMessage(Message message);

        public bool IsAtHomeRegion
        {
            get
            {
                return Vector.DistanceBetween(HomeRegion.Center, Position) <= Parameters.PlayerInHomeRegionRange;
            }    
        }

        public bool IsInDanger
        {
            get
            {
                var nearest = Ai.Instance.OpponentTeam.GetNearestPlayerToPosition(Position);

                var predictedPosition = PredictedPositionInTime(1);
                var predictedNearest = Ai.Instance.OpponentTeam.GetPredictedNearestPlayerToPosition(predictedPosition, 1);

                return Vector.DistanceBetween(nearest.Position, Position) < Parameters.DangerRange ||
                       Vector.DistanceBetween(predictedNearest.Position, predictedPosition) < Parameters.DangerRange;
            }
        }

        protected Player(FootballPlayer player) : base(player.Id)
        {
            Position = player.Position;
            Movement = player.Movement;
            KickVector = player.KickVector;

            Speed = player.Speed;
            KickPower = player.KickPower;
            Possession = player.Possession;
            Precision = player.Precision;

            SteeringBehaviorsManager = new SteeringBehaviorsManager(this);
        }

    }
}
