using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript:MonoBehaviour{

	GameControl control;
	GameObject enemyInstantiatingProjectlie;
	EnemyScript enemyScript;
	PlayerScript playerScript;
	GameObject player;
	//GameObject myProjectile;
	Transform myTransform;
	Transform projectileParent;
	Vector3 projectileLocalPosition;
	Coroutine moveProjectile;
	bool isSpecial;
	bool inList;

	void OnEnable(){
		control = GameControl.control;
		myTransform = transform;
		playerScript = PlayerScript.player;
		player = playerScript.gameObject;

	}

	public void SetVars(GameObject _enemy,bool _special){
		isSpecial = _special;
		enemyInstantiatingProjectlie = _enemy;
		enemyScript = enemyInstantiatingProjectlie.GetComponent<EnemyScript> ();
		gameObject.SetActive (true);
		StartMovingProjectile ();
	}


	void StartMovingProjectile(){
		StartProjectileCoroutine(player.transform);
	}

	IEnumerator MoveProjectleTowardsPlayer(Transform projectileDestination){ // send projectile
		while (Vector3.Distance (myTransform.position, projectileDestination.position) >= 0.1f) { // while the projectile hasn't reached destination
			if (Vector3.Distance (myTransform.position, playerScript.transform.position) <= 2f && !inList) { // if at this range add to list of projectiles set in inlist as true to only add once
				inList = true;
				control.listOfProjectiles.Add (gameObject);
			}
			myTransform.position = Vector3.MoveTowards (myTransform.position, projectileDestination.position, Time.deltaTime*.2f); // move towards destiantion
			yield return null; // pause till next frame
		}//you reached the player
		if (enemyInstantiatingProjectlie.activeSelf){ // if the enemy is still alive
			if (isSpecial) { // if this is special attack
				projectileDestination.SendMessage ("ImBeingHitSpecial", enemyScript.mySpecialDamage); // send destination with special damage
			} else {
				projectileDestination.SendMessage ("ImBeingHit", enemyScript.myDamage); // otherwise normal damage for normal projectiles
			}
		}
		DestroyProjectile (); // destory the projectile
	}

	void StartProjectileCoroutine(Transform destination){ // delagate the coroutine 
		if (moveProjectile != null) {
			StopCoroutine (moveProjectile);
		}
		moveProjectile = StartCoroutine (MoveProjectleTowardsPlayer (destination));
	}

	public void ChangeProjectileTarget(){
		StartProjectileCoroutine(enemyScript.myTransform); // change destination if bounced back to the enemy that created it
	}

	public void DestroyProjectile(){
		control.listOfProjectiles.Remove (gameObject); // remove from list
		Destroy (gameObject); // destroy this object which is a copy.
	}
}
