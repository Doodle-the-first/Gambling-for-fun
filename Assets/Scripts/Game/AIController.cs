using System;

public static class AIController
{
    // Choose a bet based on strategy and bankroll
    public static int ChooseBet(PlayerData p)
    {
        switch (p.aiStrategy)
        {
            case AIStrategy.Conservative:
                return Math.Max(10, p.bankroll / 20); // small percent
            case AIStrategy.Risky:
                return Math.Max(10, p.bankroll / 4); // larger percent
            default:
                return Math.Max(10, p.bankroll / 8);
        }
    }
}
