using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyOrbLogic : MonoBehaviour {
   
	Transform playerTransform;
	DataLoader loader;
	GameControl control;
	int scoreValue;
	Transform myTransform;
	GameObject enemyControler;

	public void SetOrbValues(int _scoreValue,GameObject _enemyControler){
		if (!gameObject.activeSelf)	gameObject.SetActive (true);
		control = GameControl.control;
		loader = DataLoader.loader;
		playerTransform = PlayerScript.player.transform;
		myTransform = transform;
		scoreValue = _scoreValue;
		enemyControler = _enemyControler;
		StartCoroutine (MoveHead ());
	}

	IEnumerator MoveHead(){
		while (Vector3.Distance(myTransform.position,playerTransform.position) >=.1f){
			myTransform.position= Vector3.MoveTowards(myTransform.position,playerTransform.position,Time.deltaTime);
			yield return null;
		}
		loader.score += scoreValue;
		control.DisplayScore (loader.score);
		control.enemyDeadInEncounterList.Add (enemyControler);// add to dead enemies
		control.enemyControlerList.Remove (enemyControler);
		//enemyControler.SetActive (false);
		Destroy(gameObject);
	}
}
