/***
 * Author: Yunhan Li 
 * Any issue please contact yunhn.lee@gmail.com
 ***/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace VRKeyboard.Utils
{
    public class GazeRaycaster : MonoBehaviour
    {

        #region MonoBehaviour Callbacks
        void Update()
        {
            // returns true after a Gear VR touchpad tap
            if (Input.GetMouseButtonDown(0))
            {

                RaycastHit hit;
                Vector3 fwd = transform.TransformDirection(Vector3.forward);

                if (Physics.Raycast(transform.position, fwd, out hit))
                {
                    // Trigger events only if we hit the keys or operation button
                    if (hit.transform.tag == "VRGazeInteractable")
                    {
                        hit.transform.GetComponent<Button>().onClick.Invoke();
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        #endregion
    }
}