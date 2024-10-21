using UnityEngine;

public class PlayerHandController : MonoBehaviour
{
    public Camera targetCamera;
    public float raycastRange = 100f;
    public GameObject playerHand;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = targetCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastRange))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("ProductBox"))
                {
                    GameObject box = hit.collider.gameObject;
                    box.transform.SetParent(playerHand.transform);
                    box.transform.localPosition = Vector3.zero;

                    Rigidbody boxRb = box.GetComponent<Rigidbody>();
                    boxRb.isKinematic = true;

                    BoxCollider boxColl = box.GetComponent<BoxCollider>();
                    boxColl.GetComponent<BoxCollider>().enabled = false;
                }
            }
        }
    }
}
