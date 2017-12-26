using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class ResumePanel : MonoBehaviour {

	[SerializeField] private VRInput input;
	[SerializeField] private VRCameraFade fader;
	[SerializeField] private Transform cameraTransform;

	[SerializeField] private VRUIAnimationClick btnMenu;
	[SerializeField] private VRUIAnimationClick btnRepeat;

	[SerializeField] private MediaManager mediaManager;

	// Use this for initialization
	void Start () {
		if(btnMenu != null)
		{
			btnMenu.OnAnimationComplete += ButtonMenuOnClick;
		}
		if (btnRepeat != null) 
		{
			btnRepeat.OnAnimationComplete += ButtonRepeatOnClick;
		}		
	}

	// Update is called once per frame
	void Update () {

	}

	private void ButtonMenuOnClick()
	{
		mediaManager.FinishExperience ();
	}

	private void ButtonRepeatOnClick()
	{
		mediaManager.InitializeVariables2 ();
		mediaManager.SelectVideo (1);
	}
}
