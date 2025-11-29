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
        // On ajoute des pièces en fonction du temps écoulé
        coins += coinsPerSecond * Time.deltaTime;

        // On met à jour le texte à l'écran
        coinsText.text = "Pièces : " + Mathf.FloorToInt(coins);
    }
}
