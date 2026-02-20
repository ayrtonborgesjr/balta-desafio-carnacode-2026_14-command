using EditorTextoUndoRedo.Console.Core;

namespace EditorTextoUndoRedo.Console.Commands;

public class InsertTextCommand : ICommand
{
    private readonly TextEditor _editor;
    private readonly string _text;

    private int _position;
    
    public InsertTextCommand(TextEditor editor, string text)
    {
        _editor = editor;
        _text = text;
    }
    
    public void Execute()
    {
        _position = _editor.GetCursorPosition();
        _editor.InsertText(_text);
    }

    public void Undo()
    {
        _editor.SetCursorPosition(_position + _text.Length);
        _editor.DeleteText(_text.Length);
    }
}