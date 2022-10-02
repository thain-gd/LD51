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

    private float TreeHeight;

    public override void _Ready()
    {
        
    }

    public void Initialize(int branchCount)
    {
        GetTreeHeight();
        SpawnBranches(branchCount);
    }

    private void GetTreeHeight()
    {
        var treeSprite = GetNode<Sprite>("Sprite");
        TreeHeight = treeSprite.Texture.GetHeight() * treeSprite.Scale.y;
    }

    private void SpawnBranches(int branchCount)
    {
        var random = new RandomNumberGenerator();
        random.Randomize();

        bool isLeftBranch = System.Math.Round(random.Randf()) == 0;
        float ySpace = TreeHeight * 0.75f / branchCount;

        float yPos = -25;
        for (int i = 0; i < branchCount; ++i)
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
    }
}
