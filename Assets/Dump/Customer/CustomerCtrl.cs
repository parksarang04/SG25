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
    private CheckoutSystem checkoutSystem;

    public bool isMoveDone = false;

    public Transform target;
    public Transform counter;
    public Transform exitPoint;
    public GameObject customerHand;
    public MoneyData[] moneyPrefabs;

    public List<Transform> targetPos = new List<Transform>();
    public List<GameObject> pickProduct = new List<GameObject>();
    public List<GameObject> shelfList = new List<GameObject>();
    

    private static int nextPriority = 0;
    private static readonly object priorityLock = new object();

    public Animator animator;

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
        checkoutSystem = FindObjectOfType<CheckoutSystem>();
        counter = GameObject.Find("Counter").transform;
        //animator.applyRootMotion = false;
        
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
            case CustomerState.GivingMoney:
                GivingMoney();
                break;
            case CustomerState.WaitingCalcPrice:
                WaitingCalcPrice();
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
                    animator.CrossFade("Walk", 0);
                    animator.ResetTrigger("MotionTrigger");
                }
                else
                {
                    ChangeState(CustomerState.WaitCounter, waitTime);
                    animator.ResetTrigger("MotionTrigger");
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
            animator.SetTrigger("MotionTrigger");
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
                    if (randomCount > 0)
                    {
                        if (shelf.productList.Count > 0)
                        {
                            GameObject productObj = shelf.productList.Pop();
                            shelf.PickUpProduct(randomCount);
                            productObj.transform.SetParent(customerHand.transform);
                            productObj.transform.localPosition = Vector3.zero;
                            productObj.SetActive(false);
                            pickProduct.Add(productObj);

                            animator.CrossFade("Pick", 0.1f);
                            animator.ResetTrigger("MotionTrigger");
                        }
                        else
                        {
                            Debug.Log("진열대가 비었어여");
                        }
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
            if (customer != this && customer.currentState == CustomerState.WaitingCalcPrice || customer.currentState == CustomerState.GivingMoney   
                || customer.currentState == CustomerState.WalkingToCounter || customer.currentState == CustomerState.PlacingProduct)
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
        if(!isCounterOccupied && pickProduct.Count > 0)
        {
            target = counter;
            agent.SetDestination(counter.position);
            ChangeState(CustomerState.WalkingToCounter, waitTime);
            animator.CrossFade("Walk", 0);
            animator.ResetTrigger("MotionTrigger");
        }
        else if (pickProduct.Count == 0)
        {
            ChangeState(CustomerState.LeavingStore, waitTime);
        }
        
    }

    void WalkingToCounter()
    {

        if (timer.IsFinished() && isMoveDone)
        {
            ChangeState(CustomerState.PlacingProduct, waitTime);
            animator.SetTrigger("MotionTrigger");
        }
    }

    void PlacingProduct()
    {
        if (timer.IsFinished() && isMoveDone)
        {
            if (pickProduct.Count > 0)
            {
                GameObject product = pickProduct[pickProduct.Count - 1];
                product.SetActive(true);
                counter.position = product.transform.position = new Vector3(0f, 0f, 0f);
                product.transform.parent = null;
                product.tag = "CounterProduct";
                pickProduct.Remove(product);
                checkoutSystem.counterProduct.Add(product);
            }
            if (pickProduct.Count == 0)
            {
                ChangeState(CustomerState.GivingMoney, waitTime);
                animator.SetTrigger("MotionTrigger");
            }
        }
    }

    void GivingMoney()
    {
        if (timer.IsFinished() && checkoutSystem.counterProduct.Count == 0)
        {
            GiveMoney(checkoutSystem.totalPrice);
            for (int i = 0; i < checkoutSystem.takeMoneys.Count; i++)
            {
                int sum = 0;
                sum += checkoutSystem.takeMoneys[i];
                Debug.Log(sum);
            }
            ChangeState(CustomerState.WaitingCalcPrice, waitTime);
        }
    }

    void WaitingCalcPrice()
    {
        if (checkoutSystem.takeMoneys.Count == 0 && checkoutSystem.counterProduct.Count == 0)
        {
            //checkoutSystem.ShowChangeAmount();
            Debug.Log(checkoutSystem.changeMoney);
            checkoutSystem.isCalculating = true;
            if (checkoutSystem.isSell == true)
            {
                checkoutSystem.isSell = false;
                ChangeState(CustomerState.LeavingStore, waitTime);
            }
            
        }
    }

    void LeavingStore()
    {
        Debug.Log("손님이 매장을 떠났습니다.");
        Destroy(gameObject);
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

    void GiveMoney(int amount)
    {
        System.Array.Sort(moneyPrefabs, (a, b) => b.value.CompareTo(a.value));
        //bool giveExactChange = Random.Range(0, 2) == 0;
        Vector3 moneyPosition = new Vector3(customerHand.transform.position.x, customerHand.transform.position.y + 0.5f, customerHand.transform.position.z);

        foreach (MoneyData money in moneyPrefabs)
        {
            if (amount >= money.value)
            {
                int count = amount / money.value;
                amount -= count * money.value;

                for (int i = 0; i < count; i++)
                {
                    GameObject moneyObj = Instantiate(money.moneyModel, moneyPosition, Quaternion.identity);
                    checkoutSystem.takeMoneys.Add(money.value);
                }
            }
        }
        //if (giveExactChange)
        //{
        //    foreach (MoneyData money in moneyPrefabs)
        //    {
        //        if (amount >= money.value)
        //        {
        //            int count = amount / money.value;
        //            amount -= count * money.value;
        //            for (int i = 0; i < count; i++)
        //            {
        //                GameObject moneyObj = Instantiate(money.moneyModel, moneyPosition, Quaternion.identity);
        //                checkoutSystem.takeMoneys.Add(money.value);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    int giveMoney = Random.Range(amount, amount + 50000);
        //    foreach (MoneyData money in moneyPrefabs)
        //    {
        //if (giveMoney >= money.value)
        //{
        //    int count = giveMoney / money.value;
        //    giveMoney -= count * money.value;

        //    for (int i = 0; i < count; i++)
        //    {
        //        GameObject moneyObj = Instantiate(money.moneyModel, customerHand.transform);
        //        checkoutSystem.takeMoneys.Add(money.value);
        //    }
        //}
        //    }
        //}
    }
}
