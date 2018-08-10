using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Permutations.Utilities;



//GameControl : GameState : GameLogic : GameData : MonoBehavior
public class GameData : MonoBehaviour {
    public bool debug;

    public enum PlayerState {
		inPlanetOrbit,
        onTheSun,
        menuSelection,
        sateliteSelection,
        inBattle,
        adjustingCam,
		puzzleSolving,
        breadCrumbAction,
		breadCrumbFlying,
        pauseGame
    }

    public Core permutationsUtilitiesCore;
    public PlayerState currentPlayerState;
    protected PlayerState tempState;
    protected bool menuToggle;

    protected PlayerScript playerScript;
	protected DataLoader loader;
	protected SelectedPlanet selectedPlanet;
	protected GameObject planet;
	protected SelectedLevel selectedLevel;
    public PiecesController piecesController;


	//public bool animEnabled;
	protected float[] enemyDamageModifier;
	protected float[] enemyHealthModifier;
    public Text displayText;
	public Text scoreDisplay;

	protected SolarSystem solarSystem;


	protected Transform mainCameraTransform;
    protected Vector3 mainCameraLocalPosition;
    protected Quaternion mainCameraLocalRotation;
    protected Coroutine moveCamera;
    protected Coroutine rotateCamera;
    public Transform cameraMenuPosition;

    public GameObject bolt;
    public Transform boltTargetTransform;

    //external objects
    public GameObject player;
    public Transform playerTransform;

    protected float touchTimer = 0;
	protected float touchMouseTimer = 0;
    protected bool finishedTravelingCallBack;
	protected Touch touch0;

	protected float originalDistance;
	protected float actualDistance;

    protected bool dragEmpty;
    protected bool dragPlanet;
    protected float unpause;
	protected float speedMultiplier;

    //layers
    protected int levelExitLayer = 9;
    protected int levelLayer = 10;
    protected int planetLayer = 11;
    protected int backToMenu = 12;
    protected int moonLevelLayer = 13;
    protected int puzzleLayer=14;
    protected int puzzlePiecesLayer = 15;
    protected int menuButtonLayer = 16;

    protected int layerPlanetMask = 1 << 11;
    protected int layerMaskSphereCollider = 1 << 17;
    //public int j = 1;

    //duplicate control vars
    protected Ray ray;
	protected RaycastHit hit;
    //variables to save


	protected Coroutine goToPlanet;
	protected Coroutine goToLevel;
    public List<GameObject> targetsList;
	public List<GameObject> enemyControlerList;
	public List<GameObject> enemyEncounterList;
    public List<GameObject> enemyDeadInEncounterList;
	public List<GameObject> enemyOrientationList;
    public bool firstInGroup;

    public List<GameObject> listOfProjectiles;

    //range setup variables
    public float rangeToAttackEnemy;
    public float rangeToEnableEnemy;
    public float rangeForPlayerToStop;

	//public GameObject puzzle;
	//public bool draggingPuzzlePiece;
	//public GameObject chestInTown;
	//public ChestInTownScript chestInTownScript;

	protected bool exitedAnim;

	public void WakeVals(){
		enemyDamageModifier = new float[6] {1f,1.5f,2f,2.5f,3f,3.5f};
		enemyHealthModifier = new float[6] {1f,1.5f,2f,2.5f,3f,3.5f};
		currentPlayerState = PlayerState.inPlanetOrbit;

    }
	public void StartVals(){
        //permutationsUtilitiesCore = new Core();
        permutationsUtilitiesCore = gameObject.AddComponent<Core>();
        playerScript = PlayerScript.player;
		loader = DataLoader.loader;
		solarSystem = new SolarSystem ();
        piecesController = PiecesController.piecesController;

        mainCameraTransform = Camera.main.transform;

		rangeToAttackEnemy = 3;
		rangeToEnableEnemy = 3;
		rangeForPlayerToStop = 1;
        //currentPlayerState = PlayerState.menuSelection;

        //      chestInTown = transform.GetChild (0).GetChild(1).gameObject;
        //chestInTownScript = chestInTown.GetComponent<ChestInTownScript> ();
        player = PlayerScript.player.gameObject;
		playerTransform = player.transform;
		listOfProjectiles = new List<GameObject> ();
		planet = solarSystem.planetList [0];
        layerPlanetMask = ~layerPlanetMask;
        layerMaskSphereCollider = ~layerMaskSphereCollider;
       // SwipeManager.swipeManger.SetMinSwipeLength(.1f);

    }


    public void MoveObject(Transform objectToMoveTransform,Vector3 originalPosition, Vector3 moveTo, float moveSpeedModifier) {
        if (objectToMoveTransform.position != moveTo) {
            objectToMoveTransform.position = Vector3.MoveTowards(originalPosition, moveTo, Time.deltaTime * moveSpeedModifier);
        } else {
			//print ("finished travel");
            finishedTravelingCallBack = true;
			exitedAnim = false;
        }
    }

    public void PauseGame() {
        if (Time.timeScale == 1.0f) {
            Time.timeScale = 0f;
            tempState = currentPlayerState;
            currentPlayerState = PlayerState.pauseGame;
        } else {
            Time.timeScale = 1.0f;
            currentPlayerState = tempState;
        }
    }



    //public Vector3 Array3ToVector3(float[] arrayToConvert) {
    //    Vector3 result = new Vector3();
    //    result.x = arrayToConvert[0];
    //    result.y = arrayToConvert[1];
    //    result.z = arrayToConvert[2];
    //    return result;
    //}

    //public float[] Vector3ToArray3(Vector3 toChange) {
    //    float[] result = new float[3];
    //    result[0] = toChange.x;
    //    result[1] = toChange.y;
    //    result[2] = toChange.z;
    //    return result;
    //}

    public void DisplayScore(int score) {
        scoreDisplay.text = score.ToString();
    }

    public void DisplayTheText(string key) {
        string temp = TextData.text.GetText(key.ToLower());
        FadeText(temp);
    }

    public void FadeText(string text) {
        displayText.text = "";
        displayText.canvasRenderer.SetAlpha(1f);
        displayText.text = text;
        displayText.CrossFadeAlpha(0, 2f, false);
    }

	public static void ShuffleArray<T>(T [] arr){// shuffle array of any type call by ShuffleArray<int>(array)
		for (int i = arr.Length - 1; i > 0; i--) { 
			int r = Random.Range (0, i + 1); // get r between the 0 and i+1
			T tmp = arr [i]; // type of array tmp var = the iteration location in array
			arr [i] = arr [r]; //swap the iteration for the random value
			arr [r] = tmp; // swap the random value for the iteration
		}

	}

    public void RotateObject(Transform objectToRotate,float directionX,float directionY,Space space) {
        objectToRotate.Rotate(mainCameraTransform.up * directionX,space); // rotate this object's transform by however the camera is located-up (or y axis) *speed*rotation speed, in the world's coordinates
        objectToRotate.Rotate(mainCameraTransform.right * directionY,space);// see above for right (x ) axis
    }
}
