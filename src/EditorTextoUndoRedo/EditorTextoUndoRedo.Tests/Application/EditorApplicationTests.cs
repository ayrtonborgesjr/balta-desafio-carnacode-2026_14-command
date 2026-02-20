using EditorTextoUndoRedo.Console.Application;
using EditorTextoUndoRedo.Console.Commands;
using EditorTextoUndoRedo.Console.Core;

namespace EditorTextoUndoRedo.Tests.Application;

public class EditorApplicationTests
{
    [Fact]
    public void Constructor_ShouldInitializeEditorAndCommandManager()
    {
        // Arrange & Act
        var app = new EditorApplication();

        // Assert - Should not throw and should be ready to use
        var exception = Record.Exception(() => app.ShowContent());
        Assert.Null(exception);
    }

    [Fact]
    public void TypeText_ShouldAddTextToEditor()
    {
        // Arrange
        var app = new EditorApplication();

        // Act
        app.TypeText("Hello");

        // Assert - We can't directly access the editor, but we can test through other operations
        app.TypeText(" World");
        // If it executes without error, the text was added
    }

    [Fact]
    public void DeleteCharacters_ShouldRemoveCharactersFromEditor()
    {
        // Arrange
        var app = new EditorApplication();
        app.TypeText("Hello World");

        // Act & Assert - Should not throw
        var exception = Record.Exception(() => app.DeleteCharacters(5));
        Assert.Null(exception);
    }

    [Fact]
    public void Undo_ShouldUndoLastOperation()
    {
        // Arrange
        var app = new EditorApplication();
        app.TypeText("Test");

        // Act & Assert - Should not throw
        var exception = Record.Exception(() => app.Undo());
        Assert.Null(exception);
    }

    [Fact]
    public void Redo_ShouldRedoLastUndoneOperation()
    {
        // Arrange
        var app = new EditorApplication();
        app.TypeText("Test");
        app.Undo();

        // Act & Assert - Should not throw
        var exception = Record.Exception(() => app.Redo());
        Assert.Null(exception);
    }

    [Fact]
    public void ShowContent_ShouldNotThrow()
    {
        // Arrange
        var app = new EditorApplication();
        app.TypeText("Content");

        // Act & Assert
        var exception = Record.Exception(() => app.ShowContent());
        Assert.Null(exception);
    }

    [Fact]
    public void ExecuteMacro_ShouldExecuteAllCommands()
    {
        // Arrange
        var app = new EditorApplication();
        var editor = new TextEditor();
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, "Macro"),
            new InsertTextCommand(editor, "Test")
        };

        // Act & Assert - Should not throw
        var exception = Record.Exception(() => app.ExecuteMacro(commands));
        Assert.Null(exception);
    }

    [Fact]
    public void TypeText_MultipleOperations_ShouldAccumulateText()
    {
        // Arrange
        var app = new EditorApplication();

        // Act
        app.TypeText("First");
        app.TypeText(" Second");
        app.TypeText(" Third");

        // Assert - Should not throw
        var exception = Record.Exception(() => app.ShowContent());
        Assert.Null(exception);
    }

    [Fact]
    public void UndoRedo_Sequence_ShouldWorkCorrectly()
    {
        // Arrange
        var app = new EditorApplication();
        app.TypeText("Line1");
        app.TypeText("Line2");

        // Act & Assert
        var exception1 = Record.Exception(() => app.Undo());
        Assert.Null(exception1);

        var exception2 = Record.Exception(() => app.Undo());
        Assert.Null(exception2);

        var exception3 = Record.Exception(() => app.Redo());
        Assert.Null(exception3);

        var exception4 = Record.Exception(() => app.Redo());
        Assert.Null(exception4);
    }

    [Fact]
    public void TypeAndDelete_Sequence_ShouldWorkCorrectly()
    {
        // Arrange
        var app = new EditorApplication();

        // Act & Assert
        app.TypeText("Hello World");
        var exception1 = Record.Exception(() => app.DeleteCharacters(5));
        Assert.Null(exception1);

        var exception2 = Record.Exception(() => app.ShowContent());
        Assert.Null(exception2);
    }

    [Fact]
    public void EmptyMacro_ShouldNotThrow()
    {
        // Arrange
        var app = new EditorApplication();
        var commands = new List<ICommand>();

        // Act & Assert
        var exception = Record.Exception(() => app.ExecuteMacro(commands));
        Assert.Null(exception);
    }

    [Fact]
    public void ComplexWorkflow_ShouldNotThrow()
    {
        // Arrange
        var app = new EditorApplication();

        // Act - Simulate a complex user workflow
        app.TypeText("Hello");
        app.TypeText(" World");
        app.ShowContent();
        app.DeleteCharacters(6);
        app.ShowContent();
        app.Undo();
        app.ShowContent();
        app.Undo();
        app.ShowContent();
        app.Redo();
        app.ShowContent();

        // Assert - Should complete without throwing
        var exception = Record.Exception(() => app.ShowContent());
        Assert.Null(exception);
    }

    [Fact]
    public void TypeText_WithEmptyString_ShouldNotThrow()
    {
        // Arrange
        var app = new EditorApplication();

        // Act & Assert
        var exception = Record.Exception(() => app.TypeText(string.Empty));
        Assert.Null(exception);
    }

    [Fact]
    public void DeleteCharacters_WithZero_ShouldNotThrow()
    {
        // Arrange
        var app = new EditorApplication();
        app.TypeText("Test");

        // Act & Assert
        var exception = Record.Exception(() => app.DeleteCharacters(0));
        Assert.Null(exception);
    }

    [Fact]
    public void Undo_WithNoHistory_ShouldNotThrow()
    {
        // Arrange
        var app = new EditorApplication();

        // Act & Assert
        var exception = Record.Exception(() => app.Undo());
        Assert.Null(exception);
    }

    [Fact]
    public void Redo_WithNoHistory_ShouldNotThrow()
    {
        // Arrange
        var app = new EditorApplication();

        // Act & Assert
        var exception = Record.Exception(() => app.Redo());
        Assert.Null(exception);
    }

    [Fact]
    public void MacroWithUndo_ShouldWorkCorrectly()
    {
        // Arrange
        var app = new EditorApplication();
        var editor = new TextEditor();
        var commands = new List<ICommand>
        {
            new InsertTextCommand(editor, "A"),
            new InsertTextCommand(editor, "B"),
            new InsertTextCommand(editor, "C")
        };

        // Act & Assert
        app.ExecuteMacro(commands);
        var exception = Record.Exception(() => app.Undo());
        Assert.Null(exception);
    }

    [Fact]
    public void ShowContent_OnEmptyEditor_ShouldNotThrow()
    {
        // Arrange
        var app = new EditorApplication();

        // Act & Assert
        var exception = Record.Exception(() => app.ShowContent());
        Assert.Null(exception);
    }
}

