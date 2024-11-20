using System;
using Shardy.FSM;
using UnityEngine;

/// <summary>
/// Object states
/// </summary>
enum State {
    Standing,
    Sitting,
    Lying,
    Jumping
};

/// <summary>
/// Object actions/triggers
/// </summary>
enum Action {
    Down,
    Up,
    Space
};

/// <summary>
/// Demo class
/// </summary>
public class Demo : MonoBehaviour {

    /// <summary>
    /// FSM instance
    /// </summary>
    FSM<State, Action> _fsm = null;

    /// <summary>
    /// Flag for test condition on jumping transition
    /// </summary>
    bool _isConditionActive = false;

    /// <summary>
    /// Test FSM
    /// </summary>
    void Start() {
        Debug.Log("initial is standing");
        _fsm = FSM<State, Action>.Builder(State.Standing)
        .State(State.Standing)
        .OnEnter((data) => {
            Debug.Log("on enter standing");
        })
        .OnExit((data) => {
            Debug.Log("on exit standing");
        })
        .To(State.Sitting).On(Action.Down)
        .To(State.Jumping).On(Action.Space).If(s => _isConditionActive)
        .State(State.Sitting)
        .OnEnter((data) => {
            Debug.Log("on enter sitting");
        })
        .OnExit((data) => {
            Debug.Log("on exit sitting");
        })
        .To(State.Lying).On(Action.Down)
        .To(State.Standing).On(Action.Up)
        // .To(State.Jumping).On(Action.Space)
        .State(State.Lying)
        .OnEnter((data) => {
            Debug.Log("on enter lying");
        })
        .OnExit((data) => {
            Debug.Log("on exit lying");
        })
        .To(State.Sitting).On(Action.Up)
        .State(State.Jumping)
        .Note("some help message here")
        .OnEnter((data) => {
            Debug.Log("on enter jumping");
        })
        .OnExit((data) => {
            Debug.Log("on exit jumping");
        })
        .To(State.Standing).On(Action.Down)
        .Build();
    }

    /// <summary>
    /// Generate structure for generating UML diagram
    /// </summary>
    public void MakeUML() {
        Debug.Log("-- begin --");
        Debug.Log(_fsm.GetUML(FSMUMLDirection.LeftToRight, "Test FSM"));
        Debug.Log("-- end --");
        Debug.Log("Paste code to generate diagram on https://www.planttext.com or http://www.plantuml.com/plantuml/uml/");
    }

    /// <summary>
    /// Apply action in FSM 
    /// </summary>
    /// <param name="action">Action/trigger</param>
    public void Trigger(int action) {
        _fsm.Trigger((Action)action);
    }

    /// <summary>
    /// Handler for change condition flag
    /// </summary>
    public void OnConditionChanged(bool value) {
        _isConditionActive = value;
    }
}
