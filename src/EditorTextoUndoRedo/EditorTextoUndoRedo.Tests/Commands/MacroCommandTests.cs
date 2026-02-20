using EditorTextoUndoRedo.Console.Commands;
using EditorTextoUndoRedo.Console.Core;

namespace EditorTextoUndoRedo.Tests.Commands;

public class MacroCommandTests
{
    [Fact]
    public void Execute_ShouldExecuteAllCommands()
    {
        // Arrange
        var editor = new TextEditor();
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, "Hello"),
            new InsertTextCommand(editor, " "),
            new InsertTextCommand(editor, "World")
        };
        var macro = new MacroCommand(commands);

        // Act
        macro.Execute();

        // Assert
        Assert.Equal("Hello World", editor.GetContent());
    }

    [Fact]
    public void Execute_ShouldExecuteCommandsInOrder()
    {
        // Arrange
        var editor = new TextEditor();
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, "First"),
            new InsertTextCommand(editor, "Second"),
            new InsertTextCommand(editor, "Third")
        };
        var macro = new MacroCommand(commands);

        // Act
        macro.Execute();

        // Assert
        Assert.Equal("FirstSecondThird", editor.GetContent());
    }

    [Fact]
    public void Undo_ShouldUndoAllCommands()
    {
        // Arrange
        var editor = new TextEditor();
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, "One"),
            new InsertTextCommand(editor, "Two"),
            new InsertTextCommand(editor, "Three")
        };
        var macro = new MacroCommand(commands);
        macro.Execute();

        // Act
        macro.Undo();

        // Assert
        Assert.Equal(string.Empty, editor.GetContent());
    }

    [Fact]
    public void Undo_ShouldUndoCommandsInReverseOrder()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Start");
        
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, "A"),
            new InsertTextCommand(editor, "B"),
            new DeleteTextCommand(editor, 1)
        };
        var macro = new MacroCommand(commands);
        macro.Execute();
        
        var contentAfterExecute = editor.GetContent();

        // Act
        macro.Undo();

        // Assert
        Assert.Equal("Start", editor.GetContent());
    }

    [Fact]
    public void ExecuteAndUndo_MultipleTimes_ShouldBeIdempotent()
    {
        // Arrange
        var editor = new TextEditor();
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, "Test"),
            new InsertTextCommand(editor, "123")
        };
        var macro = new MacroCommand(commands);

        // Act & Assert
        macro.Execute();
        Assert.Equal("Test123", editor.GetContent());

        macro.Undo();
        Assert.Equal(string.Empty, editor.GetContent());

        macro.Execute();
        Assert.Equal("Test123", editor.GetContent());

        macro.Undo();
        Assert.Equal(string.Empty, editor.GetContent());
    }

    [Fact]
    public void Execute_WithEmptyCommandList_ShouldNotThrow()
    {
        // Arrange
        var editor = new TextEditor();
        var commands = new List<ICommand>();
        var macro = new MacroCommand(commands);

        // Act & Assert
        var exception = Record.Exception(() => macro.Execute());
        Assert.Null(exception);
    }

    [Fact]
    public void Undo_WithEmptyCommandList_ShouldNotThrow()
    {
        // Arrange
        var editor = new TextEditor();
        var commands = new List<ICommand>();
        var macro = new MacroCommand(commands);
        macro.Execute();

        // Act & Assert
        var exception = Record.Exception(() => macro.Undo());
        Assert.Null(exception);
    }

    [Fact]
    public void Execute_WithMixedCommands_ShouldWorkCorrectly()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Initial");
        
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, " Text"),
            new DeleteTextCommand(editor, 4),
            new InsertTextCommand(editor, " Final")
        };
        var macro = new MacroCommand(commands);

        // Act
        macro.Execute();

        // Assert
        Assert.Equal("Initial  Final", editor.GetContent());
    }

    [Fact]
    public void Undo_WithMixedCommands_ShouldRestoreCorrectly()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Initial");
        var originalContent = editor.GetContent();
        
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, " Text"),
            new DeleteTextCommand(editor, 4),
            new InsertTextCommand(editor, " Final")
        };
        var macro = new MacroCommand(commands);
        macro.Execute();

        // Act
        macro.Undo();

        // Assert
        Assert.Equal(originalContent, editor.GetContent());
    }

    [Fact]
    public void Execute_WithSingleCommand_ShouldWorkAsNormalCommand()
    {
        // Arrange
        var editor = new TextEditor();
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, "Solo")
        };
        var macro = new MacroCommand(commands);

        // Act
        macro.Execute();

        // Assert
        Assert.Equal("Solo", editor.GetContent());
    }

    [Fact]
    public void Undo_WithSingleCommand_ShouldWorkAsNormalCommand()
    {
        // Arrange
        var editor = new TextEditor();
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, "Solo")
        };
        var macro = new MacroCommand(commands);
        macro.Execute();

        // Act
        macro.Undo();

        // Assert
        Assert.Equal(string.Empty, editor.GetContent());
    }
}

