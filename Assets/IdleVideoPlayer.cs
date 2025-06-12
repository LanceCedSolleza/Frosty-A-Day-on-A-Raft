using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IdleVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer; 
    public RawImage rawImage; 
    public float idleTime = 30f; 

    private float timer = 0f;
    private bool isIdle = false;

    public GameObject titleAudio;

    private void Start()
    {
        if (videoPlayer != null)
        {
            rawImage.gameObject.SetActive(false);
            videoPlayer.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.anyKey || Input.GetMouseButton(0))
        {
            // Reset the timer if any input is detected
            timer = 0f;

            if (isIdle)
            {
                titleAudio.SetActive(true);
                StopIdleVideo();
            }
        }
        else
        {
            // Increment the idle timer
            timer += Time.deltaTime;

            if (timer >= idleTime && !isIdle)
            {
                titleAudio.SetActive(false);
                PlayIdleVideo();
            }
        }
    }

    private void PlayIdleVideo()
    {
        isIdle = true;

        // Show and play the video
        if (videoPlayer != null)
        {
            rawImage.gameObject.SetActive(true);
            videoPlayer.gameObject.SetActive(true);
            videoPlayer.Play();
        }
    }

    private void StopIdleVideo()
    {
        isIdle = false;

        // Stop the video
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            rawImage.gameObject.SetActive(false);
            videoPlayer.gameObject.SetActive(false);
        }
    }
}
