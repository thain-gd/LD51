using Godot;
using System;

public class Tree : Node2D
{
    [Export]
    private PackedScene treeBranchPrefab;

    [Export]
    private float minYOffset;

    [Export]
    private float maxYOffset;

    [Export]
    private float minRotation;

    [Export]
    private float maxRotation;

    private static int branchNumbers = 5;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bool isLeftBranch = System.Math.Round(GD.Randf()) == 0;
        float yPos = -10;
        for (int i = 0; i < branchNumbers; ++i)
        {
            float branchRotation = (float)GD.RandRange(minRotation, maxRotation);
            float xPos = 12;
            if (isLeftBranch)
            {
                branchRotation -= branchRotation * 2;
                xPos = -12;
            }

            Node2D treeBranch = (Node2D)treeBranchPrefab.Instance();
            treeBranch.Position = new Vector2(xPos, yPos);
            treeBranch.RotationDegrees = branchRotation;
            GD.Print(treeBranch.RotationDegrees);
            AddChild(treeBranch);

            isLeftBranch = !isLeftBranch;
            yPos -= 30;
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
