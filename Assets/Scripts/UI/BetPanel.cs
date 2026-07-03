using UnityEngine;
using UnityEngine.UI;

// Connects UI input field / button to GameManager bet/roll
public class BetPanel : MonoBehaviour
{
    public InputField betInput;
    public Button rollButton;
    public Text infoText;
    public GameManager gameManager;

    void Start()
    {
        if (rollButton != null)
        {
            rollButton.onClick.AddListener(OnRoll);
        }
    }

    public void OnRoll()
    {
        if (gameManager != null)
        {
            gameManager.OnRollClicked();
        }
    }
}
