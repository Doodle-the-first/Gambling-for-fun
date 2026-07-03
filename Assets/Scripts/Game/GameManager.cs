using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Simple single-player gambling-style game manager.
// Round flow:
// 1) Players place bets (human uses UI, AIs auto-bet)
// 2) Players "roll" (random 1..6)
// 3) Highest roll wins the pot (ties split)
// 4) Update bankrolls and progression; save state

public class GameManager : MonoBehaviour
{
    public int startingBankroll = 1000;
    public int minBet = 10;

    public List<PlayerData> players = new List<PlayerData>();
    public PlayerData humanPlayer;

    public int pot = 0;

    // UI references (wired at scene creation)
    public Text hudText;
    public Text potText;
    public Button rollButton;
    public InputField betInput;
    public Text resultText;

    private System.Random rng = new System.Random();

    void Start()
    {
        // Create players if not already present
        if (players.Count == 0)
        {
            CreateDefaultPlayers();
        }

        // Load saved human bankroll if present
        SaveManager.Instance.Load(humanPlayer);

        UpdateHUD();

        // Hook UI actions
        if (rollButton != null)
            rollButton.onClick.AddListener(OnRollClicked);
    }

    void CreateDefaultPlayers()
    {
        // Human
        humanPlayer = new PlayerData("You", startingBankroll, false);
        players.Add(humanPlayer);

        // 3 AI opponents with varying risk profiles
        players.Add(new PlayerData("AI_Alice", startingBankroll, true, AIStrategy.Risky));
        players.Add(new PlayerData("AI_Bob", startingBankroll, true, AIStrategy.Balanced));
        players.Add(new PlayerData("AI_Clara", startingBankroll, true, AIStrategy.Conservative));
    }

    public void OnRollClicked()
    {
        int bet = minBet;
        if (int.TryParse(betInput.text, out int parsed))
        {
            bet = Mathf.Max(minBet, parsed);
        }

        if (humanPlayer.bankroll < bet)
        {
            resultText.text = "Insufficient funds for that bet.";
            return;
        }

        // Clear previous
        pot = 0;
        foreach (var p in players)
        {
            p.currentBet = 0;
            p.lastRoll = 0;
        }

        // Human places bet
        humanPlayer.PlaceBet(bet);
        pot += bet;

        // AIs place bets
        foreach (var p in players)
        {
            if (p.isAI)
            {
                int aiBet = AIController.ChooseBet(p);
                aiBet = Mathf.Clamp(aiBet, minBet, p.bankroll);
                p.PlaceBet(aiBet);
                pot += aiBet;
            }
        }

        // Rolls
        int best = -1;
        foreach (var p in players)
        {
            p.lastRoll = rng.Next(1, 7); // 1..6
            if (p.lastRoll > best) best = p.lastRoll;
        }

        // Determine winners (could be ties)
        List<PlayerData> winners = players.FindAll(x => x.lastRoll == best);
        int share = pot / winners.Count;

        foreach (var w in winners)
        {
            w.bankroll += share;
            w.wins += 1;
        }

        // Subtract bets from losers (bets already deducted on PlaceBet)
        // Save human
        SaveManager.Instance.Save(humanPlayer);

        // Update UI
        resultText.text = "Round results:\n";
        foreach (var p in players)
        {
            resultText.text += $"{p.name}: roll {p.lastRoll}, bankroll {p.bankroll}\n";
        }

        resultText.text += $"Winners: {string.Join(", ", winners.ConvertAll(x => x.name).ToArray())}";

        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (hudText != null && humanPlayer != null)
        {
            hudText.text = $"Bankroll: {humanPlayer.bankroll}  | Wins: {humanPlayer.wins}";
        }
        if (potText != null)
        {
            potText.text = $"Pot: {pot}";
        }
    }
}
