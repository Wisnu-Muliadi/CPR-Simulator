using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInterface
{
    /// <summary>
    /// ignore this class. mistakes happen. Now it houses testing Codes
    /// </summary>
    public class UIPatient : MonoBehaviour
    {
        public Transform target;

        // Update is called once per frame
        void Update()
        {
            transform.SetPositionAndRotation(target.position, target.rotation);
        }
    }
}