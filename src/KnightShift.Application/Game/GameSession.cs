using KnightShift.Domain.Core;

namespace KnightShift.Application.Game;

public class GameSession
{
    public GameState InitialState { get; }
    public GameState CurrentState { get; private set; }
    private readonly Stack<GameSnapshot> _undoStack = new();
    private readonly Stack<GameSnapshot> _redoStack = new();

    public GameSession(GameState initial)
    {
        InitialState = initial;
        CurrentState = initial;
    }

    public void ApplyMove(Move move)
    {
        _undoStack.Push(new GameSnapshot(CurrentState, move));
        _redoStack.Clear();
        CurrentState = CurrentState.ApplyMove(move);
    }
    
    public bool TryUndoMove()
    {
        if (_undoStack.Count == 0)
            return false;

        var snapshot = _undoStack.Pop();
        _redoStack.Push(new GameSnapshot(CurrentState, snapshot.Move));

        CurrentState = snapshot.State;
        return true;
    }

    public bool TryRedoMove()
    {
        if (_redoStack.Count == 0)
            return false;

        var snapshot = _redoStack.Pop();
        _undoStack.Push(new GameSnapshot(CurrentState, snapshot.Move));

        CurrentState = CurrentState.ApplyMove(snapshot.Move!);
        return true;
    }

    public IEnumerable<Move> GetMoves()
    {
        return _undoStack.Reverse()
            .Select(snapshot => snapshot.Move!)
            .Where(move => move != null);
    }
}
