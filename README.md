# ♟️ KnightShift – Cross-Platform Chess Engine & App

![Platform](https://img.shields.io/badge/Platform-.NET%208-blue)
![UI](https://img.shields.io/badge/UI-.NET%20MAUI-purple)
![Tests](https://img.shields.io/badge/Tests-xUnit-green)
![Focus](https://img.shields.io/badge/Focus-Chess%20Engine-orange)
![Status](https://img.shields.io/badge/Status-In%20Development-yellow)
[![CI](https://github.com/fseckel-develop/app-knightshift-chess-maui/actions/workflows/ci.yml/badge.svg)](https://github.com/fseckel-develop/app-knightshift-chess-maui/actions/workflows/ci.yml)

**[This README is outdated. Made Improvements to the code will be documented here soon. For more detailed information about the development please check out the well-maintained commit history and the structured source and test directories.**

**KnightShift** is planned to be a cross-platform chess application built with **.NET MAUI**, powered by a custom-designed chess engine implemented in C#.

The project will combine:
- ♟️ **Chess engine development** (rules, move generation, validation, evaluation, AI)
- 🧱 **Clean architecture & domain-driven design**
- 📱 **Multiple frontends** (Cross-platform GUI with .NET MAUI, console-based CLI)

The goal is to create a **fully structured, extensible chess system** — from engine to UI — with a strong focus on **clarity, maintainability, and long-term evolution**.

---
## ✨ Key Features

#### Engine (Implemented)
- Full pseudo-legal move generation (all pieces)
- Legal move validation (king safety)
- Check, checkmate, and stalemate detection
- Special rules (castling, en passant, promotion)
- Minimax-based AI with alpha-beta pruning (planned)
- Configurable difficulty

#### Mobile Application (Planned)
- Interactive chessboard UI (.NET MAUI)
- Human vs AI gameplay
- Move highlighting and history
- Game state persistence

#### CLI (Planned)
- Console-based chess interface
- Board rendering in terminal
- Move input via algebraic notation
- Useful for debugging and engine interaction

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
├── Domain          → core models (Board, Piece, Move, GameState)
├── Engine          → move generation, validation, rules, evaluation
├── Application     → orchestration / use cases (planned)
├── Infrastructure  → persistence & external systems (planned)
├── API             → backend endpoints (planned)
├── CLI             → console frontend (planned)
└── MAUI            → .NET MAUI frontend (planned)
```

#### Design Principles
- Domain-first design with chess-native modeling
- Separation of:
  - state (Domain)
  - behavior (Engine)
  - orchestration (Application)
  - presentation (CLI/MAUI)
- Modular move generation (per piece)
- High testability and explicit rule enforcement

---
## 🧪 Testing

The project uses a layered testing approach, mirroring the architecture.

#### Domain Tests
- Board behavior
- Position validation & algebraic notation
- GameState state transitions
- Domain invariants and exceptions

#### Engine Tests
- Move generation
  - Step-based (king, knight)
  - Sliding (rook, bishop, queen)
  - Pawn logic (movement, capture, promotion, en passant)
  - MoveGenerator orchestration
- Rules
  - Check detection
  - Move validation (illegal moves)
- Evaluation
  - Checkmate detection
  - Stalemate detection

This ensures:
- Clear separation of responsibilities
- High confidence in correctness
- Maintainable and extensible test structure

---
## 🚧 Current Status

This project is under active refactoring and continuous development.

#### ✅ Implemented
- Core domain model:
	- Board, Position (algebraic notation)
	- Piece, Move
	- GameState (including castling & en passant state)
- Engine:
	- Step-based movement (king, knight)
	- Sliding movement (rook, bishop, queen)
	- Pawn logic (movement, capture, promotion, en passant)
	- Move validation (prevents illegal moves)
	- Check detection
	- Game result evaluation (checkmate, stalemate)
- Comprehensive unit test suite (xUnit + FluentAssertions)

#### 🔄 In Progress
- attack maps
- Engine refinements & edge-case handling
- Improved test tooling (e.g., FEN-based setup)
- Introduction of chess constants for clarity

#### ⏳ Planned
- Minimax AI with alpha-beta pruning
- CLI frontend
- MAUI UI implementation
- Backend API & persistence
- Game replay & analysis tools

---
## 🎯 Purpose

This project is intended to:
- demonstrate advanced C# and .NET architecture
- showcase algorithmic problem solving (chess engine design)
- build a real-world MAUI application
- serve as a long-term learning and experimentation platform

---
## 🗺 Roadmap

#### Phase 1 – Engine Foundation ✅
- move generation
- legality validation
- check detection
- game result evaluation

#### Phase 2 – Search & AI
- attach maps
- move scoring
- minimax implementation
- alpha-beta pruning
- evaluation heuristics

#### Phase 3 – Playable Interface
- CLI interface
- MAUI UI
- game loop

#### Phase 4 – Persistence & Backend
- save/load games
- API layer

#### Phase 5 – Advanced Features
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