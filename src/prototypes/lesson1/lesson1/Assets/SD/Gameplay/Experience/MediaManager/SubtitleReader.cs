using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;

public class SubtitleReader : MonoBehaviour
{ 
    private Hashtable subtitlesLastSeconds;
    private Hashtable subtitlesText;
    public Text textObject;

    public SubtitleReader() {
        subtitlesLastSeconds = new Hashtable();
        subtitlesText = new Hashtable();
        FileReader();
    }

    // Use this for initialization
    void Start()
    {
    }

    void Awake()
    {
      textObject = GetComponent<UnityEngine.UI.Text>();
    }

        // Update is called once per frame
        void Update()
    {

    }

    public void FileReader()
    {
        StreamReader subtitles = new StreamReader("Assets/TextFiles/Lesson01-01.txt");
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
            int parent = line.IndexOf("]")+1;
            int dash = line.IndexOf("|");
            int between = dash - parent;
            string subtitleText = line.Substring(parent, between);

            subtitlesText.Add(lineCounter, subtitleText);
            lineCounter++;
        }
        subtitles.Close();
    }

    public string ReadSubtitleLine(long duration)
    {
        string subToReturn = "";
        ICollection hashValuesLast = subtitlesLastSeconds.Values;
        int howMany = hashValuesLast.Count;
        int[] lastSeconds = new int[howMany];
        hashValuesLast.CopyTo(lastSeconds, 0);

        ICollection hashValuesSubs = subtitlesText.Values;
        string[] subs = new string[howMany];
        hashValuesSubs.CopyTo(subs, 0);
		//nls
        // search if duration is in last subtitle second of the hash
        for (int i = 0; i < howMany ; i++)
        {
            
            if (lastSeconds[i] < duration )
            {
                //  UnityEngine.Debug.Log("lastSeconds[i] : " + lastSeconds[i]);
                //  UnityEngine.Debug.Log("entro: ");
                //devuelvo subtitulo
                subToReturn = subs[i];
            }
        }

        return subToReturn;
    }
}
