using EditorTextoUndoRedo.Console.Core;

namespace EditorTextoUndoRedo.Tests.Core;

public class TextEditorTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithEmptyContent()
    {
        // Arrange & Act
        var editor = new TextEditor();

        // Assert
        Assert.Equal(string.Empty, editor.GetContent());
        Assert.Equal(0, editor.GetCursorPosition());
        Assert.Equal(0, editor.GetLength());
    }

    [Fact]
    public void InsertText_ShouldAddTextAtCursorPosition()
    {
        // Arrange
        var editor = new TextEditor();

        // Act
        editor.InsertText("Hello");

        // Assert
        Assert.Equal("Hello", editor.GetContent());
        Assert.Equal(5, editor.GetCursorPosition());
        Assert.Equal(5, editor.GetLength());
    }

    [Fact]
    public void InsertText_ShouldAddMultipleTextsSequentially()
    {
        // Arrange
        var editor = new TextEditor();

        // Act
        editor.InsertText("Hello");
        editor.InsertText(" World");

        // Assert
        Assert.Equal("Hello World", editor.GetContent());
        Assert.Equal(11, editor.GetCursorPosition());
    }

    [Fact]
    public void InsertText_WithNullText_ShouldNotChangeContent()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Test");

        // Act
        editor.InsertText(null!);

        // Assert
        Assert.Equal("Test", editor.GetContent());
        Assert.Equal(4, editor.GetCursorPosition());
    }

    [Fact]
    public void InsertText_WithEmptyString_ShouldNotChangeContent()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Test");

        // Act
        editor.InsertText(string.Empty);

        // Assert
        Assert.Equal("Test", editor.GetContent());
        Assert.Equal(4, editor.GetCursorPosition());
    }

    [Fact]
    public void DeleteText_ShouldRemoveTextBeforeCursor()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello World");

        // Act
        var deleted = editor.DeleteText(5);

        // Assert
        Assert.Equal("Hello ", editor.GetContent());
        Assert.Equal("World", deleted);
        Assert.Equal(6, editor.GetCursorPosition());
    }

    [Fact]
    public void DeleteText_WithZeroLength_ShouldReturnEmptyString()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Test");

        // Act
        var deleted = editor.DeleteText(0);

        // Assert
        Assert.Equal(string.Empty, deleted);
        Assert.Equal("Test", editor.GetContent());
    }

    [Fact]
    public void DeleteText_WithNegativeLength_ShouldReturnEmptyString()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Test");

        // Act
        var deleted = editor.DeleteText(-5);

        // Assert
        Assert.Equal(string.Empty, deleted);
        Assert.Equal("Test", editor.GetContent());
    }

    [Fact]
    public void DeleteText_WithCursorAtZero_ShouldReturnEmptyString()
    {
        // Arrange
        var editor = new TextEditor();

        // Act
        var deleted = editor.DeleteText(5);

        // Assert
        Assert.Equal(string.Empty, deleted);
        Assert.Equal(string.Empty, editor.GetContent());
    }

    [Fact]
    public void DeleteText_WithLengthGreaterThanCursorPosition_ShouldDeleteUpToCursor()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hi");

        // Act
        var deleted = editor.DeleteText(10);

        // Assert
        Assert.Equal("Hi", deleted);
        Assert.Equal(string.Empty, editor.GetContent());
        Assert.Equal(0, editor.GetCursorPosition());
    }

    [Fact]
    public void SetCursorPosition_ShouldUpdateCursorPosition()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello World");

        // Act
        editor.SetCursorPosition(5);

        // Assert
        Assert.Equal(5, editor.GetCursorPosition());
    }

    [Fact]
    public void SetCursorPosition_WithNegativeValue_ShouldSetToZero()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello");

        // Act
        editor.SetCursorPosition(-5);

        // Assert
        Assert.Equal(0, editor.GetCursorPosition());
    }

    [Fact]
    public void SetCursorPosition_WithValueGreaterThanLength_ShouldSetToLength()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello");

        // Act
        editor.SetCursorPosition(100);

        // Assert
        Assert.Equal(5, editor.GetCursorPosition());
    }

    [Fact]
    public void InsertText_AtMiddleOfContent_ShouldInsertCorrectly()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello World");
        editor.SetCursorPosition(5);

        // Act
        editor.InsertText(" C#");

        // Assert
        Assert.Equal("Hello C# World", editor.GetContent());
        Assert.Equal(8, editor.GetCursorPosition());
    }

    [Fact]
    public void DeleteText_AtMiddleOfContent_ShouldDeleteCorrectly()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello World");
        editor.SetCursorPosition(5);

        // Act
        var deleted = editor.DeleteText(5);

        // Assert
        Assert.Equal("Hello", deleted);
        Assert.Equal(" World", editor.GetContent());
        Assert.Equal(0, editor.GetCursorPosition());
    }

    [Fact]
    public void GetContent_ShouldReturnCurrentContent()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Test Content");

        // Act
        var content = editor.GetContent();

        // Assert
        Assert.Equal("Test Content", content);
    }

    [Fact]
    public void GetLength_ShouldReturnContentLength()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("12345");

        // Act
        var length = editor.GetLength();

        // Assert
        Assert.Equal(5, length);
    }

    [Fact]
    public void SetBold_ShouldNotThrowException()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello");

        // Act & Assert
        var exception = Record.Exception(() => editor.SetBold(0, 5));
        Assert.Null(exception);
    }

    [Fact]
    public void RemoveBold_ShouldNotThrowException()
    {
        // Arrange
        var editor = new TextEditor();
        editor.InsertText("Hello");

        // Act & Assert
        var exception = Record.Exception(() => editor.RemoveBold(0, 5));
        Assert.Null(exception);
    }
}

