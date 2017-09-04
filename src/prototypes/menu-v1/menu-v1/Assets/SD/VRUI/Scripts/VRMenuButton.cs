using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMenuButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void changeUpScale(){
		transform.localScale += new Vector3(0.05F, 0.05F, 0);
	}
}
