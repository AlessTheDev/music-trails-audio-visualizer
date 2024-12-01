using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    [SerializeField] private float minSpawnX;
    [SerializeField] private float maxSpawnX;

    [SerializeField] private float yTop;
    [SerializeField] private float yBottom;

    [SerializeField] private float minForce;
    [SerializeField] private float maxForce;

    [SerializeField] private Rigidbody2D cat;

    public void SpawnCat()
    {
        bool spawnTop = Random.Range(0f, 1f) >= 0.5f;
        Rigidbody2D cat =
            Instantiate(
                this.cat, 
                new Vector2(
                    Random.Range(minSpawnX, maxSpawnX),
                    spawnTop ? yTop : yBottom
                ),
                Quaternion.identity
            );

        float force = Random.Range(minForce, maxForce);
        if (spawnTop)
        {
            force *= -1;
            cat.gravityScale *= -1;
        }

        cat.AddForce(new Vector2(0, force));
    }
}
