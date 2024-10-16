using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductBox : MonoBehaviour
{
    public List<GameObject> productObjectList = new List<GameObject>(); // �ڽ� �ȿ� ��� �ִ� ������Ʈ�� ���� List
    public List<Transform> productPosition = new List<Transform>();     // ������Ʈ�� ��ġ�� ��ġ

    public void GenerationProduct(ProductData product) // �ڽ��� ������ �� �ش� product�� �°� product�� ������Ʈ�� �����ϴ� �Լ�
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

    public GameObject RemoveProduct(GameObject productObj) // �ڽ� �ȿ� �ִ� ��ǰ���� ����� �Լ�
    {
        if (productObjectList.Count > 0)
        {
            productObjectList.Remove(productObj);
            Debug.Log("�ϳ� ���");
        }
        else
        {
            Debug.Log("�ڽ��� ����");
        }
        return null;
    }

    public GameObject InsertProduct(GameObject productObj) // ��ǰ�� �ڽ��� �ִ� �Լ�
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

    // ��ǰ ������ out �Ķ���ͷ� ����
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
