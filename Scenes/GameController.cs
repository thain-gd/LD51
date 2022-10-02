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

    private Node2D camera2d;
    private Node2D treeContainer;
    private List<Tree> trees;
    private Player player;
    private int currTreeIndex;
    private float treeXPos = -250;
    private const int MaxTrees = 4;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        camera2d = GetNode<Node2D>("Camera2D");
        treeContainer = GetNode<Node2D>("TreeContainer");

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
        player.Position = trees[currTreeIndex].Position;

        AddChild(player);
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("proceed"))
        {
            currTreeIndex = (currTreeIndex + 1) % MaxTrees;
            player.Position = trees[currTreeIndex].Position;
            
            if (currTreeIndex == 3)
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
        tree.Position = new Vector2(treeXPos, 100);

        treeContainer.AddChild(tree);
        trees.Add(tree);

        treeXPos += treeXSpace;
        ++currentBranchCount;
    }

    private void UpdateCurrentTreeIndex()
    {
        --currTreeIndex;
    }
}
