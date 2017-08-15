using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSubtitlePanel : MonoBehaviour {

    [SerializeField] TextMesh subtitle;
    [SerializeField] TextMesh sub;
    [SerializeField] GameObject panelExt;
    [SerializeField] GameObject textInfo;
    private MeshRenderer meshPanel;
    private MeshRenderer meshTextInfo;
    private string aux;
    private int count; 

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
            count++;
            if (count == 4)
            {
                ActiveMeshRenderer(meshPanel, panelExt, true);
                ActiveMeshRenderer(meshTextInfo, textInfo, true);
            }
        }
    }

    private void ActiveMeshRenderer(MeshRenderer mesh, GameObject gameObj, bool v)
    {
        mesh = gameObj.GetComponent<MeshRenderer>();
        mesh.enabled = v;
    }
}
