using Godot;
using System.Collections.Generic;

public class TreeBranch : Node2D
{
    [Export]
    private List<StreamTexture> treeBranchSprites;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void Initialize(bool isLeftBranch, float branchPercentage, Vector2 pos, float rotation)
    {
        StreamTexture selectedSprite = null;
        if (branchPercentage <= 0.4f)
        {
            selectedSprite = treeBranchSprites[0];
        }
        else if (branchPercentage <= 0.8f)
        {
            selectedSprite = treeBranchSprites[1];
        }
        else
        {
            selectedSprite = treeBranchSprites[2];
        }

        float scaleX = 1;
        if (!isLeftBranch)
        {
            rotation = -rotation;
            scaleX = -scaleX;
            pos.x = -pos.x;
        }

        Sprite sprite = GetNode<Sprite>("Sprite");
        sprite.Texture = selectedSprite;
        float branchWidth = sprite.Texture.GetSize().x;
        sprite.Offset = new Vector2(-branchWidth * 0.5f, 0);

        Position = pos;
        Scale = new Vector2(scaleX, 1);
        RotationDegrees = rotation;
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
