using Godot;
using System;

public class MainMenu : Control
{
    public override void _Ready()
    {
        GetParent().GetNode<MusicController>("MusicController").Start();
    }

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventKey eventKey)
        {
            if (eventKey.Pressed)
            {
                GetTree().ChangeScene("res://scenes/MainGame.tscn");
            }
        }
    }
}
