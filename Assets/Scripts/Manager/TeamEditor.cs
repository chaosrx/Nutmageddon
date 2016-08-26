using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TeamEditor : MonoBehaviour
{
    public GameObject popupWindow;
    public Text popupTitle;
    public Text popupText;
    public Text warning;

    public GameObject createPanel;
    public InputField teamName;
    public InputField name1;
    public InputField name2;
    public InputField name3;
    public InputField name4;
    public InputField name5;
    public Dropdown model;
    public Dropdown voice;

    void Update()
    {
        
    }

    void Start()
    {
        if(!Directory.Exists("teams"))
            Directory.CreateDirectory("teams");

        foreach (string path in Directory.GetFiles("teams"))
        {
            Variables.teamData.Add(Serializer.Load<TeamData>(path));
        }
    }

    public void SaveTeam()
    {
        if (teamName.text == "" || name1.text == "" || name2.text == "" || name3.text == "" || name4.text == "" || name5.text == "")
            ShowPopup("INCOMPLETE FIELDS", "Please ensure that all boxes are filled in.");
        else
            Serializer.Save<TeamData>("teams/" + teamName.text, new TeamData(teamName.text, new List<string>() { name1.text, name2.text, name3.text, name4.text, name5.text }, Voicepack.Normal, Model.Brown));
    }

    public void CheckNameExists()
    {
        warning.gameObject.SetActive(File.Exists("teams/" + teamName.text));
    }

    public void ShowPopup(string title, string text)
    {
        popupWindow.SetActive(true);
        popupTitle.text = title.ToUpper(); ;
        popupText.text = text;
    }
}
