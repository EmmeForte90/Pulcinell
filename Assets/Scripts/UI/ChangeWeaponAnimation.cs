using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine.Unity;
using Spine;

public class ChangeWeaponAnimation : MonoBehaviour
{
    Skin characterSkin;
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

    public void StayWeapon()
    {
        SetAnimation(Normal, false, 1f);
    }

    public void ChangeWeapon()
    {
        StartCoroutine(SetAnimationChange());        
    }
public IEnumerator SetAnimationChange()
    {
        SetAnimation(Change, false, 1f);

        yield return new WaitForSeconds(1f);

        SetAnimation(Normal, true, 1f);
    }

    
}
