using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;

public class SubtitleReader : MonoBehaviour
{ 
    private Hashtable subtitlesLastSeconds;
	private string pathSubs = "/lesson1-data/subs/"; 

    private Hashtable subtitlesText;
    public Text textObject;

    public SubtitleReader() {
        subtitlesLastSeconds = new Hashtable();
        subtitlesText = new Hashtable();
        //FileReader();
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


    /*public void FileReader()
    {
        try
        {

            //TextAsset subtitlesFile = Resources.Load("Assets/TextFiles/Lesson01-01") as TextAsset;
			TextAsset subtitlesFile = Resources.Load(Application.persistentDataPath+pathSubs+"Lesson01-01") as TextAsset;

            string text = subtitlesFile.text;
            string[] textLines = Regex.Split(text, "\n|\r|\r\n");
            //StreamReader subtitles = new StreamReader ("Assets/TextFiles/Lesson01-01.txt");
            int lineCounter = 0;
            //guardo todos los ultimos segundos de los subtitulos y su texto
            for (int i = 0; i < textLines.Length; i++)
            {

                string line = textLines[i];
                int firstBracket = line.IndexOf("[") + 1;
                int lastSquareBracket = line.IndexOf("]") - 1;
                int lastInterval = int.Parse(line.Substring(firstBracket, lastSquareBracket));

                // Add to hasmap last second interval. 
                subtitlesLastSeconds.Add(lineCounter, lastInterval);

                //agregar texto a otro hashMap
                int parent = line.IndexOf("]") + 1;
                int dash = line.IndexOf("|");
                int between = dash - parent;
                string subtitleText = line.Substring(parent, between);

                subtitlesText.Add(lineCounter, subtitleText);
                lineCounter++;
            }


        }
        catch (System.Exception ex)
        {
            Debug.Log("ERROR_OBTENIENDO_SUBTITULOS: " + ex.Message);
        }

    }*/

    public void FileReader()
    {
		try
		{
	
            StreamReader subtitles = new StreamReader(Application.persistentDataPath + pathSubs + "Lesson01-01.txt");
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
                string subtitleText = line.Substring(parent, between);

                subtitlesText.Add(lineCounter, subtitleText);
                lineCounter++;
            }
            subtitles.Close();

	    }
	    catch (System.Exception ex)
	    {
		    Debug.Log("ERROR_OBTENIENDO_SUBTITULOS: " + ex.Message);
	    }

    }

    public void RestFileReader(string videoName)
    {
        try
        {
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
                string subtitleText = line.Substring(parent, between);

                subtitlesText.Add(lineCounter, subtitleText);
                lineCounter++;
            }
            subtitles.Close();

        }
        catch (System.Exception ex)
        {
            Debug.Log("ERROR_OBTENIENDO_SUBTITULOS: " + ex.Message);
        }

    }


    public string ReadSubtitleLine(long duration)
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
}