using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public PlayerController PC;
    public ScoreController SC;
    public GameObject NotePrefab;
    public GameObject NoteParent;
    public List<GameObject> NotesInAction;
    public Vector3 SpawnPos1;
    public Vector3 SpawnPos2;
    public Vector3 SpawnPos3;
    public Vector3 SpawnPos4;
    public Vector3 SpawnPos5;
    public float Speed;
    public float WaitForSpawn;
    public float MaxYDeath;

    public int NotesCount => Notes != null ? Notes.Count : 0;
    private List<Note> Notes;
    private List<GameObject> ExtraDeleteList;

    // Start is called before the first frame update
    void Awake()
    {
        Notes = new List<Note>();
        var path = Application.streamingAssetsPath + "/notes.csv";
        var lines = File.ReadLines(path);
        foreach(var csvLine in lines)
        {
            var splitLine = csvLine.Split(';');
            //foreach(var line in splitLine) line.Trim('"');
            Notes.Add(new Note
            {
                NoteNumber = int.Parse(splitLine[0]),
                WaitBefore = float.Parse(splitLine[1]),
                Time = float.Parse(splitLine[2])
            });
        }
        Debug.Log("Notes: " + Notes.Count);
        ExtraDeleteList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!PC.GameStarted)
        {
            return;
        }
        var deleteList = new List<GameObject>();
        foreach(var note in NotesInAction)
        {
            note.transform.position -= new Vector3(0, Speed * Time.deltaTime, 0);
            //Debug.Log($"Y:{note.transform.localPosition.y}");
            if(note.transform.localPosition.y < -400)
            {
                //Debug.Log("Deleting!");
                deleteList.Add(note);
            }
            if(note.transform.localPosition.y < MaxYDeath)
            {
                SC.MissedNote();
            }
        }
        foreach(var n in ExtraDeleteList)
        {
            deleteList.Add(n);
        }
        foreach(var n in deleteList)
        {
            NotesInAction.Remove(n);
            if(ExtraDeleteList.Contains(n)) ExtraDeleteList.Remove(n);
            Destroy(n);
        }
    }

    public void DeleteNote(GameObject go)
    {
        ExtraDeleteList.Add(go);
    }

    public void StartSpawn()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(WaitForSpawn);
        foreach(var note in Notes)
        {
            var waitBefore = note.WaitBefore;
            //Debug.Log("Waiting for: " + waitBefore);
            yield return new WaitForSeconds(waitBefore);

            var go = Instantiate(NotePrefab, NoteParent.transform);
            var pos = SpawnPos1;
            switch(note.NoteNumber)
            {
                case 1:
                    pos = SpawnPos1;
                    break;
                case 2:
                    pos = SpawnPos2;
                    break;
                case 3:
                    pos = SpawnPos3;
                    break;
                case 4:
                    pos = SpawnPos4;
                    break;
                case 5:
                    pos = SpawnPos5;
                    break;
            }
            go.transform.localPosition = pos;
            NotesInAction.Add(go);
        }
        Debug.Log("Song done!");
        PC.Done();
    }
}
