using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductBox : MonoBehaviour
{
    public List<GameObject> productObjectList = new List<GameObject>(); // 박스 안에 들어 있는 오브젝트를 담은 List
    public List<Transform> productPosition = new List<Transform>();     // 오브젝트가 배치될 위치

    public void GenerationProduct(ProductData product) // 박스가 생성될 때 해당 product와 맞게 product의 오브젝트를 생성하는 함수
    {
        for (int i = 0; i < productPosition.Count; i++)
        {
            GameObject obj = Instantiate(product.ProductModel);
            obj.transform.SetParent(productPosition[i]);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(5, 5, 5);
            obj.GetComponent<BoxCollider>().enabled = false;

            productObjectList.Add(obj);
        }
    }

    public GameObject RemoveProduct(GameObject productObj) // 박스 안에 있는 상품들을 지우는 함수
    {
        if (productObjectList.Count > 0)
        {
            productObjectList.Remove(productObj);
            Debug.Log("하나 뺐어여");
        }
        else
        {
            Debug.Log("박스가 비었어여");
        }
        return null;
    }

    public GameObject InsertProduct(GameObject productObj) // 상품을 박스에 넣는 함수
    {
        Product newProduct = productObj.GetComponent<Product>();
        Product boxProduct = productObjectList[productObjectList.Count - 1].GetComponent<Product>();

        if (newProduct.product.Index == boxProduct.product.Index)
        {
            if (productObjectList.Count < productPosition.Count)
            {
                productObjectList.Add(productObj);
                productObj.transform.SetParent(productPosition[productObjectList.Count - 1]);
                productObj.transform.localPosition = Vector3.zero;
                productObj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
            }
        }
        return null;
    }

    // 상품 정보를 out 파라미터로 전달
    public void GetProductInfo(out string productName, out int productCount, out Sprite productSprite)
    {
        if (productObjectList.Count > 0)
        {
            Product product = productObjectList[0].GetComponent<Product>();

            if (product != null)
            {
                productName = product.product.name;
                productCount = productObjectList.Count;
                productSprite = product.product.image;
            }
            else
            {
                productName = "";
                productCount = 0;
                productSprite = null;
            }
        }
        else
        {
            productName = "";
            productCount = 0;
            productSprite = null;
        }
    }
}
