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
    [SerializeField]
    public EHealtBar HPBarra;
    [SerializeField]
    public bool bigEnm;
    [SerializeField]
    public bool gigaFat;
    [SerializeField]
    public bool rider;
    [SerializeField]
    public bool TallE;
    [SerializeField]
    public bool GunEnemy;

    [Header("Fisica")]
    [SerializeField]
    protected Rigidbody2D RB;
    [SerializeField ]
    public float bounceForce = 20f;
    [SerializeField ]
    public Transform Enemy;


    [Header("Tempo di movimento")]
    [SerializeField]
    public float moveSpeed;
    [SerializeField]
    public float moveTime, waitTime;
    [SerializeField]
    public float moveCount, waitCount;
    [SerializeField]
    protected Animator anim;
    [SerializeField]
    protected bool hit = false;
    protected bool isDead = false;

    [Header("parametri d'attacco")]
    public Transform Attacco;
    public LayerMask playerlayer;

    [Header("Shooting")]
    [SerializeField]
    public GameObject bullet;
    [SerializeField]
    public Transform firePoint;
    [SerializeField]
    public float timeBetweenShots;
    [SerializeField]
    public float shotCounter;
    [SerializeField]
    public float FUOCO;

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
        HP = MaxHP;
        //HPBarra.SethmaxHP(MaxHP);
    }

    #region Danno
    public void Damage()
    {
        hurtEnemy();
        HP--;
        //HPBarra.SethHP(HP);
        if (HP <= 0)
        {
        anim.SetBool("isDead", isDead);
        Instantiate(DIE, enemy.transform.position, enemy.transform.rotation);
        Destroy(gameObject);
        }

    }
    IEnumerator hurtEnemy()
    {
        anim.SetTrigger("isHurt");
        RB.AddForce(transform.up * bounceForce);        
        waitCount = 1;
        yield return new WaitForSeconds(0.5f);
        waitCount = 0;
        //RB.velocity = new Vector2(moveSpeed, RB.velocity.y);
        hit = false;
    }
    #endregion





}







