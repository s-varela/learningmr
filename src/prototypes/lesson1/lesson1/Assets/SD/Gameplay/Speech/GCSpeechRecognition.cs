using System;
using System.Collections.Generic;
using UnityEngine;

    public class GCSpeechRecognition : MonoBehaviour
    {
        public event Action<RecognitionResponse, long> RecognitionSuccessEvent;
        public event Action<string, long> RecognitionFailedEvent;

        public event Action StartedRecordEvent;
        public event Action<AudioClip> FinishedRecordEvent;
        public event Action RecordFailedEvent;

        public event Action BeginTalkingEvent;
        public event Action<AudioClip> EndTalkingEvent;


        private static GCSpeechRecognition instance;
        public static GCSpeechRecognition Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameObject("[Singleton]GCSpeechRecognition").AddComponent<GCSpeechRecognition>();

                return instance;
            }
        }


        private ServiceLocator serviceLocator;

        private ISpeechRecognitionManager speechRecognitionManager;
        private IMediaManager mediaManager;

        public ServiceLocator ServiceLocator { get { return serviceLocator; } }

        [Header("Prefab Config Settings")]
        public int currentConfigIndex = 0;
        public List<Config> configs;

        [Header("Prefab Object Settings")]
        public bool isDontDestroyOnLoad = false;
        public bool isFullDebugLogIfError = false;
        public bool isUseAPIKeyFromPrefab = false;

        [Header("Prefab Fields")]
        public string apiKey;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            if (isDontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);

            instance = this;

            serviceLocator = new ServiceLocator();
            serviceLocator.InitServices();

            mediaManager = serviceLocator.Get<IMediaManager>();
            speechRecognitionManager = serviceLocator.Get<ISpeechRecognitionManager>();

            mediaManager.StartedRecordEvent += StartedRecordEventHandler;
            mediaManager.FinishedRecordEvent += FinishedRecordEventHandler;
            mediaManager.RecordFailedEvent += RecordFailedEventHandler;
            mediaManager.BeginTalkingEvent += BeginTalkingEventHandler;
            mediaManager.EndTalkingEvent += EndTalkingEventHandler;

            speechRecognitionManager.SetConfig(configs[currentConfigIndex]);

            speechRecognitionManager.RecognitionSuccessEvent += RecognitionSuccessEventHandler;
            speechRecognitionManager.RecognitionFailedEvent += RecognitionFailedEventHandler;
        }

        private void Update()
        {
            if (instance == this)
            {
                serviceLocator.Update();
            }
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                mediaManager.StartedRecordEvent -= StartedRecordEventHandler;
                mediaManager.FinishedRecordEvent -= FinishedRecordEventHandler;
                mediaManager.RecordFailedEvent -= RecordFailedEventHandler;
                mediaManager.BeginTalkingEvent -= BeginTalkingEventHandler;
                mediaManager.EndTalkingEvent -= EndTalkingEventHandler;

                speechRecognitionManager.RecognitionSuccessEvent -= RecognitionSuccessEventHandler;
                speechRecognitionManager.RecognitionFailedEvent -= RecognitionFailedEventHandler;

                instance = null;
                serviceLocator.Dispose();
            }
        }

        public void StartRecord(bool isEnabledVoiceDetection = true)
        {
            mediaManager.IsEnabledVoiceDetection = isEnabledVoiceDetection;
            mediaManager.StartRecord();
        }

        public void StopRecord()
        {
            mediaManager.StopRecord();
        }

        public void Recognize(AudioClip clip, List<string[]> contexts, Enumerators.LanguageCode language)
        {
            speechRecognitionManager.Recognize(clip, contexts, language);
        }

        public void SetLanguage(Enumerators.LanguageCode language)
        {
            speechRecognitionManager.CurrentConfig.defaultLanguage = language;
        }

        private void RecognitionSuccessEventHandler(RecognitionResponse arg1, long arg2)
        {
            if (RecognitionSuccessEvent != null)
                RecognitionSuccessEvent(arg1, arg2);
        }

        private void RecognitionFailedEventHandler(string arg1, long arg2)
        {
            if (RecognitionFailedEvent != null)
                RecognitionFailedEvent(arg1, arg2);
        }

        private void RecordFailedEventHandler()
        {
            if (RecordFailedEvent != null)
                RecordFailedEvent();
        }

        private void BeginTalkingEventHandler()
        {
            if (BeginTalkingEvent != null)
                BeginTalkingEvent();
        }

        private void EndTalkingEventHandler(AudioClip clip)
        {
            if (EndTalkingEvent != null)
                EndTalkingEvent(clip);

            speechRecognitionManager.Recognize(clip, null, speechRecognitionManager.CurrentConfig.defaultLanguage);
        }

        private void StartedRecordEventHandler()
        {
            if (StartedRecordEvent != null)
                StartedRecordEvent();
        }

        private void FinishedRecordEventHandler(AudioClip clip)
        {
            if (FinishedRecordEvent != null)
                FinishedRecordEvent(clip);

            if (!mediaManager.IsEnabledVoiceDetection)
                speechRecognitionManager.Recognize(clip, null, speechRecognitionManager.CurrentConfig.defaultLanguage);
        }
    }
