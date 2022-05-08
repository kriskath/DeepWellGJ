using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GameNote : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    Vector3 spawnPos;
    Vector3 removePos;
    int notesShownInAdvance;
    float beatOfThisNote;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPos = MusicDisplay.Instance.StartPos.transform.position;
        removePos = MusicDisplay.Instance.EndPos.transform.position;
    }


    // Start is called before the first frame update
    void Start()
    {
        notesShownInAdvance = SongManager.Instance.GetNotesShownInAdvance();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(
            spawnPos,
            removePos,
            (notesShownInAdvance - (beatOfThisNote - SongManager.Instance.GetSongPosInBeats())) / notesShownInAdvance
        );
    }
}
