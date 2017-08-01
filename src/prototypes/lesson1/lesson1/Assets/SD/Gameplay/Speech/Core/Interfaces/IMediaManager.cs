using System;
using UnityEngine;

namespace GoogleSpeech.Plugins.GoogleCloud.SpeechRecognition
{
    public interface IMediaManager
    {
        event Action StartedRecordEvent;
        event Action<AudioClip> FinishedRecordEvent;
        event Action RecordFailedEvent;

        event Action BeginTalkingEvent;
        event Action<AudioClip> EndTalkingEvent;

        bool IsEnabledVoiceDetection { get; set; }
        bool IsCanWork { get; set; }
        bool IsRecording { get; set; }

        void StopRecord();
        void StartRecord();
    }
}