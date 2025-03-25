using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class EnemyController : MonoBehaviour, IDamagable
{
    [SerializeField] private HPUIManager hpuiManager;
    [SerializeField]
    Animator animator = null;
    [SerializeField]
    CapsuleCollider capsuleCollider = null;
    [SerializeField, Min(0)]
    int maxHp = 3;
    [SerializeField]
    float deadWaitTime = 3;
    [SerializeField]
    UnityEngine.AI.NavMeshAgent navmeshAgent = null;
    [SerializeField]
    public  PlayerController pc = null;
    [SerializeField]
    public Transform target = null;
    // アニメーターのパラメーターのIDを取得（高速化のため）
    readonly int SpeedHash = Animator.StringToHash("Speed");
    readonly int AttackHash = Animator.StringToHash("Attack");
    readonly int DeadHash = Animator.StringToHash("Dead");
    bool isDead = false;
    int hp = 0;
    Transform thisTransform;
    // ★変更２
    // ★変更２
    [SerializeField]
    float chaseDistance = 5;
    [SerializeField]
    Collider attackCollider = null;
    [SerializeField]
    int attackPower = 10;
    [SerializeField]
    float attackTime = 0.5f;
    [SerializeField]
    float attackInterval = 2;
    [SerializeField]
    float attackDistance = 2;
    // ★変更２
    public bool isAttacking = false;
    WaitForSeconds attackWait;
    WaitForSeconds attackIntervalWait;
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
        thisTransform = transform; 
        attackWait = new WaitForSeconds(attackTime);
        attackIntervalWait = new WaitForSeconds(attackInterval);
        InitEnemy();  
    }
    void Update()
    {
        if (isDead)
        {
            return;
        }
        CheckDistance();
        Move();
        UpdateAnimator();
    }
    void InitEnemy()
    {
        Hp = maxHp;
    }
    // 被ダメージ処理
    public void AddDamage(int value)
    {
        hpuiManager.HPViewer(value).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
    }
    // 死亡時の処理
    public void Dead()
    {
        isDead = true;
        pc.geinScore(40);
        capsuleCollider.enabled = false;
        animator.SetBool(DeadHash, true);
        StartCoroutine(nameof(DeadTimer));
    }
    // 死亡してから数秒間待つ処理
    IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(deadWaitTime);
        Destroy(gameObject);
    }
    void Move()
    {
        navmeshAgent.SetDestination(target.position);
    }
    // アニメーターのアップデート処理
    void UpdateAnimator()
    {
        animator.SetFloat(SpeedHash, navmeshAgent.desiredVelocity.magnitude);
    }
    
    void CheckDistance()
    {
        // プレイヤーまでの距離（二乗された値）を取得
        // sqrMagnitudeは平方根の計算を行わないので高速。距離を比較するだけならそちらを使った方が良い
        float diff = (target.position - thisTransform.position).sqrMagnitude;
        // 距離を比較。比較対象も二乗するのを忘れずに
        if (diff < attackDistance * attackDistance)
        {
            if (!isAttacking)
            {
                StartCoroutine(nameof(Attack));
            }
        }
    }
    
    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger(AttackHash);
        attackCollider.enabled = true;
        yield return attackWait;
        attackCollider.enabled = false;
        yield return attackIntervalWait;
        isAttacking = false;
    }
    void StopAttack()
    {
        StopCoroutine(nameof(Attack));
        attackCollider.enabled = false;
        isAttacking = false;
    }

}
