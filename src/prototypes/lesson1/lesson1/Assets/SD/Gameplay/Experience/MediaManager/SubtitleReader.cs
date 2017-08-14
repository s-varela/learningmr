﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;

public class SubtitleReader : MonoBehaviour
{ 
    private Hashtable subtitlesLastSeconds;
	private string pathSubs = "/lesson1-data/subs/"; 

    private Hashtable subtitlesText;
	private string lastSubtitle;

    public SubtitleReader() {
        subtitlesLastSeconds = new Hashtable();
        subtitlesText = new Hashtable();
    }

    // Use this for initialization
    void Start()
    {
    }
		
        // Update is called once per frame
        void Update()
    {

    }
		
    public void RestFileReader(string videoName)
    {
        try
        {
			string subtitleText=null;
			subtitlesLastSeconds = new Hashtable();
			subtitlesText = new Hashtable();
            string fileName = videoName.Replace("mp4", "txt");
            StreamReader subtitles = new StreamReader(Application.persistentDataPath + pathSubs + fileName);
            int lineCounter = 0;
            //guardo todos los ultimos segundos de los subtitulos y su texto
            while (!subtitles.EndOfStream)
            {
                string line = subtitles.ReadLine();
                int firstBracket = line.IndexOf("[") + 1;
                int lastSquareBracket = line.IndexOf("]") - 1;
                int lastInterval = int.Parse(line.Substring(firstBracket, lastSquareBracket));
                // Add to hasmap last second interval. 
                subtitlesLastSeconds.Add(lineCounter, lastInterval);

                //agregar texto a otro hashMap
                int parent = line.IndexOf("]") + 1;
                int dash = line.IndexOf("|");
                int between = dash - parent;
                subtitleText = line.Substring(parent, between);

                subtitlesText.Add(lineCounter, subtitleText);
                lineCounter++;
            }
			lastSubtitle=subtitleText;
            subtitles.Close();

        }
        catch (System.Exception ex)
        {
            Debug.Log("ERROR_OBTENIENDO_SUBTITULOS: " + ex.Message);
        }

    }


    public Hashtable ReadSubtitleLine(long duration)
	{
		Hashtable subtitleToReturn = new Hashtable ();
		int intDuration = (int)duration;
		ICollection hashValuesLast = subtitlesLastSeconds.Values;
		int howMany = hashValuesLast.Count;
		int[] lastSeconds = new int[howMany];
		hashValuesLast.CopyTo(lastSeconds, 0);

		ICollection hashValuesSubs = subtitlesText.Values;
		string[] subs = new string[howMany];
		hashValuesSubs.CopyTo(subs, 0);
		// search if duration is in last subtitle second of the hash
		for (int i = 0; i < howMany; i++)
		{

			if (lastSeconds[i] < intDuration)
			{
				//devuelvo subtitulo
				if (i == howMany - 1) {
					if (subtitleToReturn.ContainsKey (1)) {
						subtitleToReturn [1] = subs [i];
					} else {
						subtitleToReturn.Add (1, subs [i]);
					}
				} else {
					if (subtitleToReturn.ContainsKey (0)) {
						subtitleToReturn [0] = subs [i];
					} else {
						subtitleToReturn.Add (0, subs [i]);
					}
				}
			}
		}

		return subtitleToReturn;
	}

    /*public string ReadSubtitleLine(long duration)
    {
        try
        {
            string subToReturn = "";
            int intDuration = (int)duration;
            ICollection hashValuesLast = subtitlesLastSeconds.Values;
            int howMany = hashValuesLast.Count;
            int[] lastSeconds = new int[howMany];
            hashValuesLast.CopyTo(lastSeconds, 0);

            ICollection hashValuesSubs = subtitlesText.Values;
            string[] subs = new string[howMany];
            hashValuesSubs.CopyTo(subs, 0);
            // search if duration is in last subtitle second of the hash
            for (int i = 0; i < howMany; i++)
            {

                if (lastSeconds[i] < intDuration)
                {
                    //devuelvo subtitulo
                    subToReturn = subs[i];
                }
            }

            return subToReturn;

        }
        catch (System.Exception ex)
        {
            Debug.Log("ERROR_OBTENIENDO_SUBTITULOS: " + ex.Message);
        }
        return null;
    }*/

    public bool IsLastSubtitle(Hashtable hashsub)
    {
		return hashsub.ContainsKey(1);
	}

    public string GetHashSubValue(Hashtable hashsub)
    {
        if (hashsub.ContainsKey(1))
        {
            return hashsub[1].ToString();
        }
        if (hashsub.ContainsKey(0))
        {
            return hashsub[0].ToString();
        }
        return "";
    }
}