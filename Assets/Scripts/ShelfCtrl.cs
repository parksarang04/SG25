using System.Collections.Generic;
using UnityEngine;

public class ShelfCtrl : MonoBehaviour
{
    public List<Transform> productPosList = new List<Transform>();
    public Stack<GameObject> productList = new Stack<GameObject>();
    private ProductBox productBox;

    public bool DisplayProduct(GameObject productobj)
    {
        if (productobj == null)
        {
            Debug.Log("상품 없음");
            return false;
        }
        Transform nullPos = null;
        foreach (Transform pos in productPosList)
        {
            if (pos.childCount == 0)
            {
                nullPos = pos;
                break;
            }
        }
        if (nullPos != null)
        {
            if (productList.Count < productPosList.Count)
            {
                if (productList.Count == 0)
                {
                    Transform availablePosition = productPosList[productList.Count];
                    productobj.transform.SetParent(availablePosition);
                    productobj.transform.localPosition = Vector3.zero;
                    productobj.transform.localScale = Vector3.one;

                    productList.Push(productobj);

                    Debug.Log("진열대에 상품 넣음");
                    return true;

                }
                else if (productList.Count != 0)
                {
                    Product shelfProduct = productList.Peek().GetComponent<Product>();
                    Product newProduct = productobj.GetComponent<Product>();
                    if (shelfProduct.productData.Index == newProduct.productData.Index)
                    {
                        Transform availablePosition = productPosList[productList.Count];
                        productobj.transform.SetParent(availablePosition);
                        productobj.transform.localPosition = Vector3.zero;
                        productobj.transform.localScale = Vector3.one;

                        productList.Push(productobj);

                        Debug.Log("진열대에 상품 넣음");
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void RemoveProduct(GameObject productObj)
    {
        if (productList.Count != 0)
        {
            productList.Pop();
            Debug.Log("아이템 회수");
        }
    }
}
