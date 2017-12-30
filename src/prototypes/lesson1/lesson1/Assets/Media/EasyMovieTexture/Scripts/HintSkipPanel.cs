using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class HintSkipPanel : MonoBehaviour {

	[SerializeField] private VRUIAnimationClick btnHint;
	[SerializeField] private VRUIAnimationClick btnSkip;

	[SerializeField] private MediaManager mediaManager;

    // Use this for initialization
    void Start () {

		if(btnHint != null)
		{
            btnHint.OnAnimationComplete += ButtonHintOnClick;
		}
		if (btnSkip != null) 
		{
            btnSkip.OnAnimationComplete += ButtonSkipOnClick;
		}

	}
	
	void Update () {

	}

	private void ButtonHintOnClick()
	{
		mediaManager.GiveHint();
    }

    private void ButtonSkipOnClick()
    {
		mediaManager.ExecuteSkip();
    }

}
