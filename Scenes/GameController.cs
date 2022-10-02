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
    private MathScreen mathScreen;
    private int currTreeIndex;
    private float treeXPos = -250;
    private int treeChopSecondCount = 0;
    private bool isTreeFalling;
    private int branchPassed;
    private const int InitialTrees = 3;

    private enum Operation
    {
        Add,
        Subtract,
        Multiply
    };
    
    private RandomNumberGenerator randGenerator = new RandomNumberGenerator();

    private List<Tuple<int, int>> easyAddPairs;
    private List<Tuple<int, int>> easySubtractPairs;
    private List<Tuple<int, int>> multiplyPairs;
    private List<List<Tuple<int, int>>> numberPairLists;
    private Tuple<int, int> selectedPair;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        camera2d = GetNode<Node2D>("Camera2D");
        mathScreen = camera2d.GetNode<CanvasLayer>("CanvasLayer").GetNode<MathScreen>("MathScreen");
        treeContainer = GetNode<Node2D>("TreeContainer");
        scoreText = camera2d.GetNode<RichTextLabel>("ScoreText");


        GeneratePairs();
        PickPair();
        SpawnTrees();
        SpawnPlayer();
    }

    private void GeneratePairs()
    {
        easyAddPairs = new List<Tuple<int, int>>();
        for (int i = 1; i <= 8; ++i)
        {
            for (int j = 9 - i; j >= 1; --j)
            {
                easyAddPairs.Add(new Tuple<int, int>(i, j));
            }
        }

        easySubtractPairs = new List<Tuple<int, int>>();
        for (int i = 9; i >= 1; --i)
        {
            for (int j = 1; j <= i; ++j)
            {
                easySubtractPairs.Add(new Tuple<int, int>(i, j));
            }
        }

        multiplyPairs = new List<Tuple<int, int>>();
        for (int i = 1; i <= 4; ++i)
        {
            for (int j = 1; j <= 4; ++j)
            {
                multiplyPairs.Add(new Tuple<int, int>(i, j));
            }
        }

        numberPairLists = new List<List<Tuple<int, int>>>{ easyAddPairs, easySubtractPairs, multiplyPairs };
    }
    
    private void PickPair()
    {
        randGenerator.Randomize();

        var maxRange = Operation.Subtract;
        if (currentBranchCount > 10)
        {
            maxRange = Operation.Multiply;
        }

        int operation = randGenerator.RandiRange((int)Operation.Add, (int)maxRange);
        var pairList = numberPairLists[operation];
        var pairIndex = randGenerator.RandiRange(0, pairList.Count - 1);
        selectedPair = pairList[pairIndex];

        pairList.RemoveAt(pairIndex);
        
        UpdateMathScreen((Operation)operation);
    }

    private void UpdateMathScreen(Operation operation)
    {
        int result = 0;
        switch (operation)
        {
            case Operation.Add:
                result = selectedPair.Item1 + selectedPair.Item2;
                break;

            case Operation.Subtract:
                result = selectedPair.Item1 - selectedPair.Item2;
                break;

            case Operation.Multiply:
                result = selectedPair.Item1 * selectedPair.Item2;
                break;
        }

        mathScreen.UpdateEquation(selectedPair.Item1, selectedPair.Item2, result);
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

        choppingPlayer = player.GetNode<AudioStreamPlayer2D>("ChoppingPlayer");
        fallDownPlayer = player.GetNode<AudioStreamPlayer2D>("FallDownPlayer");
    }

    public override void _Process(float delta)
    {
        // if (Input.IsActionJustPressed("proceed"))
        // {
        //     GoToNextBranch();
        // }

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
        mathScreen.Visible = false;
        camera2d.GetNode<CanvasLayer>("GameOverLayer").GetNode<Control>("GameOverScreen").Visible = true;
    }

    public void PlayAgain()
    {
        GetTree().ReloadCurrentScene();
    }

    public void SelectAdd()
    {
        CheckResult(selectedPair.Item1 + selectedPair.Item2);
    }

    public void SelectSubtract()
    {
        CheckResult(selectedPair.Item1 - selectedPair.Item2);
    }

    public void SelectMultiply()
    {
        CheckResult(selectedPair.Item1 * selectedPair.Item2);
    }

    private void CheckResult(int result)
    {
        if (result != mathScreen.GetResult())
        {
            PickPair();
            return;
        }

        GoToNextBranch();
    }

    private void GoToNextBranch()
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
        PickPair();
    }
}
