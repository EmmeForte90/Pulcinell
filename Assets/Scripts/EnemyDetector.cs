using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [Header("Rilevatore")]
    [SerializeField]
    public Transform Detector;
    AIEnemyDefault Enemies;

    void Start()
    {
        Enemies = FindObjectOfType<AIEnemyDefault>();
    }



    private void OnTriggerStay2D(Collider2D other)
    {
        //waitaftershot();
        if (other.tag == "Player")
        {
        FindObjectOfType<AIEnemyDefault>().Shoot();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            FindObjectOfType<AIEnemyDefault>().StopAttack();
        }
    }
}
