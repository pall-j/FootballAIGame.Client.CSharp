using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class KickBall : PlayerState
    {
        public KickBall(Player player) : base(player)
        {
        }

        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}
