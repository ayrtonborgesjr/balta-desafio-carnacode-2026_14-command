using EditorTextoUndoRedo.Console.Commands;
using EditorTextoUndoRedo.Console.Core;

namespace EditorTextoUndoRedo.Tests.Commands;

public class InsertTextCommandTests
{
    [Fact]
    public void Execute_ShouldInsertTextIntoEditor()
    {
        // Arrange
        var editor = new TextEditor();
        var command = new InsertTextCommand(editor, "Hello");

        // Act
        command.Execute();

        // Assert
        Assert.Equal("Hello", editor.GetContent());
        Assert.Equal(5, editor.GetCursorPosition());
    }

    [Fact]
    public void Execute_ShouldRememberCursorPosition()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Initial");
        editor.SetCursorPosition(3);
        var command = new InsertTextCommand(editor, "XXX");

        // Act
        command.Execute();

        // Assert
        Assert.Equal("IniXXXtial", editor.GetContent());
        Assert.Equal(6, editor.GetCursorPosition());
    }

    [Fact]
    public void Undo_ShouldRemoveInsertedText()
    {
        // Arrange
        var editor = new TextEditor();
        var command = new InsertTextCommand(editor, "Hello");
        command.Execute();

        // Act
        command.Undo();

        // Assert
        Assert.Equal(string.Empty, editor.GetContent());
        Assert.Equal(0, editor.GetCursorPosition());
    }

    [Fact]
    public void Undo_ShouldRestorePreviousState()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Start");
        var command = new InsertTextCommand(editor, " End");
        command.Execute();

        // Act
        command.Undo();

        // Assert
        Assert.Equal("Start", editor.GetContent());
        Assert.Equal(5, editor.GetCursorPosition());
    }

    [Fact]
    public void ExecuteAndUndo_MultipleTimes_ShouldBeIdempotent()
    {
        // Arrange
        var editor = new TextEditor();
        var command = new InsertTextCommand(editor, "Test");

        // Act & Assert
        command.Execute();
        Assert.Equal("Test", editor.GetContent());

        command.Undo();
        Assert.Equal(string.Empty, editor.GetContent());

        command.Execute();
        Assert.Equal("Test", editor.GetContent());

        command.Undo();
        Assert.Equal(string.Empty, editor.GetContent());
    }

    [Fact]
    public void Execute_WithMultipleCommands_ShouldAccumulateText()
    {
        // Arrange
        var editor = new TextEditor();
        var command1 = new InsertTextCommand(editor, "Hello");
        var command2 = new InsertTextCommand(editor, " ");
        var command3 = new InsertTextCommand(editor, "World");

        // Act
        command1.Execute();
        command2.Execute();
        command3.Execute();

        // Assert
        Assert.Equal("Hello World", editor.GetContent());
    }

    [Fact]
    public void Undo_AtMiddlePosition_ShouldWorkCorrectly()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("StartEnd");
        editor.SetCursorPosition(5);
        var command = new InsertTextCommand(editor, "Middle");
        command.Execute();

        // Act
        command.Undo();

        // Assert
        Assert.Equal("StartEnd", editor.GetContent());
        Assert.Equal(5, editor.GetCursorPosition());
    }
}

