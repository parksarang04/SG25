using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProductBox : MonoBehaviour
{
    public Queue<GameObject> productObjectQueue = new Queue<GameObject>();
    public List<Transform> productPosition = new List<Transform>();

    public void InsertProduct(Product product)
    {
        for (int i = 0; i < productPosition.Count; i++) 
        {
            GameObject obj = Instantiate(product.ProductModel);
            obj.transform.SetParent(productPosition[i]);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);

            productObjectQueue.Enqueue(obj);
            Debug.Log(productObjectQueue.Count);
        }
    }

    public GameObject RemoveProduct(GameObject productObj)
    {
        if (productObjectQueue.Count > 0)
        {
            productObj = productObjectQueue.Dequeue();
            Debug.Log("�ϳ� ���");
        }
        else
        {
            Debug.Log("�ڽ��� ������");
        }
        return null;
    }
}
