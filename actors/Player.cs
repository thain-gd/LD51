using Godot;
using System;

public class Player : Area2D
{
    [Export]
    private StreamTexture idleSprite;
    
    [Export]
    private StreamTexture jumpSprite;

    [Export]
    private StreamTexture landSprite;

    private Sprite playerSprite;
    private AudioStreamPlayer2D audioPlayer;
    private Vector2 nextPosition;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        playerSprite = GetNode<Sprite>("Sprite");
        playerSprite.Texture = idleSprite;

        audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        nextPosition = Position;
    }

    public override void _Process(float delta)
    {
        // if (velocity.Length() > 0)
        // {
        //     velocity = velocity.Normalized() * Speed;
        //     playerSprite.Animation = "jump";
        // }
        // else
        // {
        //     playerSprite.Animation = "idle";
        // }

        if ((nextPosition - Position).Length() <= 5f)
        {
            Position = nextPosition;
            playerSprite.Texture = idleSprite;
        }
        else
        {
            Position = Position.LinearInterpolate(nextPosition, 0.15f);
            playerSprite.Texture = jumpSprite;
        }
    }

    public void JumpTo(Vector2 position)
    {
        audioPlayer.Play();
        nextPosition = position;
    }

    public void Land()
    {

    }
}
