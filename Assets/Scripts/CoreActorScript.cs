using System.Collections.Generic;
using UnityEngine;

public class CoreActorScript : MonoBehaviour {

    public int hit1Hash = Animator.StringToHash("Hit");
    public int stunHash = Animator.StringToHash("Stun");
    public int blockHash = Animator.StringToHash("Block");
    public int deathHash = Animator.StringToHash("Death");
	public int idleHash = Animator.StringToHash("Idle");
	public int healHash = Animator.StringToHash("Heal");
    public int healBlockedHash = Animator.StringToHash("HealBlocked");
	public int hitBlockedHash = Animator.StringToHash("HitBlocked");
	public int hitSpecialHash = Animator.StringToHash("HitSpecial");

    public Transform myTransform;
    public Animator anim;
    public bool heDead = false;

    public bool activeBlocking;
    public bool wait;
    //public bool checkIfHit;
    public bool stunned;

    public float maxHealth;
    public float curHealth;
	public int myDamage;
	public int mySpecialDamage;

    //public GameObject explosionContainer;

    //public List<GameObject> targetsList;
    public GameControl control;
    public List<GameObject> listOfChildren;
    public List<GameObject> listOfChildrenDisabled;
    public GameObject myHead;
    public Transform myHeadsTransform;
	public Transform myHeadsParent;	
    public float healthPerPiece;
    public DataLoader loader;
    public ParticleSystem myExplosionSystem;
    public ParticleSystem myDeathExplosionSystem;

    public void CommonStartup() {
        loader = DataLoader.loader;
        //explosion = PlayerScript.player.transform.GetChild(2).GetChild(1).GetComponent<ParticleSystem>();
        myTransform = transform;
        anim = GetComponent<Animator>();
        control = GameControl.control;
        //targetsList = controler.targetsList;
        CreateMyChildrenList();// get all the children to be disabled
        healthPerPiece = HealthPerPiece();// calculate how much health each piece is worth
        CheckCurrentHealth();// this adjusts your body to match the current health, disables all the children accordingly 
		//use the above if you want an enemy to have partial health at wake and the body to match the health.
    }

    public virtual void CreateMyChildrenList() {
        //once the models have been renamed, change this into non virtual function
    }

    public void CheckCurrentHealth() {
        if (curHealth <= maxHealth) { // if your current health is less then max health
            TransferBetweenLists((int)((curHealth - maxHealth) / healthPerPiece)); // remove enough children to match your current health
        }
    }

    public void AddjustCurrentHealth(float adj) {
		//if (maxHealth < 1) maxHealth = 1; // max health is at least 1.
		curHealth += adj; // add to curHealh (negative for damage)
		if (adj < 0) {
			myExplosionSystem.Play (false);
			if (curHealth <= 0) {
				Death();// if your health is 0 or less, you ded!
			}else{
				if (listOfChildren.Count>0) TransferBetweenLists ((int)(adj / healthPerPiece)); // if there are any disabled children enable all of this is to get rid of rounding issues for last piece on heal
			}

		} else {
			if (curHealth >= maxHealth) {
				curHealth = maxHealth; // don't exceed max health
				if (listOfChildrenDisabled.Count>0) TransferBetweenLists (listOfChildrenDisabled.Count); // if there are any disabled children enable all of this is to get rid of rounding issues for last piece on heal
			}else{
				if (listOfChildrenDisabled.Count>0) TransferBetweenLists((int)(adj / healthPerPiece)); // remove/add pieces 
			}
		}
//        print(gameObject.name + " is being hit: " + adj + " out of " + maxHealth + " works out to" + adj/healthPerPiece);
//
//              curHealth += adj;
//        if (curHealth <= 0) {
//        	Death();//do the death thing
//        }
//
//        if (curHealth > maxHealth)
//        	curHealth = maxHealth;
//        if (maxHealth < 1)
//        	maxHealth = 1;
//              TransferBetweenLists((int)(adj / healthPerPiece));
//              print(gameObject.name + " is being hit: " + adj + " out of " + maxHealth + " works out to" + adj);
//              print(gameObject.name + " each piece is worth " + healthPerPiece);
//              print(gameObject.name + " will lose" + (adj / healthPerPiece) + " rounded to" + (int)(adj / healthPerPiece));

    }

    public float HealthPerPiece() { // call this to calculate how much health each piece is worth

        healthPerPiece = maxHealth / listOfChildren.Count; // ... self explanatory
        return healthPerPiece;// it's a float people
    }

    public void BlockEndEvent() {
        activeBlocking = false;
    }

    public void AnimationByHash(int hash) {
        anim.SetTrigger(hash);
    }

    public virtual void Death() {
        heDead = true; //you ded!
    }

    public virtual void AnimationStartedEvent() {
        wait = true;
        //enemyWait = true;
    }

    public virtual void AnimationEndedEvent() {
        wait = false;
        //checkIfHit = false;
        stunned = false;
    }

    public virtual void BlockEvent() {
        activeBlocking = true;
        anim.SetTrigger(blockHash);
    }

    public virtual void IHitEnemyEvent() {
        //override on both classes, here for form.
    }

    public virtual void ImBeingHit(float damage) {
        //override on both classes, here for form.
    }

    public void PopulateChildrenToDisable(GameObject obj, string lookForString) { // recursive function to find pieces to enable/disable
        if (null == obj) // if the object doesn't exist gtfo
            return;

        foreach (Transform child in obj.transform) { // for each child of this opbject
            if (null == child) // if it's null go to next
                continue;

            if (child.name.ToLower().Contains(lookForString.ToLower())) { // if the child contains what you're looking for 
                listOfChildren.Add(child.gameObject);// add to list change this in a parameter
            }
            PopulateChildrenToDisable(child.gameObject, lookForString); // call recursive with child
        }
    }

    public void TransferBetweenLists(int piecesPerHit) {
        for (int i = 0; i < Mathf.Abs(piecesPerHit); i++) { // since pieces per hit can be negative or positive get absolute value for itterations 
            int elemnetToDisable = 0; //reset which element to disable
            if (piecesPerHit < 0 && listOfChildren.Count > 0) { // if piecesPerHit is nagative and I have children to disable
                elemnetToDisable = Random.Range(0, listOfChildren.Count - 1); // get a random element from list
                listOfChildren[elemnetToDisable].SetActive(false); // disable the game object
                listOfChildrenDisabled.Add(listOfChildren[elemnetToDisable]); // add it to the list of disabled
                listOfChildren.RemoveAt(elemnetToDisable); // remove it from this list
            } else if (piecesPerHit >= 0 && listOfChildrenDisabled.Count > 0) { // if piecesPerHit is positive and I have disabled pieces to enable
                elemnetToDisable = Random.Range(0, listOfChildrenDisabled.Count - 1); // get a random disabled piece
                listOfChildrenDisabled[elemnetToDisable].SetActive(true); // set it to active
                listOfChildren.Add(listOfChildrenDisabled[elemnetToDisable]); // remove it from the disabled list
                listOfChildrenDisabled.RemoveAt(elemnetToDisable); // add it to enabled list.
            }
        }
    }

}
