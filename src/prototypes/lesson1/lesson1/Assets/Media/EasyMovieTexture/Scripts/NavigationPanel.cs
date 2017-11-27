using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using System;

public class NavigationPanel : MonoBehaviour {

	[SerializeField] private VRInput input;
	[SerializeField] private VRCameraFade fader;
	[SerializeField] private Transform cameraTransform;
	[SerializeField] private float distance = 5;

	[SerializeField] private VRUIAnimationClick btnIzq;
	[SerializeField] private VRUIAnimationClick btn1;
	[SerializeField] private VRUIAnimationClick btn2;
	[SerializeField] private VRUIAnimationClick btn3;
	[SerializeField] private VRUIAnimationClick btn4;
	[SerializeField] private VRUIAnimationClick btnDer;
	[SerializeField] private VRUIAnimationClick btnGuia;

	[SerializeField] private TextMesh UI_Btn1Text;
	[SerializeField] private TextMesh UI_Btn2Text;
	[SerializeField] private TextMesh UI_Btn3Text;
	[SerializeField] private TextMesh UI_Btn4Text;

	[SerializeField] private MediaManager mediaManager;

	[SerializeField] Material PartLesson;
	[SerializeField] Material UI_BtnFrame;
	[SerializeField] Material MaterialOcultar;

	[SerializeField] GameObject GO_btn1;
	[SerializeField] GameObject GO_btn2;
	[SerializeField] GameObject GO_btn3;
	[SerializeField] GameObject GO_btn4;

	private int cantVideo = 0; 
	private int cantVideoAux = 0;

	private VRExperience experience = null;

	// Use this for initialization
	void Start () {
		experience = VRExperience.Instance;
		cantVideo = experience.CountVideo();
		cantVideoAux = experience.CountVideo();

		switch (cantVideo) 
		{
		case 1:
			changeMaterialOcultar (GO_btn2);
			changeMaterialOcultar (GO_btn3);
			changeMaterialOcultar (GO_btn4);
			break;
		case 2:
			changeMaterialOcultar (GO_btn3);
			changeMaterialOcultar (GO_btn4);
			break;
		case 3:
			changeMaterialOcultar (GO_btn4);
			break;
		default:
			break;
		}

		if(btnDer != null)
		{
			btnDer.OnAnimationComplete += ButtonDerClick;
		}
		if (btnIzq != null) 
		{
			btnIzq.OnAnimationComplete += ButtonIzqClick;
		}
		if (btn1 != null) 
		{
			btn1.OnAnimationComplete += Button1Click;
		}
		if (btn2 != null) 
		{
			btn2.OnAnimationComplete += Button2Click;
		}
		if (btn3 != null) 
		{
			btn3.OnAnimationComplete += Button3Click;
		}
		if (btn4 != null) 
		{
			btn4.OnAnimationComplete += Button4Click;
		}
		if (btnGuia != null) 
		{
			btnGuia.OnAnimationComplete += ButtonGuiaClick;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void ButtonDerClick()
	{
		int btn1Int = Convert.ToInt16(UI_Btn1Text.text);
		btn1Int = btn1Int + 4;
		if (btn1Int <= cantVideo) {
			UI_Btn1Text.text = btn1Int.ToString ();
			btn1Int++;
			UI_Btn2Text.text = btn1Int.ToString ();
			btn1Int++;
			UI_Btn3Text.text = btn1Int.ToString ();
			btn1Int++;
			UI_Btn4Text.text = btn1Int.ToString ();
			materialOriginal ();
			cantVideoAux = cantVideoAux - 4;
			OcultarPart();
		}
	}

	private void ButtonIzqClick()
	{
		int btn1Int = Convert.ToInt16(UI_Btn1Text.text);
		btn1Int = btn1Int - 4;
		if (btn1Int > 0) {
			UI_Btn1Text.text = btn1Int.ToString ();
			btn1Int++;
			UI_Btn2Text.text = btn1Int.ToString ();
			btn1Int++;
			UI_Btn3Text.text = btn1Int.ToString ();
			btn1Int++;
			UI_Btn4Text.text = btn1Int.ToString ();
			materialOriginal ();
			cantVideoAux = cantVideoAux + 4;
		}
	}

	private void Button1Click()
	{
		int btn1Int = Convert.ToInt16(UI_Btn1Text.text);
		mediaManager.SelectVideo (btn1Int);
	}

	private void Button2Click()
	{
		int btn2Int = Convert.ToInt16(UI_Btn2Text.text);
		mediaManager.SelectVideo (btn2Int);
	}

	private void Button3Click()
	{
		int btn3Int = Convert.ToInt16(UI_Btn3Text.text);
		mediaManager.SelectVideo (btn3Int);
	}

	private void Button4Click()
	{
		int btn4Int = Convert.ToInt16(UI_Btn4Text.text);
		mediaManager.SelectVideo (btn4Int);
	}

	private void ButtonGuiaClick()
	{
		mediaManager.ButtonGuia ();
	}

	public void colorPart()
	{
		int indice = experience.GetIndice();
		indice++;
		int btn1Int = Convert.ToInt16(UI_Btn1Text.text);
		int btn4Int = btn1Int + 3;
		if (btn1Int <= indice && btn4Int >= indice) {
			switch (indice) {
			case 1:
				changeColor (GO_btn1);
				break;
			case 2:
				changeColor (GO_btn2);
				break;
			case 3:
				changeColor (GO_btn3);
				break;
			case 4:
				changeColor (GO_btn4);
				break;
			default:
				indice = indice - 4;
				ButtonDerClick ();
				colorPart (indice);
				break;
			}
		} else 
		{
			ButtonDerClick ();
			colorPart ();
		}
	}

	private void colorPart(int indice)
	{
		switch (indice) 
		{
		case 1:
			changeColor(GO_btn1);
			break;
		case 2:
			changeColor(GO_btn2);
			break;
		case 3:
			changeColor(GO_btn3);
			break;
		case 4:
			changeColor(GO_btn4);
			break;
		default:
			indice = indice - 4;
			ButtonDerClick();
			colorPart(indice);
			break;
		}
	}

	private void changeColor(GameObject btn)
	{
		btn.GetComponent<Renderer>().material = PartLesson;
	}

	private void changeMaterialOcultar(GameObject btn)
	{
		btn.GetComponent<Renderer>().material = MaterialOcultar;
	}

	public void materialOriginal()
	{
		GO_btn1.GetComponent<Renderer>().material = UI_BtnFrame;
		GO_btn2.GetComponent<Renderer>().material = UI_BtnFrame;
		GO_btn3.GetComponent<Renderer>().material = UI_BtnFrame;
		GO_btn4.GetComponent<Renderer>().material = UI_BtnFrame;
	}

	public void OcultarPart()
	{
		switch (cantVideoAux) 
		{
		case 1:
			changeMaterialOcultar (GO_btn2);
			UI_Btn2Text.text = "";
			changeMaterialOcultar (GO_btn3);
			UI_Btn3Text.text = "";
			changeMaterialOcultar (GO_btn4);
			UI_Btn4Text.text = "";
			break;
		case 2:
			changeMaterialOcultar (GO_btn3);
			UI_Btn3Text.text = "";
			changeMaterialOcultar (GO_btn4);
			UI_Btn4Text.text = "";
			break;
		case 3:
			changeMaterialOcultar (GO_btn4);
			UI_Btn4Text.text = "";
			break;
		default:
			break;
		}
	}
}
