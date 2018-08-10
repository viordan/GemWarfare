using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownScript : MonoBehaviour {
    private GameControl control;
	private SolarSystem solarSystem;

    void Start() {
        control = GameControl.control;
		solarSystem = new SolarSystem ();
    }
    public void OnIndexChange(int index) {
        //print(gameObject.name);
        int i = int.Parse(gameObject.name);
        int j = index-1;
        if (j < 0) j = 0;
		control.GoToLevel(solarSystem.planetList[i], solarSystem.planetsListWithLevels[i][j]);
    }
}
