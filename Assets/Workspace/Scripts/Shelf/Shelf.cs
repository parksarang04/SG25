using System.Collections.Generic;
using Unity.VisualScripting;
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

    public bool PushItem(GameObject product, int productType, int productID)
    {
        Debug.Log("GetShelfType" + GetShelfType());
        Debug.Log("productType" + productType);
        if (GetShelfType() == productType)
        {
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
                if (ProductList.Count == 0)
                {
                    PlaceProduct(product, nullPos);
                    return true;
                }
                else if (ProductList[0].GetComponent<Product>().product.ID == productID)
                {
                    PlaceProduct(product, nullPos);
                    return true;
                }
                else
                {
                    Debug.Log("진열대에 있는 상품과 ID가 맞지 않아 추가할 수 없습니다.");
                    return false;
                }
            }
            else
            {
                Debug.Log("진열대에 자리 없음");
                return false;
            }
        }
        return false;
    }

    private void PlaceProduct(GameObject product, Transform position)
    {
        product.transform.SetParent(position);
        product.transform.localPosition = Vector3.zero;
        product.transform.localScale = Vector3.one;
        product.transform.localRotation = Quaternion.identity;
        ProductList.Add(product);
    }
}
