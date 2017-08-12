using System;
 using System.Collections;
 
 	public class AudioManager
 	{
         private Hashtable hashAudios;
         private String pathAudio= "Lesson1/Sounds/";
 
 		public AudioManager ()
 		{
             hashAudios = new Hashtable();
 			hashAudios.Add("Are you Paul ?", "are_you_paul");    
 			hashAudios.Add("Hi! Are you Johnny?", "are_you_johnny");
 			hashAudios.Add("Hello, my name is Michael.", "hello_my_name_is_michael");
 			hashAudios.Add("I'm from Split, Croatia.", "im_from_split_croatia");
 			hashAudios.Add("I'm from Sydney, Australia.", "im_from_sydney_australia");
 			hashAudios.Add("My name is Johnny.", "my_name_is_johnny");
 			hashAudios.Add("Nice!Nice to meet you, bye!", "nice_nice_to_meet_you_bye");
 			hashAudios.Add("Nice to meet you too, Jhonny.", "nice_to_meet_you_too_johnny");
 			hashAudios.Add("Nice to meet you Michael.", "nice_to_meet_you_michael");
 			hashAudios.Add("Nice to meet you.", "nice_to_meet_you");
             hashAudios.Add("What's your name?", "whats_your_name");
             hashAudios.Add("Yes, I am.", "yes_i_am");
             hashAudios.Add("No, Im not. Im Michael.", "no_im_not_im_michael");
             hashAudios.Add("Where are you from?", "where_are_you_from");
         }
 
         public String getAudioPathName(string name)
         {
             if (hashAudios.ContainsKey(name))
             {
                 return pathAudio+hashAudios[name].ToString();
             }
             return null;
         }
 	}
 