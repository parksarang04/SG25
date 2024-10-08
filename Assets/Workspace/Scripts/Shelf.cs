using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Shelf : MonoBehaviour
{
    public abstract void PopItem();
    public abstract void PushItem();
    public abstract int GetSize();
}
