using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    Idle,
    WalkingToShelf,
    PickingProduct,
    //WaitCounter,
    WalkingToCounter,
    PlacingProduct,
    WaitingCalcPrice,
    //GivingMoney,
    LeavingStore
}

public class Timer
{
    private float timeRemaining;

    public void Set(float time)
    {
        timeRemaining = time;
    }

    public void Update(float deltaTime)
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= deltaTime;
        }
    }

    public bool IsFinished()
    {
        return timeRemaining <= 0;
    }
}

public class CustomerCtrl : MonoBehaviour
{
    public float waitTime = 0.5f;

    public CustomerState currentState;
    private Timer timer;
    public NavMeshAgent agent;
    public bool isMoveDone = false;

    public Transform target;
    public Transform counter;
    public Transform exitPoint;

    public List<Transform> targetPos = new List<Transform>();
    public List<GameObject> pickProduct = new List<GameObject>();
    public List<GameObject> counterProduct = new List<GameObject>();
    public List<GameObject> shelfList = new List<GameObject>();



    private static int nextPriority = 0;
    private static readonly object priorityLock = new object();

    void AssignPriority()
    {
        lock (priorityLock)
        {
            agent.avoidancePriority = nextPriority;
            nextPriority = (nextPriority + 1) % 100;
        }
    }

    void Start()
    {
        timer = new Timer();
        agent = GetComponent<NavMeshAgent>();
        currentState = CustomerState.Idle;
        SearchShelfs();
    }

    void Update()
    {
        timer.Update(Time.deltaTime);

        if (!agent.hasPath && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                isMoveDone = true;
            }
        }

        switch (currentState)
        {
            case CustomerState.Idle:
                Idle();
                break;
            case CustomerState.WalkingToShelf:
                WalkingToShelf();
                break;
            case CustomerState.PickingProduct:
                PickingProduct();
                break;
            case CustomerState.WalkingToCounter:
                WalkingToCounter();
                break;

        }
    }

    void SearchShelfs()
    {
        GameObject[] shelfs = GameObject.FindGameObjectsWithTag("Shelf");

        if (shelfs != null)
        {
            for (int i = 0; i < shelfs.Length; i++)
            {
                targetPos.Add(shelfs[i].transform);
                shelfList.Add(shelfs[i]);
            }
        }
    }

    void ChangeState(CustomerState nextState, float waitTime = 0.0f)
    {
        currentState = nextState;
        timer.Set(waitTime);
    }

    void Idle()
    {
        if (timer.IsFinished())
        {
            if (shelfList.Count > 0)
            {
                if (targetPos.Count > 0)
                {
                    target = targetPos[Random.Range(0, targetPos.Count)];
                    MoveToTarget();
                    ChangeState(CustomerState.WalkingToShelf, waitTime);
                }
                else
                {
                    ChangeState(CustomerState.WalkingToCounter, waitTime);
                }
            }
        }
    }

    void MoveToTarget()
    {
        isMoveDone = false;
        if (targetPos != null)
        {
            agent.SetDestination(target.position);
        }
    }

    void WalkingToShelf()
    {
        if (timer.IsFinished() && isMoveDone)
        {
            ChangeState(CustomerState.PickingProduct, waitTime);
        }
    }

    void PickingProduct()
    {
        if (timer.IsFinished())
        {
            ShelfCtrl shelf = target.GetComponent<ShelfCtrl>();

            if (shelf != null)
            {
                int randomCount = Random.Range(0, 5);
                Debug.Log(randomCount);
                for (int i = 0; i < randomCount; i++)
                {
                    GameObject productObj = shelf.productList.Pop();
                    shelf.PickUpProduct(productObj, randomCount);
                    pickProduct.Add(productObj);
                    targetPos.Remove(target);
                }
            }
            ChangeState(CustomerState.Idle, waitTime);
        }
    }

    void WalkingToCounter()
    {
        Debug.Log("WalkingToCounter 함수 실행");
    }
}
