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
            //if (count == 4)
            //{
            //    arrayText = info.text.Split('\n');
            //    for (int i = 0; i < count; i++)
            //    {
            //        for (int j = 0; j < count; j++)
            //        {
            //            if (i == j)
            //            { info.text = "<color=#0000ffff>" + arrayText[j] + "</color>"; }
            //            else
            //            { info.text = info.text + arrayText[j]; }
            //        }
            //        info.text = "";
            //    }
            //}


            //if (count == 0)
            //{
            //    subAnt = aux;
            //    count++;
            //}
            //else if (count == 1)
            //{
            //    contenido.text = subAnt;
            //    subAnt = sub.text;
            //    count++;
            //}
            //else if (count > 1 && count < 5)
            //{
            //    contenido.text = subAnt + "\n" + contenido.text;
            //    subAnt = sub.text;
            //    count++;
            //}
            //else if (count == 5)
            //{
            //    arrayCont = contenido.text.Split('\n');
            //    contenido.text = subAnt + "\n" + arrayCont[0] + "\n" + arrayCont[1];
            //    subAnt = sub.text;
            //}
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
	}
}