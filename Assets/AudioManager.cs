using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource sfx;
    public AudioClip frostyDie;
    public AudioClip caughtFish;
    public AudioClip power;
    public AudioClip winKraken;
    public AudioClip loseKraken;
    public AudioClip spawnKraken;
    public AudioClip nextScene;
    public AudioClip strugglesfx;
    

    public AudioSource bg;
    public AudioClip morningBG;
    public AudioClip nightBG;
    public AudioClip sunsetBG;
    public AudioClip krakenBG;

    public void FrostyDie() {
        sfx.clip = frostyDie;
        sfx.Play();
    }

    public void CaughtFish()
    {
        sfx.clip = caughtFish;
        sfx.Play();
    }

    public void PowerUp()
    {
        sfx.clip = power;
        sfx.Play();
    }

    public void KrakenWin()
    {
        sfx.clip = winKraken;
        sfx.Play();
    }

    public void KrakenLose()
    {
        sfx.clip = loseKraken;
        sfx.Play();
    }

    public void KrakenSpawn()
    {
        sfx.clip = spawnKraken;
        sfx.Play();
    }

    public void PlayNextScene()
    {
        sfx.clip = nextScene;
        sfx.Play();
    }

    public void PlayStruggle()
    {
        sfx.clip = strugglesfx;
        sfx.Play();
    }



    //BACKGROUND
    public void PlayMorningBG() {
        bg.clip = morningBG;
        bg.Play();
    }

    public void PlayNightBG()
    {
        bg.clip = nightBG;
        bg.Play();
    }

    public void PlaySunsetBG()
    {
        bg.clip = sunsetBG;
        bg.Play();
    }


    public void PlayKrakenBG()
    {
        bg.clip = krakenBG;
        bg.Play();
    }



}
