using UnityEngine;
using System.Collections;
using VRStandardAssets.Utils;
using System;
using GoogleSpeech.Plugins.GoogleCloud.SpeechRecognition;

public class VRSpeechMenu : MonoBehaviour {

    [SerializeField] private VRInput input;
    [SerializeField] private VRCameraFade fader;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float distance = 5;
    [SerializeField] private GameObject menuBase;
    [SerializeField] private VRUIAnimationClick btnOK;
    [SerializeField] private VRUIAnimationClick btnCancel;
    [SerializeField] private GameObject cameraReticle;
    [SerializeField] private bool pauseOnDisplay;
	[SerializeField] private TextMesh speechRecognitionResult;
	private GCSpeechRecognition speechRecognition;


    private bool active = false;

    public event Action OnMenuShow;
    public event Action OnMenuHide;

    // Use this for initialization
    void Start () {
        input.OnCancel += ToggleMenu;
		speechRecognition = GCSpeechRecognition.Instance;
        if(btnOK != null)
        {
			btnOK.OnAnimationComplete += StartRecordButtonOnClickHandler/* ExitExperience */;
        }
        if (btnCancel != null)
        {
			btnCancel.OnAnimationComplete += StopRecordButtonOnClickHandler/*CloseMenu*/;
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

	private void OnDestroy()
	{
		//speechRecognition.RecognitionSuccessEvent -= SpeechRecognizedSuccessEventHandler;
		//speechRecognition.RecognitionFailedEvent -= SpeechRecognizedFailedEventHandler;
	}


	private void StartRecordButtonOnClickHandler()
	{
		speechRecognitionResult.text = string.Empty;
		speechRecognition.StartRecord(true/*isRuntimeDetectionToggle.isOn*/);
	}

	private void StopRecordButtonOnClickHandler()
	{
		speechRecognition.StopRecord();
	}

	private void LanguageDropdownOnValueChanged(int value)
	{
		speechRecognition.SetLanguage((Enumerators.LanguageCode)value);
	}

	private void SpeechRecognizedFailedEventHandler(string obj, long requestIndex)
	{
		speechRecognitionResult.text = "Speech Recognition failed with error: " + obj;

		/*if (!isRuntimeDetectionToggle.isOn)
		{
			speechRecognitionState.color = Color.green;
			startRecordButton.interactable = true;
			stopRecordButton.interactable = false;
		}*/
	}

	private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj, long requestIndex)
	{
		/*if (!isRuntimeDetectionToggle.isOn)
		{
			startRecordButton.interactable = true;
			speechRecognitionState.color = Color.green;
		}*/

		if (obj != null && obj.results.Length > 0)
		{
			speechRecognitionResult.text = "Speech Recognition succeeded!\n Best match: " + obj.results[0].alternatives[0].transcript;

			string other = "\nAlternatives: ";

			foreach (var result in obj.results)
			{ 
				foreach (var alternative in result.alternatives)
				{
					if (obj.results[0].alternatives[0] != alternative)
						other += alternative.transcript + ",\n";
				}
			}

			speechRecognitionResult.text += other;
		}
		else
		{
			speechRecognitionResult.text = "Speech Recognition succeeded! No words were detected.";
		}
	}

    /*private void ExitExperience()
    {
        OnMenuHide();
        CloseMenu();
        StartCoroutine(BackToMainMenu());
    }

    public IEnumerator BackToMainMenu()
    {
        yield return StartCoroutine(fader.BeginFadeOut(false));
        VRExperience.Instance.BackToMainMenu();
    }*/
}
