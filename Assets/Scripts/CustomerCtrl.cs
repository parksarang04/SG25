using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    Idle,
    WalkingToShelf,
    PickingItem,
    //WaitCounter,
    WalkingToCounter,
    PlacingItem,
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
