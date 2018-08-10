using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeScript : MonoBehaviour {

	DataLoader loader;
	PlayerScript player;
	GameControl controler;
	int healthUpgradeCost;
	int damageUpgradeCost;
	// Use this for initialization
	void Start () {
		loader = DataLoader.loader;
		player = PlayerScript.player;
		controler = GameControl.control;
		healthUpgradeCost = 1000;
		damageUpgradeCost = 1000;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp(KeyCode.N)) { // if you press S
			UpgradeHealth();
		}
		if (Input.GetKeyUp(KeyCode.M)) { // if you press S
			UpgradeDamage();
		}
	}

	void UpgradeHealth(){
		if (loader.score >= healthUpgradeCost * loader.playerHealthUpgradeLevel* loader.playerHealthUpgradeLevel) {// check the cost
			loader.score -= healthUpgradeCost * loader.playerHealthUpgradeLevel* loader.playerHealthUpgradeLevel;
			//increase the value
			loader.playerHealthUpgradeLevel++;
			player.maxHealth = loader.playerHealthUpgradeLevel * 100;
			player.curHealth = player.maxHealth;
			loader.SaveData (loader.saveFileName);
			controler.DisplayScore (loader.score);
		} else {
			print ("you broke!");
		}

	}

	void UpgradeDamage(){
		if (loader.score >= damageUpgradeCost * loader.playerDamageUpgradeLevel* loader.playerDamageUpgradeLevel) {// check the cost
			loader.score -= damageUpgradeCost * loader.playerDamageUpgradeLevel* loader.playerDamageUpgradeLevel;
			//increase the value
			loader.playerDamageUpgradeLevel++;
			player.myDamage = loader.playerDamageUpgradeLevel * 25;
			loader.SaveData (loader.saveFileName);
			controler.DisplayScore (loader.score);
		} else {
			print ("you broke!");
		}
	}

	public void UpgradeHardDamage(){
		loader.playerHardDamageUpgradeLevel++;
		loader.SaveData (loader.saveFileName);
	}
}
