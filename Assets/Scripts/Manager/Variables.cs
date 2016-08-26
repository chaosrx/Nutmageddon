using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Variables
{
    public static readonly List<TeamData> defaultTeamData = new List<TeamData>()
    { 
        new TeamData("Team a", new List<string>{ "name1", "name2", "name3", "name4", "name5"}, Voicepack.Normal, Model.Brown),
        new TeamData("Team b", new List<string>{ "name1", "name2", "name3", "name4", "name5"}, Voicepack.Normal, Model.Brown)
    };
    public static List<TeamData> teamData = new List<TeamData>();
}