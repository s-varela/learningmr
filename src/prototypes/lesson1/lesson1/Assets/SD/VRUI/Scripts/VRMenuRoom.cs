using UnityEngine;
using System;
using System.Collections;
using VRStandardAssets.Utils;
using System.Collections.Generic;
using Assets.SD.VRMenuRoom.Scripts;
using UnityEngine.SceneManagement;

public class VRMenuRoom : MonoBehaviour {

    [SerializeField] private SelectionSlider startControl;
    [SerializeField] private VRCameraFade fader;
    [SerializeField] private AudioClip bgmMenu;
    [SerializeField] private GameObject[] dataControls;

    // Use this for initialization
    void Start () {
        if (bgmMenu != null)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = bgmMenu;
            source.volume = 0.1f;
            source.loop = true;
            source.Play();
        }

	    if(startControl != null)
        {
            startControl.OnBarFilled += HandleStartControl;
        }

        InitializeControls();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitializeControls()
    {
        for (int i = 0; i < dataControls.Length; i++)
        {
            IVRControl current = dataControls[i].GetComponent<IVRControl>();
            bool update = VRExperience.Instance.TryConfigurationValue(current.GetControlName());

            if (update)
            {
                current.SetControlValue(VRExperience.Instance.GetConfigurationValue<float>(current.GetControlName()), true);
            }
            else
            {
                current.SetControlValue(0.5f, true);
            }
        }
    }

    public IEnumerator OpenMenu()
    {
        yield return StartCoroutine(fader.BeginFadeIn(false));
    }

    public IEnumerator CloseMenu()
    {
        yield return StartCoroutine(fader.BeginFadeOut(false));
    }

    private IEnumerator StartExperience()
    {
        yield return StartCoroutine(CloseMenu());
        VRExperience.Instance.StartExperience(GetMenuSettings(), true);
    }

    private void HandleStartControl()
    {
        StartCoroutine(StartExperience());
    }

    public Dictionary<string, object> GetMenuSettings()
    {
        Dictionary<string, object> settings = new Dictionary<string, object>();
        for(int i = 0; i < dataControls.Length; i ++)
        {
            IVRControl current = dataControls[i].GetComponent<IVRControl>();
            settings.Add(current.GetControlName(), current.GetControlValue());
        }

        return settings;
    }
}
