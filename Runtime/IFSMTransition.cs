using System;

namespace Shardy.FSM {

    /// <summary>
    /// Interface for FSM transition
    /// </summary>
    /// <typeparam name="TS">Types of states</typeparam>
    /// <typeparam name="TT">Types of triggers</typeparam>
    public interface IFSMTransition<TS, TT> : IFSMState<TS, TT> {
        IFSMTransition<TS, TT> On(TT trigger);
        IFSMTransition<TS, TT> If(Func<FSMTransitionCondition<TS>, bool> condition);
    }
}