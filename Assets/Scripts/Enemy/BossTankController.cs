using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTankController : BossDatabase
{

    public static new BossTankController instance;

    public enum bossStates {shooting, hurt, moving, endend};
    public bossStates currentStates;
    public bool inputStop;
    public Transform theBoss;
    
    public Animator anim;
    //
    



    // Start is called before the first frame update
    void Start()
    {
       
        currentStates = bossStates.shooting;
    }

    // Update is called once per frame
    void Update()
    {
        //Se il player è morto richiama la function in cui il boss smette di attaccare
        if (PlayerHealthController.instance.currentHealth <= 0)
        {

            StartCoroutine(Stopshooting());
        }

        //Se non è impostato lo stop dei vari input...

        if (!inputStop)
        {

            //...Il boss procede nelle sue funzioni

            #region Sparo
            switch (currentStates)
            {
                case bossStates.shooting:

                    shotCounter -= Time.deltaTime;

                    if (shotCounter <= 0)
                    {
                        shotCounter = timeBetweenShots;
                        //Ripetizione sparo


                        var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);

                        newBullet.transform.localScale = theBoss.localScale;

                    }


                    break;
#endregion

            #region Danno subito


                case bossStates.hurt:

                    if (hurtCounter > 0)
                    {


                        hurtCounter -= Time.deltaTime;

                        if (hurtCounter <= 0)
                        {
                            currentStates = bossStates.moving;

                            mineCounter = 0;

                            if (isDefeated)
                            {
                                theBoss.gameObject.SetActive(false);
                                Instantiate(explosion, theBoss.position, theBoss.rotation);

                                winPlatform.SetActive(true);
                                //AudioManager.instance.StopBossMusic();
                                currentStates = bossStates.endend;


                            }
                        }

                    }


                    break;
#endregion

            #region Movimento
                case bossStates.moving:

                    if (moveRight)
                    {
                        theBoss.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);

                        if (theBoss.position.x > rightPoint.position.x)
                        {
                            theBoss.localScale = new Vector3(2f, 2f, 1f);

                            moveRight = false;

                            EndMovemente();
                        }
                    }
                    else
                    {

                        theBoss.position -= new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);

                        if (theBoss.position.x < leftPoint.position.x)
                        {
                            theBoss.localScale = new Vector3(-2, 2f, 1f);

                            moveRight = true;

                            EndMovemente();

                        }



                    }

                    mineCounter -= Time.deltaTime;

                    if (mineCounter <= 0)
                    {
                        mineCounter = timeBetweenMines;

                        Instantiate(mine, minePoint.position, minePoint.rotation);
                    }

                    break;
            }
            #endregion


            //Calcolo danno, funziona solo nell'engine
#if UNITY_EDITOR


            if (Input.GetKeyDown(KeyCode.H))
            {
                TakeHit();

            }
#endif


        } 
    }

    //funzione del danno, Calcolo di tempo di animazione
    public void TakeHit()
    {
        currentStates = bossStates.hurt;
        hurtCounter = hurtTime;
        anim.SetTrigger("Hit");
        AudioManager.instance.PlaySFX(0);

        BossTankMine[] mines = FindObjectsOfType<BossTankMine>();

        //Spawn delle mine
        if(mines.Length > 0)
        {
            foreach(BossTankMine foundMine in mines)
            {
                foundMine.Explode();
            }
        }


        //Quando subisce il danno perde Hp e aggiorna la barraHP
        HP--;
        HPBarra.SethHP(HP);

        //Il boss muore
        if (HP <= 0)
        {
            isDefeated = true;
            Instantiate(defeatBoss, theBoss.position, theBoss.rotation);
            Destroy(gameObject);
        }

        //Funzione che aumenta la velocità di attacco quando supera una soglia di hp
        else if (HP <= 4)
        {
            timeBetweenShots /= shotSpeedUp;
            timeBetweenMines /= mineSpeedUp;

        }



    }

    //Fine dei movimenti
    public void EndMovemente()
    {
        currentStates = bossStates.shooting;

        shotCounter = 0f;

        anim.SetTrigger("StopMoving");

        hitbox.SetActive(true);
       

    }

    //Quando il player muore il boss si ferma il tempo che è settato con il Delay
    IEnumerator Stopshooting()
    {
        inputStop = true;
        yield return new WaitForSeconds(Delay);
        inputStop = false;            
            }
}
