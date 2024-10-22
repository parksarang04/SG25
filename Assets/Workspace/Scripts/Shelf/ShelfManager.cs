using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour
{
    public Shelf snackShelf;

    void Start()
    {
        snackShelf = new SnackShelf();
    }

    public void ExecuteShelfPop(int productType, GameObject product)
    {
        if (productType == 0)
        {

        }
        if (productType == 1)
        {
            snackShelf.PopItem(product, productType);
        }
    }
    public void ExecuteShelfPush(int productType, GameObject product)
    {
        if (productType == 0)
        {

        }
        if (productType == 1)
        {
            snackShelf.PushItem(product, productType);
        }
    }
}
