using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{   
    public NoteSpawner spawner;
    public ScoreController scoreCon;

    //audio
    public AudioClip Scream;
    public AudioSource ScreamSource1;
    public AudioSource ScreamSource2;
    public AudioSource ScreamSource3;
    public AudioSource ScreamSource4;
    public AudioSource ScreamSource5;
    public AudioSource MusicSource;

    //start game
    public bool GameStarted;
    public bool GameOver;
    public GameObject PlayButton;
    public float WaitForMusic;
    public float WaitForMusicEnd;

    //hit notes
    public int HitRadius;
    public int HitOffsety;
    public GameObject HitItem1;
    public GameObject HitItem2;
    public GameObject HitItem3;
    public GameObject HitItem4;
    public GameObject HitItem5;
    public Image[] HitItemImages;
    public Color NormalColor;
    public Color ClickColor;
    public float WaitForColorChange;

    public Text TimerText;

    //character
    public Animator CharacterAnimator;
    //public Sprite[] CharacterScreamSprites;

    //Finished
    public GameObject FinishedContainer;
    public Image[] StarImages;
    public Sprite EmptyStarSprite;
    public Sprite FilledStarSprite;
    public Text FinishedScoreText;
    public Text FinishedStreakText;

    //smash effect
    public Animator SmashAnim;
    public GameObject SmashAnimContainer;

    //record
    private List<Note> RecordedNotes = new List<Note>();

    // Start is called before the first frame update
    void Start()
    {
        ScreamSource1.clip = Scream;
        ScreamSource2.clip = Scream;
        ScreamSource3.clip = Scream;
        ScreamSource4.clip = Scream;
        ScreamSource5.clip = Scream;

        FinishedContainer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStarted)
        {
            var timeNow = Time.timeSinceLevelLoad;
            TimerText.text = timeNow.ToString("#s");
            var index = RecordedNotes.Count - 1;
            var note = 0;
            var noteThatIsIn = CheckIfNoteIn();
            if(Input.GetKeyDown(KeyCode.D))
            {
                ScreamSource1.Play();
                note = 1;
                CharacterAnimator.Play("Scream1Anim");

                var image = HitItemImages[0];
                image.color = ClickColor;
                StartCoroutine(WaitColorClick(image));

                SmashAnimContainer.transform.localPosition = HitItem1.transform.localPosition;
            }
            else if(Input.GetKeyDown(KeyCode.F))
            {
                ScreamSource2.Play();
                note = 2;
                CharacterAnimator.Play("Scream2Anim");

                var image = HitItemImages[1];
                image.color = ClickColor;
                StartCoroutine(WaitColorClick(image));

                SmashAnimContainer.transform.localPosition = HitItem2.transform.localPosition;
            }
            else if(Input.GetKeyDown(KeyCode.G))
            {
                ScreamSource3.Play();
                note = 3;
                CharacterAnimator.Play("Scream3Anim");

                var image = HitItemImages[2];
                image.color = ClickColor;
                StartCoroutine(WaitColorClick(image));

                SmashAnimContainer.transform.localPosition = HitItem3.transform.localPosition;
            }
            else if(Input.GetKeyDown(KeyCode.H))
            {
                ScreamSource4.Play();
                note = 4;
                CharacterAnimator.Play("Scream4Anim");

                var image = HitItemImages[3];
                image.color = ClickColor;
                StartCoroutine(WaitColorClick(image));

                SmashAnimContainer.transform.localPosition = HitItem4.transform.localPosition;
            }
            else if(Input.GetKeyDown(KeyCode.J))
            {
                ScreamSource5.Play();
                note = 5;
                CharacterAnimator.Play("Scream5Anim");

                var image = HitItemImages[4];
                image.color = ClickColor;
                StartCoroutine(WaitColorClick(image));

                SmashAnimContainer.transform.localPosition = HitItem5.transform.localPosition;
            }

            if(note > 0)
            {
                var timeDiff = 0.0f;
                if(index >= 0)
                {
                    timeDiff = timeNow - RecordedNotes[index].Time;
                }
                var myNote = new Note()
                {
                    NoteNumber = note,
                    Time = timeNow,
                    WaitBefore = timeDiff
                };
                RecordedNotes.Add(myNote);
                if(note == noteThatIsIn.noteNumber)
                {
                    //Debug.Log("You did it!");
                    scoreCon.IncrementScore();
                    spawner.DeleteNote(noteThatIsIn.go);
                    SmashAnim.Play("SmashAnim");
                }
            }
        }
        else if(!GameOver && Input.anyKeyDown)
        {
            StartGame();
        }

        //restart game / quit game
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit(0);
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }
    }

    public (int noteNumber, GameObject go) CheckIfNoteIn()
    {
        var notesInAction = spawner.NotesInAction;
        foreach(var note in notesInAction)
        {
            var notePos = note.gameObject.transform.localPosition;

            var hitItem1Pos = HitItem1.transform.localPosition;
            var hitItem2Pos = HitItem2.transform.localPosition;
            var hitItem3Pos = HitItem3.transform.localPosition;
            var hitItem4Pos = HitItem4.transform.localPosition;
            var hitItem5Pos = HitItem5.transform.localPosition;
            var diffx1 = notePos.x - hitItem1Pos.x;
            var diffy1 = notePos.y - hitItem1Pos.y - HitOffsety;
            var diffx2 = notePos.x - hitItem2Pos.x;
            var diffy2 = notePos.y - hitItem2Pos.y - HitOffsety;
            var diffx3 = notePos.x - hitItem3Pos.x;
            var diffy3 = notePos.y - hitItem3Pos.y - HitOffsety;
            var diffx4 = notePos.x - hitItem4Pos.x;
            var diffy4 = notePos.y - hitItem4Pos.y - HitOffsety;
            var diffx5 = notePos.x - hitItem5Pos.x;
            var diffy5 = notePos.y - hitItem5Pos.y - HitOffsety;
            if(Math.Abs(diffx1) <= HitRadius && Math.Abs(diffy1) <= HitRadius)
            {
                return (1, note.gameObject);
            }
            else if(Math.Abs(diffx2) <= HitRadius && Math.Abs(diffy2) <= HitRadius)
            {
                return (2, note.gameObject);
            }
            else if(Math.Abs(diffx3) <= HitRadius && Math.Abs(diffy3) <= HitRadius)
            {
                return (3, note.gameObject);
            }
            else if(Math.Abs(diffx4) <= HitRadius && Math.Abs(diffy4) <= HitRadius)
            {
                return (4, note.gameObject);
            }
            else if(Math.Abs(diffx5) <= HitRadius && Math.Abs(diffy5) <= HitRadius)
            {
                return (5, note.gameObject);
            }
        }
        return (0, null);
    }

    public void StartGame()
    {
        GameStarted = true;
        PlayButton.SetActive(false);
        spawner.StartSpawn();
        CharacterAnimator.Play("JumpAnim");
        ScreamSource1.Play();
        StartCoroutine(WaitPlayMusic());
    }

    public void Save()
    {
        Debug.Log("SAVING RECORDING");
        var path = Application.persistentDataPath + "/notes.csv";
        var csv = "";
        foreach(var note in RecordedNotes)
        {
            csv += $"{note.NoteNumber};{note.WaitBefore};{note.Time}\r\n";
        }
        using StreamWriter streamWriter = File.CreateText(path);
        streamWriter.Write(csv);
    }

    IEnumerator WaitPlayMusic()
    {
        yield return new WaitForSeconds(WaitForMusic);
        MusicSource.Play();
        CharacterAnimator.Play("ScreamAnim");
    }

    public void Done()
    {
        StartCoroutine(WaitDone());
    }

    IEnumerator WaitDone()
    {
        yield return new WaitForSeconds(WaitForMusicEnd);
        GameStarted = false;
        GameOver = true;
        //done animation etc
        CharacterAnimator.Play("JumpAnim");
        
        HitItem1.SetActive(false);
        HitItem2.SetActive(false);
        HitItem3.SetActive(false);
        HitItem4.SetActive(false);
        HitItem5.SetActive(false);

        FinishedContainer.SetActive(true);
        var score = scoreCon.Score;
        var streak = scoreCon.Streak;
        var maxScore = spawner.NotesCount;
        FinishedScoreText.text = $"{score}/{maxScore}";
        FinishedStreakText.text = $"{streak}/{maxScore}";
        
        if(score == maxScore && streak == maxScore)
        {
            foreach(var img in StarImages)
            {
                img.sprite = FilledStarSprite;
            }
        }
        else if(score > maxScore*4/5)
        {
            StarImages[0].sprite = FilledStarSprite;
            StarImages[1].sprite = FilledStarSprite;
            StarImages[2].sprite = FilledStarSprite;
            StarImages[3].sprite = FilledStarSprite;
            StarImages[4].sprite = EmptyStarSprite;
        }
        else if(score > maxScore*3/5)
        {
            StarImages[0].sprite = FilledStarSprite;
            StarImages[1].sprite = FilledStarSprite;
            StarImages[2].sprite = FilledStarSprite;
            StarImages[3].sprite = EmptyStarSprite;
            StarImages[4].sprite = EmptyStarSprite;
        }
        else if(score > maxScore*2/5)
        {
            StarImages[0].sprite = FilledStarSprite;
            StarImages[1].sprite = FilledStarSprite;
            StarImages[2].sprite = EmptyStarSprite;
            StarImages[3].sprite = EmptyStarSprite;
            StarImages[4].sprite = EmptyStarSprite;
        }
        else if(score > maxScore*1/5)
        {
            StarImages[0].sprite = FilledStarSprite;
            StarImages[1].sprite = EmptyStarSprite;
            StarImages[2].sprite = EmptyStarSprite;
            StarImages[3].sprite = EmptyStarSprite;
            StarImages[4].sprite = EmptyStarSprite;
        }
        else
        {
            StarImages[0].sprite = EmptyStarSprite;
            StarImages[1].sprite = EmptyStarSprite;
            StarImages[2].sprite = EmptyStarSprite;
            StarImages[3].sprite = EmptyStarSprite;
            StarImages[4].sprite = EmptyStarSprite;
        }
    }

    IEnumerator WaitColorClick(Image image)
    {
        yield return new WaitForSeconds(WaitForColorChange);
        image.color = NormalColor;
    }
}
