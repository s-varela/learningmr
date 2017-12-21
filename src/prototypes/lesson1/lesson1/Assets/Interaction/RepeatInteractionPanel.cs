using System;
using System.Linq;
using System.Text;
using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;

namespace Assets.Interaction
{
    public class RepeatInteractionPanel : MonoBehaviour
    {
        [SerializeField] private VRInput input;
        [SerializeField] private VRCameraFade fader;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private float distance = 5;
        [SerializeField] private VRUIAnimationClick btnTeclado;
        [SerializeField] private VRUIAnimationClick btnRec;
       	[SerializeField] private TextMesh speechRecognitionResult;
        [SerializeField] private GCSpeechRecognition speechRecognition;
        [SerializeField] GameObject teclado;
		[SerializeField] GameObject panelInfo;
        [SerializeField] GameObject noWifi;
        [SerializeField] Text keyboardInp;
        [SerializeField] Material UI_SpeechStart;
        [SerializeField] Material UI_SpeechStop;
        //[SerializeField] Material radioButtonSelected;
        //[SerializeField] Material radioButtonNotSelected;
        [SerializeField] private MediaManager mediaManager;

		[SerializeField] GameObject gifRipple;
		[SerializeField] GameObject gifProcessing;
		[SerializeField] TextMesh answer;
		[SerializeField] private Blinker blinker;
		Stopwatch counter;
		bool recording;

        // Use this for initialization
        void Start()
        {

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
            speechRecognition.RecognitionSuccessEvent += SpeechRecognizedSuccessEventHandler; // Posiblemente
            speechRecognition.RecognitionFailedEvent += SpeechRecognizedFailedEventHandler;   // redundantes
			recording = false;
			this.counter = new Stopwatch ();
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

        // Update is called once per frame
        void Update()
        {
			if (recording && ElapsedTime (10000)) {
				Timeout ();
			}
        }

        private void ButtonTecladoOnClick()
        {
            teclado.SetActive(true);
            panelInfo.SetActive(false);
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
			speechRecognition.RecognitionSuccessEvent += SpeechRecognizedSuccessEventHandler;
			speechRecognition.RecognitionFailedEvent += SpeechRecognizedFailedEventHandler;

            bool connection = CheckConnectivity.checkInternetStatus ();
		    if (connection) {
				//speechRecognitionResult.text = string.Empty;
                answer.text = "";
                //answer.text = "Recording...";
                gifRipple.SetActive (true);
                gifProcessing.SetActive (false);
                if (btnRec != null) {
                    btnRec.GetComponent<Renderer> ().material = UI_SpeechStop;
                    btnRec.OnAnimationComplete -= StartRecordButtonOnClickHandler;
                    btnRec.OnAnimationComplete += StopRecordButtonOnClickHandler;
            	}
				Restart ();
				recording = true;
			    speechRecognition.StartRecord (false);
		    } 
            else 
		    {
				noWifi.SetActive (true);
				blinker.SetComponent (ref noWifi, 10);
		    }
        }

        private void StopRecordButtonOnClickHandler()
        {
			mediaManager.SetInactiveButtonGuia ();
            if (btnRec != null)
            {
                btnRec.GetComponent<Renderer>().material = UI_SpeechStart;
                btnRec.OnAnimationComplete -= StopRecordButtonOnClickHandler;
                btnRec.OnAnimationComplete += StartRecordButtonOnClickHandler;
            }
            gifRipple.SetActive(false);
            speechRecognition.StopRecord();
            gifProcessing.SetActive(true);
        }

		private void Timeout()
		{
			if(btnRec != null)
			{
				btnRec.GetComponent<Renderer>().material = UI_SpeechStart;
				btnRec.OnAnimationComplete -= StopRecordButtonOnClickHandler;
				btnRec.OnAnimationComplete += StartRecordButtonOnClickHandler;
			}
			this.counter.Stop ();
			gifRipple.SetActive (false);
			gifProcessing.SetActive (false);
			speechRecognition.StopRecord();
			recording = false;
			answer.text = "No se detectaron palabras.";
		}

        private void LanguageDropdownOnValueChanged(int value)
        {
            value = 1;
            speechRecognition.SetLanguage((Enumerators.LanguageCode)value);
        }

        private void SpeechRecognizedFailedEventHandler(string obj, long requestIndex)
        {
            //speechRecognitionResult.text = "Error: " + obj;
			speechRecognition.RecognitionSuccessEvent -= SpeechRecognizedSuccessEventHandler;
			speechRecognition.RecognitionFailedEvent -= SpeechRecognizedFailedEventHandler;
        }

        private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj, long requestIndex)
        {
			this.counter.Stop ();
			recording = false;
            string result = "";
            if (obj != null && obj.results.Length > 0)
            {
               result = obj.results[0].alternatives[0].transcript;
            }
            gifProcessing.SetActive(false);
			answer.text = result.Trim ();
            mediaManager.ValidateAnswerRepeatPanelVoice(result);
			speechRecognition.RecognitionSuccessEvent -= SpeechRecognizedSuccessEventHandler;
			speechRecognition.RecognitionFailedEvent -= SpeechRecognizedFailedEventHandler;

        }
    }
}
