using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeDisplay : MonoBehaviour {

    public Text main;
    public Text sub;

    public GameManager manager;

    void Update()
    {
        main.text = Mathf.Ceil(manager.matchLengthDelta).ToString();
        if (manager.currentStage == CurrentStage.PreTurn)
            sub.text = Mathf.Ceil(manager.preTurnLengthDelta).ToString();
        if (manager.currentStage == CurrentStage.Turn)
            sub.text = Mathf.Ceil(manager.turnLengthDelta).ToString();
        if (manager.currentStage == CurrentStage.PostTurn)
            sub.text = Mathf.Ceil(manager.postTurnLengthDelta).ToString();
    }
}
