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
                if (pos.childCount == 0)    //productPosList�� ��� �ִ� Transforom �� ��ǰ�� �ȵ� Transform�� �ִٸ� break
                {
                    nullPos = pos;
                    Debug.Log("�ڽľ���");
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
                    Debug.Log("�����뿡 �ִ� ��ǰ�� ID�� ���� �ʾ� �߰��� �� �����ϴ�.");
                    return false;
                }
            }
            else
            {
                Debug.Log("�����뿡 �ڸ� ����");
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
