using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    abstract class FieldPlayer : Player
    {
        protected FieldPlayer(FootballPlayer player) : 
            base(player)
        {
        }
    }
}
