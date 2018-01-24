using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayInteractionPanel : MonoBehaviour 
{
	[SerializeField] private VRUIAnimationClick replay1;
	[SerializeField] private VRUIAnimationClick replay2;
	[SerializeField] private VRUIAnimationClick replay3;
	[SerializeField] private VRUIAnimationClick replay4;
	[SerializeField] private VRUIAnimationClick replay5;

	private static ReplayInteractionPanel _instance;

	[SerializeField] private MediaManager mediaManager;

	[SerializeField] private Material play;
	[SerializeField] private Material stop;

	public static ReplayInteractionPanel Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = FindObjectOfType<ReplayInteractionPanel>();
				// DontDestroyOnLoad(_instance.gameObject);
			}
			return _instance;
		}
	}

	// Use this for initialization
	void Start () 
	{
		if (replay1 != null)
		{
			replay1.OnAnimationComplete += replay1OnClik;
		}
		if (replay2 != null)
		{
			replay2.OnAnimationComplete += replay2OnClik;
		}

		if (replay3 != null)
		{
			replay3.OnAnimationComplete += replay3OnClik;
		}

		if (replay4 != null)
		{
			replay4.OnAnimationComplete += replay4OnClik;
		}

		if (replay5 != null)
		{
			replay5.OnAnimationComplete += replay5OnClik;
		}
	}

	// Update is called once per frame
	void Update () {
	}

	private void replay1OnClik()
	{
		string selectedString = "";
		selectedString = GameObject.Find("TextInfo1").GetComponent<TextMesh>().text;
		mediaManager.repeatAudio(int.Parse("0"));
		mediaManager.SelectTextInfo (1);
		PanelInfoText selectedText = PanelInfoText.Instance;
		selectedText.SetTextSelected (1);
		mediaManager.SetInactiveButtonGuia ();
//		GameObject r1 = GameObject.Find ("UI_Replay1");
//		r1.GetComponent<Renderer> ().material = stop;
	}

	private void replay2OnClik()
	{
		string selectedString = "";
		selectedString = GameObject.Find("TextInfo2").GetComponent<TextMesh>().text;
		mediaManager.repeatAudio((int.Parse("1")));
		mediaManager.SelectTextInfo (2);
		PanelInfoText selectedText = PanelInfoText.Instance;
		selectedText.SetTextSelected (2);
		mediaManager.SetInactiveButtonGuia ();
//		GameObject r2 = GameObject.Find ("UI_Replay2");
//		r2.GetComponent<Renderer> ().material = stop;
	}

	private void replay3OnClik()
	{
		string selectedString = "";
		selectedString = GameObject.Find("TextInfo3").GetComponent<TextMesh>().text;
		mediaManager.repeatAudio((int.Parse("2")));
		mediaManager.SelectTextInfo (3);
		PanelInfoText selectedText = PanelInfoText.Instance;
		selectedText.SetTextSelected (3);
		mediaManager.SetInactiveButtonGuia ();
//		GameObject r3 = GameObject.Find ("UI_Replay3");
//		r3.GetComponent<Renderer> ().material = stop;
	}

	private void replay4OnClik()
	{
		string selectedString = "";
		selectedString = GameObject.Find("TextInfo4").GetComponent<TextMesh>().text;
		mediaManager.repeatAudio((int.Parse("3")));
		mediaManager.SelectTextInfo (4);
		PanelInfoText selectedText = PanelInfoText.Instance;
		selectedText.SetTextSelected (4);
		mediaManager.SetInactiveButtonGuia ();
//		GameObject r4 = GameObject.Find ("UI_Replay4");
//		r4.GetComponent<Renderer> ().material = stop;
	}

	private void replay5OnClik()
	{
		string selectedString = "";
		selectedString = GameObject.Find("TextInfo5").GetComponent<TextMesh>().text;
		mediaManager.repeatAudio((int.Parse("4")));
		mediaManager.SelectTextInfo (5);
		PanelInfoText selectedText = PanelInfoText.Instance;
		selectedText.SetTextSelected (5);
		mediaManager.SetInactiveButtonGuia ();
//		GameObject r5 = GameObject.Find ("UI_Replay5");
//		r5.GetComponent<Renderer> ().material = stop;
	}

	public void SetMaterialPlay()
	{
		for (int i = 1; i <= 5; i++) {
			GameObject ri = GameObject.Find ("UI_Replay"+i);
			ri.GetComponent<Renderer> ().material = play;
		}
	}
}
