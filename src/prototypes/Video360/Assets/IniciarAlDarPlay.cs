using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class IniciarAlDarPlay : MonoBehaviour
{
    public VideoPlayer vp;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!vp.isPlaying)
                vp.Play();
            else
                vp.Pause();
        }
    }
}
