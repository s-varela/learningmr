using System;
using System.Collections;

	public class AudioManager
	{
        private Hashtable hashAudios;
        private String pathAudio= "Lesson1/Sounds/";

		public AudioManager ()
		{
            hashAudios = new Hashtable();
			hashAudios.Add("Are you Paul?", "are_you_paul");    
			hashAudios.Add("Hi! Are you Johnny?", "are_you_johnny");
			hashAudios.Add("Hello! My name is Michael.", "hello_my_name_is_michael");
			hashAudios.Add("I'm from Split, Croatia.", "im_from_split_croatia");
			hashAudios.Add("I'm from Sydney, Australia.", "im_from_sydney_australia");
			hashAudios.Add("My name is Johnny.", "my_name_is_johnny");
			hashAudios.Add("Nice!Nice to meet you, bye!", "nice_nice_to_meet_you_bye");
			hashAudios.Add("Nice to meet you too, Jhonny.", "nice_to_meet_you_too_johnny");
			hashAudios.Add("Nice to meet you Michael.", "nice_to_meet_you_michael");
			hashAudios.Add("Nice to meet you.", "nice_to_meet_you");
            hashAudios.Add("What's your name?", "whats_your_name");
            hashAudios.Add("Yes, I am.", "yes_i_am");
            hashAudios.Add("No, I'm not. I'm Michael.", "no_im_not_im_michael");
            hashAudios.Add("Where are you from?", "where_are_you_from");
            //Lesson 02
            hashAudios.Add("Hello Michael!", "hello_michael");
            hashAudios.Add("Hello!", "hello");
            hashAudios.Add("This is my friend, Jack.", "this_is_my_friend_jack");
            hashAudios.Add("Jack, this is my cousin, Michael.", "jack_this_is_my_cousin_michael");
            hashAudios.Add("Nice to meet you Jack!", "nice_to_meet_you_jack");
            hashAudios.Add("Nice to meet you too, Michael.", "nice_to_meet_you_too_michael");

            //Lesson 03
            hashAudios.Add("What's your name?", "whats_your_name");
            hashAudios.Add("My name is Michael Smith.", "my_name_is_michael_smith"); 
            hashAudios.Add("What's your address?", "whats_your_address");
            hashAudios.Add("My address is fourty-two, Wallabe Way. Sydney, Australia.", "my_address_is_fourty_two_wallabe_way_sydney_australia");
            hashAudios.Add("How old are you?", "how_old_are_you");
            hashAudios.Add("I'm thirty-six years old.", "im_thirty-six_years_old");
            hashAudios.Add("What's your phone number?", "whats_your_phone_number");
            hashAudios.Add("My phone number is 555369377.", "my_phone_number_is_five_five_five_three_six_nine_three_seven_seven");
            hashAudios.Add("What's your e-mail?", "whats_your_e_mail");
            hashAudios.Add("My e-mail is michael.smith.com.", "my_e_mail_is_michael_smith_com");

            //correction sounds 
            hashAudios.Add("correct", "correct");
            hashAudios.Add("incorrect", "incorrect");
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

