using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanel : MonoBehaviour {

    [SerializeField] TextMesh panel;
	[SerializeField] TextMesh subtitle;
    private string aux;
    private int count = 0;

    // Use this for initialization
    void Start () {
        aux = subtitle.text;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (!subtitle.text.Equals(aux))
        {
            aux = subtitle.text;
            if (count == 0)
            {
                panel.text = subtitle.text;
                count++;
            }
            else
            { panel.text = panel.text + "\n" + subtitle.text; }
        }
	}
}
