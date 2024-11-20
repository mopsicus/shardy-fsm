# FSM documentation

This package is an implementation of a finite state machine (FSM) for Shardy, and more. It provides a flexible API for building FSMs using a chain of states, transitions and conditions.

## 💬 Introduction

FSM is an abstract model of computation that describes a system through a set of states, the possible transitions between them, and the actions performed at those transitions.

This implementation uses a flexible (fluent) API to build and configure FSM. It is really simple and easy to use.

## 🚀 Quick start

1. Install package by any available method
2. Add `Shardy.FSM` to uses section
3. Define your states and triggers
4. Call `Builder` method
5. Add states, transtions, conditions
6. Call `Build` method to finish initialization
7. Call trigger(s)
8. Profit

## 🕹️ Usage

Use static method `FSM.Builder` to start configure your FSM – pass initial state as param:

```csharp
FSM<State, Action>.Builder(State.Standing)
```

After that, you can add any states, transitions and triggers:

```csharp
FSM<State, Action>.Builder(State.Standing)
.State(State.Standing).To(State.Sitting).On(Action.Down)
.State(State.Standing).To(State.Jumping).On(Action.Space)
.State(State.Sitting).To(State.Standing).On(Action.Up)
```

You can add callback to each state's enter and exit:

```csharp
FSM<State, Action>.Builder(State.Standing)
.State(State.Standing)
.OnEnter((data) => {
    // do something on enter state
})
.OnExit((data) => {
    // do something on exit state
})
```

To finish configure FSM call `Build` method that return FSM instance: 

```csharp
var fsm = FSM<State, Action>.Builder(State.Standing)
.State(State.Standing).To(State.Sitting).On(Action.Down)
.Build();
```

You can also add a handler as a param to the `Build` method to detect a state change:

```csharp
var fsm = FSM<State, Action>.Builder(State.Standing)
.State(State.Standing).To(State.Sitting).On(Action.Down)
.Build(changed => {
    Debug.Log(changed.Source);
    Debug.Log(changed.Destination);
    Debug.Log(changed.Trigger);
});
```

To change state call `Trigger` method:

```csharp
var fsm = FSM<State, Action>.Builder(State.Standing)
.State(State.Standing).To(State.Sitting).On(Action.Down)
.Build();

fsm.Trigger(Action.Down);
```

To change state directly without any triggers and checks, call `GoTo`:

```csharp
fsm.GoTo(State.Sitting);
```

Use `SHARDY_DEBUG` preprocessor derective to show possible debug log messages.

## 🎯 Conditions

You can also add condition(s) to transitions. When the trigger is called, it checks if all conditions are met, and if so, the transition is allowed.

```csharp
State(State.Standing).To(State.Jumping).On(Action.Space).If(YOUR_CONDITION)
```

Condition is a method that returns a function with a state and a boolean flag.

```csharp
Func<FSMTransitionCondition<State>, bool> YourCondition() {
    return s => {
        return 1 > 0;
    };
}
```

Or you can write a short variant:

```csharp
State(State.Standing).To(State.Jumping).On(Action.Space).If(s => YOUR_BOOL_VALUE)
```

## 📐 UML

FSM allows you to generate code with UML structure:

```uml
@startuml
skin rose
title TestFSM
left to right direction
agent Standing
agent Sitting
agent Lying
agent Jumping
note left of Jumping
some help message here
end note
Start --> Standing
Standing --> Sitting : Down
Standing ~~> Jumping : Space
Sitting --> Lying : Down
Sitting --> Standing : Up
Lying --> Sitting : Up
Jumping --> Standing : Down
@enduml
```

And you can render it on https://www.planttext.com or http://www.plantuml.com/plantuml/uml/ and check your FSM for correct transitions and triggers:

<p align="center">
    <picture>
        <img alt="UML diagram" height="272" width="642" src="../Media/uml-fsm.png">
    </picture>
</p>

> [!IMPORTANT] 
> Dotted lines are transitions configured with conditions.

> [!IMPORTANT] 
> If the transition does not contain a trigger, the lines will have a cross at the end.

> [!NOTE]
> Also, you can pass a file with UML content and render the diagram at url: http://www.plantuml.com/plantuml/proxy?src=XXX, where XXX is the url to your *.puml file.

To generate UML code call `GetUML` method of your FSM and pass two params: direction and title of diagram (optional).

```csharp
_fsm.GetUML(FSMUMLDirection.LeftToRight, "Test FSM");
```

The direction can be left to right and top to bottom. This is an option for rendering.

```csharp
/// <summary>
/// Direction to build diagram
/// </summary>
public enum FSMUMLDirection {
    LeftToRight,
    TopToBottom,
}
```

Also, you can also add notes to your states for UML diagram by calling `Note` method. Default direction is left.

```csharp
State(State.Standing).Note("some help message here", FSMStateNoteDirection.Bottom)
```

Types of note direction:

```csharp
/// <summary>
/// Direction for state note
/// </summary>
public enum FSMStateNoteDirection {
    Left,
    Right,
    Top,
    Bottom
}
```

> [!NOTE]
> To render a more “intuitive” diagram, the `agent` keyword is used instead of `state`.

## 📑 Reference

### FSM

```csharp
/// <summary>
/// Constructror
/// </summary>
/// <param name="data">Data to process</param>
/// <param name="handler">Callback on state changed</param>
public FSM(FSMData<TS, TT> data, Action<FSMStateData<TS, TT>> handler);

/// <summary>
/// Generate state machine with state
/// </summary>
/// <param name="state">Initial state</param>
/// <returns>FSM</returns>
public static IFSMBuilder<TS, TT> Builder(TS state);

/// <summary>
/// Current state
/// </summary>
public FSMState<TS, TT> State;

/// <summary>
/// Initial state
/// </summary>
public FSMState<TS, TT> Initial;

/// <summary>
/// Check is state in machine
/// </summary>
/// <param name="state">State</param>
bool IsStateConfigurated(TS state);

/// <summary>
/// Make transition
/// </summary>
/// <param name="destination">Destination state</param>
/// <param name="trigger">trigger</param>
void Transition(TS destination, TT trigger);

/// <summary>
/// Go to destination directly
/// </summary>
/// <param name="destination">Destination state</param>
public void GoTo(TS destination);

/// <summary>
/// Add note to state (for UML)
/// </summary>
/// <param name="state">State</param>
/// <param name="message">Message</param>
/// <param name="direction">Direction to draw</param>
public void Note(TS state, string message, FSMStateNoteDirection direction = FSMStateNoteDirection.Left);

/// <summary>
/// Call trigger to invoke transition
/// </summary>
/// <param name="trigger">Trigger</param>
public void Trigger(TT trigger);

/// <summary>
/// Generate UML string
/// </summary>
/// <param name="direction">Draw direction</param>
/// <param name="title">Title</param>
public string GetUML(FSMUMLDirection direction = FSMUMLDirection.LeftToRight, string title = null);
```

### FSMBuilder

```csharp
/// <summary>
/// Constructor
/// </summary>
/// <param name="state">Initial state</param>
public FSMBuilder(TS state);

/// <summary>
/// Add new state
/// </summary>
/// <param name="state">State</param>
public IFSMState<TS, TT> State(TS state);

/// <summary>
/// Add note to state
/// </summary>
/// <param name="message">Message</param>
/// <param name="direction">Draw direction</param>
public IFSMState<TS, TT> Note(string message, FSMStateNoteDirection direction = FSMStateNoteDirection.Left);

/// <summary>
/// Add handler for state enter actionn
/// </summary>
/// <param name="handler">Handler</param>
public IFSMState<TS, TT> OnEnter(Action<FSMStateData<TS, TT>> handler);

/// <summary>
/// Add handler for state exit actionn
/// </summary>
/// <param name="handler">Handler</param>
public IFSMState<TS, TT> OnExit(Action<FSMStateData<TS, TT>> handler);

/// <summary>
/// Add trigger to current transaction
/// </summary>
/// <param name="trigger">Trigger</param>
public IFSMTransition<TS, TT> On(TT trigger);

/// <summary>
/// Add condition to current transaction (addition to trigger)
/// </summary>
/// <param name="condition">Condition</param>
public IFSMTransition<TS, TT> If(Func<FSMTransitionCondition<TS>, bool> condition);

/// <summary>
/// Add new transition to state
/// </summary>
/// <param name="destination">Destination state</param>
public IFSMTransition<TS, TT> To(TS destination);

/// <summary>
/// Build and return FSM
/// </summary>
/// <param name="handler">Callback on state changed</param>
public FSM<TS, TT> Build(Action<FSMStateData<TS, TT>> handler = null);
```

### FSMData

```csharp
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
public void ChangeState(FSMStateData<TS, TT> data);
```

### FSMState

```csharp
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
public FSMState(TS id);

/// <summary>
/// Try to transition
/// </summary>
/// <param name="trigger">Trigger</param>
public FSMTransition<TS, TT> Transition(TT trigger);

/// <summary>
/// Add note
/// </summary>
/// <param name="message">Message</param>
/// <param name="direction">Draw direction</param>
public void Note(string message, FSMStateNoteDirection direction);

/// <summary>
/// Invoke enter callback(s)
/// </summary>
/// <param name="data">Data to pass</param>
public void Enter(FSMStateData<TS, TT> data);

/// <summary>
/// Invoke exit callback(s)
/// </summary>
/// <param name="data">Data to pass</param>
public void Exit(FSMStateData<TS, TT> data);
```

### FSMTransition

```csharp
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
public FSMTransition(TS source, TS destination);

/// <summary>
/// Check trigger exists and all conditions met
/// </summary>
/// <param name="source">Source state</param>
/// <param name="trigger">Trigger</param>
/// <returns></returns>
public bool IsAvailable(FSMState<TS, TT> source, TT trigger);

/// <summary>
/// Check conditions met and returned bool
/// </summary>
/// <param name="source">Source state</param>
public bool AreConditionsMet(TS source);
```

### FSMUMLGenerator

```csharp
/// <summary>
/// Constructor
/// </summary>
/// <param name="data">FSM data</param>
public FSMUMLGenerator(FSMData<TS, TT> data);

/// <summary>
/// Filter states, triggers names
/// </summary>
string Escape(string value);

/// <summary>
/// Generate UML
/// </summary>
/// <param name="direction">Direction to draw</param>
/// <param name="title">Title</param>
public string GetUML(FSMUMLDirection direction, string title);

/// <summary>
/// Get filtered title
/// </summary>
/// <param name="title">Title</param>
string GetTitle(string title);

/// <summary>
/// Get UML direction string
/// </summary>
/// <param name="direction">Direction</param>
string GetUMLDirection(FSMUMLDirection direction);

/// <summary>
/// Get initial state string 
/// </summary>
string GetInitial();

/// <summary>
/// Get all states
/// </summary>
string GetStates();

/// <summary>
/// Get all notes
/// </summary>
string GetNotes();

/// <summary>
/// Get all transitions with triggers
/// </summary>
string GetTransitions();
```

### IFSMBuilder

```csharp
/// <summary>
/// Interface for builder FSM
/// </summary>
/// <typeparam name="TS">Types of states</typeparam>
/// <typeparam name="TT">Types of triggers</typeparam>
public interface IFSMBuilder<TS, TT> {
    IFSMState<TS, TT> State(TS state);
    FSM<TS, TT> Build(Action<FSMStateData<TS, TT>> handler = null);
}
```

### IFSMState

```csharp
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
```

### IFSMTransition

```csharp
/// <summary>
/// Interface for FSM transition
/// </summary>
/// <typeparam name="TS">Types of states</typeparam>
/// <typeparam name="TT">Types of triggers</typeparam>
public interface IFSMTransition<TS, TT> : IFSMState<TS, TT> {
    IFSMTransition<TS, TT> On(TT trigger);
    IFSMTransition<TS, TT> If(Func<FSMTransitionCondition<TS>, bool> condition);
}
```