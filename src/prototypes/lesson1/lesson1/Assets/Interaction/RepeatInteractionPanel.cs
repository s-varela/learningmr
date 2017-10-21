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
        [SerializeField] Text keyboardInp;

       // [SerializeField] GameObject gifRipple;
      //  [SerializeField] GameObject gifProcessing;
       // [SerializeField] TextMesh answer;

        [SerializeField] Material UI_SpeechStart;
        [SerializeField] Material UI_SpeechStop;


        [SerializeField] private MediaManager mediaManager;

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
            keyboardInp.text = "";
        }

        private void repeatAudio() {
            mediaManager.repeatAudio();
        }

        private void OnDestroy()
        {
            speechRecognition.RecognitionSuccessEvent -= SpeechRecognizedSuccessEventHandler;
            speechRecognition.RecognitionFailedEvent -= SpeechRecognizedFailedEventHandler;
        }


        private void StartRecordButtonOnClickHandler()
        {

            speechRecognitionResult.text = string.Empty;
            speechRecognitionResult.text = "Recording...";
            //answer.text = "";
           // gifRipple.SetActive(true);
           // gifProcessing.SetActive(false);
            if (btnRec != null)
            {
                btnRec.GetComponent<Renderer>().material = UI_SpeechStop;
                btnRec.OnAnimationComplete -= StartRecordButtonOnClickHandler;
                btnRec.OnAnimationComplete += StopRecordButtonOnClickHandler;
            }
            speechRecognition.StartRecord(false);
        }

        private void StopRecordButtonOnClickHandler()
        {
            if (btnRec != null)
            {
                btnRec.GetComponent<Renderer>().material = UI_SpeechStart;
                btnRec.OnAnimationComplete -= StopRecordButtonOnClickHandler;
                btnRec.OnAnimationComplete += StartRecordButtonOnClickHandler;
            }
            speechRecognitionResult.text = "Stopped Recording";
            //gifRipple.SetActive(false);
            speechRecognition.StopRecord();
            //gifProcessing.SetActive(true);
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
                speechRecognitionResult.text = "No words were detected.";
            }
            //gifProcessing.SetActive(false);
            mediaManager.ValidateAnswer(speechRecognitionResult.text);
        }

    }
}
