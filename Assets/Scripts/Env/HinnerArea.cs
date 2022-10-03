using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinnerArea : MonoBehaviour
{
    [SerializeField] GameObject Area;

void Awake()
{
    //Area = GetComponent<GameObject>();
}

private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            Area.SetActive(true);        
        }
    }
private void OnTriggerEnter2D(Collider2D other)
{

    if (other.gameObject.tag == "Player")
        {
            Area.SetActive(false);        
        }
}
    

}
