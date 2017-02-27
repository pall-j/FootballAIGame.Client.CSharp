using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class Wait : PlayerState
    {
        public override void Run()
        {
        }

        public Wait(Player player) : base(player)
        {
        }
    }
}
