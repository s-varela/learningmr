using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoadPanel : MonoBehaviour
{

    [SerializeField] TextMesh info;
    [SerializeField] TextMesh subtitle;
    [SerializeField] TextMesh sub;
    private string aux;
    //private string subAnt;
    private string[] arrayText;
    private int count = 0;

    // Use this for initialization
    void Start()
    {
        aux = subtitle.text;
    }

    // Update is called once per frame
    void Update()
    {

        if (!subtitle.text.Equals(aux) && !subtitle.text.Equals(""))
        {
            aux = subtitle.text;
            sub.text = subtitle.text;
            if (count == 0)
            {
                info.text = sub.text;
                count++;
            }
            else
            {
                info.text = info.text + "\n" + sub.text;
                //count++;
            }
        }
    }

	public string[] ArrayText()
	{
		arrayText = info.text.Split ('\n');
		return arrayText;
	}

	public void DeleteSub()
	{
		info.text = "";
		count = 0;
        //AHORA HAY QUE LIMPIAR EL TEXTO DE TODOS LOS PANELES
	}

	public void colorSub (int indiceAudio, string[] array)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (i == 0 && indiceAudio == 0) 
			{	info.text = "<color=#ffff00>" + array [i] + "</color>";	} 
			else 
			{
				if (i == indiceAudio) 
				{	info.text = info.text + "\n" + "<color=#ffff00>" + array [i] + "</color>";	} 
				else 
				{
					if (i == 0) 
					{	info.text = array [i];	}
					else
					{	info.text = info.text + "\n" + array[i];	}
				}
			}
		}
	}
}