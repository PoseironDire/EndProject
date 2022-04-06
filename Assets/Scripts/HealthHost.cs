using UnityEngine;
using System.Collections.Generic;

public class HealthHost : MonoBehaviour
{
    [SerializeField] GameObject damageText;
    Dictionary<SpriteRenderer, Color> originalColor = new Dictionary<SpriteRenderer, Color>();

    void Awake()
    {
        SpriteRenderer[] children = transform.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer renderer in children)
        {
            originalColor.Add(renderer, renderer.color);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            foreach (SpriteRenderer renderer in originalColor.Keys)
            {
                if (renderer.color != Color.red)
                    renderer.color = Color.red;
                else
                    renderer.color = Color.white;
            }
            DamageIndicator indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            indicator.SetDamageText(Random.Range(1, 100));
        }
    }

    public void Coloring()
    {
        foreach (SpriteRenderer renderer in originalColor.Keys)
        {
            renderer.color = Color.Lerp(renderer.color, originalColor[renderer], 0.1f);
        }
    }
}

