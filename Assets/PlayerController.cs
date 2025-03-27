using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Oculus.Interaction.Input;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public OVRHand leftHand;
    [SerializeField] private OVRHand rightHnad;
    [SerializeField] private float waveThreshold = 0.8f; // 手を振る速度の閾値
    [SerializeField] private float stopThreshold = 0.1f; // 手が止まったとみなす閾値
    [SerializeField] private float stopTimeHold = 0.1f;
    [SerializeField] private float waveThersTime = 0.0f;
    [SerializeField] private float stoppingTime = 0.0f;
    [SerializeField] private float shotRate = 0.5f;
    [SerializeField] private GameObject[] magic;
    [SerializeField] private EnemySponer enemySponer;
    [SerializeField] private GameManager GM;
    public GameObject staf;
    private Vector3 previousPosition;
    private bool isWaving = false;
    private bool canShot = true;
    public Transform magicPoint;
    private bool[] magicShapes={false,false,false};
    // public PSMeshRendererUpdater psmru;
    int hp = 0;
    [SerializeField, Min(0)]
    int maxHp = 100;
    bool isDead = false;
    [SerializeField] private int score;
    public HPUIManager hpuim;
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    public GameObject DeadP;
    public int Hp
    {
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);
        }
        get
        {
            return hp;
        }
    }
    void Start()
    {
        hpuim.Initialize();
        DeadP.SetActive(false);
        isDead = false;
        score = 0;
        canShot = true;
        // psmru.IsActive = false;
        waveThersTime = 0.0f;
        Hp = maxHp;
    }

    public void Initialize()
    {
        hpuim.Initialize();
        DeadP.SetActive(false);
        isDead = false;
        score = 0;
        canShot = true;
        // psmru.IsActive = false;
        waveThersTime = 0.0f;
        Hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        if (hp <= 0)
        {
            Dead();
        }
        if (leftHand.IsTracked)
        {
            DetectWaveAndStop();
        }

        scoreText.text = score.ToString();
    }

    public void handGunShape()
    {
        magicShapes[1] = true;
    }
    public void unhandGunShape()
    {
        magicShapes[1] = false;
    }
    public void paaShape()
    {
        magicShapes[2] = true;
    }
    public void unpaaShape()
    {
        magicShapes[2] = false;
    }
    public void spellFire(int id)
    {

        GameObject magicObj = Instantiate(magic[id], magicPoint.position, Quaternion.LookRotation(staf.transform.up));
        magicObj.GetComponent<Rigidbody>().AddForce(staf.transform.up * magicObj.GetComponent<bulletController>().magicForce,ForceMode.Impulse);
        
    }
    

    
    private void DetectWaveAndStop()
    {
        Vector3 currentPosition = rightHnad.transform.position;
        Vector3 velocity = (currentPosition - previousPosition) / Time.deltaTime;
        previousPosition = currentPosition;
        // 手を振っているか確認
        if (velocity.magnitude > waveThreshold)
        {
            isWaving = true;
            // if (!psmru.IsActive)
            // {
            //     psmru.IsActive = true;
            //     psmru.UpdateMeshEffect();
            // }
            stoppingTime = 0.0f;
        }
        // 手が止まったか確認
        if (isWaving && velocity.magnitude < stopThreshold)
        {
            if (stoppingTime >= stopTimeHold)
            {
                isWaving = false;
                magicCast(waveThersTime);
                waveThersTime = 0.0f;
                // psmru.IsActive = false;
                // psmru.UpdateMeshEffect();
        
            }
            else
            {
                stoppingTime += Time.deltaTime;
            }
        }
        
        if (isWaving)
        {
            waveThersTime += Time.deltaTime;
        }
        // if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        // {
        //     magicCast(waveThersTime);
        // }
    }

    private void magicCast(float casttime)
    {
        if(!canShot) return;
        coolTime().AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        if (leftHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) > 0.8f)
        {
            spellFire(0);
            // 左手のジェスチャーに応じた処理を追加
        }
        else if (magicShapes[1])
        {
            spellFire(1);
        }
        else if (magicShapes[2] && casttime>2f)
        {
            spellFire(2);
        }
        
    }


    
    public void givedamage()
    {
        hpuim.HPViewer(10).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
    }

    public void geinScore(int value)
    {
        score += value;
    }

    public void Dead()
    {
        DeadP.SetActive(true);
        isDead = true;
        enemySponer.endSpawn();
        GM.EndGame();
    }

    async UniTask coolTime()
    {
        canShot = false;
        await UniTask.Delay(TimeSpan.FromSeconds(shotRate));
        canShot = true;
    }
    
}
