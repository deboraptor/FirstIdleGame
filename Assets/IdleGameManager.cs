using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;   // TextMeshPro

public class IdleGameManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText;

    public float coins = 0f;
    public float coinsPerSecond = 1f;

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
}
