using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerControl;

namespace CPR_Camera
{
    public class CPR_CamController : MonoBehaviour
    {
        public Transform rootPos;

        private Quaternion alignmentRotation;
        private readonly WaitForSeconds waitASec= new(1);

        private IEnumerator Start()
        {
            rootPos = transform.parent.transform;
            gameObject.transform.SetParent(null, true);
            while (Vector3.Distance(transform.position, rootPos.position) > .02 || Vector3.Dot(rootPos.forward, transform.up) < .99f)
            {
                yield return null;
            }
            yield return waitASec;
            enabled = false;
        }
        public IEnumerator IRealignCprRoot()
        {
            enabled = true;
            yield return waitASec;
            enabled = false;
        }
        // Update is called once per frame
        void FixedUpdate()
        {
            // be gone if there's no root pos
            if (rootPos == null) { enabled = false; return; }

            if (transform.position != rootPos.position)
            {
                transform.position = Vector3.MoveTowards(transform.position, rootPos.position, .01f);
                alignmentRotation = Quaternion.LookRotation(-rootPos.transform.up, transform.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, alignmentRotation, .05f);
            }
        }

        public void CheckRightLeftCam(PlayerCPRTargeting playerCprTarget)
        {
            Vector3 vectorTowardsPlayer = playerCprTarget.transform.position - transform.position;
            if (TryGetComponent(out CPR_CamPasser camPasser))
            {
                if (Vector3.Dot(transform.right, vectorTowardsPlayer) > 0)
                {
                    camPasser.rightSideCam[0].SetActive(true);
                    playerCprTarget.playerSit = camPasser.rightBodyPos;
                }
                else
                {
                    camPasser.leftSideCam[0].SetActive(true);
                    playerCprTarget.playerSit = camPasser.leftBodyPos;
                }
            }
        }
        public int PlayerOnLeftSide(PlayerCPRTargeting playerCprTarget)
        {
            Vector3 vectorTowardsPlayer = playerCprTarget.transform.position - transform.position;
            if (Vector3.Dot(transform.right, vectorTowardsPlayer) < 0) return 1;
            else return 0;
        }
    }
}