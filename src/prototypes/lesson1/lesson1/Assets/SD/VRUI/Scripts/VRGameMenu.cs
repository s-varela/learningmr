using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using System;

public class VRGameMenu : MonoBehaviour {

    [SerializeField] private VRInput input;
    [SerializeField] private VRCameraFade fader;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance = 5;
    [SerializeField] private GameObject menuBase;
    [SerializeField] private VRUIAnimationClick btnOK;
    [SerializeField] private VRUIAnimationClick btnCancel;
    [SerializeField] private GameObject cameraReticle;
    [SerializeField] private bool pauseOnDisplay;

    private bool active = false;

    public event Action OnMenuShow;
    public event Action OnMenuHide;

    // Use this for initialization
    void Start () {
        input.OnCancel += ToggleMenu;
		if(btnCancel != null)
        {
			btnCancel.OnAnimationComplete += ExitExperience;
        }
		if (btnOK != null)
        {
			btnOK.OnAnimationComplete += CloseMenu;
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
            cameraReticle.SetActive(false);
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
            cameraReticle.SetActive(true);
            active = true;
        }
    }

    private void CloseMenu()
    {
        if(active)
        {
            OnMenuHide();
            menuBase.SetActive(false);
            cameraReticle.SetActive(false);
            active = false;
        }
    }

    private void ExitExperience()
    {
        OnMenuHide();
        CloseMenu();
        StartCoroutine(BackToMainMenu());
    }

    public IEnumerator BackToMainMenu()
    {
        yield return StartCoroutine(fader.BeginFadeOut(false));
        VRExperience.Instance.BackToMainMenu();
    }
}
