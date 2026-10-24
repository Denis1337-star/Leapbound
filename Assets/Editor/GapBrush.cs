using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

[CustomGridBrush(false, false, false, "Gap Brush")]
public class GapBrush : GridBrush
{
    public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        if (!IsAllowed(position))
            return;

        base.Paint(grid, brushTarget, position);
    }

    public override void BoxFill(GridLayout grid, GameObject brushTarget, BoundsInt position)
    {
        foreach (Vector3Int cell in position.allPositionsWithin)
        {
            if (!IsAllowed(cell))
                continue;

            base.BoxFill(grid, brushTarget, new BoundsInt(cell, Vector3Int.one));
        }
    }

    private static bool IsAllowed(Vector3Int position)
    {
        return (position.x + position.y) % 2 == 0;
    }
}

[CustomEditor(typeof(GapBrush))]
public class GapBrushEditor : GridBrushEditor
{
}