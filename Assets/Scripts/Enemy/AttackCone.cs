using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCone : MonoBehaviour
{
    public Torretta AITor;

    public bool isLeft = false;

     void Awake()
    {
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //quando il player si trova nel cono di visione della torretta, attacca
        if(other.CompareTag("Player"))
        { if (isLeft)

            { AITor.Attack(false); }
        else
            { AITor.Attack(true);  }
                    
                    }
    }

}
