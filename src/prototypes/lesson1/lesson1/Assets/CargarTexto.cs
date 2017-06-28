using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using LitJson;
using System.IO;

public class CargarTexto : MonoBehaviour
{

    int valor = 1;
    public string[] s;
    public TextMesh text;
    JsonData myJson;

    // Use this for initialization
    void Start()
    {
        myJson = JsonMapper.ToObject(File.ReadAllText("Assets/Json.txt"));
        text.text = myJson[valor.ToString()].ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            valor++;
            text.text = myJson[valor.ToString()].ToString();
        }
    }
}
