namespace EditorTextoUndoRedo.Console.Core;

// Receiver do Command Pattern
// Responsável apenas por manipular o estado do editor
public class TextEditor
{
    private string _content;
    private int _cursorPosition;

    public TextEditor()
    {
        _content = string.Empty;
        _cursorPosition = 0;
    }

    #region Text Operations

    public void InsertText(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        _content = _content.Insert(_cursorPosition, text);
        _cursorPosition += text.Length;

        System.Console.WriteLine($"[Editor] Inserido: '{text}'");
    }

    public string DeleteText(int length)
    {
        if (length <= 0 || _cursorPosition == 0)
            return string.Empty;

        int actualLength = Math.Min(length, _cursorPosition);
        int start = _cursorPosition - actualLength;

        string deletedText = _content.Substring(start, actualLength);

        _content = _content.Remove(start, actualLength);
        _cursorPosition -= actualLength;

        System.Console.WriteLine($"[Editor] Deletado: '{deletedText}'");

        return deletedText;
    }

    #endregion

    #region Cursor Operations

    public void SetCursorPosition(int position)
    {
        if (position < 0)
            position = 0;

        if (position > _content.Length)
            position = _content.Length;

        _cursorPosition = position;
    }

    public int GetCursorPosition()
    {
        return _cursorPosition;
    }

    #endregion

    #region Formatting (Simulado)

    public void SetBold(int start, int length)
    {
        System.Console.WriteLine($"[Editor] Aplicando negrito de {start} até {start + length}");
    }

    public void RemoveBold(int start, int length)
    {
        System.Console.WriteLine($"[Editor] Removendo negrito de {start} até {start + length}");
    }

    #endregion

    #region Getters

    public string GetContent()
    {
        return _content;
    }

    public int GetLength()
    {
        return _content.Length;
    }

    #endregion
}