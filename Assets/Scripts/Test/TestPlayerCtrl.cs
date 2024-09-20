using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
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

    private ProductBox productBoxInHand;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   //마우스 커서를 화면 안에서 고정
        Cursor.visible = false;                     //마우스 커서를 보이지 않도록 설정

        cam = Camera.main;
    }

    void Update()
    {
        PlayerMove();
        CameraLook();

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("ProductBox"))
                {
                    productBoxInHand = hit.collider.GetComponent<ProductBox>();
                    hit.collider.gameObject.transform.parent = playerHand.transform;
                    hit.collider.gameObject.transform.localPosition = Vector3.zero;
                    hit.collider.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    
                }

                if (hit.collider.CompareTag("Shelf"))
                {
                    if (productBoxInHand != null)
                    {
                        ShelfCtrl shelfCtrl = hit.collider.GetComponent<ShelfCtrl>(); ;
                        if (shelfCtrl != null)
                        {
                            GameObject product = productBoxInHand.productObjectQueue.Peek();
                            bool isDisplayed = shelfCtrl.DisplayProduct(product);
                            if (isDisplayed)
                            {
                                productBoxInHand.RemoveProduct(product);
                            }
                            if (productBoxInHand.productObjectQueue.Count == 0)
                            {
                                Debug.Log("상자가 비었어요~");
                            }
                        }
                    }
                }
                if (hit.collider.CompareTag("TrashCan"))
                {
                    if (productBoxInHand.productObjectQueue.Count == 0)
                    {
                        Destroy(productBoxInHand.gameObject);
                        Debug.Log("상자 없앰");
                    }
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (hit.collider.CompareTag("Shelf"))
                {
                    if (productBoxInHand)
                    {
                        
                    }
                }
            }
        }
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
}
