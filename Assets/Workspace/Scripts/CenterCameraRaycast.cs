using UnityEngine;
using UnityEngine.UIElements;

public class CenterCameraRaycast : MonoBehaviour
{
    public GenerateCustomer GenerateCustomer;

    public Camera targetCamera;
    public float raycastRange = 100f; // ����ĳ��Ʈ�� �ִ� �Ÿ�
    public float throwForce = 150f;

    public Outline currentOutline;

    public GameObject playerHand;
    public ProductBox productBox;
    public ProductBox lastBox;
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
                if (hit.collider.CompareTag("TrashCan"))
                {
                    if (productBox.ProductList.Count == 0)
                    {
                        Destroy(productBox.gameObject);
                        productBox = null;
                    }
                }

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
                                            if (snackShelf.PushItem(productBox.ProductList[productBox.ProductList.Count - 1], boxInfo.ProductType, boxInfo.ProductID))
                                            productBox.RemoveProduct(productBox.ProductList[productBox.ProductList.Count - 1]);
                                        }
                                    }  
                                }
                                Debug.Log("SnackShelf");
                                break;
                            case DrinkShelf:
                                var drinkShelf = shelf as DrinkShelf;
                                if (productBox != null)
                                {
                                    var boxInfo = productBox.GetBoxInfo();
                                    if (boxInfo.ProductType == drinkShelf.GetShelfType())
                                    {
                                        if (drinkShelf.PushItem(productBox.ProductList[productBox.ProductList.Count - 1], boxInfo.ProductType, boxInfo.ProductID))
                                            productBox.RemoveProduct(productBox.ProductList[productBox.ProductList.Count - 1]);
                                    }
                                }
                                Debug.Log("DrinkShelf");
                                break;
                            case IcecreamShelf:
                                var icecreamShelf = shelf as IcecreamShelf;
                                if (productBox != null)
                                {
                                    var boxInfo = productBox.GetBoxInfo();
                                    if (boxInfo.ProductType == icecreamShelf.GetShelfType())
                                    {
                                        if (icecreamShelf.PushItem(productBox.ProductList[productBox.ProductList.Count - 1], boxInfo.ProductType, boxInfo.ProductID))
                                            productBox.RemoveProduct(productBox.ProductList[productBox.ProductList.Count - 1]);
                                    }
                                }
                                Debug.Log("IcecreamShelf");
                                break;
                            default:
                                Debug.Log("Unknown shelf type");
                                break;
                        }
                    }
                }
            }
            
        }
        if (Input.GetMouseButtonDown(1))                                        //��Ŭ���� ���� ��
        {
            if (hit.collider.CompareTag("Shelf"))                               //ray�� ���� ������Ʈ�� "Shelf" �±׸� ������ �ִٸ�
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
                                            productBox.InsertProduct(snackShelf.ProductList[snackShelf.ProductList.Count-1]);
                                            snackShelf.PopItem(snackShelf.ProductList[snackShelf.ProductList.Count - 1], boxInfo.ProductType);
                                        }
                                    }
                                }

                            }
                            Debug.Log("SnackShelf");
                            break;
                        case DrinkShelf:
                            var drinkShelf = shelf as DrinkShelf;
                            if (productBox != null)
                            {
                                var boxInfo = productBox.GetBoxInfo();
                                if (boxInfo.ProductType == drinkShelf.GetShelfType())
                                {
                                    Debug.Log("������ ���� " + productBox.ProductList.Count);

                                    if (productBox.ProductList.Count > 0)
                                    {
                                        productBox.InsertProduct(drinkShelf.ProductList[drinkShelf.ProductList.Count - 1]);
                                        drinkShelf.PopItem(drinkShelf.ProductList[drinkShelf.ProductList.Count - 1], boxInfo.ProductType);
                                    }
                                }
                            }
                            Debug.Log("DrinkShelf");
                            break;
                        case IcecreamShelf:
                            var icecreamShelf = shelf as IcecreamShelf;
                            if (productBox != null)
                            {
                                var boxInfo = productBox.GetBoxInfo();
                                if (boxInfo.ProductType == icecreamShelf.GetShelfType())
                                {
                                    Debug.Log("������ ���� " + productBox.ProductList.Count);

                                    if (productBox.ProductList.Count > 0)
                                    {
                                        productBox.InsertProduct(icecreamShelf.ProductList[icecreamShelf.ProductList.Count - 1]);
                                        icecreamShelf.PopItem(icecreamShelf.ProductList[icecreamShelf.ProductList.Count - 1], boxInfo.ProductType);
                                    }
                                }
                            }
                            Debug.Log("IcecreamShelf");
                            break;
                        default:
                            Debug.Log("Unknown shelf type");
                            break;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))                                                    //F�� ������ ��
        {
            if (productBox != null)                                                         //productBox�� ��� �ִٸ�
            {
                productBox.GetComponent<BoxCollider>().enabled = true;
                productBox.transform.SetParent(null);
                Rigidbody rb = productBox.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                Vector3 throwDirection = targetCamera.transform.forward;
                rb.AddForce(throwDirection * throwForce);
                productBox = lastBox;
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            uiManager.OnShopPanel();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GenerateCustomer.gameObject.SetActive(true);
        }
    }
}
