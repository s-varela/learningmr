using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using System;

public class VRDialogMenu : MonoBehaviour {

    [SerializeField] private GameObject menuBase;
    [SerializeField] private VRUIAnimationClick btnOk;
    [SerializeField] private VRUIAnimationClick btnCancel;


    public event Action OnDialogShow;
    public event Action OnDialogHiden;
    public event Action OnAcceptClick;
    public event Action OnCancelClick;

    // Use this for initialization

    void Start() {

        if (btnCancel != null)
        {
            btnCancel.OnAnimationComplete += CloseMenu;
        }
		if (btnOk != null)
        {
            btnOk.OnAnimationComplete += Accept;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void CloseMenu()
    {
        menuBase.SetActive(false);
        
    }

    private void Accept()
    {
        OnAcceptClick();
    }

}
