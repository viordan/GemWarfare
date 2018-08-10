using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedLevel {

	public GameObject enemiesToActivate;
	public GameObject playerInBattleStartingPosition;
	public GameObject breadCrumbsPath;
	public Transform breadCrumbsTrasfrom;
	public Vector3 lastBreadCrumbPosition;
	public Transform priorBreadCrumb;
	public Transform nextBreadCrumb;
    public GameObject puzzlePiecesHolder;

	//private DataLoader loader;
	//private GameControl control;
	public GameObject selectedLevel;



	public SelectedLevel(GameObject _selectedLevel){
		//loader = DataLoader.loader;
		//control = GameControl.control;
		//selectedPlanet = _selectedPlanet;
		selectedLevel = _selectedLevel;
		SetLevelVars(_selectedLevel);// set the level vars
	}

	void SetLevelVars(GameObject _selectedLevel) {
//		loader.levelKey = selectedPlanet.name+selectedLevel.name;
//		loader.SaveData (loader.saveFileName);
		selectedLevel = _selectedLevel; //not sure if this is needed... are we using this anywhere
		//levelSelected = _selectedLevel.name;//set the level's name in GameControl
		Transform _selectedLevelTransform = _selectedLevel.transform; //chache the transform for this function.
		enemiesToActivate = _selectedLevelTransform.GetChild(0).gameObject; //first child is the enemies group set this in order to activate the enemies when GoToBattle() is invoked 
        puzzlePiecesHolder = _selectedLevelTransform.GetChild(3).gameObject;
        breadCrumbsPath = _selectedLevelTransform.GetChild(2).gameObject; //third child is the breadcrubms object
		playerInBattleStartingPosition = _selectedLevelTransform.GetChild(2).GetChild(0).gameObject; //first object in breadcrumbs is the starting position
		breadCrumbsTrasfrom = breadCrumbsPath.transform;
		lastBreadCrumbPosition = breadCrumbsTrasfrom.GetChild(breadCrumbsTrasfrom.childCount - 1).position;
	}
}
