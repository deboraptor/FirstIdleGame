using UnityEngine;

public class FlowerClicker : MonoBehaviour
{
    [Header("Références")]
    public IdleGameManager gameManager;

    [Header("Shop / Prix")]
    public float price = 5f;        // prix de cette fleur dans le shop

    [Header("Gain au clic")]
    public float coinsPerClick = 1f;

    [Header("Gain automatique")]
    public bool hasIdle = true;     // <-- NOUVEAU : est-ce que cette fleur a un gain passif ?
    public float idleAmount = 0f;   // combien on gagne à chaque tick
    public float idleInterval = 1f; // toutes les combien de secondes

    private float idleTimer = 0f;

    void Awake()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<IdleGameManager>();

        // Si cette fleur n'est PAS censée avoir d'idle, on ne calcule rien
        if (!hasIdle)
        {
            idleAmount = 0f;
            return;
        }

        // Si on a laissé idleAmount à 0 dans l'Inspector,
        // on le calcule à partir du prix (10% du prix par seconde)
        if (idleAmount == 0f)
        {
            float percentPerSecond = 0.1f; // 10%
            idleInterval = 1f;
            idleAmount = price * percentPerSecond;
        }
    }

    void Update()
    {
        // Si pas d'idle sur cette fleur, on ne fait rien ici
        if (!hasIdle)
            return;

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleInterval)
        {
            idleTimer -= idleInterval;

            if (gameManager != null)
            {
                gameManager.AddCoins(idleAmount);
            }
        }
    }

    void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.AddCoins(coinsPerClick);
        }
    }
}