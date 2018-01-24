using System;
using System.Collections;
using System.Collections.Generic;

public class AppConfigType
{
    private string id;
    private string resourcesPath;
    private string matedataPath;
    private string audioPath;
    private string videosPath;
    private ArrayList videos;

    public string Id { get; set; }
    public string ResourcesPath {get;set;}
    public string MetedataPath { get; set; }
    public string AudioPath { get; set; }
    public string VideosPath { get; set; }

    public ArrayList Videos
    {
        get
        {
            if (videos == null)
            {
                this.videos = new ArrayList();
            }
                return this.videos;
        }
        set
        {
            this.videos = value;
        }
    }

    public string ScorePath { get; internal set; }
}
    