using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AudioManager
	{
        private Hashtable hashAudios;
        private String pathAudio= "Audios/";

    public AudioManager ()
		{
        HashAudios = new Hashtable();

        HashAudios.Add("1_1_1", "1_1_1");
        HashAudios.Add("1_1_2", "1_1_2");
        HashAudios.Add("1_1_3", "1_1_3");
        HashAudios.Add("1_1_4", "1_1_4");
        HashAudios.Add("1_2_1", "1_2_1");
        HashAudios.Add("1_2_2", "1_2_2");
        HashAudios.Add("1_2_3", "1_2_3");
        HashAudios.Add("1_2_4", "1_2_4");
        HashAudios.Add("1_3_1", "1_3_1");
        HashAudios.Add("1_3_2", "1_3_2");
        HashAudios.Add("1_3_3", "1_3_3");
        HashAudios.Add("1_3_4", "1_3_4");
        HashAudios.Add("1_3_5", "1_3_5");
        HashAudios.Add("1_4_1", "1_4_1");
        HashAudios.Add("1_4_2", "1_4_2");
        HashAudios.Add("1_4_3", "1_4_3");
        HashAudios.Add("1_4_4", "1_4_4");
        HashAudios.Add("1_4_5", "1_4_5");
        HashAudios.Add("1_4_6", "1_4_6");
        HashAudios.Add("1_4_7", "1_4_7");
        HashAudios.Add("1_4_8", "1_4_8");

    //Lesson 02
        HashAudios.Add("2_1_1", "2_1_1");
        HashAudios.Add("2_1_2", "2_1_2");
        HashAudios.Add("2_1_3", "2_1_3");
        HashAudios.Add("2_1_4", "2_1_4");
        HashAudios.Add("2_1_5", "2_1_5");
        HashAudios.Add("2_1_6", "2_1_6");

        //Lesson 03
        HashAudios.Add("3_1_1", "3_1_1");
        HashAudios.Add("3_1_2", "3_1_2"); 
        HashAudios.Add("3_1_3", "3_1_3");
        HashAudios.Add("3_1_4", "3_1_4");
        HashAudios.Add("3_1_5", "3_1_5");
        HashAudios.Add("3_1_6", "3_1_6");
        HashAudios.Add("3_1_7", "3_1_7");
        HashAudios.Add("3_1_8", "3_1_8");
        HashAudios.Add("3_1_9", "3_1_9");
        HashAudios.Add("3_1_10", "3_1_10");

    //correction sounds 
    HashAudios.Add("correct", "correct");
    HashAudios.Add("incorrect", "incorrect");
    }

    public Hashtable HashAudios { get; set; }

    public String getAudioPathName(string name)
        {
            if (HashAudios.ContainsKey(name))
            {
                return pathAudio+HashAudios[name].ToString();
            }
            return null;
        }


    /*IEnumerator DownloadAudios()
    {
        //hashAudiosName = audioManager.HashAudios;
        //Tomo el path de audios desde VRExperience
        string audiosPath = VRExperience.Instance.ResourcesPath + VRExperience.Instance.AudioPath;
        //HashAudios = new Hashtable();

        WWW audioLoader = new WWW("file://" + audiosPath + "1_1_1.mp3");
        HashAudios.Add("1_1_1", audioLoader.GetAudioClip(true,true));

        /* foreach (string audioKey in audios)
         {
             //string audioName = HashAudios[audioKey].ToString();
             WWW audioLoader = new WWW("file://" + audiosPath + audioKey + ".mp3");
             while (!audioLoader.isDone)
                 yield return null;
             HashAudios.Add(audioKey, audioLoader.GetAudioClip(false));
         }
         
        while (!audioLoader.isDone)
            yield return null;
    }*/



}

