using Godot;
using System;

public class MathScreen : Panel
{
    private RichTextLabel firstNumberText;
    private RichTextLabel secondNumberText;
    private RichTextLabel resultText;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var equationNode = GetNode<Control>("Equation");
        firstNumberText = equationNode.GetNode<RichTextLabel>("Number");
        secondNumberText = equationNode.GetNode<RichTextLabel>("Number2");
        resultText = equationNode.GetNode<RichTextLabel>("Result");
    }

    public void UpdateEquation(int first, int second, int result)
    {
        firstNumberText.Text = first.ToString();
        secondNumberText.Text = second.ToString();
        resultText.Text = result.ToString();
    }

    public int GetResult()
    {
        return resultText.Text.ToInt();
    }
}
