using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//GameControl : GameState : GameLogic : GameData : MonoBehavior
public class GameLogic : GameData {
    
    public void SelectPlanet(GameObject planetToSelect) {
		
		if (planetToSelect.name != loader.lastPlanetVisited) {
			selectedPlanet = new SelectedPlanet (planetToSelect);//new select planet
			planet=selectedPlanet.selectedPlanet;
			playerScript.AnimationByHash (PlayerScript.player.flyOldHash);
            if (goToPlanet != null) {
				StopCoroutine (goToPlanet);
			}
            mainCameraTransform.GetComponent<CameraFollowScript>().ToggleFollow();
            goToPlanet = StartCoroutine (GoToPlanet ());
        }
    }

	public IEnumerator GoToPlanet() { // see above funtion, same shit differnt location
		while (!finishedTravelingCallBack){
			TravelToPlanet(playerTransform,playerTransform.position,selectedPlanet.playerInPlanetOrbitPosition.position, selectedPlanet.playerLookAt, 100f, 92f); // do the travel thing
            yield return new WaitForEndOfFrame();
        }
        finishedTravelingCallBack = false;//reset travel var
		speedMultiplier = 0f;
        playerScript.allowedToHit = false; //disable hit and block we need this if coming from the planet where it's enabled
        currentPlayerState = PlayerState.inPlanetOrbit;// set state
        if (!selectedPlanet.selectedOrbit.transform.GetChild(2).gameObject.activeSelf) {
            selectedPlanet.selectedOrbit.transform.GetChild(2).gameObject.SetActive(true);
        }
        mainCameraTransform.GetComponent<CameraFollowScript>().ToggleFollow();
        foreach (Transform level in selectedPlanet.selectedPlanet.transform) {// for every level under this object
            level.gameObject.SetActive(true);//enable the levels
        }
        DisplayTheText("arrived"+selectedPlanet.selectedPlanet.name);// print this
    }
    
	public void TravelToPlanet(Transform objectToMoveTransform, Vector3 originalPosition, Vector3 moveTo, Vector3 rotateTo, float moveSpeedModifier, float whenToUnpause) {//using MoveObject and RotateObject move and rotate the object from one location to another, with speed modifiers and event when to start rotating.
		actualDistance = Vector3.Distance(playerTransform.position, moveTo);// calculate the distance for every frame 
                                                                            //print (actualDistance);

        if (actualDistance > whenToUnpause) {
			objectToMoveTransform.rotation = Quaternion.Lerp(objectToMoveTransform.rotation, Quaternion.LookRotation(rotateTo - originalPosition), Time.deltaTime * speedMultiplier);// turn slowly towards it
			speedMultiplier += .5f;
		} else {
			if (!exitedAnim) {
				
				print ("Unpause");
				playerScript.AnimationByHash (playerScript.flyExitHash);
				exitedAnim = true;
			}
			objectToMoveTransform.LookAt(rotateTo);// follow it all the way from this point on.
		}
		MoveObject(objectToMoveTransform, originalPosition, moveTo, moveSpeedModifier);
	}

    public void SelectLevel(GameObject _selectedLevel) { // level is passed as an object, we're going through the tree to pull the relevant objects.
		//enemyControlerList = new List<GameObject>();
		selectedLevel= new SelectedLevel(_selectedLevel);
		playerScript.selectedLevel = selectedLevel;
		originalDistance = Vector3.Distance(playerTransform.position, selectedLevel.playerInBattleStartingPosition.transform.position);// calculate the distance before entering function
		unpause = .15f;
		playerScript.AnimationByHash(playerScript.flyOldHash);
		if (goToLevel != null) {
			StopCoroutine (goToLevel);
		}
		goToLevel = StartCoroutine (GoToLevel ());
		//loader.levelKey = selectedPlanet.selectedPlanet.name+selectedLevel.name;
		//loader.SaveData (loader.saveFileName);
        //playerCamera.transform.parent = null; //remove the camera from the parent
       // currentPlayerState = PlayerState.travelToBattle; //set state
    }

	public IEnumerator GoToLevel() {
		while (!finishedTravelingCallBack){
		TravelToLevel(playerTransform,playerTransform.position, selectedLevel.playerInBattleStartingPosition, 50f, originalDistance, unpause);//travel to this location
			yield return new WaitForEndOfFrame();
		}
		selectedLevel.enemiesToActivate.SetActive(true); //activate enemies
		selectedLevel.puzzlePiecesHolder.SetActive(true); //activate enemies
        playerScript.allowedToHit = true; // enable hit and block
		//playerScript.OkToMove(true);
       
		DisplayTheText("arrived"+selectedLevel.selectedLevel.name);
        finishedTravelingCallBack = false; //reset the travel variable
		speedMultiplier = 0f;
		playerScript.anim.Rebind ();
		currentPlayerState = PlayerState.inBattle;
		//call whatever coroutine to count regeneration shit

    }

	public void TravelToLevel(Transform objectToMoveTransform,Vector3 originalPosition, GameObject moveTo, float moveSpeedModifier, float originalDistance, float whenToUnpause) {// revise this function wtf?
		actualDistance = Vector3.Distance(playerTransform.position, moveTo.transform.position); //distance calculated at each frame.
		//		print(actualDistance);
		if (actualDistance > originalDistance * .2f) { // if the actual distance is greater than 20% of the original distance (for the first 80% do this)
			objectToMoveTransform.rotation = Quaternion.Lerp(objectToMoveTransform.rotation, Quaternion.LookRotation(moveTo.transform.position - objectToMoveTransform.position), Time.deltaTime * speedMultiplier);// turn slowly towards it
			speedMultiplier += .5f;
		} else {
			objectToMoveTransform.rotation = Quaternion.RotateTowards(objectToMoveTransform.rotation, moveTo.transform.rotation, Time.deltaTime * 210f); //for the last 20% start rotating the player to match the position on the level
		}
		if (actualDistance < originalDistance * whenToUnpause) {
			if (!exitedAnim) {
				//print ("Unpausing to level");
				playerScript.AnimationByHash (playerScript.flyExitHash);
				exitedAnim = true;
			}
		}
		MoveObject(objectToMoveTransform,originalPosition, moveTo.transform.position, moveSpeedModifier);
	}

	public void ExitLevel() {
		if (currentPlayerState == PlayerState.puzzleSolving) {
			selectedLevel.selectedLevel.transform.parent.parent.GetComponent<OrbitMoonLogic>().SetSpin();
		}
		currentPlayerState = PlayerState.inPlanetOrbit;
        if (goToLevel != null) {
            StopCoroutine(goToLevel);
        }
        if (goToPlanet != null) {
			StopCoroutine (goToPlanet);
		}
		ResetLevel ();
		goToPlanet = StartCoroutine (GoToPlanet ());
		playerScript.AnimationByHash(playerScript.flyOldHash);
		playerScript.AnimationEndedEvent ();
		playerScript.CanNoLongerComboEvent ();
	}

	public void ResetLevel(){
//		PlayerScript.player.OrientationParenting ();
		if (playerScript.okToMove) playerScript.okToMove = false;
		playerScript.StopMoveToNextBreadcrumb ();
		enemyControlerList = new List<GameObject> ();
		targetsList= new List<GameObject> ();
		enemyOrientationList= new List<GameObject> ();
		selectedLevel.enemiesToActivate.SetActive(false); // deactivate the enemies
        selectedLevel.puzzlePiecesHolder.SetActive(false); //activate enemies
        playerScript.j = 1;

	}

	public void StartOrbitSpin() {
		if (!debug) {
			foreach (GameObject orbit in solarSystem.orbitList) {
				if (orbit.name != loader.orbitKey) {
					orbit.GetComponent<OrbitLogic>().SetSpin();
				}
			}
		}
	}

	public void DebugGame() {
		if (debug) {
			transform.GetChild(0).GetComponent<MainMenuScript>().OpenDropDownMenu();
			currentPlayerState = PlayerState.menuSelection;
		}
	}

	//overload for debug
	public void GoToLevel(GameObject _selectedPlanet, GameObject _selectedLevel) {//debug function potentially extend to any level
		selectedPlanet=new SelectedPlanet(_selectedPlanet);//new select planet
		planet=selectedPlanet.selectedPlanet;
		selectedLevel= new SelectedLevel(_selectedLevel);
		playerScript.selectedLevel = selectedLevel;
		originalDistance = Vector3.Distance(playerTransform.position, selectedLevel.playerInBattleStartingPosition.transform.position);// calculate the distance before entering function
		selectedLevel.selectedLevel.SetActive(true);
		selectedLevel.enemiesToActivate.SetActive(true); //activate enemies
        selectedLevel.puzzlePiecesHolder.SetActive(true); //activate enemies
        DisplayTheText("arrived"+selectedLevel.selectedLevel.name);
		playerScript.allowedToHit = true;
		currentPlayerState = PlayerState.inBattle;
		player.transform.rotation = selectedLevel.playerInBattleStartingPosition.transform.rotation;
		player.transform.position = selectedLevel.playerInBattleStartingPosition.transform.position;
	}

    public void ToggleMenu() {
        if (!menuToggle) menuToggle = true; else menuToggle = false;
        if (menuToggle) {
            permutationsUtilitiesCore.MoveTrasformToLocation(mainCameraTransform,cameraMenuPosition);
            currentPlayerState = PlayerState.menuSelection;
            //GoToMenu();
        } else {
            permutationsUtilitiesCore.MoveTrasformToLocation(mainCameraTransform,playerTransform);
            currentPlayerState = PlayerState.inPlanetOrbit;
            // MoveCameraBack();
        }
    }



    //public void GoToMenu() {
    //    mainCameraTransform.SetParent(null);
    //    StopAllCoroutines();
    //    moveCamera = StartCoroutine(MoveCamera(mainCameraTransform.position,cameraMenuPosition.position,1f,0f));
    //    rotateCamera = StartCoroutine(RotateCamera(mainCameraTransform.rotation,cameraMenuPosition.rotation,1f,0f));
    //}

    //public void MoveCameraBack() {
    //    mainCameraTransform.SetParent(playerTransform);
    //    mainCameraTransform.SetAsFirstSibling();
    //    StopAllCoroutines();
    //    moveCamera = StartCoroutine(MoveCamera(mainCameraTransform.position,playerTransform.GetChild(1).position,1f,0f));
    //    rotateCamera = StartCoroutine(RotateCamera(mainCameraTransform.rotation,playerTransform.GetChild(1).rotation,1f,0f));

    //}

    //IEnumerator MoveCamera(Vector3 currentPos,Vector3 newPos,float duration,float counter) {// must cache the positions otherwise the lerp recalculates the position at each step!!!!!
    //    while (counter < duration) {
    //        counter += Time.deltaTime;
    //        mainCameraTransform.position = Vector3.Lerp(currentPos,newPos,counter / duration);
    //        yield return null;
    //    }
    //}

    //IEnumerator RotateCamera(Quaternion currentRot,Quaternion newRot,float duration,float counter) {// must cache the positions otherwise the lerp recalculates the position at each step!!!!!
    //    while (counter < duration) {
    //        counter += Time.deltaTime;
    //        mainCameraTransform.rotation = Quaternion.Lerp(currentRot,newRot,counter / duration);
    //        yield return null;
    //    }
    //}


}
