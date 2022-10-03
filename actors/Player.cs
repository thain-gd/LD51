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
    private bool isJumping;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        audioPlayer = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        nextPosition = Position;
    }

    public void Initialize()
    {
        playerSprite = GetNode<Sprite>("Sprite");
        playerSprite.Texture = idleSprite;
    }

    public override void _Process(float delta)
    {
        if (isJumping)
        {
            if ((nextPosition - Position).Length() <= 5f)
            {
                Position = nextPosition;
                playerSprite.Texture = idleSprite;
                playerSprite.Scale = new Vector2(-playerSprite.Scale.x, 1);
                isJumping = false;
            }
            else
            {
                Position = Position.LinearInterpolate(nextPosition, 0.15f);
                playerSprite.Texture = jumpSprite;
            }
        }
    }

    public void JumpTo(Vector2 position, bool isLeftBranch, bool isLastBranch)
    {
        audioPlayer.Play();
        nextPosition = position;
        SetDirection(isLeftBranch);
    }

    public void SetDirection(bool isLeftBranch)
    {
        playerSprite.Scale = new Vector2(isLeftBranch ? -1 : 1, 1);
        isJumping = true;
    }
}
