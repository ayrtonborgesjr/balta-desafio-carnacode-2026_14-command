using EditorTextoUndoRedo.Console.Commands;
using EditorTextoUndoRedo.Console.Core;
using EditorTextoUndoRedo.Console.Infrastructure;

namespace EditorTextoUndoRedo.Console.Application;

// Camada de aplicação
// Orquestra comandos e expõe operações de alto nível
public class EditorApplication
{
    private readonly TextEditor _editor;
    private readonly CommandManager _commandManager;

    public EditorApplication()
    {
        _editor = new TextEditor();
        _commandManager = new CommandManager();
    }

    #region Operações de Texto

    public void TypeText(string text)
    {
        var command = new InsertTextCommand(_editor, text);
        _commandManager.ExecuteCommand(command);
    }

    public void DeleteCharacters(int count)
    {
        var command = new DeleteTextCommand(_editor, count);
        _commandManager.ExecuteCommand(command);
    }

    #endregion

    #region Undo / Redo

    public void Undo()
    {
        _commandManager.Undo();
    }

    public void Redo()
    {
        _commandManager.Redo();
    }

    #endregion

    #region Macro (Opcional)

    public void ExecuteMacro(List<ICommand> commands)
    {
        var macro = new MacroCommand(commands);
        _commandManager.ExecuteCommand(macro);
    }

    #endregion

    #region Visualização

    public void ShowContent()
    {
        System.Console.WriteLine("\n=== Editor ===");
        System.Console.WriteLine($"Conteúdo: '{_editor.GetContent()}'");
        System.Console.WriteLine($"Cursor: {_editor.GetCursorPosition()}");
        System.Console.WriteLine($"Undo disponíveis: {_commandManager.UndoCount}");
        System.Console.WriteLine($"Redo disponíveis: {_commandManager.RedoCount}");
        System.Console.WriteLine("==============\n");
    }

    #endregion
}