using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPiece:MonoBehaviour, IPiece {

    public int myId;

    void Awake() {
        GetId();
    }

    public void GetId() {
        string temp = gameObject.name.Substring(1);
        int.TryParse(temp,out myId);
        if (myId==0) {
            print(gameObject.name + " is not a valid name, it needs to be ONE letter + integer so I can parse the ID");
        }
    }
}
