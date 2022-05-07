using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDisplay : MonoBehaviour
{
    [SerializeField]
    private Transform startPos;
    public Transform StartPos => startPos;

    [SerializeField]
    private Transform endPos;
    public Transform EndPos => endPos;

}
