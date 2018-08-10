using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnemyScript : CoreActorScript {


	//animations trigger hashes

	public int myPlaceInGroup;
    public bool hitBlockedActive = false;
    public double chanceToHit;
	public double doNothing;
	//public float holdYourHorses;
    public GameObject player;
    public float distanceToPlayer;
	public bool inRange = false;
    //public Canvas myCanvas;
    public Text damageText;
    public float textTimer;
    public bool textIsDisplayed;
    public EnemyControler myControler;
    
	public int myModifier;
	public GameObject myProjectile;
	public string whoAmI;
	PlayerScript playerScript;
	public int scoreValue;
	public int myHealPower;

//	Coroutine moveProjectile;
	Transform projectileTransform;
	public bool isProjectileSpecial;

    void OnEnable() {
		playerScript = PlayerScript.player;
        player = PlayerScript.player.transform.gameObject; //cache the player gameobject
        CommonStartup ();// run the common startup from CoreActorScript
		//find out what I am
		IdentifyType();
		//get my variables
		myDamage = loader.enemyDamage * myModifier; // figure out what my damage is *** consider moving all modifiers to this level
		mySpecialDamage=myDamage*2; //*** revisit this set
		maxHealth = loader.enemyHealth * myModifier; // figure out the health *** consider moving all modifiers to this level
		curHealth = maxHealth; // current health is maxhealth
		if (listOfChildrenDisabled.Count>0) TransferBetweenLists (listOfChildrenDisabled.Count); // if there are any disabled children enable all of them this is revive logic
		if (!myHead.activeSelf) { // if my head is not active, set it active, again this is revive logic
			myHead.SetActive (true);
		}
		heDead = false; // make sur the heDead bool is false
		scoreValue=loader.gameIteration;
		activeBlocking = true; // default active blocking to true
        myHeadsTransform = myHead.transform; // cache the myHead transorm
        myControler = myTransform.parent.GetComponent<EnemyControler>(); // cache my controler's script
		if (!anim.isInitialized) anim.Rebind (); // rebind the animator in case it cached any prior state this is revive logic
		inRange=false; //reset range flag for revive logic
		projectileTransform=myProjectile.transform;
		myHead.SetActive (true);
		myHealPower = 10 * loader.gameIteration;
    }

	void IdentifyType(){
		if (anim.name.ToLower ().Contains ("topaz")) {// get the modifier per type look to the anim to get the type
			myModifier = 2;
			whoAmI = "topaz";
		} else if (anim.name.ToLower ().Contains ("spinel")) { 
			myModifier = 2;
			whoAmI = "spinel";
		}else if (anim.name.ToLower ().Contains ("ruby")) { 
			myModifier = 2;
			whoAmI = "ruby";
		}else if (anim.name.ToLower ().Contains ("quartz")) { 
			myModifier = 2;
			whoAmI = "quartz";
		}else if (anim.name.ToLower ().Contains ("blood")) { 
			myModifier = 2;
			whoAmI = "bloodstone";
		}else if (anim.name.ToLower ().Contains ("zircon")) { 
			myModifier = 2;
			whoAmI = "zircon";
		}else {
			myModifier = 3;
		}
	}

    void Update() {
		if (doNothing > 0) { // reduce pause
			doNothing -= Time.deltaTime;
		}
        print(gameObject.name+" I'm alive and taking up memeory");
        //Attack AI
        if (!heDead) { //if you're not dead
            if (inRange) { // if in range
				if (!wait && doNothing<=0) { // if no wait and no pause
					TryToHit ();
				}
            }
			if (textIsDisplayed) {
				MoveText();
			}
        }
    }

    public override void CreateMyChildrenList() {
        PopulateChildrenToDisable(gameObject, "1st");
        PopulateChildrenToDisable(gameObject, "2nd");
        PopulateChildrenToDisable(gameObject, "3rd");
    }

    public void HitBlockedAnimation() {
		if (!activeBlocking && !hitBlockedActive) {
            stunned = true;
			wait = true;
            anim.SetTrigger(hitBlockedHash);
        }
        else
        {
            anim.SetTrigger(blockHash);
        }
    }

    public void HitBlockedActiveEvent() {
        hitBlockedActive = true;
    }

    public void TryToHit() {
        chanceToHit = Random.Range(0, 100);
        if (chanceToHit >= 0) {
			if (chanceToHit >= 90) {
				//print ("hitting normal!" + chanceToHit);
				anim.SetTrigger (hit1Hash);
			} else {
				//print ("hitting special!" + chanceToHit);
				anim.SetTrigger (hitSpecialHash);
			}

        } else {
			//print ("doing nothing! " + chanceToHit);
			doNothing=SetDoNothingDelay();
			anim.SetTrigger (idleHash);
        }
    }

	public double SetDoNothingDelay(){
		return Random.Range(1.2f, 3.5f); // control pause if you roll a pause
	}

    public void DeathEvent() {
        gameObject.SetActive(false);
		if (control.targetsList.Count >= 1) {//if you are not the last enemy
			playerScript.ReassessOrientation (); // reorient yourself.
		}
		ResetText(); // reset the text, again for revive logic
    }

    public override void Death() {
        heDead = true;
		doNothing = 10f;
        control.targetsList.Remove(gameObject);
		control.enemyOrientationList.Remove (gameObject);
        myDeathExplosionSystem.Play(false);
		StartMovingHead ();
		myHead.SetActive (false);
        anim.SetTrigger(deathHash);
    }

	public override void AnimationEndedEvent() {
		wait = false;
		activeBlocking = true;
		hitBlockedActive = false;
        stunned = false;
	}

	public override void BlockEvent() {
		activeBlocking = true;
    }

    public void SpawnEndedEvent()
    {
        inRange = true;
		control.targetsList.Add(gameObject); // add this gameObject to the list in the game control
		playerScript.ReassessOrientation ();
		AnimationEndedEvent ();
    }

    public override void IHitEnemyEvent() {
		playerScript.ImBeingHit(myDamage);
	}

	public void IHitSpecialEnemyEvent() {
		//PlayerScript.player.ImBeingHitSpecial(mySpecialDamage);
		EnemySpecialAttacks thisSpecialAttack = new EnemySpecialAttacks(whoAmI,gameObject);
	}

	public override void ImBeingHit(float damage) {
		if (!activeBlocking) {
			AddjustCurrentHealth(-damage);
            if (!stunned&&!heDead) //this flag is so that stunned doesn't get played twice in a row if hit while another stun is in progress
            {
                anim.SetTrigger(stunHash);
                stunned = true;
            }
			DisplayText("-" + DataLoader.loader.playerDamage.ToString(), Color.green);
		} else {
            DisplayText("Blocked", Color.red);
            anim.SetTrigger(blockHash);
		}
	}

	public void StartMovingProjectile(){
		GameObject projectileCopy =	Instantiate (myProjectile, projectileTransform.position, projectileTransform.rotation);
		projectileCopy.GetComponent<EnemyProjectileScript> ().SetVars (gameObject, isProjectileSpecial);
		isProjectileSpecial = false;
	}

	void StartMovingHead(){
		GameObject headCopy =	Instantiate (myHead, myHead.transform.position, myHead.transform.rotation);
		headCopy.GetComponent<EnergyOrbLogic> ().SetOrbValues (scoreValue,myControler.gameObject);
	}

	public void ImBeingHitSpecial(float damage) {
		AddjustCurrentHealth(-damage);
		if (!stunned&&!heDead) //this flag is so that stunned doesn't get played twice in a row if hit while another stun is in progress
		{
			anim.SetTrigger(stunHash);
			stunned = true;
		}
		DisplayText("-" + damage.ToString(), Color.green);
	}

	public void HealEvent(){
		foreach (GameObject enemyControler in control.enemyEncounterList) {
			GameObject tempEnemy = enemyControler.GetComponent<EnemyControler> ().MyEnemy ();
			tempEnemy.GetComponent<EnemyScript> ().AddjustCurrentHealth (myHealPower);
		}
	}

	public void ReviveEnemyEvent (){
		if (control.enemyDeadInEncounterList.Count > 0) {
			int toRevive = Random.Range(0, control.enemyDeadInEncounterList.Count - 1); // get random element from list
			GameObject enemyToRevive=control.enemyDeadInEncounterList[toRevive]; // get the enemy;
			enemyToRevive.SetActive(true); // enable the enemy
			enemyToRevive.GetComponent<EnemyControler> ().SetInstantSpawn (); //spawn rightaway0
			if (!control.enemyControlerList.Contains(enemyToRevive)) control.enemyControlerList.Add (enemyToRevive);// if not in list add back to encounter
			control.enemyDeadInEncounterList.Remove(enemyToRevive); // remove from dead enemys list 

		} else {
			anim.SetTrigger (hit1Hash);
		}
	}


    public void DisplayText(string damage, Color colorValue) { // display the text
        ResetText(); // reset the values (position, value, alpha)
        damageText.color = colorValue;
        damageText.text = damage; // make it whatever the damage is (value passed in the function)
        damageText.CrossFadeAlpha(0, 0.7f, false); // fade the alpha to 0, for two seconds, consider time scale, setting it to true ignores time scale... I know, it's stupid but it's unity)
        textIsDisplayed = true; // start displaying the text, this is so the update can start showing it
    }

    public void MoveText() {
        if (damageText.transform.localPosition.y <= -.2f) { // if the y on the local position is less than -.1f
            damageText.transform.localPosition += new Vector3(0, 0.001f, 0); // increase the y
            damageText.transform.localScale += new Vector3(0.00005f, 0.00005f, 0);
        } else {
            textIsDisplayed = false; // when you get there, stop displaying the text 
        }
    }

    public void ResetText() {
        damageText.text = ""; // text is blank
        damageText.transform.localPosition = new Vector3(0.016f, -0.424f, 0); // text is positioned whereever the position is
        damageText.transform.localScale = new Vector3(0.0025f, 0.0025f, 0);
        damageText.canvasRenderer.SetAlpha(1f); // reset alpha to visibiilty
    }




}