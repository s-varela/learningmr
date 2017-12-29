using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUser : MonoBehaviour {

	[SerializeField] private VRUIAnimationClick UI_BtnUser;
	[SerializeField] private GameObject user_menu;
	[SerializeField] private GameObject menu;

	[SerializeField] private GameObject errorPanel;
	private Util util = Util.Instance;
	private PanelLanguage panelLanguage = PanelLanguage.Instance;

	// Use this for initialization
	void Start () {

		if(UI_BtnUser != null)
		{
			UI_BtnUser.OnAnimationComplete += BtnUserOnClick;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void BtnUserOnClick()
	{
		user_menu.SetActive (true);

		//menu.SetActive (false);
		Vector3 apagar = new Vector3(0.00001f,0.00001f,0.00001f);
		menu.transform.localScale = apagar;

		util.ReLoadMenuTexts(errorPanel, false);
		panelLanguage.Start ();

	}
}
