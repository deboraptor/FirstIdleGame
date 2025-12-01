using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IdleGameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI buyFlowerButtonText;

    [Header("Coins")]
    public float coins = 0f;
    public float coinsPerSecond = 1f;

    [Header("Shop - Fleurs")]
    public GameObject[] flowerPrefabs;        // une liste de pleins de fleurs
    public Transform[] flowerSpots;        // positions possibles pour les nouvelles fleurs
    public float flowerBasePrice = 10f;    // prix de base
    public float flowerPriceMultiplier = 1.5f; // le prix augmente à chaque achat
    public float extraCpsPerFlower = 0.5f; // combien de coins/s en plus par fleur achetée

    private int flowersBought = 0;
    private float currentFlowerPrice;

    void Start()
    {
        currentFlowerPrice = flowerBasePrice;
        UpdateBuyFlowerButtonText();
    }

    void Update()
    {
        // Gain automatique (idle)
        coins += coinsPerSecond * Time.deltaTime;

        // On met à jour le texte à l'écran
        coinsText.text = "Pièces : " + Mathf.FloorToInt(coins);
    }

    // appelée quand on clique sur la fleur
    public void AddCoins(float amount)
    {
        coins += amount;
    }

    // appelée par le bouton "Acheter une fleur"
    public void BuyFlower()
    {
        // assez de pièces ?
        if (coins < currentFlowerPrice)
        {
            Debug.Log("Pas assez de pièces pour acheter une fleur !");
            return;
        }

        // encore une place dispo ?
        if (flowersBought >= flowerSpots.Length)
        {
            Debug.Log("Plus de place pour de nouvelles fleurs !");
            return;
        }

        // payer
        coins -= currentFlowerPrice;

        // nouvelle fleur à l'emplacement suivant
        Transform spot = flowerSpots[flowersBought];

        // Choisir une fleur au hasard
        GameObject randomFlower = flowerPrefabs[ Random.Range(0, flowerPrefabs.Length) ];

        // L’instancier
        GameObject newFlower = Instantiate(randomFlower, spot.position, Quaternion.identity);

        // Mettre l’ordre d’affichage selon la position Y pour un effet de profondeur
        SpriteRenderer sr = newFlower.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = -(int)(spot.position.y * 100);
        }

        // on relie le gameManager au script FlowerClicker
        FlowerClicker fc = newFlower.GetComponent<FlowerClicker>();
        if (fc != null)
        {
            fc.gameManager = this;
        }

        // bonus idle : plus de coins par seconde
        coinsPerSecond += extraCpsPerFlower;

        flowersBought++;

        // augmenter le prix pour la prochaine fleur
        currentFlowerPrice *= flowerPriceMultiplier;

        UpdateBuyFlowerButtonText();
    }

    void UpdateBuyFlowerButtonText()
    {
        if (buyFlowerButtonText != null)
        {
            buyFlowerButtonText.text = "Acheter une fleur (" + Mathf.FloorToInt(currentFlowerPrice) + ")";
        }
    }
}