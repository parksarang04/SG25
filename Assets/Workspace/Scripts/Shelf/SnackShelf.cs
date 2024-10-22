using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnackShelf : Shelf
{
    public List<Transform> SnackPosList = new List<Transform>();
    public List<GameObject> SnackList = new List<GameObject>();

    public override int GetSize()
    {
        return SnackList.Count;
    }

    public override void PopItem(GameObject product, Product productType)
    {
        
    }

    public override void PushItem(GameObject product, Product productType)
    {
        Transform nullPos = null;
        foreach (Transform pos in SnackPosList)
        {
            if (pos.childCount == 0)    //productPosList�� ��� �ִ� Transforom �� ��ǰ�� �ȵ� Transform�� �ִٸ� break
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
            }
        }
    }
}
