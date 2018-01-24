using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownLanguage : MonoBehaviour {

	[SerializeField] private VRUIAnimationClick buttonLanguage;
	[SerializeField] private Dropdown dropdownLan;

	// Use this for initialization
	void Start () {
		if(buttonLanguage != null)
		{
			buttonLanguage.OnAnimationComplete += ButtonLanguageOnClick;
		}
	}

	private void ButtonLanguageOnClick()
	{
		dropdownLan.Show ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
