using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    public NoteSpawner NoteSpawner;
    public Text ScoreText;
    public Text StreakText;

    public int Score;
    public int Streak;

    // Start is called before the first frame update
    void Start()
    {
        ScoreText.text = $"{Score}/{NoteSpawner.NotesCount}";
        StreakText.text = $"{Streak}/{NoteSpawner.NotesCount}";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncrementScore()
    {
        Score++;
        Streak++;
        ScoreText.text = $"{Score}/{NoteSpawner.NotesCount}";
        StreakText.text = $"{Streak}/{NoteSpawner.NotesCount}";
    }

    public void MissedNote()
    {
        Streak = 0;
        StreakText.text = $"{Streak}/{NoteSpawner.NotesCount}";
        //notes pop animation?
    }
}
