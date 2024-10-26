using System.Collections.Generic;
using UnityEngine;

public abstract class Shelf : MonoBehaviour
{
    public List<Transform> ProductPoseList = new List<Transform>();
    public List<GameObject> ProductList = new List<GameObject>();
    public abstract int GetShelfType();
    public int GetSize()
    {
        return ProductList.Count;
    }

    public void PopItem(GameObject product, int productType)
    {
        if (GetShelfType() == productType)
        {
            if (ProductList.Count > 0)
            {
                ProductList.Remove(product);
            }
        }
    }

    public void PushItem(GameObject product, int productType)
    {
        Debug.Log("GetShelfType" + GetShelfType());
        Debug.Log("productType" + productType);
        if (GetShelfType() == productType)
        {
            Debug.Log("PushItem");
            Transform nullPos = null;
            foreach (Transform pos in ProductPoseList)
            {
                if (pos.childCount == 0)    //productPosList에 들어 있는 Transforom 중 상품이 안들어간 Transform이 있다면 break
                {
                    nullPos = pos;
                    Debug.Log("자식없음");
                    break;
                }
            }
            if (nullPos != null)
            {
                if (ProductList.Count < ProductPoseList.Count)
                {
                    Transform availablePosition = ProductPoseList[ProductList.Count];
                    product.transform.SetParent(availablePosition);
                    product.transform.localPosition = Vector3.zero;
                    product.transform.localScale = Vector3.one;
                    product.transform.localRotation = Quaternion.identity;
                    ProductList.Add(product);
                    Debug.Log("과자 진열대 들어감");
                }
                else
                {
                    Debug.Log("과자 진열대 꽉참");
                }
            }
            else
            {
                Debug.Log("진열대에 자리 없음");
            }
        }
    }
}