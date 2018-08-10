using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript: MonoBehaviour {
    Transform myTransform;
    public Transform playerTransform;
    public Transform cameraPositionOnPlayer;
    bool follow;

	void Start () {
        myTransform = transform;
	}
	
	void LateUpdate () {
        if (follow) {
            myTransform.rotation = cameraPositionOnPlayer.rotation;
            myTransform.position = cameraPositionOnPlayer.position;
        }
    }

    public void ToggleFollow() {
        if (follow) {
            myTransform.SetParent(playerTransform);
            myTransform.SetAsFirstSibling();
            follow = false;
        } else {
            myTransform.SetParent(null);
            follow = true;
        }
    }

}
