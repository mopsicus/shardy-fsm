using System;
using System.Collections.Generic;

namespace Shardy.FSM {

    /// <summary>
    /// Data for builder FSM
    /// </summary>
    /// <typeparam name="TS">Types of states</typeparam>
    /// <typeparam name="TT">Types of triggers</typeparam>
    public class FSMData<TS, TT> {

        /// <summary>
        /// Callback on state changed
        /// </summary>
        public Action<FSMStateData<TS, TT>> OnChanged = delegate { };

        /// <summary>
        /// Current state
        /// </summary>
        public FSMState<TS, TT> State = default;

        /// <summary>
        /// Initial state
        /// </summary>
        public FSMState<TS, TT> Initial = default;

        /// <summary>
        /// Dictionary of states
        /// </summary>
        public Dictionary<TS, FSMState<TS, TT>> States = new Dictionary<TS, FSMState<TS, TT>>();

        /// <summary>
        /// Call change state
        /// </summary>
        /// <param name="data">Data to pass</param>
        public void ChangeState(FSMStateData<TS, TT> data) {
            OnChanged?.Invoke(data);
        }
    }
}