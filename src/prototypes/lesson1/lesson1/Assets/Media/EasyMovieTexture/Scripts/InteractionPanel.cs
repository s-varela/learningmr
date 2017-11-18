using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;
using System.Diagnostics;

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
	Stopwatch counter;

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
		this.counter = new Stopwatch ();
		speechRecognition = GCSpeechRecognition.Instance;
		speechRecognition.RecognitionSuccessEvent += SpeechRecognizedSuccessEventHandler; // Posiblemente
		speechRecognition.RecognitionFailedEvent += SpeechRecognizedFailedEventHandler;   // redundantes
	}
	
	// Update is called once per frame
	// Update is called once per frame
	void Update () {
		if (ElapsedTime (10000)) {
			Timeout ();
		}
	}

	void Restart () 
	{
		this.counter.Reset ();
		this.counter.Start ();
	}

	bool ElapsedTime(int seconds)
	{
		return this.counter.ElapsedMilliseconds > seconds;
	}

	private void ButtonTecladoOnClick()
	{
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

		speechRecognition.RecognitionSuccessEvent += SpeechRecognizedSuccessEventHandler;
		speechRecognition.RecognitionFailedEvent += SpeechRecognizedFailedEventHandler;

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
			Restart ();
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
		this.counter.Stop ();
		speechRecognitionResult.text = "Stopped Recording";
		gifRipple.SetActive (false);
		speechRecognition.StopRecord();
		gifProcessing.SetActive (true);
	}

	private void Timeout()
	{
		speechRecognition.StopRecord();
		if(btnRec != null)
		{
			btnRec.GetComponent<Renderer>().material = UI_SpeechStart;
			btnRec.OnAnimationComplete -= StopRecordButtonOnClickHandler;
			btnRec.OnAnimationComplete += StartRecordButtonOnClickHandler;
		}
		gifRipple.SetActive (false);
		mediaManager.DisplayWarningMessage("No se detectaron palabras.");
	}

	private void LanguageDropdownOnValueChanged(int value)
	{
		value = 1;
		speechRecognition.SetLanguage((Enumerators.LanguageCode)value);
	}

	private void SpeechRecognizedFailedEventHandler(string obj, long requestIndex)
	{
		speechRecognition.RecognitionSuccessEvent -= SpeechRecognizedSuccessEventHandler;
		speechRecognition.RecognitionFailedEvent -= SpeechRecognizedFailedEventHandler;
	}

	private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj, long requestIndex)
	{
		this.counter.Stop ();
		gifProcessing.SetActive (false);
		if (obj != null && obj.results.Length > 0)
		{
			speechRecognitionResult.text = obj.results[0].alternatives[0].transcript;
		}
		else
		{
			mediaManager.DisplayWarningMessage("No se detectaron palabras.");
		}
		mediaManager.ValidateAnswer (speechRecognitionResult.text);
		speechRecognition.RecognitionSuccessEvent -= SpeechRecognizedSuccessEventHandler;
		speechRecognition.RecognitionFailedEvent -= SpeechRecognizedFailedEventHandler;
	}
}
