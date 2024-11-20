using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shardy.FSM {

    /// <summary>
    /// Builder for FSM
    /// </summary>
    /// <typeparam name="TS">Types of states</typeparam>
    /// <typeparam name="TT">Types of triggers</typeparam>
    public class FSMBuilder<TS, TT> : IFSMBuilder<TS, TT>, IFSMTransition<TS, TT> {

        /// <summary>
        /// Current data
        /// </summary>
        protected FSMData<TS, TT> Data = new FSMData<TS, TT>();

        /// <summary>
        /// Dictionary with all state 
        /// </summary>
        protected Dictionary<TS, FSMState<TS, TT>> _states = new Dictionary<TS, FSMState<TS, TT>>();

        /// <summary>
        /// Dictionary with all transitions 
        /// </summary>
        protected Dictionary<Tuple<TS, TS>, FSMTransition<TS, TT>> _transitions = new Dictionary<Tuple<TS, TS>, FSMTransition<TS, TT>>();

        /// <summary>
        /// Initial state
        /// </summary>
        protected TS _initial = default;

        /// <summary>
        /// Current state
        /// </summary>
        protected TS _state = default;

        /// <summary>
        /// Current transition
        /// </summary>
        protected Tuple<TS, TS> _transition = default;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="state">Initial state</param>
        public FSMBuilder(TS state) {
            _initial = state;
        }

        /// <summary>
        /// Add new state
        /// </summary>
        /// <param name="state">State</param>
        public IFSMState<TS, TT> State(TS state) {
            _state = state;
            if (!Data.States.ContainsKey(state)) {
                _states[_state] = new FSMState<TS, TT>(_state);
                Data.States[state] = _states[_state];
            }
            return this;
        }

        /// <summary>
        /// Add note to state
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="direction">Draw direction</param>
        public IFSMState<TS, TT> Note(string message, FSMStateNoteDirection direction = FSMStateNoteDirection.Left) {
            _states[_state].Notes.Add(new FSMStateNoteData(message, direction));
            return this;
        }

        /// <summary>
        /// Add handler for state enter actionn
        /// </summary>
        /// <param name="handler">Handler</param>
        public IFSMState<TS, TT> OnEnter(Action<FSMStateData<TS, TT>> handler) {
            _states[_state].OnEnter += handler;
            return this;
        }

        /// <summary>
        /// Add handler for state exit actionn
        /// </summary>
        /// <param name="handler">Handler</param>
        public IFSMState<TS, TT> OnExit(Action<FSMStateData<TS, TT>> handler) {
            _states[_state].OnExit += handler;
            return this;
        }

        /// <summary>
        /// Add trigger to current transaction
        /// </summary>
        /// <param name="trigger">Trigger</param>
        public IFSMTransition<TS, TT> On(TT trigger) {
            _transitions[_transition].Triggers.Add(trigger);
            return this;
        }

        /// <summary>
        /// Add condition to current transaction (addition to trigger)
        /// </summary>
        /// <param name="condition">Condition</param>
        public IFSMTransition<TS, TT> If(Func<FSMTransitionCondition<TS>, bool> condition) {
            _transitions[_transition].Conditions.Add(condition);
            return this;
        }

        /// <summary>
        /// Add new transition to state
        /// </summary>
        /// <param name="destination">Destination state</param>
        public IFSMTransition<TS, TT> To(TS destination) {
            _transition = Tuple.Create(_state, destination);
            if (!_transitions.ContainsKey(_transition)) {
                _transitions[_transition] = new FSMTransition<TS, TT>(_state, destination);
                _states[_state].Transitions[destination] = _transitions[_transition];
            }
            return this;
        }

        /// <summary>
        /// Build and return FSM
        /// </summary>
        /// <param name="handler">Callback on state changed</param>
        public FSM<TS, TT> Build(Action<FSMStateData<TS, TT>> handler = null) {
            if (Data.States[_initial] == null) {
#if SHARDY_DEBUG
                Debug.LogError($"[{FSM<TS, TT>.TAG}] initial state in NULL");
#endif
                return null;
            }
            Data.State = Data.States[_initial];
            Data.Initial = Data.State;
            return new FSM<TS, TT>(Data, handler);
        }
    }
}