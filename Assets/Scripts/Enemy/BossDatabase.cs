using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDatabase : MonoBehaviour
{
    //Database dei boss, si associa al boss in questione per non compilare altri script

    [Header("Sistema Di HP")]
    [SerializeField]
    public int HP;
    [SerializeField]
    public int MaxHP;
    [SerializeField]
    public EHealtBar HPBarra;
    [SerializeField]
    public static BossDatabase instance;
    public GameObject defeatBoss;


    [Header("Movement")]
    [SerializeField]

    public float moveSpeed;
    public Transform leftPoint, rightPoint;
    protected bool moveRight;
    public GameObject mine;
    public Transform minePoint;
    public float timeBetweenMines;
    protected float mineCounter;
    //
    [Header("Shooting")]
    [SerializeField]

    public GameObject bullet;
    public Transform firePoint;
    public float timeBetweenShots;
    protected float shotCounter;
    //
    [Header("Hurt")]
    public float hurtTime;
    protected float hurtCounter;
    public GameObject hitbox;
    [SerializeField]

    public GameObject explosion, winPlatform;
    protected bool isDefeated;
    public float shotSpeedUp, mineSpeedUp;
    public float Delay;
    


   
    void Start()
    {
        HP = MaxHP;
        HPBarra.SethmaxHP(MaxHP);
    }

   
}
