using System;

[Serializable]
public class PlayerData
{
    public string name;
    public int bankroll;
    public bool isAI;
    public int currentBet = 0;
    public int lastRoll = 0;
    public int wins = 0;
    public AIStrategy aiStrategy = AIStrategy.Balanced;

    public PlayerData(string name, int bankroll, bool isAI, AIStrategy strategy = AIStrategy.Balanced)
    {
        this.name = name;
        this.bankroll = bankroll;
        this.isAI = isAI;
        this.aiStrategy = strategy;
    }

    public void PlaceBet(int amount)
    {
        amount = Math.Max(0, amount);
        amount = Math.Min(amount, bankroll);
        currentBet = amount;
        bankroll -= amount;
    }
}
