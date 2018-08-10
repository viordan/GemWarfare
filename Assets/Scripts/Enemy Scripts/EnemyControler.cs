using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour {
	
	GameControl control;
    GameObject player;
    Transform myTransform;
    float rangeToEnable;
//    EnemyScript myEnemy;
	//Transform myHeadsTransform;
	//bool startToEnable;
	float whenToEnable;
	float timerToEnable;
	Coroutine moveHead;
	Coroutine checkDistanceToPlayer;
	Coroutine enableEnemy;
	GameObject myEnemy;


    //public bool myEnemyIsDead;
    // Use this for initialization

	void OnEnable() {// on enable
		myTransform = transform;
		myEnemy=myTransform.GetChild(0).gameObject;

		player = PlayerScript.player.gameObject;
		control = GameControl.control;
		rangeToEnable = control.rangeToEnableEnemy;
//		myEnemy = myTransform.GetChild(0).GetComponent<EnemyScript>();
		whenToEnable = Random.Range (0, 5);
		if (myTransform.GetChild (0).gameObject.activeSelf) myTransform.GetChild (0).gameObject.SetActive (false);
		if (enableEnemy != null) StopCoroutine (enableEnemy); // if delegate exists stop it
		if (checkDistanceToPlayer != null) StopCoroutine (checkDistanceToPlayer);// if delegate exists stop it
		checkDistanceToPlayer=StartCoroutine(CheckDistanceToPlayer());//start looking by assigning delegate
    }

	public void SetInstantSpawn(){
		if (!myEnemy.activeSelf){
			whenToEnable = 0;
			if (checkDistanceToPlayer != null) StopCoroutine (checkDistanceToPlayer);// if delegate exists stop it
			checkDistanceToPlayer=StartCoroutine(CheckDistanceToPlayer());//start looking by assigning delegate
		}
	}

	IEnumerator CheckDistanceToPlayer(){
        print(gameObject.name + " I'm checking stuff and taking up memeory!!!");
		while (Vector3.Distance(myTransform.position, player.transform.position) > rangeToEnable) {//check the distance
			yield return null; // if not exit this loop and check it next time
		} //if you reached the end of the loop
		if (!control.firstInGroup)// this is to make sure that we have at least one enemy enebled
		{
			whenToEnable = 0; // if you're first, enable right away
			control.firstInGroup = true; // signal that someone is first
		}
		if (enableEnemy != null) StopCoroutine (enableEnemy);
		enableEnemy=StartCoroutine (EnableEnemy ());//start the enable countdown
		control.enemyControlerList.Add(gameObject);// add the controler to the list for the player
	}

	IEnumerator	EnableEnemy(){
		while (timerToEnable <= whenToEnable) {// when to enable is the delayed enabler
			timerToEnable += Time.deltaTime;// increment the 
			yield return null;
		}
		myEnemy.SetActive(true);
		control.enemyOrientationList.Add (myTransform.GetChild (0).gameObject);
	}

	public GameObject MyEnemy(){
		return myEnemy;
	}

	void OnDisable(){
		if (checkDistanceToPlayer != null) StopCoroutine (checkDistanceToPlayer);// if delegate exists stop it
        if (enableEnemy != null) StopCoroutine(enableEnemy);// if delegate to enable, stop it
    }

}
