using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadCrumbInfo : MonoBehaviour {
	//see public void FollowBreadCrumbs() in GameLogic.cs for implementation.
	//since each BreadCrumb object is evaluated, the behavior will change the currentPlayerState based on the Breadcrumb.gameObject.name contains.
	//if you want to fly between breadcrumbs, add the word fly to the name of the BreadCrumb object. the values below are aditable from the inspector once breadcrumb is selected.
	//this script has to be attached to every breadcrumb.

	public float moveSpeed;
	public float rotateSpeed;

	public BreadCrumbInfo(){
		
		moveSpeed = 3f;
		rotateSpeed = 80f;
	}

}
