using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using System;

public class VRErrorMenu : MonoBehaviour {

    [SerializeField] private VRCameraFade fader;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance;
    [SerializeField] private GameObject menuBase;
    [SerializeField] private VRUIAnimationClick btnResume;
    [SerializeField] private bool pauseOnDisplay;


    private bool active = false;

    public event Action OnMenuShow;
    public event Action OnMenuHide;

    // Use this for initialization
    void Start () {
        //ToggleMenu();
	
		if (btnResume != null)
        {
			btnResume.OnAnimationComplete += CloseMenu;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void ToggleMenu()
    {
        if (active)
        {
            if(OnMenuShow != null && pauseOnDisplay)
                OnMenuHide();

            menuBase.SetActive(false);
            active = false;
        } else
        {
            if(OnMenuHide != null && pauseOnDisplay)
                OnMenuShow();

            if(cameraTransform != null)
            {
                Vector3 posMenu = cameraTransform.position + cameraTransform.forward * distance;
                menuBase.transform.position = posMenu;
                menuBase.transform.forward = -cameraTransform.forward;
            }
            menuBase.SetActive(true);
            active = true;
        }
    }

    private void CloseMenu()
    {
        if(active)
        {
            OnMenuHide();
            menuBase.SetActive(false);
            active = false;
        }
    }
}
