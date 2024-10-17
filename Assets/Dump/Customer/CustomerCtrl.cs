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
    public float power = 0.5f;

    public CustomerState currentState;
    private Timer timer;
    public NavMeshAgent agent;
    private CheckoutSystem checkoutSystem;

    public bool isMoveDone = false;

    public Transform target;
    public Transform counter;
    public Transform CounterPivot;
    public Transform exitPoint;
    public GameObject customerHand;
    public MoneyData[] moneyPrefabs;

    public List<Transform> targetPosList = new List<Transform>();
    public List<GameObject> pickProductList = new List<GameObject>();
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
        CounterPivot = GameObject.Find("CounterPivot").transform;
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
                targetPosList.Add(shelf.transform);
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
            animator.SetBool("Idle", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isPicking", false);
            if (shelfList.Count > 0)
            {
                if (targetPosList.Count > 0)
                {
                    target = targetPosList[Random.Range(0, targetPosList.Count)];
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
        if (targetPosList != null)
        {
            agent.SetDestination(target.position);
            animator.SetBool("isWalking", true);
            animator.SetBool("Idle", false);
            animator.SetBool("isPicking", false);
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
                    if (randomCount > 0)
                    {
                        if (shelf.productList.Count > 0)
                        {
                            animator.SetBool("isPicking", true);
                            animator.SetBool("isWalking", false);
                            animator.SetBool("Idle", false);
                            GameObject productObj = shelf.productList.Pop();
                            shelf.PickUpProduct(randomCount);
                            productObj.transform.SetParent(customerHand.transform);
                            productObj.transform.localPosition = Vector3.zero;
                            productObj.SetActive(false);
                            pickProductList.Add(productObj);    
                        }
                        else
                        {
                            Debug.Log("진열대가 비었어여");
                        }
                        targetPosList.Remove(target);
                    }
                }
                if (randomCount == 0)
                {
                    targetPosList.Remove(target);
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
                animator.SetBool("isWalking", true);
                animator.SetBool("Idle", false);
                animator.SetBool("isPicking", false);

                if (isMoveDone)
                {
                    animator.SetBool("Idle", true);
                    animator.SetBool("isWalking", false);
                }
            }
        }
        if(!isCounterOccupied && pickProductList.Count > 0)
        {
            target = counter;
            MoveToTarget();
            ChangeState(CustomerState.WalkingToCounter, waitTime);
        }
        else if (pickProductList.Count == 0)
        {
            ChangeState(CustomerState.LeavingStore, waitTime);
        }
        
    }

    void WalkingToCounter()
    {
        animator.SetBool("isWalking", false);
        if (timer.IsFinished() && isMoveDone)
        {
            Debug.Log("함수 실행");  
            ChangeState(CustomerState.PlacingProduct, waitTime);
            animator.SetBool("isPicking", true);
        }
    }

    void PlacingProduct()
    {
        if (timer.IsFinished() && isMoveDone)
        {
            if (pickProductList.Count > 0)
            {
                Vector3 counterProductPos = CounterPivot.position;
                GameObject product = pickProductList[pickProductList.Count - 1];
                product.SetActive(true);
                product.GetComponent<BoxCollider>().enabled = true;
                product.transform.position = counterProductPos;
                product.transform.parent = null;
                product.tag = "CounterProduct";
                pickProductList.Remove(product);
                checkoutSystem.counterProduct.Add(product);
                animator.SetBool("Idle", true);
                animator.SetBool("isPicking", false);
            }
            if (pickProductList.Count == 0)
            {
                ChangeState(CustomerState.GivingMoney, waitTime);
                animator.SetBool("Idle", true);
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
            animator.SetBool("Idle", true);
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
                animator.SetBool("Idle", true);
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

    public void ShakeShelf()
    {
        int randomIndex = Random.Range(0, shelfList.Count);
        var randomShelf = shelfList[randomIndex].gameObject.transform.GetComponent<ShelfCtrl>();
        int randomCount = Random.Range(0, randomShelf.productList.Count);

        Vector3 forceDirection = new Vector3(2.0f, 2.0f, 2.0f);

        for (int i = 0; i < randomCount; i++)
        {
            randomShelf.GetComponent<BoxCollider>().enabled = false;
            GameObject shelfObj = randomShelf.productList.Pop();
            shelfObj.transform.parent = null;
            shelfObj.GetComponent<BoxCollider>().enabled = true;
            var shelfObjRb = shelfObj.GetComponent<Rigidbody>();
            shelfObjRb.isKinematic = false;
            shelfObjRb.AddForce(forceDirection * power);
        }
        randomShelf.GetComponent<BoxCollider>().enabled = true;
    }
}
