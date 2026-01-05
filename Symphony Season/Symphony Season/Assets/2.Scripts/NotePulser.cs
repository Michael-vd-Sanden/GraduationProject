using UnityEngine;
using System.Collections.Generic;
using System;

public class NotePulser : MonoBehaviour
{
    public BlockPuzzleManager BlockPuzzleManager;
    public List<String> Notes;
    //Hier sla ik de notes op die in de Block Puzzle Manager onder current selected note staan;
    public Animator[] NoteAnimators;
    public int NoteIndex = 0;

    
    public void Awake()
    {
        Notes.Add("A");
        Notes.Add("B");
        Notes.Add("C");
        Notes.Add("D");
        Notes.Add("E");
        Notes.Add("F");
        Notes.Add("G");
    }

    //aangeroepen wanneer je op hold button klikt
    public void NoteShift()
    {
        var PrevNoteIndex = NoteIndex;
        NoteIndex = Notes.IndexOf(BlockPuzzleManager.currentBlockNote);

        NoteAnimators[NoteIndex].SetTrigger("Pulsing");
        NoteAnimators[PrevNoteIndex].SetTrigger("NotPulsing");
    }
    public void NoNotes()
    {
        for (int i=0; i < Notes.Count; i++)
        {
            NoteAnimators[i].SetTrigger("NotPulsing");
        }
    }
}
