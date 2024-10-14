using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductBoxGenerator : MonoBehaviour
{
    private List<ProductBoxScriptObject> productBoxInfoList = new List<ProductBoxScriptObject>();
    public GameObject BoxPrefab;
    public Transform Pivot;

    public void GetOrder(ProductBoxScriptObject info)
    {
        productBoxInfoList.Add(info);
    }

    public void GetOrder(List<ProductBoxScriptObject> infoList)
    {
        foreach (var info in infoList)
        {
            productBoxInfoList.Add(info);
        }
    }

    public void GenerateProductBox()
    {
        foreach (var item in productBoxInfoList)
        {
            var output = Instantiate(BoxPrefab, Pivot.position, Quaternion.identity) as GameObject;
            var info = output.GetComponent<ProductBoxInfo>();
            
            info.ProductName = item.ProductName;
            info.ProductType = item.ProductType;
            info.ProductCount = item.ProductCount;
        }
        productBoxInfoList.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log("ProductBoxInfo");

            var product = new ProductBoxScriptObject();

            product.ProductName = "ÄÝ¶ó";
            product.ProductCount = 9;
            product.ProductType = ProductBoxType.BEVERAGES;

            GetOrder(product);
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("GenerateProductBox");
            GenerateProductBox();
        }

        
    }
}
