using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpecialAttacks{

	GameObject thisGameObject;
	PlayerScript playerScript;
	EnemyScript enemyScript;

	public EnemySpecialAttacks(string _typeOfEnemy, GameObject _thisGameObject){
		playerScript = PlayerScript.player;
		thisGameObject = _thisGameObject;
		enemyScript=thisGameObject.GetComponent<EnemyScript> ();
		SpecialAttack (_typeOfEnemy);
	}

	void SpecialAttack(string typeOfEnemy){
		switch (typeOfEnemy) {
		case ("topaz"):
			Topaz ();
			break;
		case ("spinel"):
			Spinel ();
			break;
		case ("bloodstone"):
			Bloodstone ();
			break;
		case ("zircon"):
			Zircon ();
			break;
		default:
			break;
		}
	}

	void Topaz(){
		playerScript.ImBeingHitSpecial(enemyScript.mySpecialDamage);
	}

	void Spinel(){
		enemyScript.isProjectileSpecial = true;
		enemyScript.StartMovingProjectile();
	}

	void Bloodstone (){
		enemyScript.HealEvent ();
	}

	void Zircon (){
		enemyScript.ReviveEnemyEvent ();
	}

}
