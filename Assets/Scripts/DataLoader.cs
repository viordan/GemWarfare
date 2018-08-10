using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class PlayerData {


	//setup variables
	public bool firstTimeSetup; //this is to be set after the original setup of arrays

	//game state variables
	public int[] puzzleShuffledPieces;
	public int[] puzzleShuffledBottoms;

	public int gameIteration;
	public int playerHealthUpgradeLevel;
	public int playerDamageUpgradeLevel;
	public int playerHardDamageUpgradeLevel;
	public int hitCombosAllowed;
	public int healthRegenerationLevel;
	public int healthBackLevel;
	public string lastPlanetVisited;

//	public int playerHealth;
//	public int playerDamage;
//	public int playerHardDamage;
	public int level;
	public int score;
	public float range;
	public int moneyInTheBank;
    public bool playerHasDied;
	public int moneyOnDeadBody;

	//vector3 changed to float[3]
	public float[] playerPosition;
	public float[] playerRotation;

	//string keys for dictionaries
	public string orbitKey;
	public string levelKey;
	public string deathLevelKey;
	public List<string> visitedOrbits;
	public List<string> objectsToDisable;
	public List<string> unlockedLevels;

	public int howManyTimesPlayerHasDied;
	public int howManyTimesPlayerHasDiedAtThisLevel;
	public int howManyEnemiesKilled;
	public int howManyEnemiesKilledAtThisLevel;
	public int howManyOrbsEarnedInGame;
	public int howManyOrbsEarnedInThisLevel;
    public int howManyPiecesSoFar;
    public int howManyPiecesInTotal;
}

public class DataLoader : MonoBehaviour {

	public static DataLoader loader; // this allows you to use the script from any scene where the object is present.



	public bool firstTimeSetup;
	public int[] puzzleShuffledPieces;
	public int[] puzzleShuffledBottoms;

	public bool erraseTempFile;

	//variables to be set at runtime !!!Don't Save these in the load file
	public int enemyDamage;
	public int enemyHealth;
	public int playerHealth;
	public int playerDamage;
	public int playerHardDamage;

	//variatbles for data formater
	public BinaryFormatter bf = new BinaryFormatter();
	public PlayerData data; // object type player data
	public FileStream file; //generic filestream for serialized data 

	//this class contains the core variables to be shared between the DataLoader and GameData 
	//Only include variables that are common and necessary.
	public string saveFileName;
	//game state variables
	public int gameIteration;
	public int playerHealthUpgradeLevel;
	public int playerDamageUpgradeLevel;
	public int playerHardDamageUpgradeLevel;
	public int hitCombosAllowed;
	public int healthRegenerationLevel;
	public int healthBackLevel;
	public string lastPlanetVisited;

	public int level;
	public int score;
	public float range;
	public int moneyInTheBank;
    public bool playerHasDied;
	public int moneyOnDeadBody;

    //vector3 changed to float[3]
	public float[] playerDeathPosition;
	public float[] playerLookAt;

	//string keys for dictionaries
	public string orbitKey;
	public string levelKey;
	public string deathLevelKey;
	public List<string> visitedOrbits;
	public List<string> objectsToDisable;
	public List<string> unlockedLevels;

	//vars for splash screen
	public int howManyTimesPlayerHasDied;
	public int howManyTimesPlayerHasDiedAtThisLevel;
	public int howManyEnemiesKilled;
	public int howManyEnemiesKilledAtThisLevel;
	public int howManyOrbsEarnedInGame;
	public int howManyOrbsEarnedInThisLevel;
	public int howManyPiecesSoFar;
	public int howManyPiecesInTotal;

	void Awake() {
		if (loader == null) { //if there is not a control already in this scene
			DontDestroyOnLoad(gameObject); // don't destroy this game object
			loader = this; // the control is this object
		} else if (loader != this) { // if the control is some other instance other than this 
			Destroy(gameObject); //kill that instance
		}
		//NewVars ();
		if (erraseTempFile) {
			ErraseData ("/temp");
		}
		if (saveFileName == "") {// if the saveFileName hasn't been set by the menu, load /temp
			LoadData ("/temp");
		} else {
			LoadData (saveFileName);
		}
        saveFileName = "/temp";
		SetVariables ();
    }

	public void NewVars(){

		puzzleShuffledPieces = new int[60];
		puzzleShuffledBottoms = new int[60];
		firstTimeSetup = false;
        //print("get new vars");
		gameIteration = 1;
		playerHealthUpgradeLevel=1;
		playerDamageUpgradeLevel=1;
		playerHardDamageUpgradeLevel=1;
		hitCombosAllowed=2;
		healthRegenerationLevel=2;
		healthBackLevel=2;
//		playerHealth =100;
//		playerDamage =25;
//		playerHardDamage = 100;
		level=0;
		score=0;
		range=1;
		moneyInTheBank = 0;
		playerDeathPosition=new float[3]{0f,0f,0f};
		playerLookAt=new float[3]{0f,0f,0f};
        orbitKey = "OrbitPlanetSun";
        levelKey = "sun";
		deathLevelKey = "";
        visitedOrbits = new List<string>() {"OrbitPlanetSun"};
		unlockedLevels = new List<string> (){"sun"};
        playerHasDied = false;
		moneyOnDeadBody = 0;
		objectsToDisable = new List<string> ();
		howManyTimesPlayerHasDied=0;
		howManyTimesPlayerHasDiedAtThisLevel=0;
		howManyEnemiesKilled=0;
		howManyEnemiesKilledAtThisLevel=0;
		howManyOrbsEarnedInGame=0;
		howManyOrbsEarnedInThisLevel=0;
        howManyPiecesSoFar = 0;
        howManyPiecesInTotal = 0;
		lastPlanetVisited = "Sun";

    }

	public void SetVarsForLoading(){
        print("loading vars...");
		puzzleShuffledPieces = data.puzzleShuffledPieces;
		puzzleShuffledBottoms = data.puzzleShuffledBottoms;
		firstTimeSetup=data.firstTimeSetup;
		lastPlanetVisited = data.lastPlanetVisited;
		gameIteration = data.gameIteration;
		playerHealthUpgradeLevel=data.playerHealthUpgradeLevel;
		playerDamageUpgradeLevel=data.playerDamageUpgradeLevel;
		playerHardDamageUpgradeLevel=data.playerHardDamageUpgradeLevel;
		hitCombosAllowed= data.hitCombosAllowed;
		healthRegenerationLevel=data.healthRegenerationLevel;
		healthBackLevel=data.healthBackLevel;
//		playerHealth = data.playerHealth;
//		playerDamage = data.playerDamage;
//		playerHardDamage = data.playerHardDamage;
		range = data.range;
		level = data.level;
		score = data.score;
		moneyInTheBank = data.moneyInTheBank;
		playerDeathPosition = data.playerPosition;
		playerLookAt = data.playerRotation;
		orbitKey = data.orbitKey;
		levelKey = data.levelKey;
		deathLevelKey = data.deathLevelKey;
		visitedOrbits = data.visitedOrbits;
        playerHasDied = data.playerHasDied;
		moneyOnDeadBody = data.moneyOnDeadBody;
		objectsToDisable = data.objectsToDisable;
		unlockedLevels = data.unlockedLevels;

		howManyTimesPlayerHasDied=data.howManyTimesPlayerHasDied;
		howManyTimesPlayerHasDiedAtThisLevel=data.howManyTimesPlayerHasDiedAtThisLevel;
		howManyEnemiesKilled=data.howManyEnemiesKilled;
		howManyEnemiesKilledAtThisLevel=data.howManyEnemiesKilledAtThisLevel;
		howManyOrbsEarnedInGame=data.howManyOrbsEarnedInGame;
		howManyOrbsEarnedInThisLevel=data.howManyOrbsEarnedInThisLevel;
        howManyPiecesSoFar = data.howManyPiecesSoFar;
        howManyPiecesInTotal = data.howManyPiecesInTotal;
    }

	public void SetVarsForSaving(){
        //
		print("saving vars...");
		data.puzzleShuffledPieces = puzzleShuffledPieces;
		data.puzzleShuffledBottoms = puzzleShuffledBottoms;
		data.firstTimeSetup = firstTimeSetup;
		data.lastPlanetVisited = lastPlanetVisited;
		data.gameIteration = gameIteration;
		data.playerHealthUpgradeLevel = playerHealthUpgradeLevel;
		data.playerDamageUpgradeLevel = playerDamageUpgradeLevel;
		data.playerHardDamageUpgradeLevel = playerHardDamageUpgradeLevel;
		data.hitCombosAllowed=hitCombosAllowed;
		data.healthRegenerationLevel=healthRegenerationLevel;
		data.healthBackLevel=healthBackLevel;
//		data.playerHealth = playerHealth; 
//		data.playerDamage = playerDamage;
//		data.playerHardDamage = playerHardDamage;
		data.range = range;
		data.level = level;
		data.score = score;
		data.moneyInTheBank = moneyInTheBank;
		data.playerPosition = playerDeathPosition;
		data.playerRotation = playerLookAt;
		data.orbitKey = orbitKey;
		data.levelKey = levelKey;
		data.deathLevelKey = deathLevelKey;
		data.visitedOrbits = visitedOrbits;
        data.playerHasDied = playerHasDied;
		data.moneyOnDeadBody = moneyOnDeadBody;
		data.objectsToDisable = objectsToDisable;
		data.unlockedLevels = unlockedLevels;

		data.howManyTimesPlayerHasDied=howManyTimesPlayerHasDied;
		data.howManyTimesPlayerHasDiedAtThisLevel=howManyTimesPlayerHasDiedAtThisLevel;
		data.howManyEnemiesKilled=howManyEnemiesKilled;
		data.howManyEnemiesKilledAtThisLevel=howManyEnemiesKilledAtThisLevel;
		data.howManyOrbsEarnedInGame=howManyOrbsEarnedInGame;
		data.howManyOrbsEarnedInThisLevel=howManyOrbsEarnedInThisLevel;
        data.howManyPiecesSoFar = howManyPiecesSoFar;
        data.howManyPiecesInTotal = howManyPiecesInTotal;
    }

	public void LoadData(string _fileToLoad){
		
		if (File.Exists(Application.persistentDataPath + _fileToLoad)) {
			file = File.Open(Application.persistentDataPath + _fileToLoad,FileMode.Open); // load found file from the path into the generic file stream
			data = (PlayerData)bf.Deserialize(file); //cast whatever you found in the file as the playerdata object
			file.Close(); // close that bitch, you done!
			//change the values into the local variables
			SetVarsForLoading();
		} else {
			NewVars ();
		}
	}

	public void SaveData(string _saveFileName) {// this is using the global savefilename in loader to be called everywhere without setting
		data = new PlayerData(); // get a blank playerdata
		//load the current values in the object
		SetVarsForSaving();
		file = File.Create(Application.persistentDataPath + _saveFileName); //create the file with the specified file path (dynamic per platform)
		bf.Serialize(file,data);//serialize the object to the file we just created
		file.Close();//close that bitch, you done!
	}

	public void ErraseData(string _saveFileName) {
		if (File.Exists(Application.persistentDataPath + _saveFileName)){// if the file exists
			File.Delete(Application.persistentDataPath + _saveFileName);//delete file
		}
	}

	void SetVariables(){ // this runs on wake and figures shit out based on variables
		//enemy values the numbers are the base values
		enemyDamage = gameIteration * 10;
		enemyHealth = gameIteration * 100;
		//player values the numbers are the base values
		playerHealth = playerHealthUpgradeLevel * 100;
		playerDamage = playerDamageUpgradeLevel * 25;
		playerHardDamage = playerHardDamageUpgradeLevel * 100;
	}



}

