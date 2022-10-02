using Godot;
using System;

public class Player : Area2D
{
    [Export]
    public int Speed = 400;

    private AnimatedSprite animatedSprite;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
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
            animatedSprite.Animation = "jump";
        }
        else
        {
            animatedSprite.Animation = "idle";
        }

        Position += velocity * delta;
    }
}
