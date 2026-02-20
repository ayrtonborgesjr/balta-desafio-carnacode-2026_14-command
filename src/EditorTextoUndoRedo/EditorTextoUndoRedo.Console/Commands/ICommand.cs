namespace EditorTextoUndoRedo.Console.Commands;

public interface ICommand
{
    void Execute();
    void Undo();
}