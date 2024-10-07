using UnityEngine;

public class TestPlayerCtrl : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    [Header("Look")]
    public float mouseSpeed;
    public float yRotation;
    public float xRotation;
    public Camera cam;

    public GameObject playerHand;

    [HideInInspector]
    public bool canLook = true;

    public ProductBox productBox;
    private ShelfCtrl shelf;
    private TestShop testShop;
    private CheckoutSystem checkoutSystem;
    public UIManager uiManager;
    public string enteredAmount = "";
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   //마우스 커서를 화면 안에서 고정
        Cursor.visible = false;                     //마우스 커서를 보이지 않도록 설정

        cam = Camera.main;

        checkoutSystem = GetComponent<CheckoutSystem>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        PlayerMove();
        CameraLook();

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))                                //좌클릭 했을 때
            {
                if (hit.collider.CompareTag("ProductBox"))                  //ray에 닿은 콜라이더가 갖고 있는 태그가 "ProductBox"라면
                {
                    if (productBox == null)
                    {
                        productBox = hit.collider.GetComponent<ProductBox>();
                        hit.collider.gameObject.transform.parent = playerHand.transform;    //ray에 닿은 "ProductBox" 태그를 가진 오브젝트를 playerHand 자식에 넣는다.
                        hit.collider.gameObject.transform.localPosition = Vector3.zero;
                        hit.collider.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);   //ray에 닿은 "ProductBox" 태그를 가진 오브젝트의 크기를 x:0.3, y:0.3, z:0.3으로 바꾼다.
                    }
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
                            }
                            if (productBox.productObjectList.Count == 0)
                            {
                                Debug.Log("상자가 비었어요~");
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
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.F))                                                    //F를 눌렀을 때
            {
                if (productBox != null)                                                         //productBox를 들고 있다면
                {
                    productBox.transform.position = hit.point + new Vector3(0f, 0.5f, 0f);     //들고 있던 productBox는 hit한 포인트에서 y로 0.5f 높은 곳으로 이동한다.
                    productBox.transform.localScale = Vector3.one;
                    productBox.transform.SetParent(null);
                    productBox = null;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            //bool panelActive = testShop.ProductListPanel.activeSelf;
            //if (panelActive)
            //{
            //    testShop.ProductListPanel.gameObject.SetActive(false);
            //}
            //if (!panelActive)
            //{
            //    testShop.ProductListPanel.gameObject.SetActive(true);
            //}
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
        //if (checkoutSystem.isCalculating)
        //{
        //    for (KeyCode key = KeyCode.Keypad0; key <= KeyCode.Keypad9; key++)
        //    {
        //        if (Input.GetKeyDown(key))
        //        {
        //            int numberPressed = key - KeyCode.Keypad0;
        //            enteredAmount += numberPressed.ToString();
        //            uiManager.ShowInputChangeText();
        //        }
        //    }
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //        if (!string.IsNullOrEmpty(enteredAmount))
        //        {
        //            checkoutSystem.changeMoney = int.Parse(enteredAmount);
        //            Debug.Log("입력한 거스름돈" + checkoutSystem.changeMoney);
        //            checkoutSystem.CalculateChange();
        //            enteredAmount = "";
        //        }
        //    }
        //}
        
    }

    void CameraLook()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    void PlayerMove()
    {
        float Horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");

        Vector3 moveVec = transform.forward * Vertical + transform.right * Horizontal;

        transform.position += moveVec.normalized * moveSpeed * Time.deltaTime;
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
