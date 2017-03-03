using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class GoalKeeperGlobalState : PlayerState
    {
        private PlayerGlobalState PlayerGlobalState { get; set; }

        public GoalKeeperGlobalState(Player player) : base(player)
        {
            PlayerGlobalState = new PlayerGlobalState(player);
        }

        public override void Run()
        {
            if (Player.CanKickBall(Ai.Instance.Ball))
            {
                Player passTargetPlayer;
                if (Ai.Instance.MyTeam.TryGetSafePass(Player, out passTargetPlayer))
                {
                    var passTarget = Player.PassBall(Ai.Instance.Ball, passTargetPlayer);
                    MessageDispatcher.Instance.SendMessage(new ReceivePassMessage(passTarget), passTargetPlayer);
                }
                else
                {
                    // find a safe direction and kick the ball there
                    var x = GameClient.FieldWidth / 2;
                    var safeDirectionFound = false;

                    for (int y = 10; y < GameClient.FieldHeight; y += 5)
                    {
                        var target = new Vector(x, y);
                        if (Ai.Instance.MyTeam.IsKickSafe(Player, target))
                        {
                            Player.KickBall(Ai.Instance.Ball, target);
                            safeDirectionFound = true;
                            break;
                        }
                    }

                    if (!safeDirectionFound)
                    {
                        // kick randomly
                        var target = new Vector(x, Ai.Random.Next(1, (int)GameClient.FieldHeight - 1));
                        Player.KickBall(Ai.Instance.Ball, target);
                    }
                }
            }
            PlayerGlobalState.Run();
        }

        public override bool ProcessMessage(Message message)
        {
            if (message is ReceivePassMessage)
            {
                Player.StateMachine.ChangeState(new InterceptBall(Player));
                return true;
            }
            if (message is GoDefaultMessage)
            {
                Player.StateMachine.ChangeState(new DefendGoal(Player));
                return true;
            }

            return PlayerGlobalState.ProcessMessage(message);
        }
    }
}
