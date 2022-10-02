using Godot;
using System;
using System.Collections.Generic;

public class GameController : Node2D
{
    [Export]
    private PackedScene treePrefab;

    [Export]
    private PackedScene playerPrefab;

    [Export]
    private float treeXSpace = 250;

    [Export]
    private int currentBranchCount = 3;

    [Export]
    private AudioStream chopSfx;

    [Export]
    private AudioStream treeFallSfx;

    private Node2D camera2d;
    private Node2D treeContainer;
    private AudioStreamPlayer2D audioPlayer2d;
    private List<Tree> trees;
    private Player player;
    private int currTreeIndex;
    private float treeXPos = -250;
    private int treeChopSecondCount = 0;
    private const int MaxTrees = 3;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        camera2d = GetNode<Node2D>("Camera2D");
        treeContainer = GetNode<Node2D>("TreeContainer");
        audioPlayer2d = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");

        SpawnTrees();
        SpawnPlayer();
    }

    private void SpawnTrees()
    {
        currTreeIndex = 0;
        trees = new List<Tree>();

        for (int i = 0; i < MaxTrees; ++i)
        {
            SpawnNewTree();
        }
    }

    private void SpawnPlayer()
    {
        player = (Player)playerPrefab.Instance();
        player.Position = trees[currTreeIndex].GetBranchPosition();

        AddChild(player);
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("proceed"))
        {
            Tree currentTree = trees[currTreeIndex];
            player.Position = currentTree.GetBranchPosition();

            if (currentTree.IsLastBranch())
            {
                currTreeIndex = (currTreeIndex + 1) % MaxTrees;
            }
            else
            {
                currentTree.UpdateBranchIndex();
            }
            
            if (currTreeIndex == 2)
            {
                UpdateTrees();
            }
        }

        camera2d.Position = camera2d.Position.LinearInterpolate(player.Position, 0.12f);
    }

    private void UpdateTrees()
    {
        RemoveFirstTree();
        SpawnNewTree();
        UpdateCurrentTreeIndex();
    }

    private void RemoveFirstTree()
    {
        Tree firstTree = trees[0];
        treeContainer.RemoveChild(firstTree);
        trees.Remove(firstTree);

        firstTree.QueueFree();
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

    private void UpdateCurrentTreeIndex()
    {
        --currTreeIndex;
    }

    public void OnTreeCutdownTimerTimeout()
    {
        audioPlayer2d.Stream = chopSfx;
        audioPlayer2d.Play();

        ++treeChopSecondCount;
        GD.Print(treeChopSecondCount);
        // Falling tree
        if (treeChopSecondCount == 10)
        {
            treeChopSecondCount = 0;

            audioPlayer2d.Stream = treeFallSfx;
            audioPlayer2d.Play();
        }
    }
}
