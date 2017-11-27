using UnityEngine;

public class Util :MonoBehaviour
{
    private static Util instance;

    public static Util Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Util();
            }
            return instance;
        }
    }

    public void ShowErrorPanelByRef(GameObject errorPanel,string msg)
    {
        errorPanel.SetActive(true);
        string msgFinal="";
        TextMesh textObject = GameObject.Find("UI_ErrorDialogText").GetComponent<TextMesh>();
        string[] splitMsg = msg.Split(' ');
        int i = 0;
        foreach (string msgPart in splitMsg)
        {
            if (i != 5)
            {
          
                i++;
                if (msgPart.Length > 35)
                {
                    msgFinal = msgFinal.Substring(0, 35) + "\n" + msgFinal.Substring(35, msgFinal.Length - 35);
                }else
                {
                    msgFinal += msgPart + " ";
                }
            }
            else
            {
                msgFinal += msgPart + " \n";
                i = 0;
            }
         
        }
        textObject.text = msgFinal;
    }

   
}