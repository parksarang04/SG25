using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Shelf : MonoBehaviour
{
    public int ShelfType;
    public abstract void PopItem(GameObject product, int productType);
    public abstract void PushItem(GameObject product, int productType);
    public abstract int GetSize();
}