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
    [SerializeField] private GameObject Zombi;
    public float span = 10f;
    private float currentTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime > span){
            spawn();
            currentTime = 0f;
        }
    }
    
    void spawn()
    {
        GameObject zombi = Instantiate(Zombi, sponePoint[Random.Range(0, sponePoint.Length)].position, Quaternion.identity);
        zombi.GetComponent<EnemyController>().target = Player.transform;
        zombi.GetComponent<EnemyController>().pc = pc;
    }
}
