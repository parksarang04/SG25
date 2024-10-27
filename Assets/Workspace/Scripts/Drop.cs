using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    public GameObject Trash;
    private int maxTrashCount = 2;
    private int currentTrashCount = 0;
    private Coroutine dropCoroutine;

    void OnTriggerEnter(Collider guest)
    {
        if(guest.gameObject.CompareTag("Room"))
        {
            Debug.Log("Enter");
            if (dropCoroutine == null)
            {
                dropCoroutine = StartCoroutine(DropTrash());
            }
        }
    }

    IEnumerator DropTrash()
    {
        while(currentTrashCount < maxTrashCount)
        {
            float waitTime = Random.Range(1f,3f);
            yield return new WaitForSeconds(waitTime);
            DropPos();
        }
    }

    private void DropPos()
    {
        if (currentTrashCount >= maxTrashCount) return;

        Vector3 dropPosition = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        Instantiate(Trash, dropPosition, Quaternion.identity);

        currentTrashCount++;
        
        if (currentTrashCount >= maxTrashCount)
        {
            if (dropCoroutine != null)
            {
                StopCoroutine(dropCoroutine);
                dropCoroutine = null;
            }
            Debug.Log("end");
        }
    }

    void OnTriggerExit(Collider guest)
    {
        if(guest.gameObject.CompareTag("Room"))
        {
            Debug.Log("Exit");
            if (dropCoroutine != null)
            {
                StopCoroutine(dropCoroutine);
                dropCoroutine = null;
            }
        }
    }

    /*void RidTrash()
    {
        ray = playerCam.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out hit, MaxDistance))
        {
            if(hit.collider.gameObject.CompareTag("Trash"))
            {
                trashObj = hit.collider.gameObject;
                Destroy(trashObj);
            }
        }
    }*/
}
