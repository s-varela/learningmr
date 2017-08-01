namespace GoogleSpeech.Plugins.GoogleCloud.SpeechRecognition
{
    public interface IVoiceDetectionManager
    {
        bool CheckVoice(byte[] data);
    }
}