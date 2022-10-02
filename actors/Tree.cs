using Godot;
using System.Collections.Generic;

public class Tree : Node2D
{
    [Export]
    private PackedScene treeBranchPrefab;

    [Export]
    private List<StreamTexture> trunkSprites;

    [Export]
    private PackedScene trunkPrefab;

    [Export]
    private Vector2 minOffset = new Vector2(10, 25);

    [Export]
    private Vector2 maxOffset = new Vector2(20, 40);

    [Export]
    private float minRotation;

    [Export]
    private float maxRotation;

    private RandomNumberGenerator randomGenerator;
    private List<TreeBranch> branches;
    private int currentBranchIndex = 0;

    private static int TrunkCount = 3;

    public void Initialize(int branchCount)
    {
        randomGenerator = new RandomNumberGenerator();
        randomGenerator.Randomize();

        SpawnTrunks(branchCount);
        SpawnBranches(branchCount);
    }

    private void SpawnTrunks(int branchCount)
    {
        for (int i = 0; i < TrunkCount; ++i)
        {
            StreamTexture sprite = System.Math.Round(randomGenerator.Randf()) == 0 ? trunkSprites[0] : trunkSprites[1];
            float yScale = System.Math.Round(randomGenerator.Randf()) == 0 ? -1 : 1;

            Sprite trunk = (Sprite)trunkPrefab.Instance();
            float trunkHeight = trunk.Texture.GetSize().y;
            trunk.Position = new Vector2(0, -trunkHeight * 0.5f - trunkHeight*i);
            trunk.Scale = new Vector2(1, yScale);

            AddChild(trunk);
        }

        ++TrunkCount;
    }

    private void SpawnBranches(int branchCount)
    {
        branches = new List<TreeBranch>();

        bool isLeftBranch = System.Math.Round(randomGenerator.Randf()) == 0;
        float yPos = -180;
        for (int i = 0; i < branchCount; ++i)
        {
            float branchRotation = randomGenerator.RandfRange(minRotation, maxRotation);
            float xPos =  randomGenerator.RandfRange(-minOffset.x, -maxOffset.x);

            TreeBranch treeBranch = (TreeBranch)treeBranchPrefab.Instance();
            treeBranch.Initialize(isLeftBranch, (i+1)/(float)branchCount, new Vector2(xPos, yPos), branchRotation);
            
            AddChild(treeBranch);
            branches.Add(treeBranch);

            isLeftBranch = !isLeftBranch;
            yPos -= randomGenerator.RandfRange(minOffset.y, maxOffset.y);
        }
    }

    public Vector2 GetBranchPosition()
    {
        TreeBranch branch = branches[currentBranchIndex];
        
        float xOffset = branch.IsLeftBranch ? -branch.GetBranchWidth()*0.37f : branch.GetBranchWidth()*0.37f;
        return branches[currentBranchIndex].GlobalPosition + new Vector2(xOffset, 0);
    }

    public void UpdateBranchIndex()
    {
        ++currentBranchIndex;
    }

    public bool IsLastBranch()
    {
        return currentBranchIndex == branches.Count-1;
    }
}
