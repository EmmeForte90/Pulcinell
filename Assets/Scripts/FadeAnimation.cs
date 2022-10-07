using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity.AttachmentTools;
using Spine.Unity;
using Spine;

public class FadeAnimation : MonoBehaviour
{
    public static FadeAnimation instance;
    private SkeletonGraphic skelGraph;
    [SerializeField] public AnimationReferenceAsset FadeIn;
    [SerializeField] public AnimationReferenceAsset FadeOut;

    private void Awake()
    {
        instance = this;
        skelGraph = this.GetComponent<SkeletonGraphic>();
    }

    public void SetAnimation(AnimationReferenceAsset animation, bool loop, float timeScale)
    {
        skelGraph.AnimationState.SetAnimation(0, animation, loop).TimeScale = timeScale;
    }

    public void OnFadeIn()
    {
        SetAnimation(FadeIn, false, 1f);
    }

    public void OnFadeOut()
    {
        SetAnimation(FadeOut, false, 1f);
    }
}
