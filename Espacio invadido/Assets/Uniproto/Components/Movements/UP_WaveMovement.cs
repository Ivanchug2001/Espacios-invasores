using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UP_WaveMovement : MonoBehaviour {

    public enum MovementType
    {
        Horizontal, Vertical
    }

    [SerializeField] MovementType movementType = MovementType.Horizontal;
    [SerializeField] float amplitude = 1;
    [SerializeField] float frequency = 1;

    float randomTime;
    float lastIncrement;
    Rigidbody2D cmpRigidbody;

    [SerializeField] bool randomizeInitialOffset = false;
    float time;

    float initialX;

    void Awake()
    {
        randomTime = randomizeInitialOffset ? Random.Range(-1.0f, 1.0f) * Mathf.PI : 0;

        cmpRigidbody = GetComponent<Rigidbody2D>();

        Vector3 position = transform.position;
        if(movementType == MovementType.Horizontal)
        {
            position.x += amplitude * Mathf.Sin(frequency * randomTime);
        }
        else
        {
            position.y += amplitude * Mathf.Sin(frequency * randomTime);
        }        
        transform.position = position;
    }

    void OnEnable()
    {
        
    }

    void FixedUpdate () {
        time += Time.deltaTime;

        Vector3 velocity = cmpRigidbody.velocity;
        if(movementType == MovementType.Horizontal)
        {
            velocity.x = CalculateOscilation();
        }
        else
        {
            velocity.y = CalculateOscilation();
        }
        cmpRigidbody.velocity = velocity;
    }

    float CalculateOscilation()
    {
        return frequency * amplitude * Mathf.Cos(frequency * (time + randomTime));
    }
}
