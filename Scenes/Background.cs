using Godot;
using System;

public class Background : Sprite
{
    [Export]
    private float scrollingSpeed = 200;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        Position -= new Vector2(scrollingSpeed * delta, 0);
    }
}
