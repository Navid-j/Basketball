using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour
{

    public float speed = 4f;
    public float maxY = 6f;
    public float minY = -6f;

    private int direction = 1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime * direction);

        if (transform.position.y >= maxY)
            direction = -1;
        else if (transform.position.y <= minY)
            direction = 1;

    }
}
