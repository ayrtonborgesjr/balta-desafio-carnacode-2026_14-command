using EditorTextoUndoRedo.Console.Core;

namespace EditorTextoUndoRedo.Console.Commands;

public class DeleteTextCommand : ICommand
{
    private readonly TextEditor _editor;
    private readonly int _length;

    private string _deletedText = string.Empty;
    private int _position;

    public DeleteTextCommand(TextEditor editor, int length)
    {
        _editor = editor;
        _length = length;
    }

    public void Execute()
    {
        _position = _editor.GetCursorPosition();
        _deletedText = _editor.DeleteText(_length);
    }

    public void Undo()
    {
        _editor.SetCursorPosition(_position - _deletedText.Length);
        _editor.InsertText(_deletedText);
    }
}