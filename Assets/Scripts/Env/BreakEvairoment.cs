using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakEvairoment : MonoBehaviour
{

    public static BreakEvairoment instance;

    public GameObject Box;
    public GameObject Break;
    [Range(0, 100)] public float chanceToDrop;
    public GameObject collectible;
    private Animator anim;


    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update



    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Dealdamage()
    {

        anim.SetTrigger("Destroy");
        

       
            Instantiate(Break, Box.transform.position, Box.transform.rotation);

            Destroy(Box.gameObject);


        



    }


}
