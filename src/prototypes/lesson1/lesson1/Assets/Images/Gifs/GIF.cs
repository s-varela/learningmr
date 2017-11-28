using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GIF : MonoBehaviour {

	[SerializeField] Texture2D[] frames;
	[SerializeField] int fps = 10;

	// Update is called once per frame
	void Update () 
	{
		int index = (int)(Time.time * fps) % (frames.Length);
		GetComponent<RawImage> ().texture = frames [index];
	}
}
