using UnityEngine;
using System.Collections;

public class VRUI : MonoBehaviour {

    [SerializeField] private bool applyToSelf = true;
    [SerializeField] private bool currentIsFrame = false;
    [SerializeField] private bool affectChildren = false;

	// Use this for initialization
	void Start () {
        if (applyToSelf) ConfigureForVR(gameObject, currentIsFrame);
        if (affectChildren) ConfigureChildrenForVR(gameObject);
	}

    private void ConfigureChildrenForVR(GameObject element)
    {
        for(int i = 0; i < element.transform.childCount; i ++)
        {
            GameObject child = element.transform.GetChild(i).gameObject;

            ConfigureForVR(child, false);
            ConfigureChildrenForVR(child);
        }
    }

    private void ConfigureForVR(GameObject element, bool isFrame)
    {
        MeshRenderer mr = element.GetComponent<MeshRenderer>();
        if (mr != null)
        {
            mr.material.renderQueue = isFrame? 4300 : 4500;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
