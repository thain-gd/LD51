using Godot;
using System;

public class MusicController : Node2D
{
    [Export]
    private AudioStreamSample startMusic;

    [Export]
    private AudioStreamSample loopMusic;

    private AudioStreamPlayer audioPlayer;

    private bool finishedStartMusic;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
    }

    public override void _Process(float delta)
    {
        if (!finishedStartMusic)
        {
            if (audioPlayer.GetPlaybackPosition() >= startMusic.GetLength())
            {
                finishedStartMusic = true;
                audioPlayer.Stream = loopMusic;
                Reset();
                GD.Print("Switch to loop");
            }
        }
        else
        {
            if (audioPlayer.GetPlaybackPosition() >= loopMusic.GetLength())
            {
                Reset();
                GD.Print("Reset Loop");
            }
        }
    }

    public void Start()
    {
        finishedStartMusic = false;
        audioPlayer.Stream = startMusic;
        audioPlayer.Play();
        GD.Print("Start Playing");
    }

    public void Stop()
    {
        audioPlayer.Stop();
    }

    private void Reset()
    {
        Stop();
        audioPlayer.Play();
    }
}
