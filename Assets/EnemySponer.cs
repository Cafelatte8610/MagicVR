using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySponer : MonoBehaviour
{
    [SerializeField] private GameObject Player;
    [SerializeField] private PlayerController pc;
    [SerializeField] private Transform[] sponePoint;
    [SerializeField] private Transform EnemyParent;
    [SerializeField] private GameObject Zombi;
    public float span = 10f;
    private float currentTime = 10f;
    private bool canSpawn = false;
    public GameObject StartUI;

    // Start is called before the first frame update
    void Start()
    {
        canSpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawn) return;
        currentTime += Time.deltaTime;

        if(currentTime > span){
            spawn();
            currentTime = 0f;
        }
    }
    
    void spawn()
    {
        GameObject zombi = Instantiate(Zombi, sponePoint[Random.Range(0, sponePoint.Length)].position, Quaternion.identity);
        zombi.transform.SetParent(EnemyParent);
        zombi.GetComponent<EnemyController>().target = Player.transform;
        zombi.GetComponent<EnemyController>().pc = pc;
    }

    public void startSpawn()
    {
        canSpawn = true;
        StartUI.SetActive(false);
    }
    
    public void endSpawn()
    {
        canSpawn = false;
    }
    
}
