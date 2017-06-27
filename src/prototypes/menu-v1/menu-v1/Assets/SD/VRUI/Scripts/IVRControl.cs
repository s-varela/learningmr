using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.SD.VRMenuRoom.Scripts
{
    interface IVRControl
    {
        string GetControlName();
        float GetControlValue();
        void SetControlValue(float value, bool normalized);
    }
}
