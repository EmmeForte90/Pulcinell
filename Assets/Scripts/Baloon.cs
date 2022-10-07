using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine;
using Spine.Unity;

public class Baloon : MonoBehaviour
{
	[SerializeField] private string[] texts;
	private int indPos;
	public TMPro.TextMeshProUGUI textSign;
	private bool isOpen=false;
	private bool textCont=false;
	public GameObject signToon;
        
	
	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player")
        {
			//anim.SetTrigger("appear)");
            signToon.SetActive(true);
			isOpen = true;
            attiva_testo(indPos);
            //FindObjectOfType<anmBal>().Appear();	
            }
	}
	
	private void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player")
        {
            //anim.SetTrigger("disappear)");
            //FindObjectOfType<anmBal>().Disappear();
             signToon.SetActive(false);
			isOpen = false;
            
		}
	}

    IEnumerator scrivi_testo(string testo){
		textSign.SetText("");
		string textTemp="";
		foreach (char letter in testo.ToCharArray()){
			textTemp += letter;
			textSign.SetText(textTemp);
			yield return null;
		}
		textCont=false;
	}
	
	private void attiva_testo(int position){
		textCont=true;
		textSign.SetText(texts[position]);
		StopAllCoroutines();
		StartCoroutine(scrivi_testo(texts[position]));
		indPos++;
	}
   
}
