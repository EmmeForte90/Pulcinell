using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine.Unity;
using Spine;

public class ChangeWeaponAnimation : MonoBehaviour
{
    private SkeletonGraphic skelGraph;
    public AnimationReferenceAsset Normal;
    public AnimationReferenceAsset Change;

    private void Awake()
    {
        skelGraph = this.GetComponent<SkeletonGraphic>();
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skelGraph.AnimationState.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public void ChangeWeapon(int id)
    {
        StartCoroutine(SetAnimationChange()); 

        if(id == 0)
        {
            var skeleton = skelGraph.Skeleton;
            skeleton.SetSkin("Normal");   
        }else if(id == 1)
        {
            var skeleton = skelGraph.Skeleton;
            skeleton.SetSkin("Normal");   
        }
        else if(id == 2)
        {
            var skeleton = skelGraph.Skeleton;
            skeleton.SetSkin("Rapid");

        }else if(id == 3)
        {
            var skeleton = skelGraph.Skeleton;
            skeleton.SetSkin("Shotgun");

        }else if(id == 4)
        {
            var skeleton = skelGraph.Skeleton;
            skeleton.SetSkin("Bomb");

        }
       
    }


public IEnumerator SetAnimationChange()
    {
        SetAnimation(Change, false, 1f);

        yield return new WaitForSeconds(0.9f);

        SetAnimation(Normal, true, 1f);
    }

    
}
