using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float minSpeed;
    [SerializeField] private float oscillationSpeed;
    [SerializeField] private float range;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color highlightedColor;

    private int direction = 1;
    private BulletSpawner spawner;

    private float t;
    private float startX;

    private void Start()
    {
        startX = transform.localPosition.x;
    }
    private void Update()
    {
        t += oscillationSpeed * Time.deltaTime * spawner.SpeedMultiplier;

        transform.localPosition = new Vector2(
            startX + range * Mathf.Sin(t) * direction, 
            transform.localPosition.y + (minSpeed + speed * spawner.SpeedMultiplier) * Time.deltaTime
        );

        Color color = Color.Lerp(normalColor, highlightedColor, spawner.RangedValue);
        spriteRenderer.color = color;

        trailRenderer.startColor = color;
        trailRenderer.endColor = new Color(color.r, color.g, color.b, Mathf.Clamp01(color.a - 0.2f));
    }

    public void AssignSpawner(BulletSpawner bulletSpawner)
    {
        spawner = bulletSpawner;
    }

    public void SetDirection(int direction)
    {
        this.direction = direction;
    }
}
