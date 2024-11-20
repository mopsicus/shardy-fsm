using System;

namespace Shardy.FSM {

    /// <summary>
    /// Interface for builder FSM
    /// </summary>
    /// <typeparam name="TS">Types of states</typeparam>
    /// <typeparam name="TT">Types of triggers</typeparam>
    public interface IFSMBuilder<TS, TT> {
        IFSMState<TS, TT> State(TS state);
        FSM<TS, TT> Build(Action<FSMStateData<TS, TT>> handler = null);
    }
}