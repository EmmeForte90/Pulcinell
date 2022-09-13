using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadeCallBack : MonoBehaviour
{
    private bool isFirstUpdate = true;

    private void Update()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate = false;
            Loader.LoaderCallBack();
        }
    }
}
