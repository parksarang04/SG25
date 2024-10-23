using UnityEngine;
using UnityEngine.UIElements;

public class CenterCameraRaycast : MonoBehaviour
{
    public Camera targetCamera;
    public float raycastRange = 100f; // ����ĳ��Ʈ�� �ִ� �Ÿ�

    public Outline currentOutline;

    public GameObject playerHand;
    public ProductBox productBox;
    public ProductBox lastBox;
    private ShelfCtrl shelf;
    private CheckoutSystem checkoutSystem;
    public CustomerCtrl customer;
    public UIManager uiManager;

    void Start()
    {
       

        //cam = Camera.main;

        checkoutSystem = GetComponent<CheckoutSystem>();
        uiManager = FindObjectOfType<UIManager>();
        customer = FindObjectOfType<CustomerCtrl>();
    }
    void Update()
    {
        PerformRaycast();
    }

    void PerformRaycast()
    {
        Ray ray = targetCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // ����ĳ��Ʈ�� �浹�� ��ü�� �ִ��� Ȯ���մϴ�.
        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            Outline newOutline = hit.collider.gameObject.GetComponent<Outline>();

            // ��ü�� �����ϸ� �ش� ��ü�� �̸��� �α׷� ����մϴ�.
            if (hit.collider.gameObject.tag == "Item")
            {
                Debug.Log("Hit Object: " + hit.collider.gameObject.name);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hit.collider.CompareTag("Door"))
                {
                    TestDoorCtrl hitDoor = hit.collider.GetComponent<TestDoorCtrl>();
                    hitDoor.ChangeDoorState();
                }
            }
            if (currentOutline != newOutline)
            {
                if (currentOutline != null)
                {
                    currentOutline.OutlineWidth = 0; // ���� �ƿ����� ����
                    currentOutline = null;
                }

                currentOutline = newOutline;

                if (currentOutline != null)
                {
                    currentOutline.OutlineWidth = 10; // ���ο� �ƿ����� ����
                }
            }
            if (Input.GetMouseButtonDown(0))                                //��Ŭ�� ���� ��
            {
                if (hit.collider.CompareTag("ProductBox"))  // ProductBox �±׿� ����� ��
                {
                    var p = hit.collider.GetComponent<ProductBoxInfo>();
                    productBox = hit.collider.GetComponent<ProductBox>();
                    if (productBox != null)
                    {
                        var boxCollider = productBox.transform.gameObject.GetComponent<BoxCollider>();
                        boxCollider.enabled = false;

                        // world traqnsform position
                        productBox.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        productBox.transform.SetParent(playerHand.transform);
                        productBox.transform.localPosition = Vector3.zero;
                        productBox.transform.localRotation = Quaternion.identity;
                        uiManager.OnProductBoxInfo(p.ProductName, p.ProductCount);

                        uiManager.OnProductBoxPanel();
                    }
                    //if (p != null)
                    //{
                    //    var boxCollider = p.transform.gameObject.GetComponent<BoxCollider>();                        
                    //    boxCollider.enabled = false;

                    //    // world traqnsform position
                    //    p.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    //    p.transform.SetParent(playerHand.transform);
                    //    p.transform.localPosition = Vector3.zero; 
                    //    p.transform.localRotation = Quaternion.identity;
                    //}



                    // 1. �ڽ� Ŭ��
                    // 2. �ڽ� ���� (�ڽ� �ȿ� �ִ� ��ǰ �̸�, ����, ����, Ÿ��)
                    // 3. �ڽ� �̵� (���� ���ڿ� ������ ���� �� �ְ� ���ڰ� ����� �� ������)
                }

                if (hit.collider.CompareTag("Shelf"))                       // ���� �ݶ��̴��� ���� �ִ� �±װ� "Shelf"�� ��
                {
                    var shelf = hit.collider.gameObject.GetComponent<Shelf>();
                    if (shelf != null)
                    {
                        switch (shelf)
                        {
                            case SnackShelf:
                                { 
                                    var snackShelf = shelf as SnackShelf;
                                    if (productBox != null)
                                    {
                                        var boxInfo = productBox.GetBoxInfo();
                                        if (boxInfo.ProductType == snackShelf.GetShelfType())
                                        {
                                            Debug.Log("������ ���� " + productBox.ProductList.Count);

                                            if (productBox.ProductList.Count > 0)
                                            {
                                                snackShelf.PushItem(productBox.ProductList[0], boxInfo.ProductType);
                                                productBox.ProductList.Remove(productBox.ProductList[0]);
                                            }
                                        }
                                    }
                                }
                                Debug.Log("SnackShelf");
                                break;
                            case DrinkShelf:
                                Debug.Log("DrinkShelf");
                                break;
                            default:
                                Debug.Log("Unknown shelf type");
                                break;
                        }
                    }
                }
            }
            if (hit.collider.CompareTag("CounterProduct"))
            {
                GameObject counterProductObj = hit.collider.gameObject;
                checkoutSystem.SelectedProduct(counterProductObj);
            }
            if (hit.collider.CompareTag("Money"))
            {
                Money moneyObj = hit.collider.GetComponent<Money>();
                //checkoutSystem.takeMoneys.Add(moneyObj.money.value);  
                //checkoutSystem.takeMoney += moneyObj.money.value;
                //checkoutSystem.takeMoneys.Remove(moneyObj.money.value);
                checkoutSystem.Calculate(moneyObj);
                Destroy(moneyObj.gameObject);
            }
        }
        if (Input.GetMouseButtonDown(1))                                        //��Ŭ���� ���� ��
        {
            if (hit.collider.CompareTag("Shelf"))                               //ray�� ���� ������Ʈ�� "Shelf" �±׸� ������ �ִٸ�
            {
                //SnackShelf hitShelf = hit.collider.GetComponent<SnackShelf>();    //ray�� ���� ������Ʈ���Լ� ShelfCtrl ������Ʈ�� ���� �´�.
                //if (hitShelf.SnackList.Count != 0)                            //hitShelf�� ��ǰ�� �ϳ��� ���� �ִٸ�
                //{
                //    GameObject productObj = hitShelf.SnackList[hitShelf.SnackList.Count - 1];        //productObj�� hitShelf�� ���� �ִ� ��ǰ ����� ���� ���� �ִ� ������Ʈ �����͸� ������.
                //    var boxInfo = productBox.GetComponent<ProductBoxInfo>();
                //    productBox.InsertProduct(productObj);
                //    hitShelf.PopItem(productObj, boxInfo.ProductType);
                //    uiManager.OnProductBoxInfo(boxInfo.ProductName, boxInfo.ProductCount);

                //}
            }
        }
        if (Input.GetKeyDown(KeyCode.F))                                                    //F�� ������ ��
        {
            if (productBox != null)                                                         //productBox�� ��� �ִٸ�
            {
                productBox.GetComponent<BoxCollider>().enabled = true;
                productBox.transform.SetParent(null);
                productBox = lastBox;
            }
            else
            {
                uiManager.CloseProductBoxPanel();
            }
        }
    }
}
