using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses
{
    class PlayerSteeringBehaviours
    {
        private Player Player { get; set; }

        public PlayerSteeringBehaviours(Player player)
        {
            this.Player = player;
        }
    }
}
