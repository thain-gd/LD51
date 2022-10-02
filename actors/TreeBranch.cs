using Godot;
using System.Collections.Generic;

public class TreeBranch : Node2D
{
    [Export]
    private List<StreamTexture> treeBranchSprites;

    public bool IsLeftBranch {get; private set;}

    private Sprite branchSprite;

    public void Initialize(bool isLeftBranch, float branchPercentage, Vector2 pos, float rotation)
    {
        IsLeftBranch = isLeftBranch;

        StreamTexture selectedTexture = null;
        if (branchPercentage <= 0.4f)
        {
            selectedTexture = treeBranchSprites[0];
        }
        else if (branchPercentage <= 0.8f)
        {
            selectedTexture = treeBranchSprites[1];
        }
        else
        {
            selectedTexture = treeBranchSprites[2];
        }

        float scaleX = 1;
        if (!isLeftBranch)
        {
            rotation = -rotation;
            scaleX = -scaleX;
            pos.x = -pos.x;
        }

        branchSprite = GetNode<Sprite>("Sprite");
        branchSprite.Texture = selectedTexture;
        float branchWidth = branchSprite.Texture.GetSize().x;
        branchSprite.Offset = new Vector2(-branchWidth * 0.5f, 0);

        Position = pos;
        Scale = new Vector2(scaleX, 1);
        RotationDegrees = rotation;
    }

    public float GetBranchWidth()
    {
        return branchSprite.Texture.GetSize().x * branchSprite.Scale.x;
    }
}
