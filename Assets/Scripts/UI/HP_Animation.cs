using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine.Unity;
using Spine;

public class HP_Animation : MonoBehaviour
{
    private SkeletonGraphic skelGraph;
    public AnimationReferenceAsset full;
    public AnimationReferenceAsset broke;

    private void Awake()
    {
        skelGraph = this.GetComponent<SkeletonGraphic>();
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skelGraph.AnimationState.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public void restoreHP()
    {
        SetAnimation(full, false, 1f);
    }

    public void removeHP()
    {
        SetAnimation(broke, false, 1f);
    }
}