using UnityEngine;

public class PlayerHealthController : MonoBehaviour

{
    public static PlayerHealthController instance;

[Header("Quantità")]
    [SerializeField] public int currentHealth, maxHealth;
    [SerializeField] public int currentBullet, maxBullet;

[Header("Invincibilità")]

    [SerializeField] public float invincibleLength;
    private float invincibleCounter;

[Header("Morte")]
    [SerializeField]public GameObject deathEffect;

    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentBullet = maxBullet;

    }

    // Update is called once per frame
    void Update()
    {
        if(invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;

            if(invincibleCounter <= 0) 
            {
                //theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
            }
        }
    }

    public void Dealdamage()
    {
        if(invincibleCounter <= 0)
        {
            currentHealth--;

        if(currentHealth <= 0)
        {
            currentHealth = 0;

                Instantiate(deathEffect, transform.position, transform.rotation);
                //PlayerController.instance.gameObject.SetActive(false);
                //PlayerController.instance.stopInput = true;
                FindObjectOfType<GameSession>().ProcessPlayerDeath();


            } 
        else
        {
            invincibleCounter = invincibleLength;
                //theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
                //PlayerController.instance.knockBack();
                //AudioManager.instance.PlaySFX(9);
            }
            UIController.instance.UpdateHealthDisplay();
    }
}

public void HealPlayer()
    {
        //currentHealth = maxHealth;
        currentHealth++;
    if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.instance.UpdateHealthDisplay();
    }

    public void RechargeBull()
    {
        //currentHealth = maxHealth;
        currentBullet++;
        if (currentBullet > maxBullet)
        {
            currentBullet = maxBullet;
        }
        UIController.instance.UpdateBulletDisplay();
    }

}
