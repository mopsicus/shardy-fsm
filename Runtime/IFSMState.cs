using System;

namespace Shardy.FSM {

    /// <summary>
    /// Interface for FSM state
    /// </summary>
    /// <typeparam name="TS">Types of states</typeparam>
    /// <typeparam name="TT">Types of triggers</typeparam>
    public interface IFSMState<TS, TT> : IFSMBuilder<TS, TT> {
        IFSMTransition<TS, TT> To(TS state);
        IFSMState<TS, TT> Note(string message, FSMStateNoteDirection direction = FSMStateNoteDirection.Left);
        IFSMState<TS, TT> OnEnter(Action<FSMStateData<TS, TT>> handler);
        IFSMState<TS, TT> OnExit(Action<FSMStateData<TS, TT>> handler);
    }
}