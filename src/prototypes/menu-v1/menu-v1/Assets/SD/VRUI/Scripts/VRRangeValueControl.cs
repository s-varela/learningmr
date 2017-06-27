using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using VRStandardAssets.Utils;
using Assets.SD.VRMenuRoom.Scripts;
using System;

public class VRRangeValueControl : MonoBehaviour, IVRControl {

    [SerializeField] private string controlName = "undefined";
    [SerializeField] private int rangeMin;
    [SerializeField] private int rangeMax;
    [SerializeField] private int step;
    [SerializeField] private bool normalized;
    [SerializeField] private VRUIAnimationClick increaseButton;
    [SerializeField] private VRUIAnimationClick decreaseButton;
    [SerializeField] private Image slider;
    [SerializeField] private AudioClip audioButtonPlus;
    [SerializeField] private AudioClip audioButtonMinus;

    private AudioSource source;

    private float currentValue = 0;

    // Use this for initialization
    void Start ()
    {
        if(increaseButton != null)
        {
            increaseButton.OnClick += Increase;
        }

        if(decreaseButton != null)
        {
            decreaseButton.OnClick += Decrease;
        }

       source = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update ()
    {
        slider.fillAmount = currentValue / rangeMax;
    }

    private void UpdateRangeValue(RangeUpdate action)
    {
        currentValue = Mathf.Clamp(currentValue + (int)action * step, rangeMin, rangeMax);
    }

    private void Increase()
    {
        UpdateRangeValue(RangeUpdate.Increase);
        PlaySound(audioButtonPlus);
    }

    private void Decrease()
    {
        UpdateRangeValue(RangeUpdate.Decrease);
        PlaySound(audioButtonMinus);
    }

    private void PlaySound(AudioClip sound)
    {
        if (sound != null)
        {
            source.clip = sound;
            source.volume = GetCurrentNormalizedValue();
            source.loop = false;
            source.Play();
        }
    }

    public float GetCurrentNormalizedValue()
    {
        return currentValue / rangeMax;
    }

    public string GetControlName()
    {
        return controlName;
    }

    public float GetControlValue()
    {
        return normalized ? GetCurrentNormalizedValue() : currentValue;
    }

    public void SetControlValue(float value, bool normalized)
    {
        currentValue = Mathf.Clamp(normalized ? value * rangeMax : value, rangeMin, rangeMax);
    }

}

public enum RangeUpdate
{
    Increase = 1,
    Decrease = -1
}
