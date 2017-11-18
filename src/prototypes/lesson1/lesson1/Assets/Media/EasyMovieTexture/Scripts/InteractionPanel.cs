using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class InteractionPanel : MonoBehaviour {

	[SerializeField] private VRInput input;
	[SerializeField] private VRCameraFade fader;
	[SerializeField] private Transform cameraTransform;
	[SerializeField] private float distance = 5;

	[SerializeField] private VRUIAnimationClick btnTeclado;
	[SerializeField] private VRUIAnimationClick btnRec;

	[SerializeField] private TextMesh speechRecognitionResult;
	[SerializeField] private GCSpeechRecognition speechRecognition;

	[SerializeField] GameObject panelSub;
	[SerializeField] GameObject panelInput;
	[SerializeField] GameObject teclado;
    [SerializeField] GameObject hintButton;
    [SerializeField] GameObject skipButton;
    [SerializeField] Text keyboardInp;

	[SerializeField] GameObject panelAnswer;
	[SerializeField] GameObject panelQuestion;
	[SerializeField] GameObject panelHint;
	[SerializeField] GameObject panelHintText;

	[SerializeField] GameObject gifRipple;
	[SerializeField] GameObject gifProcessing;
	[SerializeField] GameObject noWifi;
	[SerializeField] TextMesh answer;

	[SerializeField] Material UI_SpeechStart;
	[SerializeField] Material UI_SpeechStop;


	[SerializeField] private MediaManager mediaManager;
	[SerializeField] private Blinker blinker;

    // Use this for initialization
    void Start () {

        if (btnRec != null)
		{
			btnRec.OnAnimationComplete += StartRecordButtonOnClickHandler;
		}
		if (btnTeclado != null) 
		{
			btnTeclado.OnAnimationComplete += ButtonTecladoOnClick;
		}

		noWifi.SetActive (false);
		speechRecognition = GCSpeechRecognition.Instance;
		speechRecognition.RecognitionSuccessEvent += SpeechRecognizedSuccessEventHandler;
		speechRecognition.RecognitionFailedEvent += SpeechRecognizedFailedEventHandler;
	}
	
	// Update is called once per frame
	// Update is called once per frame
	void Update () {

	}

	private void ButtonTecladoOnClick()
	{
		mediaManager.SetInactiveButtonGuia ();
		panelSub.SetActive (false);
		panelInput.SetActive (false);
		teclado.SetActive (true);
        hintButton.SetActive(false);
        skipButton.SetActive(false);
		panelQuestion.SetActive (false);
		panelHintText.SetActive (false);
		panelHint.SetActive (false);
		panelAnswer.SetActive (false);
        keyboardInp.text = "";
    }


	private void OnDestroy()
	{
		speechRecognition.RecognitionSuccessEvent -= SpeechRecognizedSuccessEventHandler;
		speechRecognition.RecognitionFailedEvent -= SpeechRecognizedFailedEventHandler;
	}


	private void StartRecordButtonOnClickHandler()
	{
		mediaManager.SetInactiveButtonGuia ();
		bool connection = CheckConnectivity.checkInternetStatus ();
		if (connection) {
			speechRecognitionResult.text = string.Empty;
			speechRecognitionResult.text = "Recording...";
			answer.text = "";
			gifRipple.SetActive (true);
			gifProcessing.SetActive (false);
			if (btnRec != null) {
				btnRec.GetComponent<Renderer> ().material = UI_SpeechStop;
				btnRec.OnAnimationComplete -= StartRecordButtonOnClickHandler;
				btnRec.OnAnimationComplete += StopRecordButtonOnClickHandler;
			}
			speechRecognition.StartRecord (false);
		} else 
		{
			noWifi.SetActive (true);
			blinker.SetComponent (ref noWifi, 10);
			mediaManager.DisplayWarningMessage("No hay conexión a Internet");
		}
	}

	private void StopRecordButtonOnClickHandler()
	{
		if(btnRec != null)
		{
			btnRec.GetComponent<Renderer>().material = UI_SpeechStart;
			btnRec.OnAnimationComplete -= StopRecordButtonOnClickHandler;
			btnRec.OnAnimationComplete += StartRecordButtonOnClickHandler;
		}
		speechRecognitionResult.text = "Stopped Recording";
		gifRipple.SetActive (false);
		speechRecognition.StopRecord();
		gifProcessing.SetActive (true);
	}

	private void LanguageDropdownOnValueChanged(int value)
	{
		value = 1;
		speechRecognition.SetLanguage((Enumerators.LanguageCode)value);
	}

	private void SpeechRecognizedFailedEventHandler(string obj, long requestIndex)
	{
		speechRecognitionResult.text = "Error: " + obj;
	}

	private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj, long requestIndex)
	{
		if (obj != null && obj.results.Length > 0)
		{
			speechRecognitionResult.text = /*"Speech Recognition succeeded!\n Best match: " + */obj.results[0].alternatives[0].transcript;

//			string other = "\nAlternatives: ";
//
//			foreach (var result in obj.results)
//			{ 
//				foreach (var alternative in result.alternatives)
//				{
//					if (obj.results[0].alternatives[0] != alternative)
//						other += alternative.transcript + ",\n";
//				}
//			}

			//speechRecognitionResult.text += other;
		}
		else
		{
			gifProcessing.SetActive (false);
			mediaManager.DisplayWarningMessage("No se detectaron palabras.");
		}
		gifProcessing.SetActive (false);
		mediaManager.ValidateAnswer (speechRecognitionResult.text);
	}
}
