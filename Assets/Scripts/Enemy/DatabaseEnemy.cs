using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatabaseEnemy : MonoBehaviour

{
    public static DatabaseEnemy instance;

    [Header("Sistema Di HP")]
    [SerializeField]
    public int HP;
    [SerializeField]
    public int MaxHP;
    [SerializeField]
    public GameObject enemy;
    
    [Header("Fisica")]
    [SerializeField]
    protected Rigidbody2D RB;
    [SerializeField ]
    public float bounceForce = 20f;
    [SerializeField] 
    public float timeHurt = 0.5f;
    [SerializeField ]
    public Transform Enemy;
    AIEnemyGun enGun;
    AIEnemyDefault enDefault;

    [Header("Tempo di movimento")]
    [SerializeField]
    public float moveSpeed;
     [SerializeField]
    public float runSpeed;
    [SerializeField]
    public float moveTime, waitTime;
    [SerializeField]
    public float moveCount, waitCount;
    [SerializeField]
    protected Animator anim;
    protected bool hit = false;
    protected bool isDead = false;
    protected bool movingRight = true;
    protected bool isAttack = false;

    [Header ("Morte")]
    public GameObject DIE;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        enDefault = GetComponent<AIEnemyDefault>();
        enGun = GetComponent<AIEnemyGun>();
        HP = MaxHP;
        //HPBarra.SethmaxHP(MaxHP);
    }

#region Danno e Morte

public void Hurt()
{
    StartCoroutine(HitEnemy());
}

// Cooldown dell'attacco
public IEnumerator HitEnemy()
    {
        moveCount = 0;
        //Il nemico si ferma 
        anim.SetTrigger("isHurt");
        AudioManager.instance.PlaySFX(5);
        RB.AddForce(transform.up * bounceForce);
        yield return new WaitForSeconds(0.5f);
        //Si ferma per mezzo secondo
        moveCount = 1;
        //Poi riparte
        isAttack = false;
    }

    public void Damage()
    {
        HP--;
        //HPBarra.SethHP(HP);
        if (HP <= 0)
        {
        anim.SetBool("isDead", isDead);
        Instantiate(DIE, enemy.transform.position, enemy.transform.rotation);
        Destroy(gameObject);
        }

    }

    #endregion

}







