using System;
using System.Collections.Generic;

namespace Shardy.FSM {

    /// <summary>
    /// Direction for state note
    /// </summary>
    public enum FSMStateNoteDirection {
        Left,
        Right,
        Top,
        Bottom
    }

    /// <summary>
    /// Data for state note
    /// </summary>
    public readonly struct FSMStateNoteData {

        /// <summary>
        /// Direction to draw note
        /// </summary>
        public FSMStateNoteDirection Direction { get; }

        /// <summary>
        /// Message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="direction">Direction to draw</param>
        public FSMStateNoteData(string message, FSMStateNoteDirection direction) {
            Message = message;
            Direction = direction;
        }
    }

    /// <summary>
    /// Data for state change event
    /// </summary>
    public readonly struct FSMStateData<TS, TT> {

        /// <summary>
        /// Source state
        /// </summary>
        public FSMState<TS, TT> Source { get; }

        /// <summary>
        /// Destination state
        /// </summary>
        public FSMState<TS, TT> Destination { get; }

        /// <summary>
        /// Trigger
        /// </summary>
        public TT Trigger { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source state</param>
        /// <param name="destination">Destination state</param>
        /// <param name="trigger">Trigger</param>
        public FSMStateData(FSMState<TS, TT> source, FSMState<TS, TT> destination, TT trigger) {
            Source = source;
            Destination = destination;
            Trigger = trigger;
        }
    }

    /// <summary>
    /// State class for FSM
    /// </summary>
    /// <typeparam name="TS">Types of states</typeparam>
    /// <typeparam name="TT">Types of triggers</typeparam>
    public class FSMState<TS, TT> {

        /// <summary>
        /// Callback(s) on enter state
        /// </summary>
        public Action<FSMStateData<TS, TT>> OnEnter = delegate { };

        /// <summary>
        /// Callback(s) on exit state
        /// </summary>
        public Action<FSMStateData<TS, TT>> OnExit = delegate { };

        /// <summary>
        /// Current state
        /// </summary>
        public TS Id { get; private set; } = default;

        /// <summary>
        /// State notes
        /// </summary>
        public List<FSMStateNoteData> Notes = new List<FSMStateNoteData>();

        /// <summary>
        /// Transitions dictionary
        /// </summary>
        public Dictionary<TS, FSMTransition<TS, TT>> Transitions = new Dictionary<TS, FSMTransition<TS, TT>>();

        /// <summary>
        /// State constructor
        /// </summary>
        public FSMState(TS id) {
            Id = id;
        }

        /// <summary>
        /// Try to transition
        /// </summary>
        /// <param name="trigger">Trigger</param>
        public FSMTransition<TS, TT> Transition(TT trigger) {
            foreach (var transition in Transitions.Values) {
                if (transition.IsAvailable(this, trigger)) {
                    return transition;
                }
            }
            return null;
        }

        /// <summary>
        /// Add note
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="direction">Draw direction</param>
        public void Note(string message, FSMStateNoteDirection direction) {
            Notes.Add(new FSMStateNoteData(message, direction));
        }

        /// <summary>
        /// Invoke enter callback(s)
        /// </summary>
        /// <param name="data">Data to pass</param>
        public void Enter(FSMStateData<TS, TT> data) {
            OnEnter?.Invoke(data);
        }

        /// <summary>
        /// Invoke exit callback(s)
        /// </summary>
        /// <param name="data">Data to pass</param>
        public void Exit(FSMStateData<TS, TT> data) {
            OnExit?.Invoke(data);
        }
    }
}
