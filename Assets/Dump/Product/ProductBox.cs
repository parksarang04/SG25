using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProductBox : MonoBehaviour
{
    public List<GameObject> productObjectList = new List<GameObject>();     //박스 안에 들어 있는 오브젝트를 담은 List
    public List<Transform> productPosition = new List<Transform>();         //오브젝트가 배치될 위치

    public void GenerationProduct(ProductData product)      //박스가 생성될 때 해당 product와 맞게 product의 오브젝트를 생성하는 함수
    {
        for (int i = 0; i < productPosition.Count; i++) 
        {
            GameObject obj = Instantiate(product.ProductModel);
            obj.transform.SetParent(productPosition[i]);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

            productObjectList.Add(obj);
        }
    }

    public GameObject RemoveProduct(GameObject productObj)  //박스 안에 있는 상품들을 지우는 함수
    {
        if (productObjectList.Count > 0)            //박스 안에 상품이 하나라도 있다면
        {
            productObjectList.Remove(productObj);   //productObjectList에서 해당 상품을 지운다.
            Debug.Log("하나 뺐어여");
        }
        else
        {
            Debug.Log("박스가 비었ㅎ어여");
        }
        return null;
    }

    public GameObject InsertProduct(GameObject productObj)      //상품을 박스에 넣는 함수
    {
        Product newProduct = productObj.GetComponent<Product>();
        Product boxProduct = productObjectList[productObjectList.Count - 1].GetComponent<Product>();    //들어올 상품과 박스 안에 들어있는 상품의 Product 컴포넌트를 갖고 온 후

        if (newProduct.product.Index == boxProduct.product.Index)   //두 상품의 Index가 같고
        {
            if (productObjectList.Count < productPosition.Count)    //박스 내에 있는 상품 수가 박스 칸 수보다 적다면
            {
                productObjectList.Add(productObj);                  //해당 상품을 productObjectList에 넣는다.
                productObj.transform.SetParent(productPosition[productObjectList.Count - 1]);
                productObj.transform.localPosition = Vector3.zero;
                productObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            }
        } 
        return null;
    }
}
