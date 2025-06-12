using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehavior : MonoBehaviour
{
    public Animator animator;
    public Animator krakenAnimator;

    void Start()
    {
    }

    public void PlayScoreGainedAnimation()
    {

        if (animator == null)
        {
            Debug.LogError("Animator is not set!");
            return;
        }

        Debug.Log("play animation");
        animator.SetTrigger("Scored");
        Debug.Log("animation played");

    }

    public void FrostyDied()
    {
        animator.SetTrigger("Died");
    }

    public void FrostyStruggle() {
        animator.SetTrigger("Struggle");
    }

    public void PlayKrakenHurt()
    {
        krakenAnimator.SetTrigger("KrakenHurt");
    }
    public void PlayKrakenDie()
    {
        krakenAnimator.SetTrigger("KrakenDie");
    }
}
