using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Meta.WitAi;
using UnityEngine;

public class playerGiveDamage : MonoBehaviour
{
    private bool candamage = true;
    public PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        candamage = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if(!candamage) return;
            pc.givedamage();
            ddamagecast();

            // if (other.gameObject.GetComponent<EnemyController>().isAttacking)
            // {
            //     pc.givedamage();
            //     ddamagecast();
            // }
        }
    }


    async UniTask ddamagecast()
    {
        candamage = false;
        await UniTask.Delay(2000);
        candamage = true;
    }
    
}
