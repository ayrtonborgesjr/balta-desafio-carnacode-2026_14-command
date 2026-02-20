using EditorTextoUndoRedo.Console.Commands;

namespace EditorTextoUndoRedo.Console.Infrastructure;

// Invoker do Command Pattern
// Responsável por gerenciar histórico e controle de execução
public class CommandManager
{
    private readonly Stack<ICommand> _undoStack;
    private readonly Stack<ICommand> _redoStack;

    public CommandManager()
    {
        _undoStack = new Stack<ICommand>();
        _redoStack = new Stack<ICommand>();
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();

        _undoStack.Push(command);
        _redoStack.Clear(); // Novo comando invalida o histórico de Redo
    }

    public void Undo()
    {
        if (_undoStack.Count == 0)
            return;

        var command = _undoStack.Pop();
        command.Undo();

        _redoStack.Push(command);
    }

    public void Redo()
    {
        if (_redoStack.Count == 0)
            return;

        var command = _redoStack.Pop();
        command.Execute();

        _undoStack.Push(command);
    }

    public void ClearHistory()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }

    public int UndoCount => _undoStack.Count;
    public int RedoCount => _redoStack.Count;
}