using Godot;
using System;
using System.Collections.Generic;

public class GameController : Node2D
{
    [Export]
    private PackedScene treePrefab;

    private List<Tree> trees;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        trees = new List<Tree>();

        const int initialTrees = 3;
        int xPos = -250;
        for (int i = 0; i < initialTrees; ++i)
        {
            Tree tree = (Tree)treePrefab.Instance();
            tree.Position = new Vector2(xPos, 100);

            AddChild(tree);
            trees.Add(tree);

            xPos += 250;
        }
    }
}
