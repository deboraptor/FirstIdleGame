using UnityEngine;

public class FlowerClicker : MonoBehaviour
{
    public IdleGameManager gameManager;   // On va le lier dans l’Inspector
    public float coinsPerClick = 1f;

    void OnMouseDown()
    {
        // Cette fonction est appelée quand on clique sur l’objet
        gameManager.AddCoins(coinsPerClick);
    }
}