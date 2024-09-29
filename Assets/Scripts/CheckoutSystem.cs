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
        counterProduct.Remove(productObj);
        Product product = productObj.GetComponent<Product>();
        totalPrice += product.product.sellCost;
        Debug.Log(totalPrice);
    }

    public void CalculateChange()
    {

    }
}