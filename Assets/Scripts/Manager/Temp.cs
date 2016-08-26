using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Temp : MonoBehaviour {
    public List<TeamData> td;

	void Update () {
        td = Variables.teamData;
	}
}
