using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState {

	//public GameObject selectedOrbit;
	//public GameObject selectedPlanet;
	//public Transform selectedPlanetTransform;
	//public Vector3 playerInPlanetOrbitPosition;
	//public GameObject selectedLevel;

	private SolarSystem solarSystem;
	private DataLoader loader;
	private GameControl control;
	private SelectedPlanet selectedPlanet;



	public StartState() {
		solarSystem = new SolarSystem ();
		loader = DataLoader.loader;
		control = GameControl.control;
		selectedPlanet = new SelectedPlanet (selectedPlanet.selectedOrbit.transform.GetChild(1).gameObject);
		//in each object that can be turned on/off check list of objects to disable from loader.objectsToDisable.Contains(gameobject.name);  using System.Linq;
		//control.selectedLevel = solarSystem.levelsDictionary[loader.levelKey];
		//print("setting StartState");
		//        if (loader.playerHasDied) {  ***DEADBODY LOGIC***  MAY GO IN SOME FUTURE TIME
		//            playerDeadBodyTransform.gameObject.SetActive(true);
		//        }
		selectedPlanet.selectedOrbit = solarSystem.orbitDictionary[loader.orbitKey];

		selectedPlanet.selectedPlanet = selectedPlanet.selectedOrbit.transform.GetChild(1).gameObject;
		selectedPlanet.selectedPlanetTransform = selectedPlanet.selectedPlanet.transform;
		selectedPlanet.playerInPlanetOrbitPosition.position = selectedPlanet.selectedOrbit.transform.GetChild(0).position;
		if (selectedPlanet.selectedPlanet.name.ToLower() == "sun") {
			selectedPlanet.playerLookAt = selectedPlanet.selectedPlanetTransform.parent.GetChild(2).GetChild(0).position;
		} else {
			selectedPlanet.playerLookAt = selectedPlanet.selectedPlanetTransform.position;

		}
		foreach (Transform level in selectedPlanet.selectedPlanetTransform) {
			level.gameObject.SetActive(true);
		}
		control.playerTransform.position = selectedPlanet.selectedOrbit.transform.GetChild(0).position;
		control.playerTransform.LookAt(selectedPlanet.playerLookAt);
		//loader.playerHasDied = false;
	}

}
