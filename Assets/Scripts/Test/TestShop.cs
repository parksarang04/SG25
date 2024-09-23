using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestShop : MonoBehaviour
{
    public GameObject ProductListPanel;
    public GameObject ProductListContent;
    public ProductData[] products;
    public Button productButton;
    private ProductBox productBox;
    public GameObject productBoxObj;

    private void Start()
    {
        products = Resources.LoadAll<ProductData>("");
        GenerateproductButton();
    }

    void GenerateproductButton()
    {
        for (int i = 0; i < products.Length; i++)
        {
            int index = i;
            Button productBtn = Instantiate(productButton, ProductListContent.transform);
            TextMeshProUGUI productName = productBtn.GetComponentInChildren<TextMeshProUGUI>();
            Image productImage = productBtn.GetComponent<Image>();

            if (productBtn != null && products[index] != null)
            {
                productName.text = products[index].name;
                productImage = products[index].image;
                productBtn.onClick.AddListener(() => OnProductButtonClick(products[index]));
            }
        }
    }

    public void OnProductButtonClick(ProductData product)
    {
        GameObject BoxObj = Instantiate(productBoxObj);
        ProductBox productBox = BoxObj.GetComponent<ProductBox>();
        productBox.GenerationProduct(product);
    }
}
