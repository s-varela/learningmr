using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSubtitlePanel : MonoBehaviour {

    [SerializeField] TextMesh subtitle;
    [SerializeField] TextMesh sub;
	private string aux;

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
        }
    }
}
