using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkShelf : Shelf
{
    public int maxCount = 10;
    public int currentCount = 0;

    public override int GetSize()
    {
        return currentCount;
    }

    public override void PopItem()
    {
        if (currentCount > 0)
        {
            currentCount--;
            Debug.Log($"PopItem {currentCount}");
        }
    }

    public override void PushItem()
    {
        if (currentCount < maxCount)
        {
            currentCount++;
            Debug.Log($"PushItem {currentCount}");
        }
    }
}
