using UnityEngine;
using System.Collections.Generic;
using System;

public class NotePulser : MonoBehaviour
{
    [Header("-------------- Required Objects")]
    private BlockPuzzleManager BlockPuzzleManager;
    public Animator[] NoteAnimators;

    [Header("-------------- Background Values (do not change)")]
    [SerializeField] private List<int> noteIndexes;
    public List<String> Notes; 
    public string note; //van blockpuzzlemanager

    
    public void Awake()
    {
        BlockPuzzleManager = GetComponent<BlockPuzzleManager>();
        Notes.Add("C");
        Notes.Add("C#");
        Notes.Add("Db");
        Notes.Add("D");
        Notes.Add("D#");
        Notes.Add("Eb");
        Notes.Add("E");
        Notes.Add("F");
        Notes.Add("F#");
        Notes.Add("Gb");
        Notes.Add("G");
        Notes.Add("G#");
        Notes.Add("Ab");
        Notes.Add("A");
        Notes.Add("A#");
        Notes.Add("Bb");
        Notes.Add("B");
    }

    //aangeroepen wanneer je op hold button klikt
    public void NoteShift()
    {
        //NoNotes();
        //NoteIndex = Notes.IndexOf(BlockPuzzleManager.currentBlockNote);
        note = BlockPuzzleManager.currentBlockNote;
        CheckNoteIndex();
    }

    private void CheckNoteIndex()
    {
        switch (note)
        {
            case "C":
                noteIndexes.Add(0);
                noteIndexes.Add(1);
                break;
            case "C#":
                noteIndexes.Add(2);
                noteIndexes.Add(3);
                break;
            case "Db":
                noteIndexes.Add(4);
                noteIndexes.Add(5);
                break;
            case "D":
                noteIndexes.Add(6);
                noteIndexes.Add(7);
                break;
            case "D#":
                noteIndexes.Add(8);
                noteIndexes.Add(9);
                break;
            case "Eb":
                noteIndexes.Add(10);
                noteIndexes.Add(11);
                break;
            case "E":
                noteIndexes.Add(12);
                noteIndexes.Add(13);
                break;
            case "F":
                noteIndexes.Add(14);
                noteIndexes.Add(15);
                break;
            case "F#":
                noteIndexes.Add(16);
                noteIndexes.Add(17);
                break;
            case "Gb":
                noteIndexes.Add(18);
                noteIndexes.Add(19);
                break;
            case "G":
                noteIndexes.Add(20);
                noteIndexes.Add(21);
                break;
            case "G#":
                noteIndexes.Add(22);
                noteIndexes.Add(23);
                break;
            case "Ab":
                noteIndexes.Add(24);
                break;
            case "A":
                noteIndexes.Add(25);
                break;
            case "A#":
                noteIndexes.Add(26);
                break;
            case "Bb":
                noteIndexes.Add(27);
                break;
            case "B":
                noteIndexes.Add(28);
                break;
        }
        foreach (int n in noteIndexes) 
        { NoteAnimators[n].SetTrigger("Pulsing"); }
    }

    //let go
    public void NoNotes()
    {
        foreach (int n in noteIndexes)
        { NoteAnimators[n].SetTrigger("NotPulsing"); }
        noteIndexes.Clear();
    }
}
