using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovTelepronter : MonoBehaviour {

    [SerializeField] TextMesh contTelepronter;
	[SerializeField] TextMesh subtitle;
    private float mov = 0.0093f;
    private float movY;
    private string aux;
    private int count = 0;
    private Vector3 pos;

    // Use this for initialization
    void Start () {
        aux = subtitle.text;
        pos = contTelepronter.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (!subtitle.text.Equals("") && (count < 4))
        {
            movY = contTelepronter.transform.position.y;
            movY = movY + mov;
            //pos.y = pos.y + mov;
            //contTelepronter.transform.position = new Vector3(contTelepronter.transform.position.x, contTelepronter.transform.position.y + mov, contTelepronter.transform.position.z);
            //contTelepronter.transform.position = pos;
            contTelepronter.transform.position = new Vector3(contTelepronter.transform.position.x, movY, contTelepronter.transform.position.z);
            if (!subtitle.text.Equals(aux))
            {
                aux = subtitle.text;
                count++;
            }
        }
    }
}
