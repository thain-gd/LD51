using Godot;
using System;

public class Player : AnimatedSprite
{
    [Export]
    public int Speed = 400;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        var velocity = Vector2.Zero; // The player's movement vector.

        if (Input.IsActionPressed("right_arrow"))
        {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("left_arrow"))
        {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("down_arrow"))
        {
            velocity.y += 1;
        }

        if (Input.IsActionPressed("up_arrow"))
        {
            velocity.y -= 1;
        }

        if (velocity.Length() > 0)
        {
            velocity = velocity.Normalized() * Speed;
            Play();
        }
        else
        {
            Stop();
        }

        Position += velocity * delta;
    }
}
