using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class SubReader : MonoBehaviour
{
    private Hashtable subtitlesLastSeconds;
    private Hashtable subtitlesText;


    public SubReader() {

        //read file
        fileReader();
    }

    public string fileReader() {

        StreamReader subtitles = new StreamReader("Assets/TextFiles/Lesson01-01.txt");

        int lineCounter = 0;
        //guardo todos los ultimos segundos de los subtitulos y su texto
        while (!subtitles.EndOfStream)
        {
            lineCounter++;
            string line = subtitles.ReadLine();

            int firstComma = line.IndexOf(",");
            int lastSquareBracket = line.IndexOf("]");
            string lastInterval = line.Substring(firstComma, lastSquareBracket-firstComma);
            // Add to hasmap last second interval. 
            subtitlesLastSeconds.Add(lineCounter, lastInterval);

            //agregar texto a otro hashMap

            int lastBar = line.IndexOf("|");            
            string subtitleText = line.Substring(lastSquareBracket, lastBar - lastSquareBracket);
            subtitlesText.Add(lineCounter, subtitleText);
        }

        subtitles.Close();

        return "";
    }

    public string readSubtitleLine(int duration) {

        ICollection hashValues = subtitlesLastSeconds.Values;
        
        // search if duration is in last subtitle second of the hash
        foreach (int val in subtitlesLastSeconds.Keys){
            int subtitleLastSecond = (int)subtitlesLastSeconds[val];
            if (duration == subtitleLastSecond) {
                    //devuelvo subtitulo

            }
        }
        return "";
    }
}