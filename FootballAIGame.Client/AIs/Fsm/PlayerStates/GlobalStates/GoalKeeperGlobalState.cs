using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.Messaging.Messages;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates.GlobalStates
{
    class GoalKeeperGlobalState : PlayerState
    {
        private PlayerGlobalState PlayerGlobalState { get; set; }

        public GoalKeeperGlobalState(Player player, FsmAI footballAI) : base(player, footballAI)
        {
            PlayerGlobalState = new PlayerGlobalState(player, footballAI);
        }

        public override void Run()
        {
            if (Player.CanKickBall(AI.Ball))
            {
                Player passTargetPlayer;
                if (AI.MyTeam.TryGetSafePass(Player, out passTargetPlayer))
                {
                    var passTarget = Player.PassBall(AI.Ball, passTargetPlayer);
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
                        if (AI.MyTeam.IsKickSafe(Player, target))
                        {
                            Player.KickBall(AI.Ball, target);
                            safeDirectionFound = true;
                            break;
                        }
                    }

                    if (!safeDirectionFound)
                    {
                        // kick randomly
                        var target = new Vector(x, FsmAI.Random.Next(1, (int)GameClient.FieldHeight - 1));
                        Player.KickBall(AI.Ball, target);
                    }
                }
            }
            PlayerGlobalState.Run();
        }

        public override bool ProcessMessage(IMessage message)
        {
            if (message is ReceivePassMessage)
            {
                Player.StateMachine.ChangeState(new InterceptBall(Player, AI));
                return true;
            }
            if (message is GoDefaultMessage)
            {
                Player.StateMachine.ChangeState(new DefendGoal(Player, AI));
                return true;
            }

            return PlayerGlobalState.ProcessMessage(message);
        }
    }
}
