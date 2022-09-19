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

    [Header("parametri d'attacco")]
    public Transform Attacco;
    public LayerMask playerlayer;

    [Header ("Morte")]
    public GameObject DIE;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        HP = MaxHP;
        HPBarra.SethmaxHP(MaxHP);
    }

    #region Danno
    public void Damage()
    {
        anim.SetTrigger("isHurt");
        hurtEnemy();
        HP--;
        HPBarra.SethHP(HP);
        if (HP <= 0)
        {
           Instantiate(DIE, enemy.transform.position, enemy.transform.rotation);
           Destroy(enemy.gameObject);
        }

    }
    IEnumerator hurtEnemy()
    {
        waitCount = 1;
        yield return new WaitForSeconds(0.5f);
        waitCount = 0;
        hit = false;
    }
    #endregion





}







