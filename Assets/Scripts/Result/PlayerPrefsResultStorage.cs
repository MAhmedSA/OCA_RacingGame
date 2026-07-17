using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsResultStorage : IResultStorage
{
    
    public void Save(IReadOnlyList<string> winners)
    {
        for (int i = 0; i < winners.Count; i++)
        {
            string winner = winners[i];

            // save winner with index
            
            PlayerPrefs.SetString($"Winner_{i+1}", winner);
            
        }

        PlayerPrefs.Save();
    }
}