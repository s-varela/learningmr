using UnityEngine;

public class Util
{
    public static void ShowErrorPanel(string msg)
    {
        GameObject panelErrorObj = GameObject.Find("ErrorMenu");
        TextMesh textObject = GameObject.Find("UI_ErrorDialogText").GetComponent<TextMesh>();
        textObject.text = msg;
    }
}