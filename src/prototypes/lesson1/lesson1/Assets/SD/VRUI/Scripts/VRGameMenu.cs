using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using System;

public class VRGameMenu : MonoBehaviour {

    [SerializeField] private VRInput input;
    [SerializeField] private VRCameraFade fader;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance;
    [SerializeField] private GameObject menuBase;
    [SerializeField] private VRUIAnimationClick btnReturn;
    [SerializeField] private VRUIAnimationClick btnResume;
    [SerializeField] private bool pauseOnDisplay;
	[SerializeField] GameObject sphere;


    private bool active = false;

    public event Action OnMenuShow;
    public event Action OnMenuHide;

    // Use this for initialization
    void Start () {
        input.OnCancel += ToggleMenu;
		if(btnReturn != null)
        {
			btnReturn.OnAnimationComplete += ExitExperience;
        }
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

			sphere.SetActive (false);
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
			sphere.SetActive (true);
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
			sphere.SetActive (false);
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
