# 🧱 Brick Breaker Arcade

A modular, extensible **Unity-based brick breaker** game built with clean architecture, testability, and strict adherence to:

- **Domain-Driven Design (DDD)**
- **SOLID principles**
- **Martin Fowler’s architecture patterns**

---

## 🎮 Features

- Classic brick breaker core loop
- Physics-based paddle and ball control
- Multiple block behaviours:
  - Reflecting
  - Exploding
  - Moving (linear/diagonal)
  - Slowing down the game
- Level-based progression with `LevelDesigner`
- Dynamic block spawning via `BlockSpawner` and `BlockFactory`
- Color-coded behaviour-to-visual mapping
- Scoring system tied to player actions
- Responsive UI: Pause, Win, Game Over, HUD
- Sound system with 8-bit theme and FX
- Particle effects for block destruction
- Fully testable with `Unity Test Framework`
- Input handled via Unity’s **Input System**
- Game state transitions managed by `GameHandler`

---

## 🛠 Technologies

- **Unity 2022 LTS**
- **C# 10 / .NET 4.x**
- **Unity Test Framework**
- **Unity Input System**
- Service Locator (IoC-light) for infrastructure decoupling

---

## 🧱 Architectural Highlights

- **Bounded contexts**: Level design, gameplay, UI, input, scoring
- **Behavior-based blocks**: Extend functionality via composable MonoBehaviours
- **Builder pattern** for configuring blocks and behaviours
- **Strategy and coordinator patterns** for destruction handling
- **Immutable config objects** for all behaviour state
- **Event-based or deferred destruction coordination**
- Fully automated CI/CD pipeline with GitHub Actions:
  - Build for multiple platforms
  - Pre-release tagging (`-alpha`)
  - Version bumping
  - Artifact packaging and publishing

---

## 📁 Project Structure

```plaintext
Assets/
├── Scripts/
│   ├── Blocks/               # All block-related behaviours (e.g., Explode, Reflect, Slow)
│   ├── GameHandler/          # Game state, win/lose logic, pause handling
│   ├── Input/                # Input abstraction layer
│   ├── Level/                # Level design, block placement logic
│   ├── SaveSystem/           # (Optional) Persistent data
│   ├── Score/                # Scoring + win conditions
│   ├── SharedKernel/         # Common interfaces, base classes, value objects
│   └── UI/                   # Menus, HUD, and transitions
│
├── Prefabs/
│   ├── Blocks/               # Block prefab base
│   └── UI/                   # Canvas-based UIs
│
├── Resources/
│   ├── Sound/                # Music, sound effects
│   └── Configs/              # JSON/Scriptable configs if used
│
├── Tests/                    # EditMode + PlayMode test coverage
│
└── Scenes/                   # Main game scenes
```


## 🚀 Getting Started
1. Open project with Unity 2022+

2. Press Play in the MainScene

3. Use W/S or Arrow keys to move paddle

4. Break all blocks to win!

Or download any **Release**.

##  Roadmap
1. Power-ups (multi-ball, laser paddle, shield)

2. Boss blocks (multi-hit + attack)

3. Save/load system for high scores

4. Custom level editor

5. Mobile touch support