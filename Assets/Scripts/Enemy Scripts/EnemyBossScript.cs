using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossScript : CoreActorScript {


	//animations trigger hashes
   // public int hit1Hash = Animator.StringToHash("Hit1");
    public int hit1v2Hash = Animator.StringToHash("Hit1v2");
    public int hit2Hash = Animator.StringToHash("Hit2");
    public int hit3Hash = Animator.StringToHash("Hit3");
    public int hit4Hash = Animator.StringToHash("Hit4");
    public int[] hits;

    public bool hitBlockedActive = false;
    public double chanceToHit;
	public float holdYourHorses;
    public GameObject player;
    public float distanceToPlayer;
	bool inRange = false;
    

    void Start() {
		activeBlocking = true;
        holdYourHorses = Random.Range(1, 3);
        player = PlayerScript.player.transform.gameObject;
        hits = new int[5] { hit1Hash, hit1v2Hash, hit2Hash, hit3Hash, hit4Hash };
        CommonStartup ();
    }
    void FixedUpdate() {
        distanceToPlayer = Vector3.Distance(myTransform.position, player.transform.position);
	
		if (!inRange && distanceToPlayer < control.rangeToAttackEnemy) {
            inRange = true;
			control.targetsList.Add(gameObject); // add this gameObject to the list in the game control
        }
    }
    void Update() {
        //Attack AI
        if (!heDead) {
            if (inRange) {
                holdYourHorses -= Time.deltaTime;
				if (!wait && holdYourHorses <= 0) {
                    TryToHit();
                }
            }
        }
    }

    public void HitBlockedAnimation() {
        //boss hitBlock behavior in here if any
    }

    public void HitBlockedActiveEvent() {
        hitBlockedActive = true;
    }

    public void TryToHit() {
        chanceToHit = Random.Range(0, 100);
        if (chanceToHit >= 50) {
            int hitRandomHash = Random.Range(0, hits.Length);
            anim.SetTrigger(hits[hitRandomHash]);
        } else {
            holdYourHorses = Random.Range(1, 3);
        }
    }

    public void DeathEvent() {
        Destroy(gameObject);
        myTransform.parent.GetChild(1).gameObject.SetActive(true);
    }

    public override void Death() {
        heDead = true;
        wait = true;
        holdYourHorses = 10f;
		control.targetsList.Remove(gameObject);
        anim.SetTrigger(deathHash);// this calls the DeathEvent
		
    }

	public override void AnimationEndedEvent() {
		wait = false;
		activeBlocking = true;
		hitBlockedActive = false;
		holdYourHorses = 1f;
	}

	public override void BlockEvent() {
		activeBlocking = true;
	}

	public override void IHitEnemyEvent() {
		PlayerScript.player.SendMessage("ImBeingHit");
	}

	public override void ImBeingHit(float damage) {

		if (!activeBlocking) {
			//CameraShake.Shake(0.15f, .25f);
			if (holdYourHorses <= 0) {
				anim.SetTrigger(stunHash);
			}
            //explosion.transform.position = myTransform.transform.position;
            //explosion.Play();
            AddjustCurrentHealth(-10);
		} else {
			anim.SetTrigger(blockHash);
		}
		wait = false;
		//RemoveChildrenElements ();
	}

}