using System.Collections.Generic;
using UnityEngine;

public class ShelfCtrl : MonoBehaviour
{
    public List<Transform> productPosList = new List<Transform>();
    public List<GameObject> productList = new List<GameObject>();
    private ProductBox productBox;

    public bool DisplayProduct(GameObject product)
    {
        if (product == null)
        {
            Debug.Log("��ǰ ����");
            return false;
        }
        Transform nullPos = null;
        foreach (Transform pos in productPosList)
        {
            if (pos.childCount == 0)
            {
                nullPos = pos;
                break;
            }
        }
        if (nullPos != null)
        {
            if (productList.Count < productPosList.Count)
            {

                Transform availablePosition = productPosList[productList.Count];
                product.transform.SetParent(availablePosition);
                product.transform.localPosition = Vector3.zero;
                product.transform.localScale = Vector3.one;

                productList.Add(product);

                Debug.Log("�����뿡 ��ǰ ����");
                return true;
            }
        }

        Debug.Log("������ �� ��");
        return false;


    }

}
