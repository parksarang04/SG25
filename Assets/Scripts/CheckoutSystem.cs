using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckoutSystem : MonoBehaviour
{
    public List<GameObject> counterProduct = new List<GameObject>();
    public List<GameObject> takeMoneys = new List<GameObject>();

    public int totalPrice = 0;

    public void SelectedProduct(GameObject productObj)
    {
        productObj = counterProduct[counterProduct.Count - 1];
        Product product = productObj.GetComponent<Product>();
        counterProduct.Remove(productObj);
        totalPrice += product.product.sellCost;
        Destroy(productObj);
        Debug.Log(totalPrice);
    }

    public void CalculateChange()
    {

    }
}