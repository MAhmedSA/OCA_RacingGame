using System.Collections.Generic;
public class WinnerResolver 
{
    List<string> winners = new();

    public IReadOnlyList<string> Winners => winners;
    public bool IsComplete => winners.Count >= 3;

    public void AddWinner(string player)
    {
        if (IsComplete) return;
        winners.Add(player);
    }

    public void Reset()
    {
        winners.Clear();
    }
}
