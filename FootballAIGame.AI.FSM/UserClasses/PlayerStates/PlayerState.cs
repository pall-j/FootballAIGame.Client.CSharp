using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    abstract class PlayerState : State<Player>
    {
        protected Player Player { get; set; }

        protected PlayerState(Player player) : base(player)
        {
            Player = player;
        }
    }
}
