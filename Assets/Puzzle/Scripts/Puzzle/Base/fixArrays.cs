using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class fixArrays : MonoBehaviour {

    //public Transform tops;
    //public Transform arrayToFix;
    public Transform[] bottomsArray;
    public Transform[] topsArray;
    public Transform[] topsShadowArray;
    public Transform[] bottomsShadowArray;
    public Transform bottomsObjects;
    public Transform topsObjects;
    public Transform bottomsShadowObjects;
    public Transform topsShadowObjects;
    public Material topMaterial;
    public Material bottomMaterial;
    public Material topShadowMaterial;
    public Material bottomShadowMaterial;

    void Start() {
        bottomsArray = new Transform[60];
        topsArray = new Transform[60];
        bottomsShadowArray = new Transform[60];
        topsShadowArray = new Transform[60];
        FixArrays();
    }
    void FixArrays() {
        //populate arrays
        for (int i=0;i<60;i++) {
            bottomsArray[i] = bottomsObjects.GetChild(i);
            topsArray[i] = topsObjects.GetChild(i);
            bottomsShadowArray[i] = bottomsShadowObjects.GetChild(i);
            topsShadowArray[i] = topsShadowObjects.GetChild(i);
        }

        for (int i = 0; i < bottomsArray.Length; i++) {

            //create objects for pieces
            GameObject bottomCube = CreateCube("BottomHolder",i); 
            GameObject topCube = CreateCube("TopHolder",i);
            GameObject centerCube = CreateCube("CenterHolder",i);

            //create objects for shadows
            GameObject bottomCubeS = CreateCube("BottomHolder",i);
            GameObject topCubeS = CreateCube("TopHolder",i);
            GameObject centerCubeS = CreateCube("TopCenter",i);

            //set the top piece
            topsArray[i].gameObject.layer = 21;
            topsArray[i].gameObject.AddComponent<BoxCollider>();
            topsArray[i].gameObject.AddComponent<TopPiece>();
            topsArray[i].gameObject.GetComponent<Renderer>().material = topMaterial;
            //set bottom piece
            bottomsArray[i].gameObject.AddComponent<BottomPiece>();
            bottomsArray[i].gameObject.GetComponent<Renderer>().material = bottomMaterial;
            //set top shadow piece
            topsShadowArray[i].gameObject.AddComponent<TopPieceShadow>();
            topsShadowArray[i].gameObject.GetComponent<Renderer>().material = topShadowMaterial;
            //set bottom shadow piece
            bottomsShadowArray[i].gameObject.AddComponent<BottomPieceShadow>();
            bottomsShadowArray[i].gameObject.GetComponent<Renderer>().material = bottomShadowMaterial;
            //parent bottom cubes
            bottomCube.transform.SetParent(bottomsArray[i]); // parent to bottom for positioning 
            bottomCubeS.transform.SetParent(bottomsShadowArray[i]); // parent to bottom for positioning 
            //parent top cubes
            topCube.transform.SetParent(bottomCube.transform); // parent to bottom for positioing
            topCubeS.transform.SetParent(bottomCubeS.transform); // parent to bottom for positioing
            //parent center cubes
            centerCube.transform.SetParent(topsArray[i]); //parent to top
            centerCubeS.transform.SetParent(topsShadowArray[i]); //parent to top

            // position regular cubes
            bottomCube.transform.localPosition = new Vector3(0,0,0); // center bottom.
            topCube.transform.localPosition = new Vector3(0,1,0); // position top on top of it.
            centerCube.transform.localPosition = new Vector3(0,0,0); // create center on top piece.
            // position shadow cubes
            bottomCubeS.transform.localPosition = new Vector3(0,0,0); // center bottom.
            topCubeS.transform.localPosition = new Vector3(0,1,0); // position top on top of it.
            centerCubeS.transform.localPosition = new Vector3(0,0,0); // create center on top piece.

            //re-parent top cube (easier to position through previous parenting relative to the bottom cube)
            topCube.transform.SetParent(topsArray[i].transform); // parent to top piece
            topCubeS.transform.SetParent(topsShadowArray[i].transform); // parent to top piece

            // do the parent to child thing for the cubes
            ParentToChild(bottomsArray[i],bottomCube.transform); // fix parenting
            ParentToChild(bottomsShadowArray[i],bottomCubeS.transform); // fix parenting
            ParentToChild(topsArray[i],centerCube.transform); // fix pareting 
            ParentToChild(topsShadowArray[i],centerCubeS.transform); // fix pareting 
            //yay!
        }
    }

    GameObject CreateCube(string _name,int _iteration) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        DestroyImmediate(cube.GetComponent<BoxCollider>());
        cube.GetComponent<MeshRenderer>().enabled = false;
        cube.name = _name + (_iteration + 1).ToString();
        return cube;
    }

    void ParentToChild(Transform _parent,Transform _child) {
        _child.SetParent(_parent.parent);// set the child on the same node level as the parent 
        _parent.SetParent(_child);// parent to the child
    }


}
