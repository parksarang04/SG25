using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCameraRaycast : MonoBehaviour
{
    public Camera targetCamera;
    public float raycastRange = 100f; // ����ĳ��Ʈ�� �ִ� �Ÿ�

    public Outline currentOutline;

    public GameObject playerHand;
    public ProductBox productBox;
    public ProductBox lastBox;  
    private ShelfCtrl shelf;
    private TestShop testShop;
    private CheckoutSystem checkoutSystem;
    public CustomerCtrl customer;
    public UIManager uiManager;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   //���콺 Ŀ���� ȭ�� �ȿ��� ����
        Cursor.visible = false;                     //���콺 Ŀ���� ������ �ʵ��� ����

        //cam = Camera.main;

        checkoutSystem = GetComponent<CheckoutSystem>();
        uiManager = FindObjectOfType<UIManager>();
        customer = FindObjectOfType<CustomerCtrl>();
    }
    void Update()
    {
        PerformRaycast();

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            uiManager.OnShopPanel();
            if (uiManager.isPanelOn == true)
            {
                IsPanelOn();
            }
            else
            {
                IsPanelOff();
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            customer.ShakeShelf();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            customer.gameObject.SetActive(!customer.gameObject.activeInHierarchy);
        }

        if (productBox != null)
        {
            uiManager.OnProductBoxPanel();  // productBox�� ���� �� �г� ����
            uiManager.OnProductBoxInfo();  // productBox ���� ����
        }
        else
        {
            uiManager.CloseProductBoxPanel();  // productBox�� ������ �г� �ݱ�
        }
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
                    //var p = hit.collider.GetComponent<ProductBoxInfo>();
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

                        uiManager.OnProductBoxPanel();  // productBox �г� �ѱ�
                        uiManager.OnProductBoxInfo();  // productBox ���� ����
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

                if (hit.collider.CompareTag("Shelf"))                       //���� �ݶ��̴��� ���� �ִ� �±װ� "Shelf"�� ��
                {
                    if (productBox != null)                                 //productBox�� ��� �ִٸ�
                    {
                        ShelfCtrl shelfCtrl = hit.collider.GetComponent<ShelfCtrl>(); ; //ray�� ���� ������Ʈ���Լ� ShelfCtrl ������Ʈ�� ���� �´�.
                        if (shelfCtrl != null)                                          //ShelfCtrl�� null�� �ƴ� ��
                        {
                            GameObject productObj = productBox.productObjectList[productBox.productObjectList.Count - 1];    //productObj�� productBox.productObjectList�� ������ �ε��� ������Ʈ�̴�.
                            bool isDisplayed = shelfCtrl.DisplayProduct(productObj);    //�ߺ� üũ�� ���� DisplayProduct�� ��ȯ���� bool�� �߱� ������ ������ �Ǿ��ٸ� true�� ��ȯ�Ѵ�.
                            if (isDisplayed)    //������ �Ǿ��� ��
                            {
                                productBox.RemoveProduct(productObj);       //productBox�� RemoveProduct �Լ��� productObj ���ڸ� �����Ѵ�.
                                uiManager.OnProductBoxInfo();
                            }
                            if (productBox.productObjectList.Count == 0)
                            {
                                uiManager.CloseProductBoxPanel();
                            }
                        }
                    }
                }
                if (hit.collider.CompareTag("TrashCan"))                    //ray�� ���� ������Ʈ�� "TrashCan" �±׸� ������ ������
                {
                    if (productBox.productObjectList.Count == 0)            //��� �ִ� productBox�� ��ǰ�� �ϳ��� ���ٸ�
                    {
                        Destroy(productBox.gameObject);                     //��� �ִ� productBox�� ���ش�.
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
                    ShelfCtrl hitShelf = hit.collider.GetComponent<ShelfCtrl>();    //ray�� ���� ������Ʈ���Լ� ShelfCtrl ������Ʈ�� ���� �´�.
                    if (hitShelf.productList.Count != 0)                            //hitShelf�� ��ǰ�� �ϳ��� ���� �ִٸ�
                    {
                        GameObject productObj = hitShelf.productList.Peek();        //productObj�� hitShelf�� ���� �ִ� ��ǰ ����� ���� ���� �ִ� ������Ʈ �����͸� ������.
                        productBox.InsertProduct(productObj);
                        hitShelf.MoveProductToBox(productObj);
                        uiManager.OnProductBoxInfo();
                    }
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
    void IsPanelOn()
    {
        Cursor.lockState = CursorLockMode.Locked;   //���콺 Ŀ���� ȭ�� �ȿ��� ����
        Cursor.visible = false;
    }
    void IsPanelOff()
    {
        Cursor.lockState = CursorLockMode.None;   //���콺 Ŀ���� ȭ�� �ȿ��� ����
        Cursor.visible = true;                     //���콺 Ŀ���� ������ �ʵ��� ����           //���콺 Ŀ���� ������ �ʵ��� ����
    }
}
