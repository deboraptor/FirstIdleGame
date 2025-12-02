using UnityEngine;

public class FlowerClicker : MonoBehaviour
{
    [Header("Références")]
    public IdleGameManager gameManager;
    public ParticleSystem clickFx;   // FX posé sur le pétale

    [Header("Shop / Prix")]
    public float price = 5f;

    [Header("Gain au clic")]
    public float coinsPerClick = 1f;

    [Header("Gain automatique")]
    public bool hasIdle = true;
    public float idleAmount = 0f;
    public float idleInterval = 1f;

    private float idleTimer = 0f;
    private FlowerPop flowerPop;

    void Awake()
    {
        // récupère le FlowerPop sur la même fleur
        flowerPop = GetComponent<FlowerPop>();

        // récupère GameManager automatiquement si pas mis dans l’inspecteur
        if (gameManager == null)
            gameManager = FindObjectOfType<IdleGameManager>();

        // si cette fleur n’a pas d’idle, on s’assure que ça reste à 0
        if (!hasIdle)
        {
            idleAmount = 0f;
            return;
        }

        // si idleAmount laissé à 0, on le calcule depuis le prix
        if (idleAmount == 0f)
        {
            float percentPerSecond = 0.1f; // 10% du prix par seconde
            idleInterval = 1f;
            idleAmount = price * percentPerSecond;
        }
    }

    void Update()
    {
        if (!hasIdle || gameManager == null)
            return;

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleInterval)
        {
            idleTimer -= idleInterval;
            gameManager.AddCoins(idleAmount);
        }
    }

    void OnMouseDown()
    {
        if (gameManager == null)
            return;

        // 1) gain de pièces + popup
        gameManager.AddCoins(coinsPerClick);
        gameManager.SpawnCoinPopup(transform.position, coinsPerClick);

        // 2) pop de la fleur
        if (flowerPop != null)
            flowerPop.PlayPop();

        // 3) FX de particules sur le pétale
        if (clickFx != null)
        {
            Debug.Log("PLAY FX sur " + clickFx.name);

            // on force un restart propre
            clickFx.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            clickFx.Play(true);
        }
        else
        {
            Debug.LogWarning("clickFx est NULL sur " + name);
        }
    }
}