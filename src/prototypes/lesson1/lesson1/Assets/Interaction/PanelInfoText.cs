using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelInfoText : MonoBehaviour 
{
	[SerializeField] private VRUIAnimationClick panelInfo1;
	[SerializeField] private VRUIAnimationClick panelInfo2;
	[SerializeField] private VRUIAnimationClick panelInfo3;
	[SerializeField] private VRUIAnimationClick panelInfo4;
	[SerializeField] private VRUIAnimationClick panelInfo5;

	private static PanelInfoText _instance;

	[SerializeField] private MediaManager mediaManager;
	private int textSelected;

	public static PanelInfoText Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<PanelInfoText>();
				// DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	// Use this for initialization
	void Start () 
	{
		textSelected = 1;

		if (panelInfo1 != null)
		{
			panelInfo1.OnAnimationComplete += panelInfo1OnClik;
		}
		if (panelInfo2 != null)
		{
			panelInfo2.OnAnimationComplete += panelInfo2OnClik;
		}

		if (panelInfo3 != null)
		{
			panelInfo3.OnAnimationComplete += panelInfo3OnClik;
		}

		if (panelInfo4 != null)
		{
			panelInfo4.OnAnimationComplete += panelInfo4OnClik;
		}

		if (panelInfo5 != null)
		{
			panelInfo5.OnAnimationComplete += panelInfo5OnClik;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	private void panelInfo1OnClik()
	{
		mediaManager.selectTextInfo (1);
		textSelected = 1;
	}

	private void panelInfo2OnClik()
	{
		mediaManager.selectTextInfo (2);
		textSelected = 2;
	}

	private void panelInfo3OnClik()
	{
		mediaManager.selectTextInfo (3);
		textSelected = 3;
	}

	private void panelInfo4OnClik()
	{
		mediaManager.selectTextInfo (4);
		textSelected = 4;
	}

	private void panelInfo5OnClik()
	{
		mediaManager.selectTextInfo (5);
		textSelected = 5;
	}

	public int WhichTextSelected() {
		return textSelected;
	}

	public void SetTextSelected(int intSelected) {
		textSelected = intSelected;
	}
}
