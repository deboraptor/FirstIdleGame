using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;   // <--- IMPORTANT pour Button

public class IdleGameManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI coinsText;   // Texte "Pièces : X"
    public GameObject shopPanel;        // Panel du shop

    [Header("Shop UI - Boutons")]
    public Button[] flowerButtons;      // 1 bouton par fleur, dans le même ordre que flowerPrefabs

    [Header("Coins")]
    public float coins = 0f;

    [Header("Shop - Fleurs")]
    public GameObject[] flowerPrefabs;  // Prefabs de fleurs
    public Transform[] flowerSpots;     // Emplacements dans la scène

    private int flowersBought = 0;      // nombre de fleurs déjà placées
    private int nextFlowerIndex = 0;    // index de la prochaine fleur à acheter

    void Start()
    {
        // On ferme le shop au début
        if (shopPanel != null)
            shopPanel.SetActive(false);

        // Met à jour quels boutons sont cliquables
        UpdateButtonsInteractable();
    }

    void Update()
    {
        if (coinsText != null)
        {
            coinsText.text = "Pièces : " + Mathf.FloorToInt(coins);
        }
    }

    public void AddCoins(float amount)
    {
    coins += amount;
    Debug.Log("AddCoins(" + amount + ")  → total = " + coins);
    }


    // ACHAT D'UNE FLEUR PRÉCISE (appelé par les boutons du shop)
    public void BuyFlowerByIndex(int index)
    {
        // 1) On vérifie que c'est bien la prochaine fleur dans l'ordre
        if (index != nextFlowerIndex)
        {
            Debug.Log("Tu dois d'abord acheter la fleur précédente !");
            return;
        }

        // 2) Encore une place dispo ?
        if (flowersBought >= flowerSpots.Length)
        {
            Debug.Log("Plus de place pour de nouvelles fleurs !");
            UpdateButtonsInteractable();
            return;
        }

        // 3) On récupère le prefab
        if (index < 0 || index >= flowerPrefabs.Length)
            return;

        GameObject prefab = flowerPrefabs[index];

        // 4) On lit son prix sur le FlowerClicker
        FlowerClicker prefabData = prefab.GetComponent<FlowerClicker>();
        if (prefabData == null)
        {
            Debug.LogError("Le prefab " + prefab.name + " n'a pas de FlowerClicker !");
            return;
        }

        float price = prefabData.price;

        // 5) Assez de pièces ?
        if (coins < price)
        {
            Debug.Log("Pas assez de pièces pour acheter " + prefab.name + " (prix : " + price + ")");
            return;
        }

        // 6) On paye
        coins -= price;

        // 7) On instancie sur le prochain spot
        Transform spot = flowerSpots[flowersBought];
        GameObject newFlower = Instantiate(prefab, spot.position, Quaternion.identity);

        // 8) On relie le GameManager
        FlowerClicker fc = newFlower.GetComponent<FlowerClicker>();
        if (fc != null)
            fc.gameManager = this;

        // 9) Ordre d'affichage
        SpriteRenderer sr = newFlower.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingOrder = flowersBought;

        flowersBought++;

        // 10) On passe à la fleur suivante
        nextFlowerIndex++;

        // 11) Met à jour les boutons (grise celui acheté, active le suivant, etc.)
        UpdateButtonsInteractable();
    }

    // Active/désactive les boutons selon la progression
    void UpdateButtonsInteractable()
    {
        bool hasFreeSpot = flowersBought < flowerSpots.Length;

        for (int i = 0; i < flowerButtons.Length; i++)
        {
            if (flowerButtons[i] == null)
                continue;

            // On ne peut cliquer que sur la fleur "suivante"
            bool canBuyThisOne = (i == nextFlowerIndex) && hasFreeSpot;

            flowerButtons[i].interactable = canBuyThisOne;
        }
    }

    public void OpenShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }
}