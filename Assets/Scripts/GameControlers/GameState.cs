using System.Collections.Generic;
using UnityEngine;
//GameControl : GameState : GameLogic : GameData : MonoBehavior
public class GameState : GameLogic {

    public void StartOrbitSpin() {
   //     if (!debug) {
			//foreach (GameObject orbit in solarSystem.orbitList) {
   //             if (orbit.name != loader.orbitKey) {
   //                 orbit.GetComponent<OrbitLogic>().SetSpin();
   //             }
   //         }
   //     }
    }

    public void DebugGame() {
        if (debug) {
            transform.GetChild(0).GetComponent<MainMenuScript>().OpenDropDownMenu();
            currentPlayerState = PlayerState.menuSelection;
        }
    }

//    public void GetSolarSystem() {
//        thisScene.GetRootGameObjects(rootObjects); // get the root objects
//        levelsDictionary.Add("sun", player);//default value
//        foreach (GameObject obj in rootObjects) { // for every root object
//            if (obj.name.ToLower().Contains("orbit")) { // if it has orbitplanet inname
//                orbitDictionary.Add(obj.name, obj);
//                levelsList = new List<GameObject>(); // list of my levels
//                orbitList.Add(obj); // add it to my orbits
//                planetList.Add(obj.transform.GetChild(1).gameObject); // add the the second child to my planets
//                foreach (Transform level in obj.transform.GetChild(1)) { // go through planet's children
//                    levelsList.Add(level.gameObject); // add the levels to levels list
//                    levelsDictionary.Add(obj.transform.GetChild(1).gameObject.name + level.gameObject.name, level.gameObject);
//                }
//                planetsListWithLevels.Add(levelsList); // add my list to the the list of lists
//            }
//        }
//    }

//    public void StartState() {
//        //in each object that can be turned on/off check list of objects to disable from loader.objectsToDisable.Contains(gameobject.name);  using System.Linq;
//		selectedLevel = solarSystem.levelsDictionary[loader.levelKey];
//        print("setting StartState");
////        if (loader.playerHasDied) {  ***DEADBODY LOGIC***  MAY GO IN SOME FUTURE TIME
////            playerDeadBodyTransform.gameObject.SetActive(true);
////        }
//		selectedOrbit = solarSystem.orbitDictionary[loader.orbitKey];
//
//        selectedPlanet = selectedOrbit.transform.GetChild(1).gameObject;
//        selectedPlanetTransform = selectedPlanet.transform;
//        playerInPlanetOrbitPosition = selectedOrbit.transform.GetChild(0).position;
//        if (selectedPlanet.name.ToLower() == "sun") {
//            playerLookAt = selectedPlanetTransform.parent.GetChild(2).GetChild(0).position;
//        } else {
//            playerLookAt = selectedPlanetTransform.position;
//
//        }
//        foreach (Transform level in selectedPlanetTransform) {
//            level.gameObject.SetActive(true);
//        }
//        playerTransform.position = selectedOrbit.transform.GetChild(0).position;
//        playerTransform.LookAt(playerLookAt);
//        //loader.playerHasDied = false;
//    }

//    public void GoToLevel(GameObject _selectedPlanet, GameObject _selectedLevel) {//debug function potentially extend to any level
//        //null the  values
//        if (enemiesToActivate) {
//            enemiesToActivate.SetActive(false); //activate enemies
//        }
//        if (selectedLevel) {
//            selectedLevel.SetActive(false);
//        }
//		//PlayerScript.player.OkToMove (false);
//        selectedLevel = null;
//        selectedPlanet = null;
//        selectedPlanetTransform = null;
//        breadCrumbsPath = null;
//        breadCrumbsTrasfrom = null;
//        playerInBattleStartingPosition = null;
//        //set values
//        selectedPlanet = _selectedPlanet; //global vars
//        selectedLevel = _selectedLevel;
//
//        selectedPlanetTransform = _selectedPlanet.transform; //get the transform
//        planetSelected = _selectedPlanet.name;// asign the name of the planet for debug
//        playerInPlanetOrbitPosition = selectedPlanetTransform.parent.GetChild(0).position; //get the relative player position
//
//        selectedLevel.SetActive(true);
//        Transform _selectedLevelTransform = selectedLevel.transform; //chache the transform for this function.
//        enemiesToActivate = _selectedLevelTransform.GetChild(0).gameObject; //first child is the enemies group set this in order to activate the enemies when GoToBattle() is invoked 
//        breadCrumbsPath = _selectedLevelTransform.GetChild(2).gameObject; //third child is the breadcrubms object
//        playerInBattleStartingPosition = _selectedLevelTransform.GetChild(2).GetChild(0).gameObject; //first object in breadcrumbs is the starting position
//        playerLookAt = selectedPlanetTransform.position;
//        enemiesToActivate.SetActive(true); //activate enemies
//        //currentPlayerState = PlayerState.inBattle;
//		//PlayerScript.player.OkToMove(true);
//        PlayerScript.player.allowedToHit = true;
//        breadCrumbsTrasfrom = breadCrumbsPath.transform;
//        lastBreadCrumbPosition = breadCrumbsTrasfrom.GetChild(breadCrumbsTrasfrom.childCount - 1).position;
//        //move player
//        player.transform.rotation = playerInBattleStartingPosition.transform.rotation;
//        player.transform.position = playerInBattleStartingPosition.transform.position;
//    }
}
