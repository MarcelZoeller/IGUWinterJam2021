using System;
using Animancer;
using UnityEngine;

public sealed class PlayAnimation : MonoBehaviour
{
    [SerializeField] private AnimancerComponent animancer;
    [SerializeField] public AnimationClip clip1, clip2;

    private void OnEnable()
    {
        //animancer.Play(clip);

        // You can manipulate the animation using the returned AnimancerState:
        //var state = animancer.Play(clip);
        //state.Speed = ...                  // See the Fine Control examples.
        //state.Time = ...                   // See the Fine Control examples.
        //state.NormalizedTime = ...         // See the Fine Control examples.
       // state.Events.OnEnd = ...           // See End Events.

        // If the animation was already playing, it will continue from the current time.
        // So to force it to play from the beginning you can just reset the Time:
        PlayAnim(animancer, clip1);
    }

    public void PlayExitAnim()
    {
        PlayAnim(animancer, clip2);
        Debug.Log("got it");
    }

    void PlayAnim(AnimancerComponent animancerTemp, AnimationClip clip)
    {
        var state = animancerTemp.Play(clip);
        state.Time = 0;
    }
}