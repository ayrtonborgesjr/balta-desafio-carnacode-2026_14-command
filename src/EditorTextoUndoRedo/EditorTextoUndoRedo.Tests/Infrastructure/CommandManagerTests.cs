using EditorTextoUndoRedo.Console.Commands;
using EditorTextoUndoRedo.Console.Core;
using EditorTextoUndoRedo.Console.Infrastructure;

namespace EditorTextoUndoRedo.Tests.Infrastructure;

public class CommandManagerTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithEmptyStacks()
    {
        // Arrange & Act
        var manager = new CommandManager();

        // Assert
        Assert.Equal(0, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void ExecuteCommand_ShouldExecuteCommandAndAddToUndoStack()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();
        var command = new InsertTextCommand(editor, "Test");

        // Act
        manager.ExecuteCommand(command);

        // Assert
        Assert.Equal("Test", editor.GetContent());
        Assert.Equal(1, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void ExecuteCommand_ShouldClearRedoStack()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();
        var command1 = new InsertTextCommand(editor, "First");
        var command2 = new InsertTextCommand(editor, "Second");
        
        manager.ExecuteCommand(command1);
        manager.Undo();

        // Act
        manager.ExecuteCommand(command2);

        // Assert
        Assert.Equal(1, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void Undo_ShouldUndoLastCommand()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();
        var command = new InsertTextCommand(editor, "Test");
        manager.ExecuteCommand(command);

        // Act
        manager.Undo();

        // Assert
        Assert.Equal(string.Empty, editor.GetContent());
        Assert.Equal(0, manager.UndoCount);
        Assert.Equal(1, manager.RedoCount);
    }

    [Fact]
    public void Undo_WithEmptyUndoStack_ShouldDoNothing()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();

        // Act
        manager.Undo();

        // Assert
        Assert.Equal(0, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void Undo_ShouldMoveCommandToRedoStack()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();
        var command = new InsertTextCommand(editor, "Test");
        manager.ExecuteCommand(command);

        // Act
        manager.Undo();

        // Assert
        Assert.Equal(0, manager.UndoCount);
        Assert.Equal(1, manager.RedoCount);
    }

    [Fact]
    public void Redo_ShouldReExecuteLastUndoneCommand()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();
        var command = new InsertTextCommand(editor, "Test");
        manager.ExecuteCommand(command);
        manager.Undo();

        // Act
        manager.Redo();

        // Assert
        Assert.Equal("Test", editor.GetContent());
        Assert.Equal(1, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void Redo_WithEmptyRedoStack_ShouldDoNothing()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();

        // Act
        manager.Redo();

        // Assert
        Assert.Equal(0, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void Redo_ShouldMoveCommandToUndoStack()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();
        var command = new InsertTextCommand(editor, "Test");
        manager.ExecuteCommand(command);
        manager.Undo();

        // Act
        manager.Redo();

        // Assert
        Assert.Equal(1, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void MultipleUndoRedo_ShouldWorkCorrectly()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();
        var command1 = new InsertTextCommand(editor, "First");
        var command2 = new InsertTextCommand(editor, "Second");
        var command3 = new InsertTextCommand(editor, "Third");

        manager.ExecuteCommand(command1);
        manager.ExecuteCommand(command2);
        manager.ExecuteCommand(command3);

        // Act & Assert
        Assert.Equal("FirstSecondThird", editor.GetContent());
        Assert.Equal(3, manager.UndoCount);

        manager.Undo();
        Assert.Equal("FirstSecond", editor.GetContent());
        Assert.Equal(2, manager.UndoCount);
        Assert.Equal(1, manager.RedoCount);

        manager.Undo();
        Assert.Equal("First", editor.GetContent());
        Assert.Equal(1, manager.UndoCount);
        Assert.Equal(2, manager.RedoCount);

        manager.Redo();
        Assert.Equal("FirstSecond", editor.GetContent());
        Assert.Equal(2, manager.UndoCount);
        Assert.Equal(1, manager.RedoCount);

        manager.Redo();
        Assert.Equal("FirstSecondThird", editor.GetContent());
        Assert.Equal(3, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void ClearHistory_ShouldEmptyBothStacks()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();
        var command1 = new InsertTextCommand(editor, "First");
        var command2 = new InsertTextCommand(editor, "Second");
        
        manager.ExecuteCommand(command1);
        manager.ExecuteCommand(command2);
        manager.Undo();

        // Act
        manager.ClearHistory();

        // Assert
        Assert.Equal(0, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void ClearHistory_WithEmptyStacks_ShouldNotThrow()
    {
        // Arrange
        var manager = new CommandManager();

        // Act & Assert
        var exception = Record.Exception(() => manager.ClearHistory());
        Assert.Null(exception);
        Assert.Equal(0, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void ExecuteCommand_AfterUndo_ShouldClearRedoHistory()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();
        var command1 = new InsertTextCommand(editor, "First");
        var command2 = new InsertTextCommand(editor, "Second");
        var command3 = new InsertTextCommand(editor, "Third");

        manager.ExecuteCommand(command1);
        manager.ExecuteCommand(command2);
        manager.Undo();

        // Act
        manager.ExecuteCommand(command3);

        // Assert
        Assert.Equal("FirstThird", editor.GetContent());
        Assert.Equal(2, manager.UndoCount);
        Assert.Equal(0, manager.RedoCount);
    }

    [Fact]
    public void UndoCount_ShouldReflectCurrentUndoStackSize()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();

        // Act & Assert
        Assert.Equal(0, manager.UndoCount);

        manager.ExecuteCommand(new InsertTextCommand(editor, "1"));
        Assert.Equal(1, manager.UndoCount);

        manager.ExecuteCommand(new InsertTextCommand(editor, "2"));
        Assert.Equal(2, manager.UndoCount);

        manager.Undo();
        Assert.Equal(1, manager.UndoCount);
    }

    [Fact]
    public void RedoCount_ShouldReflectCurrentRedoStackSize()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();

        // Act & Assert
        Assert.Equal(0, manager.RedoCount);

        manager.ExecuteCommand(new InsertTextCommand(editor, "1"));
        manager.ExecuteCommand(new InsertTextCommand(editor, "2"));
        Assert.Equal(0, manager.RedoCount);

        manager.Undo();
        Assert.Equal(1, manager.RedoCount);

        manager.Undo();
        Assert.Equal(2, manager.RedoCount);

        manager.Redo();
        Assert.Equal(1, manager.RedoCount);
    }

    [Fact]
    public void ComplexScenario_WithDeleteAndInsert_ShouldWorkCorrectly()
    {
        // Arrange
        var manager = new CommandManager();
        var editor = new TextEditor();

        // Act & Assert
        manager.ExecuteCommand(new InsertTextCommand(editor, "Hello World"));
        Assert.Equal("Hello World", editor.GetContent());

        manager.ExecuteCommand(new DeleteTextCommand(editor, 5));
        Assert.Equal("Hello ", editor.GetContent());

        manager.Undo();
        Assert.Equal("Hello World", editor.GetContent());

        manager.Undo();
        Assert.Equal(string.Empty, editor.GetContent());

        manager.Redo();
        Assert.Equal("Hello World", editor.GetContent());

        manager.Redo();
        Assert.Equal("Hello ", editor.GetContent());
    }
}

