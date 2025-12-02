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
    Vector3 initialScale;

    void Awake()
    {
        initialScale = transform.localScale;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        // monter vers le haut
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        // scale
        float t = timer / lifetime;
        transform.localScale = initialScale * Mathf.Lerp(startScale, endScale, t);

        // fin de vie
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}