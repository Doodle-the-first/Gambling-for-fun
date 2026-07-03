using UnityEngine;
using UnityEngine.UI;

// Simple UI hook for displaying basic info and connecting to GameManager
public class HUD : MonoBehaviour
{
    public GameManager gameManager;
    public Text bankrollText;
    public Text potText;
    public Text resultText;

    void Update()
    {
        if (gameManager == null) return;
        if (bankrollText != null && gameManager.humanPlayer != null)
            bankrollText.text = "Bankroll: " + gameManager.humanPlayer.bankroll;
        if (potText != null)
            potText.text = "Pot: " + gameManager.pot;
    }
}
