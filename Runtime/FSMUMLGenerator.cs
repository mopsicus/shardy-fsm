using System.Text;

namespace Shardy.FSM {

    /// <summary>
    /// Direction to build diagram
    /// </summary>
    public enum FSMUMLDirection {
        LeftToRight,
        TopToBottom,
    }

    /// <summary>
    /// UML text generator for https://www.planttext.com, http://www.plantuml.com/plantuml/uml/
    /// </summary>
    /// <typeparam name="TS">Types of states</typeparam>
    /// <typeparam name="TT">Types of triggers</typeparam>
    public class FSMUMLGenerator<TS, TT> {

        /// <summary>
        /// Log tag
        /// </summary>
        public const string TAG = "UML";

        /// <summary>
        /// List of available chars for filtering names
        /// </summary>        
        const string AVAILABLE_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789._-";

        /// <summary>
        /// Format for whole UML
        /// </summary>
        const string UML_MASK = "@startuml\nskin rose\ntitle {0}\n{1} direction\n{2}{3}{4}{5}@enduml";

        /// <summary>
        /// Mask for initial state
        /// </summary>
        const string INITIAL_MASK = "Start --> {0}\n";

        /// <summary>
        /// Mask for states
        /// </summary>
        const string STATE_MASK = "agent {0}\n";

        /// <summary>
        /// Mask for note
        /// </summary>
        /// <value></value>
        const string NOTE_MASK = "note {0} of {1}\n{2}\nend note\n";

        /// <summary>
        /// Arror for state transition
        /// </summary>
        const string TRANSITION_ARROW = "-->";

        /// <summary>
        /// Arror for state transition with conditions
        /// </summary>
        const string TRANSITION_CONDITION_ARROW = "~~>";

        /// <summary>
        /// Mask for state transition
        /// </summary>
        const string TRANSITION_MASK = "{0} {1} {2} : {3}\n";

        /// <summary>
        /// Mask for state transition without triggers
        /// </summary>
        const string TRANSITION_NO_TRIGGERS_MASK = "{0} --+ {1}\n";

        /// <summary>
        /// Left to right direction
        /// </summary>
        const string LEFT_RIGHT = "left to right";

        /// <summary>
        /// Top to bottom direction
        /// </summary>
        const string TOP_BOTTOM = "top to bottom";

        /// <summary>
        /// Current FSM data
        /// </summary>
        readonly FSMData<TS, TT> _data = default;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="data">FSM data</param>
        public FSMUMLGenerator(FSMData<TS, TT> data) {
            _data = data;
        }

        /// <summary>
        /// Filter states, triggers names
        /// </summary>
        string Escape(string value) {
            var sb = new StringBuilder();
            foreach (var item in value) {
                if (AVAILABLE_CHARS.Contains(item)) {
                    sb.Append(item);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generate UML
        /// </summary>
        /// <param name="direction">Direction to draw</param>
        /// <param name="title">Title</param>
        public string GetUML(FSMUMLDirection direction, string title) {
            return string.Format(UML_MASK, GetTitle(title), GetUMLDirection(direction), GetStates(), GetNotes(), GetInitial(), GetTransitions());
        }

        /// <summary>
        /// Get filtered title
        /// </summary>
        /// <param name="title">Title</param>
        string GetTitle(string title) {
            return Escape(!string.IsNullOrEmpty(title) ? title : TAG);
        }

        /// <summary>
        /// Get UML direction string
        /// </summary>
        /// <param name="direction">Direction</param>
        string GetUMLDirection(FSMUMLDirection direction) {
            return direction == FSMUMLDirection.LeftToRight ? LEFT_RIGHT : TOP_BOTTOM;
        }

        /// <summary>
        /// Get initial state string 
        /// </summary>
        string GetInitial() {
            return string.Format(INITIAL_MASK, Escape(_data.Initial.Id.ToString()));
        }

        /// <summary>
        /// Get all states
        /// </summary>
        string GetStates() {
            var sb = new StringBuilder();
            foreach (var state in _data.States) {
                sb.Append(string.Format(STATE_MASK, Escape(state.Key.ToString())));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get all notes
        /// </summary>
        string GetNotes() {
            var sb = new StringBuilder();
            foreach (var state in _data.States) {
                foreach (var note in state.Value.Notes) {
                    sb.Append(string.Format(NOTE_MASK, note.Direction.ToString().ToLower(), Escape(state.Key.ToString()), note.Message));
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Get all transitions with triggers
        /// </summary>
        string GetTransitions() {
            var sb = new StringBuilder();
            foreach (var state in _data.States) {
                foreach (var transition in state.Value.Transitions) {
                    if (transition.Value.Triggers.Count == 0) {
                        sb.Append(string.Format(TRANSITION_NO_TRIGGERS_MASK, Escape(transition.Value.Source.ToString()), Escape(transition.Value.Destination.ToString())));
                    } else {
                        var arrow = transition.Value.Conditions.Count == 0 ? TRANSITION_ARROW : TRANSITION_CONDITION_ARROW;
                        foreach (var trigger in transition.Value.Triggers) {
                            sb.Append(string.Format(TRANSITION_MASK, Escape(transition.Value.Source.ToString()), arrow, Escape(transition.Value.Destination.ToString()), Escape(trigger.ToString())));
                        }
                    }
                }
            }
            return sb.ToString();
        }
    }
}