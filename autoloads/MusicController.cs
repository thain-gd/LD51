using Godot;
using System;

public class MusicController : Node2D
{
    private AudioStreamPlayer audioPlayer;
    private AudioStreamSample musicStream;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        musicStream = ((AudioStreamSample)GetNode<AudioStreamPlayer>("AudioStreamPlayer").Stream);
    }

    public override void _Process(float delta)
    {
        if (audioPlayer.GetPlaybackPosition() >= 50)
        {
            Stop();
            Start();
        }
    }

    public void Start()
    {
        audioPlayer.Play();
    }

    public void Stop()
    {
        audioPlayer.Stop();
    }
}
