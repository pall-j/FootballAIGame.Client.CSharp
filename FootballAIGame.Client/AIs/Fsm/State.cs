using FootballAIGame.Client.AIs.Fsm.Messaging;

namespace FootballAIGame.Client.AIs.Fsm
{
    /// <summary>
    /// Provides the base class from which the classes that represent states are derived.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity to which the state belongs.</typeparam>
    abstract class State<TEntity>
    {
        /// <summary>
        /// Gets or sets the entity to which this instance belongs.
        /// </summary>
        /// <value>
        /// The <see cref="TEntity"/> to which this instance belongs.
        /// </value>
        protected TEntity Entity { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FsmAI"/> instance to which this instance belongs.
        /// </summary>
        /// <value>
        /// The <see cref="FsmAI"/> instance to which this instance belongs.
        /// </value>
        protected FsmAI AI { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="State{TEntity}"/> class.
        /// </summary>
        /// <param name="entity">The entity to which this instance belongs.</param>
        /// <param name="footballAI">The <see cref="FsmAI"/> instance to which this state belongs.</param>
        protected State(TEntity entity, FsmAI footballAI)
        {
            Entity = entity;
            AI = footballAI;
        }

        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public virtual void Enter() { }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Occurs when the entity leaves this state.
        /// </summary>
        public virtual void Exit() { }

        /// <summary>
        /// Processes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>true</c> if the specified message was handled; otherwise, <c>false</c>
        /// </returns>
        public virtual bool ProcessMessage(IMessage message)
        {
            return false;
        }
    }
}
