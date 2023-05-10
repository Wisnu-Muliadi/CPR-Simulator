using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFps : MonoBehaviour
{
    public int fps_limit = 60;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = fps_limit;
    }
}
