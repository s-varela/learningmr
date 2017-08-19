using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSubtitlePanel : MonoBehaviour {

    [SerializeField] TextMesh subtitle;
    [SerializeField] TextMesh sub;
    //[SerializeField] GameObject panelExt;
    //[SerializeField] GameObject textInfo;
	//private MeshRenderer meshPanel;
	//private MeshRenderer meshTextInfo;
	private int count;
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
			count++;
			if (count == 4) {
				//ActiveMeshRenderer(meshPanel, panelExt, true);
				//ActiveMeshRenderer(meshTextInfo, textInfo, true);
			}
        }
    }

//    public void ActiveObject()
//    {
//        ActiveMeshRenderer(meshPanel, panelExt, true);
//        ActiveMeshRenderer(meshTextInfo, textInfo, true);
//    }
//
//    public void DesactiveObject()
//    {
//        ActiveMeshRenderer(meshPanel, panelExt, false);
//        ActiveMeshRenderer(meshTextInfo, textInfo, false);
//    }
//
//    private void ActiveMeshRenderer(MeshRenderer mesh, GameObject gameObj, bool v)
//    {
//        mesh = gameObj.GetComponent<MeshRenderer>();
//        mesh.enabled = v;
//    }
}
