using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelLanguage : MonoBehaviour 
{
	[SerializeField] private VRUIAnimationClick flag1;
	[SerializeField] private VRUIAnimationClick flag2;
	[SerializeField] private VRUIAnimationClick flag3;

	[SerializeField] private Material MatFlag1;
	[SerializeField] private Material MatFlag1BW;
	[SerializeField] private Material MatFlag2;
	[SerializeField] private Material MatFlag2BW;
	[SerializeField] private Material MatFlag3;
	[SerializeField] private Material MatFlag3BW;

	// Use this for initialization
	void Start () 
	{
		if (flag1 != null)
		{
			flag1.OnAnimationComplete += flag1OnClik;
		}
		if (flag2 != null)
		{
			flag2.OnAnimationComplete += flag2OnClik;
		}
		if (flag3 != null)
		{
			flag3.OnAnimationComplete += flag3OnClik;
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void flag1OnClik()
	{
		GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1;
		GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2BW;
		GameObject.Find("UI_Lan3").GetComponent<Renderer>().material = MatFlag3BW;
	}

	private void flag2OnClik()
	{
		GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1BW;
		GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2;
		GameObject.Find("UI_Lan3").GetComponent<Renderer>().material = MatFlag3BW;
	}

	private void flag3OnClik()
	{
		GameObject.Find("UI_Lan1").GetComponent<Renderer>().material = MatFlag1BW;
		GameObject.Find("UI_Lan2").GetComponent<Renderer>().material = MatFlag2BW;
		GameObject.Find("UI_Lan3").GetComponent<Renderer>().material = MatFlag3;
	}
}
