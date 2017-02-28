﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class KickBall : PlayerState
    {
        public KickBall(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            Ai.Instance.MyTeam.ControllingPlayer = Player;
            Run(); // run immediately
        }

        public override void Run()
        {
            var team = Ai.Instance.MyTeam;

            if (team.PassReceiver != null)
            {
                Player.StateMachine.ChangeState(new PursueBall(Player));
                return;
            }

            Vector target;
            if (team.TryGetShotOnGoal(Player, out target))
            {
                Player.KickBall(Ai.Instance.Ball, target);
                Player.StateMachine.ChangeState(new Default(Player));
                return;
            }

            Player passTarget;
            if (Player.IsInDanger && team.TryGetSafePass(Player, out passTarget))
            {
                Player.KickBall(Ai.Instance.Ball, passTarget.Position);
                MessageDispatcher.Instance.SendMessage(new ReceivePassMessage(passTarget.Position), passTarget);
                Player.StateMachine.ChangeState(new Default(Player));
                return;
            }

            Player.StateMachine.ChangeState(new Dribble(Player));
        }

        public override void Exit()
        {
            if (Ai.Instance.MyTeam.ControllingPlayer == Player)
                Ai.Instance.MyTeam.ControllingPlayer = null;
        }
    }
}