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
    //[SerializeField]
    //public EHealtBar HPBarra;
    /*[SerializeField]
    public bool NormalEnemy;
    [SerializeField]
    public bool bigEnm;
    [SerializeField]
    public bool gigaFat;
    [SerializeField]
    public bool rider;
    [SerializeField]
    public bool TallE;
    [SerializeField]
    public bool GunEnemy;*/

    [Header("Fisica")]
    [SerializeField]
    protected Rigidbody2D RB;
    [SerializeField ]
    public float bounceForce = 20f;
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
    protected bool PunchNow = false;


    [Header ("Morte")]
    public GameObject DIE;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        enDefault = GetComponent<AIEnemyDefault>();
        enGun = GetComponent<AIEnemyGun>();
        HP = MaxHP;
        //HPBarra.SethmaxHP(MaxHP);
    }

#region Morte

public void Hurt()
{
    StartCoroutine(HitEnemy());
}

// Cooldown dell'attacco
public IEnumerator HitEnemy()
    {
        //Attacco.gameObject.SetActive(false);
        moveCount = 0;
        //Il nemico si ferma 
        anim.SetTrigger("isHurt");
        RB.AddForce(transform.up * bounceForce);
        yield return new WaitForSeconds(0.5f);
        //Si ferma per mezzo secondo
        moveCount = 1;
        //Poi riparte
        isAttack = false;
        //Attacco.gameObject.SetActive(true);

    }

    public void Damage()
    {
        //HitEnemy();
        HP--;
        //HPBarra.SethHP(HP);
        if (HP <= 0)
        {
        anim.SetBool("isDead", isDead);
        Instantiate(DIE, enemy.transform.position, enemy.transform.rotation);
        Destroy(gameObject);
        }

    }

    /*IEnumerator hurtEnemy()
    {
        anim.SetTrigger("isHurt");
        RB.AddForce(transform.up * bounceForce);        
        waitCount = 1;
        yield return new WaitForSeconds(0.5f);
        waitCount = 0;
        //RB.velocity = new Vector2(moveSpeed, RB.velocity.y);
        hit = false;
    }*/

    #endregion

}







