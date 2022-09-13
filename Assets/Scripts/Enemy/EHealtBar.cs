using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EHealtBar : MonoBehaviour
{
    public static EHealtBar instance;

    public Slider slider;
    public Color Low;
    public Color High;
    public Vector3 Offset;
    public Image fill;
    public Gradient gradient;
    public bool isBoss;
    public Image potrait;

    //Settaggio degli HP
    public void SethHP(int LP)
    {
        slider.value = LP;

        //fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    // Settaggio degli HP massimi
    public void SethmaxHP(int LP)
    {
        slider.maxValue = LP;
        slider.value = LP;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBoss)
        {
            //La barra HP segue il nemico
            slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + Offset);
        }
        else if(isBoss)
        {
            //La barra Hp del boss si attiva in basso alla schermata
            potrait.gameObject.SetActive(true);

        }
       
    }
}
