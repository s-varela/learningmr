using System;
using System.Linq;
using System.Text;
using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Interaction
{
    public class radioButtonInteraction : MonoBehaviour
    {
        [SerializeField]
        private VRUIAnimationClick r1;
        [SerializeField]
        private VRUIAnimationClick r2;
        [SerializeField]
        private VRUIAnimationClick r3;
        [SerializeField]
        private VRUIAnimationClick r4;
        [SerializeField]
        private VRUIAnimationClick r5;

        [SerializeField]
        private MediaManager mediaManager;


        // Use this for initialization
        void Start()
        {

            if (r1 != null)
            {
                r1.OnAnimationComplete += radioButton1;
            }
            if (r2 != null)
            {
                r2.OnAnimationComplete += radioButton2;
            }

            if (r3 != null)
            {
                r3.OnAnimationComplete += radioButton3;
            }

            if (r4 != null)
            {
                r4.OnAnimationComplete += radioButton4;
            }

            if (r5 != null)
            {
                r5.OnAnimationComplete += radioButton5;
            }

        }

        private void radioButton1() {
            GameObject rad = GameObject.Find("UI_RadioButton1");
            mediaManager.selectRadioButton(rad,1);
        }

        private void radioButton2()
        {
            GameObject rad = GameObject.Find("UI_RadioButton2");
            mediaManager.selectRadioButton(rad,2);
        }

        private void radioButton3()
        {
            GameObject rad = GameObject.Find("UI_RadioButton3");
            mediaManager.selectRadioButton(rad,3);
        }

        private void radioButton4()
        {
            GameObject rad = GameObject.Find("UI_RadioButton4");
            mediaManager.selectRadioButton(rad,4);
        }

        private void radioButton5()
        {
            GameObject rad = GameObject.Find("UI_RadioButton5");
            mediaManager.selectRadioButton(rad,5);
        }

    }
}
