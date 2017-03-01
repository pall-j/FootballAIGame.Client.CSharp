using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    class Defender : FieldPlayer
    {
        public Defender(FootballPlayer player) : base(player)
        {
            StateMachine = new FiniteStateMachine<Player>(this, new Default(this), new DefenderGlobalState(this));
        }

        public override void ProcessMessage(Message message)
        {
            StateMachine.ProcessMessage(this, message);
        }

    }
}
