using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameStatus")]
public class GameStatus : ScriptableObject
{
    public bool bounded;
    public float minX, maxX, minY, maxY;

    public float rightXOffset = 10;

    public float uiBasicBounds = 5;

    public void UpdateBounds(float xOffset, float yOffset)
    {
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + xOffset + uiBasicBounds;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - xOffset - uiBasicBounds - rightXOffset;
        minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + yOffset + uiBasicBounds;
        maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - yOffset - uiBasicBounds;
    }
}
