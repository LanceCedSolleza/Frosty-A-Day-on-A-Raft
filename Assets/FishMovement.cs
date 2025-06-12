using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f;
    public float swimAmplitude = 0.5f; // Height of the swimming motion
    public float swimFrequency = 2f;
    private Vector2 direction;

    public float screenOut;

    private float initialY;

    void Start()
    {
        // Save the initial Y position of the fish
        initialY = transform.position.y;
    }

    public void SetDirection(Vector2 moveDirection)
    {
        direction = moveDirection;

        // Flip sprite based on direction while preserving scale
        if (direction == Vector2.right)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (direction == Vector2.left)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }


    void Update()
    {
        Vector3 horizontalMovement = direction * speed * Time.deltaTime;

        float verticalOffset = Mathf.Sin(Time.time * swimFrequency) * swimAmplitude;

        transform.position += horizontalMovement + new Vector3(0, verticalOffset, 0) * Time.deltaTime;


        // Despawn when off-screen
        if (Mathf.Abs(transform.position.x) > Camera.main.orthographicSize * Camera.main.aspect + screenOut)
        {
            Destroy(gameObject);
        }
    }
}
