using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Blinker : MonoBehaviour {
	
	public Stopwatch blinkingTimer;
	public bool blinking;
	public GameObject obj;

	// Use this for initialization
	void Start () {
		blinkingTimer = new Stopwatch();
		blinking = false;
		obj = null;
	}
	
	// Update is called once per frame
	void Update()
	{ 
		if(blinking)
		{ 
			SpriteBlinkingEffect();
		}
	}

	public void Trigger(GameObject obj)
	{
		blinking = true;
		blinkingTimer.Start();
		this.obj = obj;
		this.obj.SetActive(true);
	}

	private bool ElapsedTime(int milliSeconds)
	{
		return blinkingTimer.ElapsedMilliseconds > milliSeconds;
	}

	void SpriteBlinkingEffect()
	{
		if (ElapsedTime (3000)) {
			this.obj.SetActive(false);
			blinkingTimer.Reset();
		} 
		else
		{
			this.obj.SetActive(true);
		}
	}
}
