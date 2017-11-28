using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class CheckConnectivity : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static bool checkInternetStatus()
	{
		try
		{
			using (var client = new WebClient())
			{
				using (client.OpenRead("http://clients3.google.com/generate_204"))
				{
					return true;
				}
			}
		}
		catch
		{
			return false;
		}
	}
}
