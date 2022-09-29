using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spine;
using Spine.Unity;

public class Baloon : MonoBehaviour
{
	[SerializeField] private string[] testi;
	private int index_posizioni;
	public TMPro.TextMeshProUGUI testo_cartello;
	private bool bool_apribile=false;
	private bool bool_in_scrittura=false;
	public GameObject cartello_toon;
        
	
	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player")
        {
			//anim.SetTrigger("appear)");
            cartello_toon.SetActive(true);
			bool_apribile = true;
            attiva_testo(index_posizioni);
            //FindObjectOfType<anmBal>().Appear();	
            }
	}
	
	private void OnTriggerExit2D(Collider2D other){
		if (other.tag == "Player")
        {
            //anim.SetTrigger("disappear)");
            //FindObjectOfType<anmBal>().Disappear();
             cartello_toon.SetActive(false);
			bool_apribile = false;
            
		}
	}

    IEnumerator scrivi_testo(string testo){
		testo_cartello.SetText("");
		string testo_temp="";
		foreach (char lettera in testo.ToCharArray()){
			testo_temp += lettera;
			testo_cartello.SetText(testo_temp);
			yield return null;
		}
		bool_in_scrittura=false;
	}
	
	private void attiva_testo(int posizione){
		bool_in_scrittura=true;
		testo_cartello.SetText(testi[posizione]);
		StopAllCoroutines();
		StartCoroutine(scrivi_testo(testi[posizione]));
		index_posizioni++;
	}
   
}
