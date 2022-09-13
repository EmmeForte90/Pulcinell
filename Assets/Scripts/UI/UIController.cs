using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public static UIController instance;

    public Image Hp1, Hp2, Hp3;
    public Image Bull1, Bull2, Bull3;
    public Sprite BullFull, BullEmpty;

    public Sprite hpFull, hpEmpty;

    public Text pizzaText;

    public Image fadeScreen;
    public float fadeSpeed;
    private bool shouldFadeToBlack, shouldFadeFromeBack;


    public GameObject levelCompleteText;


    private void Awake()
    {
        instance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        UpdatePizzaCount();
        FadeFromBlack();

    }

    // Update is called once per frame
    void Update()
    {

        if (shouldFadeToBlack)
        {

            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 1f)
            {
                shouldFadeToBlack = false;

            }

        }

        if (shouldFadeFromeBack)
        {

            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
            {
                shouldFadeFromeBack = false;

            }


        }

    }

    public void UpdatePizzaCount()
    {
        pizzaText.text = LevelManager.instance.pizzaCollected.ToString();
    }

    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromeBack = false;
    }

    public void FadeFromBlack()
    {
        shouldFadeFromeBack = true;
        shouldFadeToBlack = false;

    }

    public void UpdateHealthDisplay()
    {

        switch (PlayerHealthController.instance.currentHealth)
        {

            case 3:
                Hp3.sprite = hpFull;
                Hp2.sprite = hpFull;
                Hp1.sprite = hpFull;

                break;

            case 2:
                Hp3.sprite = hpEmpty;
                Hp2.sprite = hpFull;
                Hp1.sprite = hpFull;

                break;

            case 1:
                Hp3.sprite = hpEmpty;
                Hp2.sprite = hpEmpty;
                Hp1.sprite = hpFull;

                break;

            case 0:
                Hp3.sprite = hpEmpty;
                Hp2.sprite = hpEmpty;
                Hp1.sprite = hpEmpty;

                break;

            default:
                Hp3.sprite = hpEmpty;
                Hp2.sprite = hpEmpty;
                Hp1.sprite = hpEmpty;

                break;
        }


    }
    //if(PlayerHealthController.instance.currentHealth == 3)
    //{


    //Hp3.sprite = hpFull;


    public void UpdateBulletDisplay()
    {
        switch (PlayerHealthController.instance.currentBullet)

        {

            case 3:
                Bull3.sprite = BullFull;
                Bull2.sprite = BullFull;
                Bull1.sprite = BullFull;

                break;

            case 2:
                Bull3.sprite = BullEmpty;
                Bull2.sprite = BullFull;
                Bull1.sprite = BullFull;

                break;

            case 1:
                Bull3.sprite = BullEmpty;
                Bull2.sprite = BullEmpty;
                Bull1.sprite = BullFull;

                break;

            case 0:
                Bull3.sprite = BullEmpty;
                Bull2.sprite = BullEmpty;
                Bull1.sprite = BullEmpty;

                break;

            default:
                Bull3.sprite = BullEmpty;
                Bull2.sprite = BullEmpty;
                Bull1.sprite = BullEmpty;

                break;

        }

        

    }
}
