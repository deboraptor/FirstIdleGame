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
    // 0) sécurité : index valide ?
    if (index < 0 || index >= flowerPrefabs.Length)
        return;

    // 1) progression : on ne peut acheter que la "prochaine" fleur
    if (index != nextFlowerIndex)
    {
        Debug.Log("Tu dois d'abord acheter la fleur précédente !");
        return;
    }

    // 2) encore une place dispo ?
    if (flowersBought >= flowerSpots.Length)
    {
        Debug.Log("Plus de place pour de nouvelles fleurs !");
        UpdateButtonsInteractable();
        return;
    }

    // 3) on récupère le prefab
    GameObject prefab = flowerPrefabs[index];

    // 4) on lit son prix
    FlowerClicker prefabData = prefab.GetComponent<FlowerClicker>();
    if (prefabData == null)
    {
        Debug.LogError("Le prefab " + prefab.name + " n'a pas de FlowerClicker !");
        return;
    }

    float price = prefabData.price;

    // 5) assez de pièces ?
    if (coins < price)
    {
        Debug.Log("Pas assez de pièces pour acheter " + prefab.name + " (prix : " + price + ")");
        return;
    }

    // 6) on paye
    coins -= price;

    // 7) on instancie sur le prochain SPOT
    Transform spot = flowerSpots[flowersBought];

    Debug.Log("Je spawn " + prefab.name + " sur " + spot.name + " à la position " + spot.position);

    GameObject newFlower = Instantiate(prefab, spot.position, Quaternion.identity, spot);

    // on centre par rapport au spot
    newFlower.transform.localPosition = Vector3.zero;

    // 8) lien avec le GameManager
    FlowerClicker fc = newFlower.GetComponent<FlowerClicker>();
    if (fc != null)
        fc.gameManager = this;

    // 9) ordre d'affichage
    SpriteRenderer sr = newFlower.GetComponent<SpriteRenderer>();
    if (sr != null)
        sr.sortingOrder = flowersBought;

    // 10) on passe au spot suivant
    flowersBought++;

    // 11) on passe à la fleur suivante du shop
    nextFlowerIndex++;

    // 12) on met à jour quels boutons sont cliquables
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