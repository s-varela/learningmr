using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

using GoogleSpeech.Plugins.GoogleCloud.SpeechRecognition;

    public class Speech : MonoBehaviour
    {
		[SerializeField] private VRInput input;
		[SerializeField] private VRCameraFade fader;
		[SerializeField] private Transform cameraTransform;
		[SerializeField] private float distance = 5;
		//[SerializeField] private GameObject menuBase;
		[SerializeField] private VRUIAnimationClick btnOK;
		[SerializeField] private VRUIAnimationClick btnCancel;

		[SerializeField] private TextMesh speechRecognitionResult;

        private GCSpeechRecognition speechRecognition;

        private Button startRecordButton,
                       stopRecordButton;

        private Image speechRecognitionState;



        private Toggle isRuntimeDetectionToggle;

        private Dropdown languageDropdown;
	

        private void Start()
        {

			//input.OnCancel += ToggleMenu;
			if(btnOK != null)
			{
				btnOK.OnAnimationComplete += StartRecordButtonOnClickHandler;
			}
			if (btnCancel != null)
			{
				btnCancel.OnAnimationComplete += StopRecordButtonOnClickHandler;
			}



            speechRecognition = GCSpeechRecognition.Instance;
            speechRecognition.RecognitionSuccessEvent += SpeechRecognizedSuccessEventHandler;
            speechRecognition.RecognitionFailedEvent += SpeechRecognizedFailedEventHandler;

            /*startRecordButton = transform.Find("Canvas/Button_StartRecord").GetComponent<Button>();
            stopRecordButton = transform.Find("Canvas/Button_StopRecord").GetComponent<Button>();

            speechRecognitionState = transform.Find("Canvas/Image_RecordState").GetComponent<Image>();

            speechRecognitionResult = transform.Find("Canvas/Text_Result").GetComponent<Text>();

            isRuntimeDetectionToggle = transform.Find("Canvas/Toggle_IsRuntime").GetComponent<Toggle>();

            languageDropdown = transform.Find("Canvas/Dropdown_Language").GetComponent<Dropdown>();


            startRecordButton.onClick.AddListener(StartRecordButtonOnClickHandler);
            stopRecordButton.onClick.AddListener(StopRecordButtonOnClickHandler);

            speechRecognitionState.color = Color.white;
            startRecordButton.interactable = true;
            stopRecordButton.interactable = false;

            languageDropdown.ClearOptions();

            for (int i = 0; i < Enum.GetNames(typeof(Enumerators.LanguageCode)).Length; i++)
            {
                languageDropdown.options.Add(new Dropdown.OptionData(((Enumerators.LanguageCode)i).ToString()));
            }

            languageDropdown.onValueChanged.AddListener(LanguageDropdownOnValueChanged);

            languageDropdown.value = 1;

*/

        }

        private void OnDestroy()
        {
            speechRecognition.RecognitionSuccessEvent -= SpeechRecognizedSuccessEventHandler;
            speechRecognition.RecognitionFailedEvent -= SpeechRecognizedFailedEventHandler;
        }


        private void StartRecordButtonOnClickHandler()
        {
           /* startRecordButton.interactable = false;
            stopRecordButton.interactable = true;
            speechRecognitionState.color = Color.red;
             */
            speechRecognitionResult.text = string.Empty;
           
            speechRecognition.StartRecord(isRuntimeDetectionToggle.isOn);
        }

        private void StopRecordButtonOnClickHandler()
        {
            stopRecordButton.interactable = false;
            speechRecognitionState.color = Color.yellow;
            speechRecognition.StopRecord();
        }

        private void LanguageDropdownOnValueChanged(int value)
        {
            speechRecognition.SetLanguage((Enumerators.LanguageCode)value);
        }

        private void SpeechRecognizedFailedEventHandler(string obj, long requestIndex)
        {
            speechRecognitionResult.text = "Speech Recognition failed with error: " + obj;

            if (!isRuntimeDetectionToggle.isOn)
            {
                speechRecognitionState.color = Color.green;
                startRecordButton.interactable = true;
                stopRecordButton.interactable = false;
            }
        }

        private void SpeechRecognizedSuccessEventHandler(RecognitionResponse obj, long requestIndex)
        {
            if (!isRuntimeDetectionToggle.isOn)
            {
                startRecordButton.interactable = true;
                speechRecognitionState.color = Color.green;
            }

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
    }