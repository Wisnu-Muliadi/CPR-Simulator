using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CaptionPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> _captionPool;
    private readonly List<TextMeshProUGUI> _captionText;

    void Start()
    {
        for (int i = 0; i < _captionPool.Count; i++)
        {
            _captionText.Add(_captionPool[i].GetComponent<TextMeshProUGUI>());
        }
    }
}
