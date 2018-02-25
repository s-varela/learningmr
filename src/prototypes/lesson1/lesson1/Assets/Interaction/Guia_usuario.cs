using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Interaction
{
    public class Guia_usuario : MonoBehaviour
    {

        [SerializeField]
        private VRUIAnimationClick btnSiguiente;
        [SerializeField]
        private MediaManager mediaManager;

        void Start()
        {

            if (btnSiguiente != null)
            {
                btnSiguiente.OnAnimationComplete += Siguiente;
            }
        }

        private void Siguiente()
        {
            mediaManager.pasarPanelGuia();
        }
    }
}
