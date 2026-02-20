using EditorTextoUndoRedo.Console.Application;
using EditorTextoUndoRedo.Console.Commands;
using EditorTextoUndoRedo.Console.Core;

Console.WriteLine("=== Editor de Texto - Command Pattern ===\n");

var app = new EditorApplication();

// ---------------------------
// Operações básicas
// ---------------------------

Console.WriteLine(">> Digitando texto...");
app.TypeText("Hello");
app.TypeText(" World");
app.ShowContent();

Console.WriteLine(">> Deletando 6 caracteres...");
app.DeleteCharacters(6); // remove " World"
app.ShowContent();

// ---------------------------
// Undo
// ---------------------------

Console.WriteLine(">> Undo 1:");
app.Undo();
app.ShowContent();

Console.WriteLine(">> Undo 2:");
app.Undo();
app.ShowContent();

// ---------------------------
// Redo
// ---------------------------

Console.WriteLine(">> Redo 1:");
app.Redo();
app.ShowContent();

// ---------------------------
// Macro (exemplo opcional)
// ---------------------------

Console.WriteLine(">> Executando Macro (!!!)");
            
// Criamos um novo editor interno apenas para exemplo didático
var editor = new TextEditor();

var commands = new List<ICommand>
{
    new InsertTextCommand(editor, "C# "),
    new InsertTextCommand(editor, "Design "),
    new InsertTextCommand(editor, "Patterns")
};

var macroApp = new EditorApplication();
macroApp.ExecuteMacro(commands);
macroApp.ShowContent();

Console.WriteLine("=== Fim da execução ===");