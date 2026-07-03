using UnityEngine;

// Simple save manager using PlayerPrefs for demo. In production use file-based save.
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(this.gameObject); return; }
        Instance = this; DontDestroyOnLoad(this.gameObject);
    }

    public void Save(PlayerData player)
    {
        if (player == null) return;
        PlayerPrefs.SetInt(player.name + "_bankroll", player.bankroll);
        PlayerPrefs.SetInt(player.name + "_wins", player.wins);
        PlayerPrefs.Save();
    }

    public void Load(PlayerData player)
    {
        if (player == null) return;
        if (PlayerPrefs.HasKey(player.name + "_bankroll"))
        {
            player.bankroll = PlayerPrefs.GetInt(player.name + "_bankroll");
            player.wins = PlayerPrefs.GetInt(player.name + "_wins");
        }
    }
}
