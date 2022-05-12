using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDisplay : MonoBehaviour
{
    public static MusicDisplay Instance { get; private set; }

    [SerializeField]
    private Transform startPos;
    public Transform StartPos => startPos;

    [SerializeField]
    private Transform endPos;
    public Transform EndPos => endPos;

    [SerializeField]
    private Collider2D musicHitRadius;
    public Collider2D MusicHitRadius => musicHitRadius;

    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
