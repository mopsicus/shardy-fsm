using System;
using UnityEngine;

namespace Shardy.FSM {

    /// <summary>
    /// Finite state machine
    /// </summary>
    /// <typeparam name="TS">Type of states</typeparam>
    /// <typeparam name="TT">Type of triggers</typeparam>
    public class FSM<TS, TT> {

        /// <summary>
        /// Log tag
        /// </summary>
        public const string TAG = "FSM";

        /// <summary>
        /// Current FSM data
        /// </summary>
        readonly FSMData<TS, TT> _data = null;

        /// <summary>
        /// Constructror
        /// </summary>
        /// <param name="data">Data to process</param>
        /// <param name="handler">Callback on state changed</param>
        public FSM(FSMData<TS, TT> data, Action<FSMStateData<TS, TT>> handler) {
            if (data == null) {
#if SHARDY_DEBUG
                Debug.LogError($"[{TAG}] data is NULL");
#endif
                return;
            }
            _data = data;
            if (handler != null) {
                _data.OnChanged += handler;
            }
        }

        /// <summary>
        /// Generate state machine with state
        /// </summary>
        /// <param name="state">Initial state</param>
        /// <returns>FSM</returns>
        public static IFSMBuilder<TS, TT> Builder(TS state) {
            return new FSMBuilder<TS, TT>(state);
        }

        /// <summary>
        /// Current state
        /// </summary>
        public FSMState<TS, TT> State {
            get {
                return _data.State;
            }
        }

        /// <summary>
        /// Initial state
        /// </summary>
        public FSMState<TS, TT> Initial {
            get {
                return _data.Initial;
            }
        }

        /// <summary>
        /// Check is state in machine
        /// </summary>
        /// <param name="state">State</param>
        bool IsStateConfigurated(TS state) {
            if (!_data.States.ContainsKey(state)) {
#if SHARDY_DEBUG
                Debug.LogError($"[{TAG}] destination is not configurated: {state}");
#endif
                return false;
            }
            return true;
        }

        /// <summary>
        /// Make transition
        /// </summary>
        /// <param name="destination">Destination state</param>
        /// <param name="trigger">trigger</param>
        void Transition(TS destination, TT trigger) {
            if (destination == null || trigger == null) {
#if SHARDY_DEBUG
                Debug.LogError($"[{TAG}] transition can't go to destination: {destination}, trigger: {trigger}");
#endif
                return;
            }
            var previous = _data.State;
            if (!IsStateConfigurated(destination)) {
                return;
            }
            _data.State = _data.States[destination];
            if (_data.State.Equals(previous)) {
#if SHARDY_DEBUG
                Debug.LogWarning($"[{TAG}] transition destination is the same as source");
#endif                
                return;
            }
            var data = new FSMStateData<TS, TT>(previous, _data.State, trigger);
            previous.Exit(data);
            _data.State.Enter(data);
            _data.ChangeState(data);
        }

        /// <summary>
        /// Go to destination directly
        /// </summary>
        /// <param name="destination">Destination state</param>
        public void GoTo(TS destination) {
            if (_data.States.TryGetValue(destination, out _)) {
                Transition(destination, default);
            }
        }

        /// <summary>
        /// Add note to state (for UML)
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="message">Message</param>
        /// <param name="direction">Direction to draw</param>
        public void Note(TS state, string message, FSMStateNoteDirection direction = FSMStateNoteDirection.Left) {
            if (!IsStateConfigurated(state)) {
                return;
            }
            _data.States[state].Note(message, direction);
        }

        /// <summary>
        /// Call trigger to invoke transition
        /// </summary>
        /// <param name="trigger">Trigger</param>
        public void Trigger(TT trigger) {
            if (trigger == null) {
#if SHARDY_DEBUG
                Debug.LogError($"[{TAG}] trigger is NULL");
#endif                
                return;
            }
            var transition = _data.State.Transition(trigger);
            if (transition != null) {
                Transition(transition.Destination, trigger);
            }
        }

        /// <summary>
        /// Generate UML string
        /// </summary>
        /// <param name="direction">Draw direction</param>
        /// <param name="title">Title</param>
        public string GetUML(FSMUMLDirection direction = FSMUMLDirection.LeftToRight, string title = null) {
            return new FSMUMLGenerator<TS, TT>(_data).GetUML(direction, title);
        }
    }
}