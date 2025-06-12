using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarpoonCollisionHandler : MonoBehaviour
{
    public GameObject hookedFish = null; // The fish that the harpoon hooks
 
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public bool HasHookedFish()
    {
        return hookedFish != null;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("something caught "+ collision.name);
        if (collision.CompareTag("Fish") && hookedFish == null) // Make sure only one fish is hooked
        {
            hookedFish = collision.gameObject; // Store the fish reference
            hookedFish.GetComponent<Collider2D>().enabled = false; // Disable the fish collider to avoid further collisions
        }
        if (collision.CompareTag("Dynamite") && hookedFish == null) {
            hookedFish = collision.gameObject;
           
            hookedFish.GetComponent<Collider2D>().enabled = false; // Disable the fish collider to avoid further collisions
        }

        if (collision.CompareTag("Mine") && hookedFish == null)
        {
            hookedFish = collision.gameObject;

            hookedFish.GetComponent<Collider2D>().enabled = false; 
        }

        if (collision.CompareTag("OneUp") && hookedFish == null)
        {
            hookedFish = collision.gameObject;

            hookedFish.GetComponent<Collider2D>().enabled = false; 
        }

        if (collision.CompareTag("X2") && hookedFish == null)
        {
            hookedFish = collision.gameObject;
            ScoreManager.Instance.ApplyScoreMultiplier(2);

            hookedFish.GetComponent<Collider2D>().enabled = false;
        }

        if (collision.CompareTag("X4") && hookedFish == null)
        {
            hookedFish = collision.gameObject;
            ScoreManager.Instance.ApplyScoreMultiplier(4);

            hookedFish.GetComponent<Collider2D>().enabled = false;
        }



    }
}
