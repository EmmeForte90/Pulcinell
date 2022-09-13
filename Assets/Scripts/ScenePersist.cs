using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
   void Awake()
    {
        int numScenePersists = FindObjectsOfType<ScenePersist>().Length;
        //Variabile per la lunghezza del valore
        if (numScenePersists > 1)
        //Se è maggiore di 1
        {
            Destroy(gameObject);
            //Quest'oggetto viene distrutto
        }
        else //Altrimenti
        {
            DontDestroyOnLoad(gameObject);
            //Lo preserva
        }
    }
    public void ResetScenePersist()
    {
        Destroy(gameObject);
        //Distrugge quest'oggetto
    }
}
