using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Voicepack { Normal, Robot, German }
public enum Model { Brown, Orange, Grey }

[System.Serializable]

public class TeamData {

    // This is different than the Team class, in that this only holds basic data, which is used to create the teams. This class will be saved.

    public string teamName;
    public List<string> playerNames;
    public Voicepack voice;
    public Model model;

    public TeamData(string teamName, List<string> playerNames, Voicepack voice, Model model)
    {
        this.teamName = teamName;
        this.playerNames = playerNames;
        this.voice = voice;
        this.model = model;
    }
}
