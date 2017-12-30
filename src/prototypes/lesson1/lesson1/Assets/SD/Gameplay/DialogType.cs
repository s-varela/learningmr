using System;
using System.Collections;
using System.Collections.Generic;

public class DialogType
{
    private long start;
    private string text;
    private bool requiredInput;
    private bool listen;
    private ArrayList answers;
    private bool finish;

    public long Start{get;set;}
    public string Text { get; set; }
    public bool RequiredInput { get; set; }
    public bool Listen { get; set; }
    public bool Finish { get; set; }
    public String audioId{ get; set; }

    public ArrayList Answers
    {
        get
        {
            if (answers == null)
            {
                this.answers = new ArrayList();
            }
                return this.answers;
        }
        set
        {
            this.answers = value;
        }
    }



}
    