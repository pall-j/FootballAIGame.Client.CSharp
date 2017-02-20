using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGameClient.SimulationEntities;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    abstract class Player : FootballPlayer
    {
        public abstract void Update();

        public abstract void OnMessage(Message message);

        protected Player(FootballPlayer player)
        {
            this.Position = player.Position;
            this.Movement = player.Movement;
            this.Kick = player.Kick;

            this.Speed = player.Speed;
            this.KickPower = player.KickPower;
            this.Possession = player.Possession;
            this.Precision = player.Precision;
        }
    }
}
