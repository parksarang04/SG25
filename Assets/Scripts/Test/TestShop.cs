using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestShop : MonoBehaviour
{
    public GameObject ProductListPanel;
    public GameObject ProductListContent;
    public ProductData[] products;
    public Button[] productButtons;
    private ProductBox productBox;
    public GameObject productBoxObj;

    private void Start()
    {
        productButtons = ProductListContent.GetComponentsInChildren<Button>();
        AssignProductsToButtons();
    }

    void AssignProductsToButtons()
    {
        for (int i = 0; i < productButtons.Length; i++)
        {
            int index = i;
            productButtons[index].onClick.AddListener(() => OnProductButtonClick(products[index]));
        }
    }

    public void OnProductButtonClick(ProductData product)
    {
        Debug.Log(product.name);

        GameObject BoxObj = Instantiate(productBoxObj);
        ProductBox productBox = BoxObj.GetComponent<ProductBox>();
        productBox.GenerationProduct(product);
    }
}
