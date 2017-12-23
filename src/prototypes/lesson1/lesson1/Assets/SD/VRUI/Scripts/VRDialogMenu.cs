using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using System;

public class VRDialogMenu : MonoBehaviour {

    //[SerializeField] private VRInput input;
    [SerializeField] private VRCameraFade fader;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance;
    [SerializeField] private GameObject menuBase;
    [SerializeField] private VRUIAnimationClick btnOk;
    [SerializeField] private VRUIAnimationClick btnCancel;
    [SerializeField] private bool pauseOnDisplay;
    [SerializeField] GameObject sphere;

    private bool active = false;

    public event Action OnDialogShow;
    public event Action OnDialogHiden;
    public event Action OnAcceptClick;
    public event Action OnCancelClick;

    // Use this for initialization

    void Awake()
    {
   
        ToggleMenu();

        if (btnCancel != null)
        {
            btnCancel.OnAnimationComplete += CloseMenu;
        }
        if (btnOk != null)
        {
            btnOk.OnAnimationComplete += Accept;
        }
    }

    void Start() {

        ToggleMenu();

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

    private void ToggleMenu()
    {
		sphere.SetActive (true);
        menuBase.SetActive(true);
        active = true;
    }

    private void CloseMenu()
    {
        menuBase.SetActive(false);
        sphere.SetActive(false);
        active = false;
        
    }

    private void Accept()
    {
        active = false;
        OnAcceptClick();
        //menuBase.SetActive(false);
        //sphere.SetActive(false);
    }

}
