using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class PaginatedPanel : MonoBehaviour {

	[SerializeField] private VRInput input;
	[SerializeField] private VRCameraFade fader;
	[SerializeField] private Transform cameraTransform;

	[SerializeField] private VRUIAnimationClick btnNext;
	[SerializeField] private VRUIAnimationClick btnPrevious;

	[SerializeField] private MediaManager mediaManager;

    // Use this for initialization
    void Start () {

		if(btnNext != null)
		{
            btnNext.OnAnimationComplete += ButtonNextOnClick;
		}
		if (btnPrevious != null) 
		{
            btnPrevious.OnAnimationComplete += ButtonPreviousOnClick;
		}

	}
	
	void Update () {

	}

	private void ButtonNextOnClick()
	{
		mediaManager.SetInactiveButtonGuia ();
        mediaManager.NextPage();
    }

    private void ButtonPreviousOnClick()
    {
		mediaManager.SetInactiveButtonGuia ();
        mediaManager.PreviousPage();
    }

}
