using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Set sprite so we know what the correct input is
public class GameNote : MonoBehaviour
{

    [SerializeField] SpriteRenderer spriteRenderer;

    Vector3 spawnPos;
    Vector3 removePos;
    Vector3 hitPos;
    int notesShownInAdvance;


    float beatOfThisNote; //we fill this data on instantiation from Song
    public float BeatOfThisNote(float beat) => beatOfThisNote = beat;

    char keyOfThisNote; //we fill this data on instantiation from Song, use input system later
    public char KeyOfThisNote 
    {
        get { return keyOfThisNote; }   // get method
        set { keyOfThisNote = value; }  // set method
    }

    bool isInput;
    public bool IsInput 
    { 
        get { return isInput; }   // get method
        set { isInput = value; }  // set method
    }

    private void Awake()
    {
        spawnPos = MusicDisplay.Instance.StartPos.transform.position;
        removePos = MusicDisplay.Instance.EndPos.transform.position;
        hitPos = MusicDisplay.Instance.MusicHitRadius.transform.position;
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
            ((notesShownInAdvance - (beatOfThisNote - SongManager.Instance.GetSongPosInBeats())) / notesShownInAdvance) *
                (hitPos.x - spawnPos.x)/(removePos.x - spawnPos.x) // scale so reach hit box on beat
        );

        // Destroy note at hit box if not input
        if (!isInput && transform.position.x <= hitPos.x) {
            InputSystem.Instance.DestroyNote(this.gameObject, true);
        }

        // Destroy note at end point
        if (transform.position == removePos)
        {
            InputSystem.Instance.DestroyNote(this.gameObject, false);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
