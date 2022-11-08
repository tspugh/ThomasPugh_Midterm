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

    public float bottomYOffset;

    public void UpdateBounds(float xOffset, float yOffset)
    {
        minX = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x;// + xOffset + uiBasicBounds;
        maxX = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x;// - xOffset - uiBasicBounds - rightXOffset;
        minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;// + yOffset + uiBasicBounds + bottomYOffset;
        maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;// - yOffset - uiBasicBounds;
        float aspect = (maxX - minX) / (maxY - minY);
        float desired = 2048f / 1152f;
        if(aspect<desired)
        {
            float screensz = 1f / desired * (maxX - minX);
            Debug.Log(screensz);
            float whole = 1f / aspect * (maxX - minX);
            maxY -= (whole - screensz) / 2f + 1f/17f * screensz;
            minY += (whole - screensz) / 2f + 1f/14f*screensz;

            screensz = desired * (maxY - minY);
            maxX -= screensz * 9f / 40f;
            minX += screensz * 3f / 80f;
        }
    }
}
