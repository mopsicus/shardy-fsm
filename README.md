<a href="./README.md">![Static Badge](https://img.shields.io/badge/english-118027)</a>
<a href="./README.ru.md">![Static Badge](https://img.shields.io/badge/russian-0390fc)</a>
<p align="center">
    <picture>
        <source media="(prefers-color-scheme: dark)" srcset="Media/logo-fsm-dark.png">
        <source media="(prefers-color-scheme: light)" srcset="Media/logo-fsm.png">
        <img alt="FSM for Shardy" height="256" width="256" src="Media/logo-fsm.png">
    </picture>
</p>
<h3 align="center">FSM for Shardy</h3>
<h4 align="center">Framework for online games and apps</h4>
<p align="center">
    <a href="#quick-start">Quick start</a> · <a href="./Documentation~/index.md">Docs</a> · <a href="https://github.com/mopsicus/shardy-unity">Unity client</a> · <a href="https://github.com/mopsicus/shardy">Shardy</a> · <a href="https://github.com/mopsicus/shardy-fsm/issues">Report Bug</a>
</p>

# 💬 Overview

This package is an implementation of a finite state machine (FSM) for Shardy, and more. It provides a flexible API for building FSMs using a chain of methods and conditions.

> [!NOTE]
> Shardy is a framework for online games and applications for Node.js. It provides the basic functionality for building microservices solutions: mobile, social, web, multiplayer games, realtime applications, chats, middleware services, etc.
> 
> [Read about Shardy](https://github.com/mopsicus/shardy) 💪

# ✨ Features

- Flexible API: states, triggers, conditions
- Custom state and trigger types
- UML diagram generation (notes support)
- Good reference materials: docs, snippets, samples
- No 3rd party libs

# 🚀 Usage

### Installation

Get it from [releases page](https://github.com/mopsicus/shardy-fsm/releases) or add the line to `Packages/manifest.json` and module will be installed directly from Git url:

```
"com.mopsicus.shardy.fsm": "https://github.com/mopsicus/shardy-fsm.git",
```

### Environment setup

For a better experience, you can set up an environment for local development. Since Shardy and all modules (like this) are developed with VS Code, all settings are provided for it.

1. Use `Monokai Pro` or `eppz!` themes
2. Use `FiraCode` font
3. Install extensions:
    - C#
    - C# Dev Kit
    - Unity
4. Enable `Inlay Hints` in C# extension
5. Install `Visual Studio Editor` package in Unity
6. Put `.editorconfig` in root project directory
7. Be cool

### Quick start

1. Install package
2. Add `Shardy.FSM` to uses section
3. Define your states and triggers
4. Call `Builder` method
5. Add states, transtions, conditions
6. Call `Build` method to finish initialization
7. Call trigger(s)
8. Profit

```csharp
using Shardy.FSM;

/// <summary>
/// Test FSM
/// </summary>
void Test() {
    _fsm = FSM<State, Action>.Builder(State.Standing)
    .State(State.Standing)
    .OnEnter((data) => {
        Debug.Log("on enter standing");
    })
    .To(State.Sitting).On(Action.Down)
    .To(State.Jumping).On(Action.Space).If(s => _isConditionActive)
    .State(State.Sitting)
    .To(State.Lying).On(Action.Down)
    .To(State.Standing).On(Action.Up)
    .State(State.Lying)
    .To(State.Sitting).On(Action.Up)
    .State(State.Jumping)
    .Note("some help message here")
    .OnExit((data) => {
        Debug.Log("on exit jumping");
    })
    .To(State.Standing).On(Action.Down)
    .Build();

    _fsm.Trigger(Action.Down);
}
```

### Conditions

You can also add condition(s) to transitions. When the trigger is called, it checks if all conditions are met, and if so, the transition is allowed.

```csharp
State(State.Standing).To(State.Jumping).On(Action.Space).If(YOUR_CONDITION)
```

### UML

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
        <img alt="UML diagram" height="272" width="642" src="Media/uml-fsm.png">
    </picture>
</p>

> [!IMPORTANT] 
> Dotted lines are transitions configured with conditions.

> [!NOTE]
> You can also pass a file with UML content and render the diagram at url: http://www.plantuml.com/plantuml/proxy?src=XXX, where XXX is the url to your *.puml file.

### Demo

See the sample section to get a [demo app](./Samples~/Demo). This demo contains a simple FSM example with a small number of states and triggers.

_Tested in Unity 2022.3.x._

# 🏗️ Contributing

We invite you to contribute and help improve FSM for Shardy. Please see [contributing document](./CONTRIBUTING.md). 🤗

You also can contribute to the Shardy project by:

- Helping other users 
- Monitoring the issue queue
- Sharing it to your socials
- Referring it in your projects

# 🤝 Support

You can support Shardy by using any of the ways below:

* Bitcoin (BTC): 1VccPXdHeiUofzEj4hPfvVbdnzoKkX8TJ
* USDT (TRC20): TMHacMp461jHH2SHJQn8VkzCPNEMrFno7m
* TON: UQDVp346KxR6XxFeYc3ksZ_jOuYjztg7b4lEs6ulEWYmJb0f
* Visa, Mastercard via [Boosty](https://boosty.to/mopsicus/donate)
* MIR via [CloudTips](https://pay.cloudtips.ru/p/9f507669)

# ✉️ Contact

Before you ask a question, it is best to search for existing [issues](https://github.com/mopsicus/shardy-fsm/issues) that might help you. Anyway, you can ask any questions and send suggestions by [email](mailto:mail@mopsicus.ru) or [Telegram](https://t.me/mopsicus).

# 🔑 License

FSM for Shardy is licensed under the [MIT License](./LICENSE.md). Use it for free and be happy. 🎉