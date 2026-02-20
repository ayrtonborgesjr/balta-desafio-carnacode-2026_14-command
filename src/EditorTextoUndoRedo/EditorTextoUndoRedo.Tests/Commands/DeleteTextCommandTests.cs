using EditorTextoUndoRedo.Console.Commands;
using EditorTextoUndoRedo.Console.Core;

namespace EditorTextoUndoRedo.Tests.Commands;

public class DeleteTextCommandTests
{
    [Fact]
    public void Execute_ShouldDeleteTextFromEditor()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello World");
        var command = new DeleteTextCommand(editor, 5);

        // Act
        command.Execute();

        // Assert
        Assert.Equal("Hello ", editor.GetContent());
        Assert.Equal(6, editor.GetCursorPosition());
    }

    [Fact]
    public void Execute_ShouldStoreCursorPosition()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Testing");
        var cursorBefore = editor.GetCursorPosition();
        var command = new DeleteTextCommand(editor, 3);

        // Act
        command.Execute();

        // Assert
        Assert.Equal("Test", editor.GetContent());
        Assert.Equal(cursorBefore - 3, editor.GetCursorPosition());
    }

    [Fact]
    public void Undo_ShouldRestoreDeletedText()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello World");
        var command = new DeleteTextCommand(editor, 5);
        command.Execute();

        // Act
        command.Undo();

        // Assert
        Assert.Equal("Hello World", editor.GetContent());
        Assert.Equal(11, editor.GetCursorPosition());
    }

    [Fact]
    public void Undo_ShouldRestorePreviousState()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Complete Text");
        var originalContent = editor.GetContent();
        var originalCursor = editor.GetCursorPosition();
        var command = new DeleteTextCommand(editor, 5);
        command.Execute();

        // Act
        command.Undo();

        // Assert
        Assert.Equal(originalContent, editor.GetContent());
        Assert.Equal(originalCursor, editor.GetCursorPosition());
    }

    [Fact]
    public void ExecuteAndUndo_MultipleTimes_ShouldBeIdempotent()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("TestText");
        var command = new DeleteTextCommand(editor, 4);

        // Act & Assert
        command.Execute();
        Assert.Equal("Test", editor.GetContent());

        command.Undo();
        Assert.Equal("TestText", editor.GetContent());

        command.Execute();
        Assert.Equal("Test", editor.GetContent());

        command.Undo();
        Assert.Equal("TestText", editor.GetContent());
    }

    [Fact]
    public void Execute_WithZeroLength_ShouldNotChangeContent()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello");
        var command = new DeleteTextCommand(editor, 0);

        // Act
        command.Execute();

        // Assert
        Assert.Equal("Hello", editor.GetContent());
    }

    [Fact]
    public void Execute_WithLengthGreaterThanContent_ShouldDeleteAllContent()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hi");
        var command = new DeleteTextCommand(editor, 100);

        // Act
        command.Execute();

        // Assert
        Assert.Equal(string.Empty, editor.GetContent());
        Assert.Equal(0, editor.GetCursorPosition());
    }

    [Fact]
    public void Undo_AfterDeletingAllContent_ShouldRestoreEverything()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Full Content");
        var command = new DeleteTextCommand(editor, 100);
        command.Execute();

        // Act
        command.Undo();

        // Assert
        Assert.Equal("Full Content", editor.GetContent());
        Assert.Equal(12, editor.GetCursorPosition());
    }

    [Fact]
    public void Execute_WithCursorAtMiddle_ShouldDeleteCorrectly()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello World");
        editor.SetCursorPosition(5);
        var command = new DeleteTextCommand(editor, 3);

        // Act
        command.Execute();

        // Assert
        Assert.Equal("He World", editor.GetContent());
        Assert.Equal(2, editor.GetCursorPosition());
    }

    [Fact]
    public void Undo_AfterMiddlePositionDelete_ShouldRestoreCorrectly()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello World");
        editor.SetCursorPosition(5);
        var command = new DeleteTextCommand(editor, 3);
        command.Execute();

        // Act
        command.Undo();

        // Assert
        Assert.Equal("Hello World", editor.GetContent());
        Assert.Equal(5, editor.GetCursorPosition());
    }
}

