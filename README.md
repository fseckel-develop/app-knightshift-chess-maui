# ♟️ KnightShift – Cross-Platform Chess Engine & App

![Platform](https://img.shields.io/badge/Platform-.NET%208-blue)
![UI](https://img.shields.io/badge/UI-.NET%20MAUI-purple)
![Tests](https://img.shields.io/badge/Tests-xUnit-green)
![Focus](https://img.shields.io/badge/Focus-Chess%20Engine-orange)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)
[![CI](https://github.com/fseckel-develop/app-knightshift-chess-maui/actions/workflows/ci.yml/badge.svg)](https://github.com/fseckel-develop/app-knightshift-chess-maui/actions/workflows/ci.yml)

**KnightShift** is planned to be a cross-platform chess application built with **.NET MAUI**, powered by a custom-designed chess engine implemented in C#.

The project will combine:
- ♟️ **Chess engine development** (move generation, rules, AI)
- 🧱 **Clean architecture & domain modeling**
- 📱 **Cross-platform UI with .NET MAUI**

The goal is to create a **fully structured, extensible chess system** — from engine to UI — with a strong focus on **clarity, maintainability, and long-term evolution**.

---
## ✨ Key Features (Planned)

#### Engine
- Full legal move generation
- Check, checkmate, and stalemate detection
- Special rules (castling, en passant, promotion)
- Minimax-based AI with alpha-beta pruning
- Configurable difficulty

#### Application
- Interactive chessboard UI (.NET MAUI)
- Human vs AI gameplay
- Move highlighting and history
- Game state persistence

#### Backend
- Game storage & replay
- Player profiles
- Online multiplayer
- Rating system (ELO)

---
## 🏗 Architecture

The project follows a **layered architecture** with strong separation of concerns:

```text
KnightShift
│
├── Domain          → chess rules & core models
├── Engine          → move generation, validation, AI
├── Application     → orchestration / use cases
├── Infrastructure  → persistence & external systems
├── API             → backend endpoints
└── Mobile          → .NET MAUI frontend
```

#### Design Principles
- Clear separation between state, rules, and execution
- Domain-first design with chess-native modeling (files/ranks)
- Modular engine (piece-based move generation)
- Testability and extensibility as core goals

---
## 🚧 Current Status

This project is under active refactoring and continuous development.

#### ✅ Implemented
- Core domain model:
  - Board representation
  - Piece, Move, Position
  - GameState
- Domain-specific exceptions
- Initial project structure

#### 🔄 In Progress
- Move generation architecture
- Piece-specific move logic
- Rule validation pipeline

#### ⏳ Planned
- AI (minimax + alpha-beta)
- MAUI UI implementation
- Backend API & persistence
- Game replay & analysis tools

---
## 🧪 Testing (Planned)

Unit tests for:
- move generation
- rule validation
- edge cases (checkmate, stalemate)

Integration tests for:
- full game simulations

---
## 🎯 Purpose

This project is intended to:
- demonstrate advanced C# and .NET architecture
- showcase algorithmic problem solving (chess engine)
- build a real-world MAUI application
- serve as a long-term learning and experimentation platform

---
## 🗺 Roadmap

#### Phase 1 – Engine Foundation
- piece move generation
- legality validation
- check detection

#### Phase 2 – Playable Game
- AI opponent
- MAUI board UI
- game loop

#### Phase 3 – Persistence & Backend
- save/load games
- API layer

#### Phase 4 – Advanced Features
- analysis mode
- puzzle system
- multiplayer

---
## 💡 Future Ideas
- Chess training mode (tactics, puzzles)
- AI evaluation visualization
- Opening explorer
- Web frontend using same backend

---
## 🚀 Getting Started (Coming Soon)

Instructions for building and running the project will be added as the system becomes runnable.

---
## 🙌 Motivation

KnightShift is both a technical challenge and a personal project, combining a new-found passion for chess with software architecture and system design.