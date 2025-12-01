using UnityEngine;
using TMPro;

public class FloatingCoin : MonoBehaviour
{
    public TextMeshPro text;     // optionnel : pour afficher +1
    public float floatSpeed = 1f;
    public float lifetime = 0.7f;
    public float startScale = 1f;
    public float endScale = 0.7f;

    float timer = 0f;
    SpriteRenderer[] spriteRenderers;

    public void Init(float amount)
    {
        if (text != null)
            text.text = "+" + amount;
    }

    void Awake()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // monter vers le haut
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // scale
        float t = timer / lifetime;
        transform.localScale = Vector3.one * Mathf.Lerp(startScale, endScale, t);

        // fade-out (alpha)
        float alpha = 1f - t;
        foreach (var sr in spriteRenderers)
        {
            var c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
        if (text != null)
        {
            var c = text.color;
            c.a = alpha;
            text.color = c;
        }

        // fin de vie
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}