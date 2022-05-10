using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameNote : MonoBehaviour
{

    Vector3 spawnPos;
    Vector3 removePos;
    int notesShownInAdvance;


    float beatOfThisNote; //we fill this data on instantiation from Song
    public float BeatOfThisNote(float beat) => beatOfThisNote = beat;


    private void Awake()
    {
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

        if (transform.position == removePos)
        {
            //call code to build stress
            //Invoke.?
            Destroy(gameObject);
        }
    }
}
