using System;
using System.Collections;
using System.Collections.Generic;

public class UserConfigType
{
    private string id;
    private string userFirstName;
    private string userLastName;
    private string userEmail;
    private string userLanguage;
    private Dictionary<string, UserQualificationType> userQualifications = new Dictionary<string, UserQualificationType>();

    public string Id { get; set; }
    public string UserFirstName { get;set;}
    public string UserLastName { get; set; }
    public string UserEmail { get; set; }
    public string UserLanguage { get; set; }
    public Dictionary<string, UserQualificationType> UserQualifications { get; set; }

}
    