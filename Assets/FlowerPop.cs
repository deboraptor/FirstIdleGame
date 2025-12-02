using UnityEngine;
using System.Collections;

public class FlowerPop : MonoBehaviour
{
    public float popScale = 1.15f;   // jusqu'où elle grossit
    public float popDuration = 0.12f;

    Vector3 baseScale;
    bool isPopping = false;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    public void PlayPop()
    {
        if (!isPopping)
            StartCoroutine(PopCo());
    }

    IEnumerator PopCo()
    {
        isPopping = true;

        float half = popDuration * 0.5f;
        float t = 0f;

        // montée : 1 → popScale
        while (t < half)
        {
            t += Time.deltaTime;
            float k = t / half;
            float s = Mathf.Lerp(1f, popScale, k);
            transform.localScale = baseScale * s;
            yield return null;
        }

        // descente : popScale → 1
        t = 0f;
        while (t < half)
        {
            t += Time.deltaTime;
            float k = t / half;
            float s = Mathf.Lerp(popScale, 1f, k);
            transform.localScale = baseScale * s;
            yield return null;
        }

        transform.localScale = baseScale;
        isPopping = false;
    }
}