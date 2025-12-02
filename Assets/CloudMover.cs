using UnityEngine;

public class CloudMover : MonoBehaviour
{
    [Header("Vitesse")]
    public float minSpeed = 0.3f;
    public float maxSpeed = 0.6f;

    [Header("Limites en X")]
    public float leftX = -15f;   // hors écran à gauche
    public float rightX = 15f;   // hors écran à droite

    float speed;

    void Start()
    {
        // vitesse aléatoire pour chaque nuage
        speed = Random.Range(minSpeed, maxSpeed);

        // petite variation de taille
        float s = Random.Range(0.8f, 1.4f);
        transform.localScale = new Vector3(s, s, 1f);
    }

    void Update()
    {
        // avancer vers la droite
        transform.position += Vector3.right * speed * Time.deltaTime;

        // quand il sort à droite, on le remet à gauche
        if (transform.position.x > rightX)
        {
            float y = transform.position.y; // tu peux random ici si tu veux
            transform.position = new Vector3(leftX, y, transform.position.z);
        }
    }
}