using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    class Forward : FieldPlayer
    {
        public FiniteStateMachine<Forward> StateMachine { get; set; }

        public Forward(FootballPlayer player) : base(player)
        {
        }

        public override PlayerAction GetAction()
        {
            throw new NotImplementedException();
        }

        public override bool ProcessMessage(Message message)
        {
            return false;
        }
    }
}
