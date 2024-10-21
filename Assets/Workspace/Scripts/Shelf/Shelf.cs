using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shelf : MonoBehaviour
{
    public abstract void PopItem(GameObject product, Product productType);
    public abstract void PushItem(GameObject product, Product productType);
    public abstract int GetSize();
}