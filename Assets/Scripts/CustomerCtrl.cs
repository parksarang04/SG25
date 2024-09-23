using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    Idle,
    WalkingToShelf,
    PickingProduct,
    WaitCounter,
    WalkingToCounter,
    PlacingProduct,
    WaitingCalcPrice,
    GivingMoney,
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
    public GameObject customerHand;

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
        counter = GameObject.Find("Counter").transform;
        currentState = CustomerState.Idle;
        SearchShelfs();
        AssignPriority();
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
            case CustomerState.WaitCounter:
                WaitCounter();
                break;
            case CustomerState.WalkingToCounter:
                WalkingToCounter();
                break;
            case CustomerState.PlacingProduct:
                PlacingProduct();
                break;
            case CustomerState.WaitingCalcPrice:
                WaitingCalcPrice();
                break;
            case CustomerState.GivingMoney:
                GivingMoney();
                break;
            case CustomerState.LeavingStore:
                LeavingStore();
                break;

        }
    }

    void SearchShelfs()
    {
        GameObject[] shelfs = GameObject.FindGameObjectsWithTag("Shelf");

        if (shelfs != null)
        {
            foreach (GameObject shelf in shelfs)
            {
                targetPos.Add(shelf.transform);
                shelfList.Add(shelf);
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
                    ChangeState(CustomerState.WaitCounter, waitTime);
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
                int randomCount = Random.Range(0, 2);
                Debug.Log(randomCount);
                for (int i = 0; i < randomCount; i++)
                {
                    if (randomCount > 0)
                    {
                        GameObject productObj = shelf.productList.Pop();
                        shelf.PickUpProduct(randomCount);
                        productObj.transform.SetParent(customerHand.transform);
                        productObj.transform.localPosition = Vector3.zero;
                        productObj.SetActive(false);
                        pickProduct.Add(productObj);
                        targetPos.Remove(target);
                    }
                }
                if (randomCount == 0)
                {
                    targetPos.Remove(target);
                }
            }
            ChangeState(CustomerState.Idle, waitTime);
        }
    }

    void WaitCounter()
    {
        CustomerCtrl[] allCustomers = FindObjectsOfType<CustomerCtrl>();
        bool isCounterOccupied = false;
        foreach (var customer in allCustomers)
        {
            if (customer != this && customer.currentState == CustomerState.WalkingToCounter)
            {
                isCounterOccupied = true;
                break;
            }
        }

        if (isCounterOccupied)
        {
            Transform availablePosition = GetAvailableCounterLinePosition();
            if (availablePosition != null)
            {
                target = availablePosition;
                agent.SetDestination(availablePosition.position);
            }
        }
        else
        {
            ChangeState(CustomerState.WalkingToCounter, waitTime);
        }

    }

    void WalkingToCounter()
    {
        target = counter;
        agent.SetDestination(counter.position);

        if (timer.IsFinished())
        {
            ChangeState(CustomerState.PlacingProduct, waitTime);
        }
    }

    void PlacingProduct()
    {
        foreach (GameObject product in pickProduct)
        {
            product.SetActive(true);
            counter.position = product.transform.position = new Vector3(0f, 0f, 0f);
            product.transform.parent = null;    
            pickProduct.Remove(product);
            counterProduct.Add(product);    
        }
    }

    void WaitingCalcPrice()
    {

    }

    void GivingMoney()
    {

    }

    void LeavingStore()
    {

    }

    Transform GetAvailableCounterLinePosition()
    {
        GameObject[] counterLine = GameObject.FindGameObjectsWithTag("CounterLine");

        foreach (GameObject pos in counterLine)
        {
            bool positionOccupied = false;
            CustomerCtrl[] allcustomers = FindObjectsOfType<CustomerCtrl>();
            foreach (var customer in allcustomers)
            {
                if (customer != this && customer.target == pos.transform)
                {
                    positionOccupied = true;
                    break;
                }
            }

            if (!positionOccupied)
            {
                return pos.transform;
            }
        }
        return null;
    }
}
