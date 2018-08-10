using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedPlanet {
	
	public GameObject selectedOrbit;
	public GameObject selectedPlanet;
	public Transform selectedPlanetTransform;
	public Transform playerInPlanetOrbitPosition;
	public Vector3 playerLookAt;

	private SolarSystem solarSystem;
	private DataLoader loader;
	private GameControl control;

	public SelectedPlanet(GameObject planetToSelect) {

		loader = DataLoader.loader;
		control = GameControl.control;
		solarSystem = new SolarSystem ();

		//before selecting the planet
		//Debug.Log ("Selected Planet Name " + selectedPlanet.name);
		if (loader.lastPlanetVisited != planetToSelect.name){
            GameObject lastOrbit = solarSystem.orbitDictionary[loader.orbitKey];

            if (!control.debug) {
                lastOrbit.SendMessage("SetSpin");//set it spinning again
            }
            if (lastOrbit.transform.GetChild(2).gameObject.activeSelf) {
                lastOrbit.transform.GetChild(2).gameObject.SetActive(false);
            }
			foreach (Transform level in solarSystem.planetDictionary[loader.lastPlanetVisited].transform) { //disable all the levels
				level.gameObject.SetActive (false);

			}
            
		}
		//after selecting the planet
		SetPlanetVars(planetToSelect);//set the variables for the selected planet

        if (selectedPlanet.name == "Sun") {// if you're on the sun.
			//transform.GetChild(0).GetChild(1).GetComponent<ChestInTownScript>().OpenPanel();
			//unpause = 92f;
			//playerScript.AnimationByHash(PlayerScript.player.flyOldHash);
			Vector3 rotateTo = new Vector3(PlayerScript.player.transform.position.x, selectedPlanetTransform.parent.GetChild(1).position.y, PlayerScript.player.transform.position.z);// rotate the view point around the landing position
			selectedPlanetTransform.parent.GetChild(2).LookAt(rotateTo); //get the lookat point
			playerLookAt = selectedPlanetTransform.parent.GetChild(2).GetChild(0).position;

		} else {
			playerLookAt = selectedPlanetTransform.position;
            
            if (!control.debug) {
                selectedOrbit.GetComponent<OrbitLogic>().SetSpin();
            }
            //unpause = 92f;
            //playerScript.AnimationByHash(PlayerScript.player.flyOldHash);
        }
		//currentPlayerState = PlayerState.travelToPlanetOrbit;//set state
	}

	public void SetPlanetVars(GameObject _selectedPlanet) {
		selectedPlanet = _selectedPlanet;
		selectedPlanetTransform = _selectedPlanet.transform;
		selectedOrbit = selectedPlanetTransform.parent.gameObject;
		playerInPlanetOrbitPosition = selectedPlanetTransform.parent.GetChild(0); //get the relative player position
		//originalDistance = Vector3.Distance(playerTransform.position, playerInPlanetOrbitPosition);// calculate the distance
		//save the vars
		if (!loader.visitedOrbits.Contains (selectedOrbit.name)) {//if the orbit is not already in this list
			loader.visitedOrbits.Add(selectedOrbit.name);//add it to the list
		}
		loader.lastPlanetVisited = selectedPlanet.name;//add it to the list
		loader.orbitKey = selectedOrbit.name;// set the orbit string key
		loader.SaveData (loader.saveFileName);//save the file.
	}

}
