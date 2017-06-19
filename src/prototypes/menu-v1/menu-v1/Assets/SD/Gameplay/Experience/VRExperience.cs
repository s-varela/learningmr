using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class VRExperience : MonoBehaviour {

    private static VRExperience _instance;

    private Dictionary<string, object> configuration = new Dictionary<string, object>();

    public static VRExperience Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<VRExperience>();
                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (this != _instance)
        {
            Destroy(gameObject);
        }
    }

    public void StartExperience(Dictionary<string, object> config, bool overwriteSettings)
    {
        foreach (KeyValuePair<string, object> pair in config)
        {
            if (!configuration.ContainsKey(pair.Key) || overwriteSettings)
            { 
                configuration[pair.Key] = pair.Value;
            }
        }
        
        SceneManager.LoadScene("Experience", LoadSceneMode.Single);
    }

    public void StartExperience()
    {
        SceneManager.LoadScene("Experience", LoadSceneMode.Single);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public bool TryConfigurationValue(string name)
    {
        return configuration != null && configuration.ContainsKey(name);
    }

    public T GetConfigurationValue<T>(string name)
    {
        try
        {
            return (T) configuration[name];
        } 
        catch
        {
            return default(T);
        }
    }
}
