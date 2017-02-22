using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class Wait : State<Player>
    {
        private Wait() { }

        private static Wait _instance;

        public static Wait Instance
        {
            get { return _instance ?? (_instance = new Wait()); }
        }

        public override void Run(Player player)
        {
        }
    }
}
