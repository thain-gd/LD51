using Godot;
using System;

public class Tree : Node2D
{
    [Export]
    private PackedScene treeBranchPrefab;

    [Export]
    private float minYOffset = 25;

    [Export]
    private float maxYOffset = 40;

    [Export]
    private float minRotation;

    [Export]
    private float maxRotation;

    private static int branchNumbers = 5;

    private float TreeHeight;

    public override void _Ready()
    {
        GetTreeHeight();
        SpawnBranches();
    }

    private void GetTreeHeight()
    {
        var treeSprite = GetNode<Sprite>("Sprite");
        TreeHeight = treeSprite.Texture.GetHeight() * treeSprite.Scale.y;
    }

    private void SpawnBranches()
    {
        var random = new RandomNumberGenerator();
        random.Randomize();

        bool isLeftBranch = System.Math.Round(random.Randf()) == 0;
        float ySpace = TreeHeight * 0.75f / branchNumbers;

        float yPos = -25;
        for (int i = 0; i < branchNumbers; ++i)
        {
            float branchRotation = random.RandfRange(minRotation, maxRotation);
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
            float yOffset = random.RandfRange(minYOffset, maxYOffset);
            yPos -= (ySpace + yOffset);
        }

        ++branchNumbers;
    }
}
