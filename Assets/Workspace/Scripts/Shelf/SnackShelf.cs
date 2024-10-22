using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackShelf : Shelf
{
    public List<Transform> SnackPosList = new List<Transform>();
    public List<GameObject> SnackList = new List<GameObject>();

    public int SnackShelfType = 1;
    public bool isSnack = false;

    public void GetSnackShelfType()
    {
        ShelfType = SnackShelfType;
    }   

    public override int GetSize()
    {
        return SnackList.Count;
    }

    public override void PopItem(GameObject product, int productType)
    {
        if (base.ShelfType == productType)
        {
            if (SnackList.Count > 0)
            {
                SnackList.Remove(product);
            }
        }
    }

    public override void PushItem(GameObject product, int productType)
    {
        if (base.ShelfType == productType)
        {
            isSnack = true;
            Transform nullPos = null;
            foreach (Transform pos in SnackPosList)
            {
                if (pos.childCount == 0)    //productPosList에 들어 있는 Transforom 중 상품이 안들어간 Transform이 있다면 break
                {
                    nullPos = pos;
                    break;
                }
            }
            if (nullPos != null)
            {
                if (SnackList.Count < SnackPosList.Count)
                {
                    Transform availablePosition = SnackPosList[SnackList.Count];
                    product.transform.SetParent(availablePosition);
                    product.transform.localPosition = Vector3.zero;
                    product.transform.localScale = Vector3.one;
                    product.transform.localRotation = Quaternion.identity;
                    SnackList.Add(product);
                    isSnack = false;
                }
                else
                {
                    Debug.Log("과자 진열대 꽉참");
                }
            }
        }
    }
}
