using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCustomer : MonoBehaviour
{
    public GameObject customerPrefab;
    public float spawnRateMin = 5f;
    public float spawnRateMax = 10f;

    private float spawnRate;
    private float spawntime = 0f;

    void Start()
    {
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
    }
    void Update()
    {
        spawntime += Time.deltaTime;

        if (spawntime >= spawnRate)
        {
            spawntime = 0f;
            GameObject customer = Instantiate(customerPrefab);
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }
}
