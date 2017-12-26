using System;
using System.Collections;
using System.Collections.Generic;

public class UserQualificationType
{
    private const int EXPERIENCE_POINT = 10;
    private string lessonId;
    private string userId;
    private int successCount = 0;
    private int skipCount = 0;
    private int helpCount = 0;
    private int repeatCount = 0;
    private int lessonExperience = 0;

    public string UserId { get; set; }
    public string LessonId { get; set; }
    public int SuccessCount { get; set; }
    public int SkipCount { get; set; }
    public int HelpCount { get; set; }
    public int RepeatCount { get; set; }

    public int GetTotalPoint()
    {
        return SuccessCount * EXPERIENCE_POINT;
    }
}
    