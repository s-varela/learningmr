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
    [SerializeField] private MediaManager mediaManager;
    //[SerializeField] private float distance;

    private bool active = false;

    public event Action OnDialogShow;
    public event Action OnDialogHiden;
    public event Action OnAcceptClick;
    public event Action OnCancelClick;

    // Use this for initialization

    void Awake()
    {
        //input.OnCancel += ToggleMenu;
        UnityEngine.Debug.Log("[VRMediaMenu][Start] " + "Inicializando");
    

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

        UnityEngine.Debug.Log("[VRMediaMenu][Start] " + "Inicializando");

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
        if (active)
        {
            if(OnDialogShow != null && pauseOnDisplay)
                OnDialogHiden();

			sphere.SetActive (false);
            menuBase.SetActive(false);
            active = false;
        } else
        {
            if(OnDialogHiden != null && pauseOnDisplay)
                OnDialogShow();

           if(cameraTransform != null)
            {
                Vector3 posMenu = cameraTransform.position + cameraTransform.forward * distance;
                menuBase.transform.position = posMenu;
                menuBase.transform.forward = -cameraTransform.forward;
            }

			sphere.SetActive (true);
            menuBase.SetActive(true);
            active = true;
        }
    }

    private void CloseMenu()
    {
        UnityEngine.Debug.Log("[VRMediaMenu][Cancel] " + "Click en Cancel");
        if (active)
        {
            OnCancelClick();
            //menuBase.SetActive(false);
            active = false;
        }
    }

    private void Accept()
    {
        UnityEngine.Debug.Log("[VRMediaMenu][Accept] " + "Click en Ok");
        OnAcceptClick();
        //menuBase.SetActive(false);
        active = false;
    }

}
