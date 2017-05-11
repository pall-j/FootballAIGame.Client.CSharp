using FootballAIGame.Client.AIs.Fsm.Messaging;

namespace FootballAIGame.Client.AIs.Fsm
{
    /// <summary>
    /// Represents the finite state machine.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to which this state machine belongs.</typeparam>
    class FiniteStateMachine<TEntity>
    {
        /// <summary>
        /// Gets the current state of the entity.
        /// </summary>
        /// <value>
        /// The current state of the entity.
        /// </value>
        public State<TEntity> CurrentState { get; private set; }

        /// <summary>
        /// Gets the global state of the entity.
        /// </summary>
        /// <value>
        /// The global state of the entity.
        /// </value>
        public State<TEntity> GlobalState { get; private set; }

        /// <summary>
        /// Gets or sets the entity to which this instance belongs.
        /// </summary>
        /// <value>
        /// The <see cref="TEntity"/> to which this instance belongs.
        /// </value>
        public TEntity Entity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FiniteStateMachine{TEntity}"/> class.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="startState">The entity's initial state.</param>
        /// <param name="globalState">The entity's global state.</param>
        public FiniteStateMachine(TEntity entity, State<TEntity> startState, State<TEntity> globalState)
        {
            Entity = entity;
            CurrentState = startState;
            GlobalState = globalState;
        }

        /// <summary>
        /// Changes the entity's current state. Calls the states' enter and exit methods accordingly.
        /// </summary>
        /// <param name="newState">The new state.</param>
        public void ChangeState(State<TEntity> newState)
        {
            if (CurrentState != null)
                CurrentState.Exit();

            CurrentState = newState;
            CurrentState.Enter();
        }

        /// <summary>
        /// Updates the state machine. Should be called every simulation step after the game state is loaded and before the
        /// entity's action is retrieved.
        /// </summary>
        public void Update()
        {
            if (GlobalState != null)
                GlobalState.Run();
            if (CurrentState != null)
                CurrentState.Run();
        }

        /// <summary>
        /// Processes the specified message.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="message">The message.</param>
        public void ProcessMessage(TEntity entity, IMessage message)
        {
            if (CurrentState != null && CurrentState.ProcessMessage(message))
                return;

            if (GlobalState != null)
                GlobalState.ProcessMessage(message);
        }
    }
}
