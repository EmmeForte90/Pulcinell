using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [Header("Tempo di esplosione")]
    [SerializeField] public float lifeTime;

    [Header("Se è un esplosione")]
    [SerializeField] public bool isExplosion;
    [SerializeField] public float intensity;
    [SerializeField] public float time;







    void Update()
    {
        Destroy(gameObject, lifeTime);
        if(isExplosion)
        {
            CinemachineShake.instance.ShakeCamera(intensity, time);
        }
    }
}
