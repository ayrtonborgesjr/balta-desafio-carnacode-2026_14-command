namespace EditorTextoUndoRedo.Console.Commands;

public class MacroCommand : ICommand
{
    private readonly IList<ICommand> _commands;

    public MacroCommand(IList<ICommand> commands)
    {
        _commands = commands;
    }

    public void Execute()
    {
        foreach (var command in _commands)
        {
            command.Execute();
        }
    }

    public void Undo()
    {
        for (int i = _commands.Count - 1; i >= 0; i--)
        {
            _commands[i].Undo();
        }
    }
}