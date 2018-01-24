using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptMenuUser : MonoBehaviour {

	[SerializeField] private VRUIAnimationClick btnTecladoName;
	[SerializeField] private VRUIAnimationClick btnTecladoSurname;
	[SerializeField] private VRUIAnimationClick btnConfirmar;
	[SerializeField] private TextMesh name;
	[SerializeField] private TextMesh surname;
	[SerializeField] private Text keyboardInp;

	[SerializeField] private GameObject teclado;
	[SerializeField] private GameObject user_Menu;
	[SerializeField] private GameObject menu;

	[SerializeField] private GameObject errorPanel;
	private Util util = Util.Instance;

	private bool isName = false;

	// Use this for initialization
	void Start () {

		if(btnTecladoName != null)
		{
			btnTecladoName.OnAnimationComplete += ButtonTecladoNameOnClick;
		}
		if (btnTecladoSurname != null) 
		{
			btnTecladoSurname.OnAnimationComplete += ButtonTecladoSurnameOnClick;
		}
		if (btnConfirmar != null) 
		{
			btnConfirmar.OnAnimationComplete += ButtonConfirmarOnClick;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void ButtonTecladoNameOnClick()
	{
		teclado.SetActive (true);
		user_Menu.SetActive (false);
		keyboardInp.text = "";
		isName = true;
	}

	private void ButtonTecladoSurnameOnClick()
	{
		teclado.SetActive (true);
		user_Menu.SetActive (false);
		keyboardInp.text = "";
		isName = false;
	}

	private void ButtonConfirmarOnClick()
	{
		teclado.SetActive (false);
		user_Menu.SetActive (false);

		//menu.SetActive (true);
		Vector3 menuScale = new Vector3(0.1f,0.1f,0.1f);
		menu.transform.localScale = menuScale;

		util.ReLoadMenuTexts(errorPanel, true);
	}

	public void KeyboardOKButtonUser()
	{
		if (isName) {
			name.text = keyboardInp.text;
		} else {
			surname.text = keyboardInp.text;
		}
		teclado.SetActive (false);
		user_Menu.SetActive (true);
	}

	public void KeyboardExitButtonUser()
	{
		teclado.SetActive (false);
		user_Menu.SetActive (true);
	}
}
