using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject errorPanel;

    private string log;
	// Use this for initialization
	private Util util = Util.Instance;

	void Start()
	{
		LoadConfigMenu();
	}

	private void LoadConfigMenu()
	{
		try
        {
            util.ReLoadMenuTexts(errorPanel, true);

        }
        catch (Exception e)
		{
			log = "Exception: " + e.Message + "\n" + e.StackTrace;
            util.ShowErrorPanelByRef(errorPanel, log);
        }
	}


    private void Awake()
	{
        LoadConfigMenu();
    }

	// Update is called once per frame
	void Update()
	{

	}
}
