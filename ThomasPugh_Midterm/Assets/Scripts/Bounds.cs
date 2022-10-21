using UnityEngine;
using System.Collections;

public static class Bounds
{
    public static Vector4 GetBoundariesOfGame(float extension)
    {
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        return new Vector4(min.x-extension,max.x+extension,min.y-extension,max.y+extension);
    }

    public static bool IsOutsideBounds(Vector3 pos, float extension)
    {
        Vector4 boundary = GetBoundariesOfGame(extension);
        return (pos.x < boundary.x || pos.x > boundary.y || pos.y < boundary.z || pos.y > boundary.w);
    }
}

