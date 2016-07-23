using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TeamColour { Blue, Red, Yellow, Green }

public class Team
{
    public string teamName;
    public TeamColour teamColour;
    public List<Player> players;
    public int currentPlayer = 0;

    public int TeamHealth
    {
        get
        {
            int health = 0;
            foreach (Player player in players)
                health += player.health;
            return health;
        }
    }

    public void NextPlayer()
    {
        currentPlayer++;
        if (currentPlayer == players.Count)
            currentPlayer = 0;
        if (players[currentPlayer].health == 0)
            if (TeamHealth > 0)
                NextPlayer();
    }

    public void SetPlayers(List<Player> players)
    {
        this.players = players;
        foreach (Player player in players)
            player.team = this;
    }
}