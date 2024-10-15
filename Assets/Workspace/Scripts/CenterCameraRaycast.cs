using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterCameraRaycast : MonoBehaviour
{
    public Camera targetCamera;
    public float raycastRange = 100f; // 레이캐스트의 최대 거리

    public Outline currentOutline;

    public GameObject playerHand;
    public ProductBox productBox;
    private ShelfCtrl shelf;
    private TestShop testShop;
    private CheckoutSystem checkoutSystem;
    public UIManager uiManager;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   //마우스 커서를 화면 안에서 고정
        Cursor.visible = false;                     //마우스 커서를 보이지 않도록 설정

        //cam = Camera.main;

        checkoutSystem = GetComponent<CheckoutSystem>();
        uiManager = FindObjectOfType<UIManager>();
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

        if (productBox != null)
        {
            uiManager.OnProductBoxPanel();  // productBox가 있을 때 패널 열기
            uiManager.OnProductBoxInfo();  // productBox 정보 갱신
        }
        else
        {
            uiManager.CloseProductBoxPanel();  // productBox가 없으면 패널 닫기
        }
    }

    void PerformRaycast()
    {
        Ray ray = targetCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        // 레이캐스트가 충돌한 물체가 있는지 확인합니다.
        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            Outline newOutline = hit.collider.gameObject.GetComponent<Outline>();

            // 물체를 감지하면 해당 물체의 이름을 로그로 출력합니다.
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
                    currentOutline.OutlineWidth = 0; // 이전 아웃라인 해제
                    currentOutline = null;
                }

                currentOutline = newOutline;

                if (currentOutline != null)
                {
                    currentOutline.OutlineWidth = 10; // 새로운 아웃라인 적용
                }   
            }
            if (Input.GetMouseButtonDown(0))                                //좌클릭 했을 때
            {
                if (hit.collider.CompareTag("ProductBox"))  // ProductBox 태그에 닿았을 때
                {
                    var p = hit.collider.GetComponent<ProductBoxInfo>();
                    
                    if (p != null)
                    {
                        var boxCollider = p.transform.gameObject.GetComponent<BoxCollider>();                        
                        boxCollider.enabled = false;

                        // world traqnsform position
                        p.transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        p.transform.SetParent(playerHand.transform);
                        p.transform.localPosition = Vector3.zero; 
                        p.transform.localRotation = Quaternion.identity;
                    }

                    //var productName = 
                    //    hit.collider.gameObject.transform.parent = playerHand.transform;    //ray에 닿은 "ProductBox" 태그를 가진 오브젝트를 playerHand 자식에 넣는다.
                    //    hit.collider.gameObject.transform.localPosition = Vector3.zero;
                    //    hit.collider.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    //    uiManager.OnProductBoxPanel();  // productBox 패널 켜기
                    //    uiManager.OnProductBoxInfo();  // productBox 정보 갱신
                    //}

                    // 1. 박스 클릭
                    // 2. 박스 정보 (박스 안에 있는 상품 이름, 개수, 사진, 타입)
                    // 3. 박스 이동 (집고 상자에 물건이 있을 땐 넣고 상자가 비었을 땐 버린다)
                }

                if (hit.collider.CompareTag("Shelf"))                       //닿은 콜라이더가 갖고 있는 태그가 "Shelf"일 때
                {
                    if (productBox != null)                                 //productBox를 들고 있다면
                    {
                        ShelfCtrl shelfCtrl = hit.collider.GetComponent<ShelfCtrl>(); ; //ray에 닿은 오브젝트에게서 ShelfCtrl 컴포넌트를 갖고 온다.
                        if (shelfCtrl != null)                                          //ShelfCtrl이 null이 아닐 때
                        {
                            GameObject productObj = productBox.productObjectList[productBox.productObjectList.Count - 1];    //productObj는 productBox.productObjectList의 마지막 인덱스 오브젝트이다.
                            bool isDisplayed = shelfCtrl.DisplayProduct(productObj);    //중복 체크를 위해 DisplayProduct의 반환형을 bool로 했기 때문에 진열이 되었다면 true를 반환한다.
                            if (isDisplayed)    //진열이 되었을 때
                            {
                                productBox.RemoveProduct(productObj);       //productBox의 RemoveProduct 함수에 productObj 인자를 전달한다.
                                uiManager.OnProductBoxInfo();
                            }
                            if (productBox.productObjectList.Count == 0)
                            {
                                uiManager.CloseProductBoxPanel();
                            }
                        }
                    }
                }
                if (hit.collider.CompareTag("TrashCan"))                    //ray에 닿은 오브젝트가 "TrashCan" 태그를 가지고 있으며
                {
                    if (productBox.productObjectList.Count == 0)            //들고 있는 productBox에 상품이 하나도 없다면
                    {
                        Destroy(productBox.gameObject);                     //들고 있는 productBox를 없앤다.
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
            if (Input.GetMouseButtonDown(1))                                        //우클릭을 했을 때
            {
                if (hit.collider.CompareTag("Shelf"))                               //ray에 닿은 오브젝트가 "Shelf" 태그를 가지고 있다면
                {
                    ShelfCtrl hitShelf = hit.collider.GetComponent<ShelfCtrl>();    //ray에 닿은 오브젝트에게서 ShelfCtrl 컴포넌트를 갖고 온다.
                    if (hitShelf.productList.Count != 0)                            //hitShelf가 상품을 하나라도 갖고 있다면
                    {
                        GameObject productObj = hitShelf.productList.Peek();        //productObj는 hitShelf가 갖고 있는 상품 목록의 가장 위에 있는 오브젝트 데이터를 가진다.
                        productBox.InsertProduct(productObj);
                        hitShelf.MoveProductToBox(productObj);
                        uiManager.OnProductBoxInfo();
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.F))                                                    //F를 눌렀을 때
            {
                if (productBox != null)                                                         //productBox를 들고 있다면
                {
                    productBox.transform.SetParent(null);
                    productBox = null;
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
        Cursor.lockState = CursorLockMode.Locked;   //마우스 커서를 화면 안에서 고정
        Cursor.visible = false;
    }
    void IsPanelOff()
    {
        Cursor.lockState = CursorLockMode.None;   //마우스 커서를 화면 안에서 고정
        Cursor.visible = true;                     //마우스 커서를 보이지 않도록 설정           //마우스 커서를 보이지 않도록 설정
    }
}
