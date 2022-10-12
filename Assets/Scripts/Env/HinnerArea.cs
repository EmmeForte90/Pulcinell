using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinnerArea : MonoBehaviour
{
    [SerializeField] GameObject Area;
    [SerializeField] GameObject Black1;
    [SerializeField] GameObject Black2;
    [SerializeField] GameObject Black3;
    [SerializeField] GameObject Black4;





void Awake()
{
    //Area = GetComponent<GameObject>();
}

private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Player")
        {
            Area.SetActive(true);
            Black1.SetActive(false);  
            Black2.SetActive(false);
            Black3.SetActive(false);  
            Black4.SetActive(false);      
      
        
        }
    }
private void OnTriggerEnter2D(Collider2D other)
{

    if (other.gameObject.tag == "Player")
        {
            Area.SetActive(false);  
            Black1.SetActive(true);  
            Black2.SetActive(true);
            Black3.SetActive(true);  
            Black4.SetActive(true);        
        }
}
    

}
