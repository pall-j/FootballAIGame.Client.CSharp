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
                return Vector.DistanceBetween(Ai.Instance.OpponentTeam.GetNearestPlayerToPosition(
                    Position).Position, Position) < Parameters.DangerRange;
            }
        }

        protected Player(FootballPlayer player) : base(player.Id)
        {
            this.Position = player.Position;
            this.Movement = player.Movement;
            this.KickVector = player.KickVector;

            this.Speed = player.Speed;
            this.KickPower = player.KickPower;
            this.Possession = player.Possession;
            this.Precision = player.Precision;

            SteeringBehaviorsManager = new SteeringBehaviorsManager(this);
        }

    }
}
