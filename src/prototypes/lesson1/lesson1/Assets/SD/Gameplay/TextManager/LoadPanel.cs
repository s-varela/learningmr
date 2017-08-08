using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour {

    [SerializeField] TextMesh contenido;
	[SerializeField] TextMesh subtitle;
    [SerializeField] TextMesh sub;
    private string aux;
    private string subAnt;
    private string[] arrayCont;
    private int count = 0;

    // Use this for initialization
    void Start () {
        aux = subtitle.text;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (!subtitle.text.Equals(aux) && !subtitle.text.Equals(""))
        {
            aux = subtitle.text;
            sub.text = subtitle.text;
            if (count == 0)
            {
                subAnt = aux;
                count++;
            }
            else if (count == 1)
            {
                contenido.text = subAnt;
                subAnt = sub.text;
                count++;
            }
            else if (count > 1 && count < 5)
            {
                contenido.text = subAnt + "\n" + contenido.text;
                subAnt = sub.text;
                count++;
            }
            else if (count == 5)
            {
                arrayCont = contenido.text.Split('\n');
                contenido.text = subAnt + "\n" + arrayCont[0] + "\n" + arrayCont[1];
                subAnt = sub.text;
            }
        }
	}
}