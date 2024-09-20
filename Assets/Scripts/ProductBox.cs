using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProductBox : MonoBehaviour
{
    public List<GameObject> productObjectList = new List<GameObject>();
    public List<Transform> productPosition = new List<Transform>();

    public void GenerationProduct(ProductData product)
    {
        for (int i = 0; i < productPosition.Count; i++) 
        {
            GameObject obj = Instantiate(product.ProductModel);
            obj.transform.SetParent(productPosition[i]);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

            productObjectList.Add(obj);
            Debug.Log(productObjectList.Count);
        }
    }

    public GameObject RemoveProduct(GameObject productObj)
    {
        if (productObjectList.Count > 0)
        {
            productObjectList.Remove(productObj);
            Debug.Log("하나 뺐어여");
        }
        else
        {
            Debug.Log("박스가 비었ㅎ어여");
        }
        return null;
    }

    public GameObject InsertProduct(GameObject productObj)
    {
        Product newProduct = productObj.GetComponent<Product>();
        Product boxProduct = productObjectList[productObjectList.Count - 1].GetComponent<Product>();

        if (newProduct.productData.Index == boxProduct.productData.Index)
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
}
