using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerScript : CoreActorScript {

    public static PlayerScript player; // this allows you to use the script from any scene where the object is present.

	Coroutine moveToNextBreadcrumb;
	Coroutine rotateMe;
	Coroutine regenerateCoroutine;

    public int test;

	public int hit2Hash = Animator.StringToHash("Hit2");
	public int hit3Hash = Animator.StringToHash("Hit3");
	public int hit4Hash = Animator.StringToHash("Hit4");
	public int hit5Hash = Animator.StringToHash("Hit5");

	public int hardHitHash = Animator.StringToHash("HardHit");
    public int walkHash = Animator.StringToHash("Walk");
    public int flyHash = Animator.StringToHash("Fly");
    public int flyOldHash = Animator.StringToHash("FlyOld");
	public int flyExitHash = Animator.StringToHash("FlyExit");
	public int defaultHash = Animator.StringToHash("Default");

    public bool allowedToHit;
    public bool okToMove;
	//vars for orientation
	public Transform myMarker;
	Vector3 measureTo;
	Vector3 defaultFacing;
	List<float> distancesToAssess;

	public SelectedLevel selectedLevel;
	float breadCrumbMoveSpeed;
	float breadCrumbRotateSpeed;

	int myHardDamage;

	public int j = 1;

	int comboCounter=1;

	bool animationEnabled;// ***Remove this, this is to prevent from calling the animation multiple times




    void Awake() {//singleton logic
        if (player == null) { //if there is not a player already in this scene
            player = this; // the player is this object
        }
    }
    void Start() {

        CommonStartup ();
		activeBlocking = false;
//		multiplier = 100;
		myDamage = loader.playerDamage;
		myHardDamage = loader.playerHardDamage;
		maxHealth = loader.playerHealth;
		curHealth = maxHealth;
		if (loader.healthRegenerationLevel>0){
			regenerateCoroutine = StartCoroutine (RegenerateHealth ());
		}
		breadCrumbMoveSpeed = 3f;
		breadCrumbRotateSpeed = 80f;

    }

    void Update() {
		Debug.DrawRay (myTransform.position, myTransform.forward, Color.red);

		//print( AssessOrientation ());
		if(control.currentPlayerState.ToString()=="inBattle"){
			if (control.enemyControlerList.Count > 0) { // if it's time to think
				for (int i = 0; i < control.enemyControlerList.Count; ++i) {// through the results of enemyControlers in range list
					float disToEnemy = Vector3.Distance (myTransform.position, control.enemyControlerList [i].transform.position);  
					if (disToEnemy <= control.rangeForPlayerToStop) {
						if (okToMove) {
							OkToMove (false);
						}
					}
				}
			}else {// if the list of controlers in range is 0 or less
				if (!okToMove){
					OkToMove(true); // go ahead and move
				}
				control.firstInGroup = false; // place in groupholder is 0
			}
		}

    }

    public void PlayerAttack() {
        if (allowedToHit) {
            if (!wait) {
                StartHitAnimation();
            }
        }
    }

    public void PlayerBlock() {
        if (allowedToHit) {
            if (!wait) {
                BlockEvent();// do the block
            }
        }
    }

    public void PlayerHardHit() {
        if (allowedToHit) {
            if (!wait) {
                anim.SetTrigger(hardHitHash);
            }
        }
    }


	public void OkToMove(bool value){
		
		if (value) {
			//OrientationParenting ();
			control.targetsList= new List<GameObject> ();
			control.enemyOrientationList= new List<GameObject> ();
			okToMove = true;
			animationEnabled = false;
			//from follow breadcrumbs
			if (moveToNextBreadcrumb != null) {
				StopCoroutine (moveToNextBreadcrumb);
			}
			moveToNextBreadcrumb = StartCoroutine (MoveToNextBreadcrumb ());
			if (rotateMe != null) {
				StopCoroutine (rotateMe);
			}
			//print ("lenght of list is " + control.enemyControlerList.Count);
		} else {
			okToMove = false;
			GetMarkerPosition();
			AnimationByHash (idleHash);
			if (moveToNextBreadcrumb != null) {
				StopCoroutine (moveToNextBreadcrumb);
			}
			//ReassessOrientation ();
			//reset state
			wait=false;
			activeBlocking = false;
			//checkIfHit = false;
			stunned = false;
			control.enemyEncounterList= new List<GameObject>(control.enemyControlerList);
			control.enemyDeadInEncounterList = new List<GameObject> ();

		}
	}

	public IEnumerator MoveToNextBreadcrumb(){
		while (myTransform.position != selectedLevel.lastBreadCrumbPosition) { // if you haven't reached the last breadcrumb
			if (myTransform.position == selectedLevel.breadCrumbsTrasfrom.GetChild (j).position) { // j is always 1 ahead of the player (next marker) since it starts as 1.  if you reach the next marker.
				breadCrumbMoveSpeed = selectedLevel.breadCrumbsTrasfrom.GetChild (j).GetComponent<BreadCrumbInfo> ().moveSpeed; // set the movespeedmodifier to whatever that breadcrumb has
				breadCrumbRotateSpeed = selectedLevel.breadCrumbsTrasfrom.GetChild (j).GetComponent<BreadCrumbInfo> ().rotateSpeed; // set the rotatepeedmodifier to whatever that breadcrumb has
				j++;//increment the breadcrumb
			}
			else if (selectedLevel.breadCrumbsTrasfrom.GetChild (j - 1).name.ToLower ().Contains ("fly")) {//if prior breadcrumb name contains "fly"
				myTransform.position = Vector3.MoveTowards (myTransform.position, selectedLevel.breadCrumbsTrasfrom.GetChild (j).position, Time.deltaTime * breadCrumbMoveSpeed); //move towards next breadcrumb
				myTransform.rotation = Quaternion.RotateTowards (myTransform.rotation, selectedLevel.breadCrumbsTrasfrom.GetChild (j).rotation, Time.deltaTime * breadCrumbRotateSpeed);//rotate towards next breadcrumb
				//selectedLevel.priorBreadCrumb = selectedLevel.breadCrumbsTrasfrom.GetChild (j - 1);//set prior breadcrumb this is for distance measurement
				//selectedLevel.nextBreadCrumb = selectedLevel.breadCrumbsTrasfrom.GetChild (j);//set next breadcrumb this is for distance measurement
				if (!animationEnabled) {
					print ("I'm Starting to fly");
					AnimationByHash (flyOldHash);
					animationEnabled = true;
				}

			} else {// otherwise just move normally between the breadcrumbs.
				myTransform.position = Vector3.MoveTowards (myTransform.position, selectedLevel.breadCrumbsTrasfrom.GetChild (j).position, Time.deltaTime * breadCrumbMoveSpeed); //move towards next breadcrumb
				myTransform.rotation = Quaternion.RotateTowards (myTransform.rotation, selectedLevel.breadCrumbsTrasfrom.GetChild (j).rotation, Time.deltaTime * breadCrumbRotateSpeed);//rotate towards next breadcrumb
				if (!animationEnabled) {
					//print ("I'm Starting to walk");
					AnimationByHash (walkHash);
					animationEnabled = true;
				}
			}
			yield return new WaitForEndOfFrame();
		} 
		AnimationByHash (idleHash);
		print ("level end");
	}

	public void StopMoveToNextBreadcrumb(){// this needs to exist in the same script as the coroutine to avoid a warning as error
		if (moveToNextBreadcrumb != null) {
			StopCoroutine (moveToNextBreadcrumb);
		}

	}
	// this spins off a different thread to run in the background The coroutine has to be started and stoped from the main thread
	// when using startcoroutine first delegate the coroutine in memory otherwise you cannot stop it and the coroutine does 
	// not stop.
	public IEnumerator RegenerateHealth(){
		while (true){
			while (curHealth < maxHealth) {
				AddjustCurrentHealth (maxHealth * .01f * loader.healthRegenerationLevel);
				yield return new WaitForSeconds (1f);
			}
			yield return null;
		}
	}
		
    public override void CreateMyChildrenList() {
        PopulateChildrenToDisable(gameObject, "b1");
        PopulateChildrenToDisable(gameObject, "b2");
        PopulateChildrenToDisable(gameObject, "b3");
        PopulateChildrenToDisable(gameObject, "b4");
    }

	public override void AnimationEndedEvent() {
		wait = false;
		//checkIfHit = false;
		stunned = false;
		comboCounter = 1;
		activeBlocking = false;
	}

	public void StartHitAnimation(){//combo logic
		print ("combo counter " + comboCounter);
		switch (comboCounter) {
		case 1:
			anim.SetTrigger (hit1Hash);
			break;
		case 2:
			anim.SetTrigger (hit2Hash);
			break;
		case 3:
			anim.SetTrigger (hit3Hash);
			break;
		case 4:
			anim.SetTrigger (hit4Hash);
			break;
		case 5:
			anim.SetTrigger (hit5Hash);
			break;
		}
	}

	public void CanNoLongerComboEvent(){
		wait = true;
		comboCounter = 1;
		print ("resetting combocounter");
	}
	public void ICanComboEvent(){
		wait = false;
		comboCounter++;
		print ("incrementing combocounter to "+comboCounter);
	}

    public override void IHitEnemyEvent() {
		ICanComboEvent ();
		foreach (GameObject target in control.targetsList.ToArray()){
			target.GetComponent<EnemyScript>().ImBeingHit(myDamage);
        }
		foreach (GameObject projectile in control.listOfProjectiles) {
			projectile.SendMessage ("ChangeProjectileTarget");
		}
    }

	public void IHitEnemyHardEvent() {
		foreach (GameObject target in control.targetsList.ToArray()){
			target.GetComponent<EnemyScript>().ImBeingHitSpecial(myHardDamage);
		}
		foreach (GameObject projectile in control.listOfProjectiles) {
			projectile.SendMessage ("ChangeProjectileTarget");
		}
	}

	public override void ImBeingHit(float damage) {
		if (!activeBlocking) {
           // checkIfHit = true;
            anim.SetTrigger(stunHash);
            //takingDamage = true;
            //explosion.Play();
            AddjustCurrentHealth(-damage);
            if (!CameraShake.shaking) {
                CameraShake.Shake(0.1f, .005f);
            }
        } else {
            //CameraShake.Shake(0.25f, .45f);
            //for (int i = 0; i < targetsList.Count; ++i)  {// through the results of coliders in range array
			foreach (GameObject target in control.targetsList.ToArray()){
                target.GetComponent<EnemyScript>().HitBlockedAnimation();
            }
        }
    }

	public void ImBeingHitSpecial(float damage) {
		//checkIfHit = true;
		anim.SetTrigger(stunHash);
		//takingDamage = true;
		//explosion.Play();
		AddjustCurrentHealth(-damage);
		if (!CameraShake.shaking) {
			CameraShake.Shake(0.1f, .005f);
		}
	}

	public void ReassessOrientation() {
		
		if (rotateMe != null) {
			print ("stoping orientation");
			StopCoroutine (rotateMe);
		}
		print ("starting orientation");
		rotateMe=StartCoroutine(RotateMe(AssessOrientation()));

	}
//		if (myTransform.rotation!=rotateTo.rotation){
//			myTransform.rotation = Quaternion.RotateTowards(myTransform.rotation, rotateTo.rotation, Time.deltaTime * multiplier);// turn slowly towards it
//		}else{
//			myOrientation = PlayerOrientation.none;
//		}
//
//	}

	public void GetMarkerPosition(){
		measureTo = myMarker.position;
		defaultFacing = myTransform.forward;
	}

	public Vector3 AssessOrientation(){
		distancesToAssess = new List<float> ();
		if (control.targetsList.Count > 1) {// if there's more than one target
			Vector3 minVector3=Vector3.zero; // temporary min target
			Vector3 maxVector3=Vector3.zero; // temporary max target
			foreach (GameObject target in control.targetsList) { // iterate
				float distanceToMarker = Vector3.Distance (measureTo, target.transform.position); // get the distance from target to marker
				distancesToAssess.Add (distanceToMarker); // add to list
				if (distanceToMarker == distancesToAssess.Min ()) { // if the distance is the smallest 
					minVector3 = target.transform.position; // this is the min target
				}
				if (distanceToMarker == distancesToAssess.Max ()) { // if the distance is the largest
					maxVector3 = target.transform.position; // this is the max target
				}
			}
			return (minVector3 + maxVector3) / 2; // return midpoint
		} else if (control.targetsList.Count == 1) {
			return control.targetsList [0].transform.position;
		} else {
			return defaultFacing;
		}
	}

	IEnumerator RotateMe(Vector3 target){
		while (AngleValue (target) >= 5f) {
			//print (AngleValue());
			float dir = 0;// get the rotation direction as 0
			if (AngleDir (myTransform.forward, (target - myTransform.position), myTransform.up) == 1f) {
				dir = 1; // if on right it is 1
			} else {
				dir = -1; // else it's negative
			}
			myTransform.RotateAround (myTransform.position, myTransform.up, dir * 100f * Time.deltaTime);// this does the rotate
			yield return null;
		} 
		print ("Ended orientation");
	}

	float AngleValue(Vector3 target){
		//project on plane it's a perpendicular projection through a plane going through (0,0,0) and with the "normal" heading of myTransform.up aka (0,1,0)
		Vector3 projection = Vector3.ProjectOnPlane (target, myTransform.up);
		Vector3 myProjection = Vector3.ProjectOnPlane (myTransform.position, myTransform.up);
		Vector3	targetDir = (projection - myProjection).normalized; 
		//get the dot (multiplication of vectors)
		float dot = Vector3.Dot (targetDir, myTransform.forward);
		//return the angle value
		return Mathf.Acos (dot) * Mathf.Rad2Deg;
	}

	float AngleDir(Vector3 forward, Vector3 targetDir, Vector3 up){ // this functions returns -1 for left 1 for right
		Vector3 perp = Vector3.Cross (forward, targetDir);
		float dir = Vector3.Dot (perp, up);
		if (dir > 0f) {
			return 1f;
		} else if (dir < 0f) {
			return -1f;
		} else {
			return 0f;
		}
	}
}