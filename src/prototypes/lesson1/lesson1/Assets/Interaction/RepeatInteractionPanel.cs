using System;
using System.Linq;
using System.Text;
using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;
using UnityEngine.UI;

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
        [SerializeField] private VRUIAnimationClick btnRepeat;
       	[SerializeField] private TextMesh speechRecognitionResult;
        [SerializeField] private GCSpeechRecognition speechRecognition;
        [SerializeField] GameObject teclado;
		[SerializeField] GameObject panelInfo;
        [SerializeField] GameObject noWifi;
        [SerializeField] Text keyboardInp;
        [SerializeField] Material UI_SpeechStart;
        [SerializeField] Material UI_SpeechStop;
        [SerializeField] Material radioButtonSelected;
        [SerializeField] Material radioButtonNotSelected;
        [SerializeField] private MediaManager mediaManager;

		[SerializeField] GameObject gifRipple;
		[SerializeField] GameObject gifProcessing;
		[SerializeField] TextMesh answer;

        public int selectedNumberRadio;
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

            if (btnRepeat != null)
            {
                btnRepeat.OnAnimationComplete += repeatAudio;
            }

			noWifi.SetActive (false);
            speechRecognition = GCSpeechRecognition.Instance;
            speechRecognition.RecognitionSuccessEvent += SpeechRecognizedSuccessEventHandler;
            speechRecognition.RecognitionFailedEvent += SpeechRecognizedFailedEventHandler;
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void ButtonTecladoOnClick()
        {
            teclado.SetActive(true);
            panelInfo.SetActive(false);
            keyboardInp.text = "";
        }

        private void repeatAudio() {

            string selectedString = "";
            radioButtonInteraction selectedRadio = radioButtonInteraction.Instance;
            int selecRadio = selectedRadio.WhichRadioSelected();
            selectedString = GameObject.Find("TextInfo" + selecRadio).GetComponent<TextMesh>().text;

            mediaManager.repeatAudio(selectedString);
        }

        private void OnDestroy()
        {
            speechRecognition.RecognitionSuccessEvent -= SpeechRecognizedSuccessEventHandler;
            speechRecognition.RecognitionFailedEvent -= SpeechRecognizedFailedEventHandler;
        }

        private void StartRecordButtonOnClickHandler()
        {
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
			    speechRecognition.StartRecord (false);
		    } 
            else 
		    {
				noWifi.SetActive (true);
		    }
        }

        private void StopRecordButtonOnClickHandler()
        {
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

        private void LanguageDropdownOnValueChanged(int value)
        {
            value = 1;
            speechRecognition.SetLanguage((Enumerators.LanguageCode)value);
        }

        private void SpeechRecognizedFailedEventHandler(string obj, long requestIndex)
        {
            //speechRecognitionResult.text = "Error: " + obj;
        }

        private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj, long requestIndex)
        {
            string result = "";
            if (obj != null && obj.results.Length > 0)
            {
               result = obj.results[0].alternatives[0].transcript;
            }
            else
            {
                result = "No words were detected.";
            }
            gifProcessing.SetActive(false);
			answer.text = result.Trim ();
            mediaManager.ValidateAnswerRepeatPanelVoice(result);
        }
    }
}
