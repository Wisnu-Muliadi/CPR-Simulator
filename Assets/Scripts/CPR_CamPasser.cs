using UnityEngine;

namespace CPR_Camera
{
    public class CPR_CamPasser : MonoBehaviour
    {
        public GameObject[] rightSideCam = new GameObject[4];
        public GameObject[] leftSideCam = new GameObject[4];

        public Transform rightBodyPos, leftBodyPos;

        public void ActivateCam(int index, bool activate)
        {
            if (rightSideCam[0].activeSelf)
            {
                rightSideCam[index].SetActive(activate);
            }
            else
            {
                leftSideCam[index].SetActive(activate);
            }
        }
        public bool ActiveCam(int index)
        {
            if (rightSideCam[0].activeSelf)
            {
                return rightSideCam[index].activeSelf;
            }
            else
            {
                return leftSideCam[index].activeSelf;
            }
        }
    }
}