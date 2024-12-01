using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingCat : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float destroyRange;

    private int rotatingDirection;

    private void Start()
    {
        rotatingDirection =
            Random.Range(0f, 1f) >= 0.5f
            ? 1
            : -1;
    }

    private void Update()
    {
        transform.Rotate(rotationSpeed * rotatingDirection * Time.deltaTime * Vector3.forward);

        if(transform.position.magnitude >= destroyRange)
        {
            Destroy(gameObject);
        }
    }
}
