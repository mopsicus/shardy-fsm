using System;
using System.Collections.Generic;

namespace Shardy.FSM {

    /// <summary>
    /// Condition structure to check directions
    /// </summary>
    public readonly struct FSMTransitionCondition<TS> {

        /// <summary>
        /// Condition source
        /// </summary>
        public TS Source { get; }

        /// <summary>
        /// Condition destination
        /// </summary>
        public TS Destination { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="source">Source state</param>
        /// <param name="destination">Destination state</param>
        public FSMTransitionCondition(TS source, TS destination) {
            Source = source;
            Destination = destination;
        }
    }

    /// <summary>
    /// Transition class for FSM
    /// </summary>
    /// <typeparam name="TS">Types of states</typeparam>
    /// <typeparam name="TT">Types of triggers</typeparam>
    public class FSMTransition<TS, TT> {

        /// <summary>
        /// Transition source
        /// </summary>        
        public TS Source { get; private set; } = default;

        /// <summary>
        /// Transition destination
        /// </summary>
        public TS Destination { get; private set; } = default;

        /// <summary>
        /// List of triggers
        /// </summary>
        public List<TT> Triggers = new List<TT>();

        /// <summary>
        /// List of conditions
        /// </summary>
        public List<Func<FSMTransitionCondition<TS>, bool>> Conditions = new List<Func<FSMTransitionCondition<TS>, bool>>();

        /// <summary>
        /// Constructor for transiton
        /// </summary>
        /// <param name="source">Source state</param>
        /// <param name="destination">Destination state</param>
        public FSMTransition(TS source, TS destination) {
            Source = source;
            Destination = destination;
        }

        /// <summary>
        /// Check trigger exists and all conditions met
        /// </summary>
        /// <param name="source">Source state</param>
        /// <param name="trigger">Trigger</param>
        /// <returns></returns>
        public bool IsAvailable(FSMState<TS, TT> source, TT trigger) {
            return Triggers.Contains(trigger) && AreConditionsMet(source.Id);
        }

        /// <summary>
        /// Check conditions met and returned bool
        /// </summary>
        /// <param name="source">Source state</param>
        public bool AreConditionsMet(TS source) {
            return Conditions.TrueForAll(h => h(new FSMTransitionCondition<TS>(source, Destination)));
        }
    }
}
