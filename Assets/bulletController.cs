using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class bulletController : MonoBehaviour
{
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public float magicForce = 10f;
    [SerializeField] private bool canPen=false;

    [SerializeField] private int magicDamage = 40;
    // Start is called before the first frame update
    void Start()
    {
        bulletTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter (Collider co)
    {
        if(co.gameObject.tag=="Player") return;
        if(hitPrefab != null)
        {
            
            var hitpos = co.ClosestPointOnBounds(this.transform.position);
            Vector3 boundVec = this.transform.position - hitpos;
            var hitVFX = Instantiate(hitPrefab,hitpos, Quaternion.LookRotation(boundVec.normalized));
            var psHit = hitVFX.GetComponent<ParticleSystem>();
            if (co.gameObject.tag == "Enemy")
            {
                co.gameObject.GetComponent<IDamagable>().AddDamage(magicDamage);
            }
            if (psHit != null) 
            {
                Destroy(hitVFX, psHit.main.duration);
            }
            else
            {
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitVFX, psChild.main.duration);
            }
        }

        if (!canPen)
        {
            Destroy(gameObject);
        }
       
    }

    public async UniTask bulletTimer()
    {
        await UniTask.Delay(10000);
        Destroy(gameObject);
    }
}
