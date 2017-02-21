using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    class Defender : FieldPlayer
    {
        public FiniteStateMachine<Defender> StateMachine { get; set; }

        public PlayerAction Action { get; set; }

        public Defender(FootballPlayer player) : base(player)
        {
            Action = new PlayerAction();
            StateMachine = new FiniteStateMachine<Defender>(this, MoveToHomeRegion<Defender>.Instance, 
                PlayerStates.GlobalStates.DefenderGlobalState.Instance);
        }

        public override PlayerAction GetAction()
        {
            Action.Kick = new Vector(0, 0); // state machine might change it

            StateMachine.Update();

            Action.Movement = Vector.Sum(SteeringBehaviours.CalculateAccelerationVector(), Movement);

            return Action;
        }

        public override void ProcessMessage(Message message)
        {
            StateMachine.ProcessMessage(message);
        }

        public override void InitialStateEnter()
        {
            StateMachine.CurrentState.Enter(this);
        }
    }
}
