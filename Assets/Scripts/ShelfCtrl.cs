using System.Collections.Generic;
using UnityEngine;

public class ShelfCtrl : MonoBehaviour
{
    public List<Transform> productPosList = new List<Transform>();      //상품 배치할 위치들 목록
    public Stack<GameObject> productList = new Stack<GameObject>();     //진열대 안에 들어 있는 상품 목록
    private ProductBox productBox;

    public bool DisplayProduct(GameObject productobj)
    {
        Transform nullPos = null;
        foreach (Transform pos in productPosList)
        {
            if (pos.childCount == 0)    //productPosList에 들어 있는 Transforom 중 상품이 안들어간 Transform이 있다면 break
            {
                nullPos = pos;          
                break;
            }
        }
        if (nullPos != null)                                //비어 있는 Transform이 있으며
        {
            if (productList.Count < productPosList.Count)   //진열대 내에 있는 상품 수가 진열대 칸 수보다 적고
            {
                if (productList.Count == 0)                 //지금 넣을 오브젝트가 productList의 첫 오브젝트라면
                {
                    Transform availablePosition = productPosList[productList.Count];    
                    productobj.transform.SetParent(availablePosition);                  
                    productobj.transform.localPosition = Vector3.zero;
                    productobj.transform.localScale = Vector3.one;

                    productList.Push(productobj);           //productList에 해당 오브젝트를 Push 한다.

                    Debug.Log("진열대에 상품 넣음");
                    return true;

                }
                else if (productList.Count != 0)            //지금 넣을 오브젝트가 productList의 첫 오브젝트가 아니면
                {
                    Product shelfProduct = productList.Peek().GetComponent<Product>();
                    Product newProduct = productobj.GetComponent<Product>();            //productList의 가장 위에 있는 오브젝트와 넣을 오브젝트의 Product 컴포넌트를 갖고 온 후
                    if (shelfProduct.product.Index == newProduct.product.Index)         //둘의 product Index를 비교하여 Index가 같을 경우
                    {
                        Transform availablePosition = productPosList[productList.Count];
                        productobj.transform.SetParent(availablePosition);
                        productobj.transform.localPosition = Vector3.zero;
                        productobj.transform.localScale = Vector3.one;

                        productList.Push(productobj);                                   //productList에 해당 오브젝트를 Push 한다.

                        Debug.Log("진열대에 상품 넣음");
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void MoveProductToBox(GameObject productObj)
    {
        if (productList.Count != 0)     //진열대 내에 들어있는 상품이 있다면
        {
            productList.Pop();          //productList의 가장 위에 있는 상품을 지운다.
        }
    }

    public void PickUpProduct()
    {

    }
}
