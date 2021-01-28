using states.controllers;
using UnityEngine;

namespace models.sparepart
{
    [RequireComponent(typeof(InputManagerController))]
    public class ObjectStateManager : MonoBehaviour, ISparePart
    {
        public InputManagerController imc;

        private void Awake()
        {
            imc = GetComponent<InputManagerController>();
        }
        
    }
}