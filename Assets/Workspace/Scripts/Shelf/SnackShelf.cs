using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackShelf : Shelf
{
    public List<Transform> SnackPosList = new List<Transform>();
    public List<GameObject> SnackList = new List<GameObject>();
    public int maxCount = 0;

    public override int GetSize()
    {
        return SnackList.Count;
    }

    public override void PopItem(GameObject product, Product productType)
    {
        
    }

    public override void PushItem(GameObject product, Product productType)
    {
        
    }
}
