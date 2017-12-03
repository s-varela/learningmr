using System;
using System.Linq;
using System.Text;
using UnityEngine;
using VRStandardAssets.Utils;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Interaction
{

    public class RadioButtonInteraction : MonoBehaviour
    {
        [SerializeField] private VRUIAnimationClick r1;
        [SerializeField] private VRUIAnimationClick r2;
        [SerializeField] private VRUIAnimationClick r3;
        [SerializeField] private VRUIAnimationClick r4;
        [SerializeField] private VRUIAnimationClick r5;

        private static RadioButtonInteraction _instance;

        [SerializeField] private MediaManager mediaManager;
        private int radioSelected;

        public static RadioButtonInteraction Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<RadioButtonInteraction>();
                    // DontDestroyOnLoad(_instance.gameObject);
                }
                return _instance;
            }
        }

        // Use this for initialization
        void Start()
        {
            radioSelected = 1;

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

        private void radioButton1() {
            GameObject rad = GameObject.Find("UI_RadioButton1");
            //mediaManager.selectRadioButton(rad,1);
            radioSelected = 1;
        }

        private void radioButton2()
        {
            GameObject rad = GameObject.Find("UI_RadioButton2");
            //mediaManager.selectRadioButton(rad,2);
            radioSelected = 2;
        }

        private void radioButton3()
        {
            GameObject rad = GameObject.Find("UI_RadioButton3");
            //mediaManager.selectRadioButton(rad,3);
            radioSelected = 3;
        } 

        private void radioButton4()
        {
            GameObject rad = GameObject.Find("UI_RadioButton4");
            //mediaManager.selectRadioButton(rad, 4);
            radioSelected = 4;
        }

        private void radioButton5()
        {
            GameObject rad = GameObject.Find("UI_RadioButton5");
            //mediaManager.selectRadioButton(rad,5);
            radioSelected = 5;
        }

        public int WhichRadioSelected() {
            return radioSelected;
        }
    }
}
