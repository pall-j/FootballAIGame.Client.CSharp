using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    abstract class Player : FootballPlayer
    {
        public Region HomeRegion { get; set; }

        public PlayerSteeringBehaviours SteeringBehaviours { get; set; }

        public abstract PlayerAction GetAction();

        public abstract void ProcessMessage(Message message);

        protected Player(FootballPlayer player)
        {
            this.Position = player.Position;
            this.Movement = player.Movement;
            this.Kick = player.Kick;

            this.Speed = player.Speed;
            this.KickPower = player.KickPower;
            this.Possession = player.Possession;
            this.Precision = player.Precision;

            SteeringBehaviours = new PlayerSteeringBehaviours(this);
        }

        public abstract void InitialStateEnter();
    }
}
