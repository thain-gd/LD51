using Godot;
using System;
using System.Collections.Generic;

public class GameController : Node2D
{
    [Export]
    private PackedScene treePrefab;

    [Export]
    private PackedScene playerPrefab;

    private RichTextLabel scoreText;

    [Export]
    private float treeXSpace = 250;

    [Export]
    private int currentBranchCount = 3;

    [Export]
    private int timeUntilTreeFalling = 10;

    private Node2D camera2d;
    private Node2D treeContainer;
    private AudioStreamPlayer2D choppingPlayer;
    private AudioStreamPlayer2D fallDownPlayer;
    private List<Tree> trees;
    private Player player;
    private int currTreeIndex;
    private float treeXPos = -250;
    private int treeChopSecondCount = 0;
    private bool isTreeFalling;
    private int branchPassed;
    private const int InitialTrees = 3;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        camera2d = GetNode<Node2D>("Camera2D");
        treeContainer = GetNode<Node2D>("TreeContainer");
        choppingPlayer = GetNode<AudioStreamPlayer2D>("ChoppingPlayer");
        fallDownPlayer = GetNode<AudioStreamPlayer2D>("FallDownPlayer");
        scoreText = camera2d.GetNode<RichTextLabel>("ScoreText");

        SpawnTrees();
        SpawnPlayer();
    }

    private void SpawnTrees()
    {
        currTreeIndex = 0;
        trees = new List<Tree>();

        for (int i = 0; i < InitialTrees; ++i)
        {
            SpawnNewTree();
        }
    }

    private void SpawnPlayer()
    {
        player = (Player)playerPrefab.Instance();
        player.Position = trees[currTreeIndex].GetBranchPosition();
        trees[currTreeIndex].UpdateBranchIndex();

        AddChild(player);
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("proceed"))
        {
            Tree currentTree = trees[currTreeIndex];

            player.JumpTo(currentTree.GetBranchPosition());

            if (currTreeIndex == trees.Count - 1)
            {
                SpawnNewTree();
            }

            if (currentTree.IsLastBranch())
            {
                currTreeIndex = (currTreeIndex + 1) % trees.Count;
            }
            else
            {
                currentTree.UpdateBranchIndex();
            }

            UpdateScore();
        }

        camera2d.Position = camera2d.Position.LinearInterpolate(player.Position, 0.12f);
    
        if (isTreeFalling)
        {
            trees[0].Rotation = Mathf.LerpAngle(trees[0].Rotation, Mathf.Deg2Rad(-90), 0.03f);
            if (Mathf.Abs(trees[0].RotationDegrees + 90) <= 0.05f)
            {
                isTreeFalling = false;
                RemoveFirstTree();
            }
        }
    }

    private void SpawnNewTree()
    {
        Tree tree = (Tree)treePrefab.Instance();
        tree.Initialize(currentBranchCount);
        tree.Position = new Vector2(treeXPos, 300);

        treeContainer.AddChild(tree);
        trees.Add(tree);

        treeXPos += treeXSpace;
        ++currentBranchCount;
    }

    private void RemoveFirstTree()
    {
        Tree firstTree = trees[0];
        treeContainer.RemoveChild(firstTree);
        trees.Remove(firstTree);

        --currTreeIndex;

        firstTree.QueueFree();
    }

    public void OnTreeCutdownTimerTimeout()
    {
        choppingPlayer.Play();

        ++treeChopSecondCount;
        GD.Print(treeChopSecondCount);
        // Falling tree
        if (treeChopSecondCount == timeUntilTreeFalling)
        {
            fallDownPlayer.Play();
            treeChopSecondCount = 0;
            isTreeFalling = true;

            if (currTreeIndex == 0)
            {
                GetNode<Timer>("TreeCutdownTimer").Stop();
                ShowGameOverScreen();
            }
        }
    }

    public void UpdateScore()
    {
        ++branchPassed;
        scoreText.Text = branchPassed.ToString();
    }

    private void ShowGameOverScreen()
    {
        camera2d.GetNode<Control>("GameOverScreen").Visible = true;
    }

    public void PlayAgain()
    {
        GetTree().ReloadCurrentScene();
    }
}
