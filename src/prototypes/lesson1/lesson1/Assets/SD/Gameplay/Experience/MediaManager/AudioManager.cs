using System;
using System.Collections;

	public class AudioManager
	{
        private Hashtable hashAudios;
        private String pathAudio= "Lesson1/Sounds/";

		public AudioManager ()
		{
            hashAudios = new Hashtable();
			hashAudios.Add("1_2_3", "are_you_paul");    
			hashAudios.Add("1_2_1", "are_you_johnny");
			hashAudios.Add("1_1_1", "hello_my_name_is_michael");
			hashAudios.Add("1_4_5", "im_from_split_croatia");
			hashAudios.Add("1_4_7", "im_from_sydney_australia");
			hashAudios.Add("1_1_3", "my_name_is_johnny");
			hashAudios.Add("Nice!Nice to meet you, bye!", "nice_nice_to_meet_you_bye");
			hashAudios.Add("1_3_5", "nice_to_meet_you_too_johnny");
			hashAudios.Add("1_3_2", "nice_to_meet_you_michael");
			hashAudios.Add("Nice to meet you.", "nice_to_meet_you");
            hashAudios.Add("1_1_2", "whats_your_name");
            hashAudios.Add("1_1_4", "whats_your_name_question");
            hashAudios.Add("1_2_2", "yes_i_am");
            hashAudios.Add("1_2_4", "no_im_not_im_michael");
        //video 3 lesson 1
        hashAudios.Add("1_3_1", "hello_my_name_is_michael_v2");
        hashAudios.Add("1_3_3", "whats_your_name_v2");
        hashAudios.Add("1_3_4", "my_name_is_johnny_v2");
        //video 4 lesson 1
        hashAudios.Add("1_4_2", "whats_your_name_v4");
        hashAudios.Add("1_4_1", "hello_my_name_is_michael_v4");
        hashAudios.Add("1_4_3", "my_name_is_johnny_v4");
        hashAudios.Add("1_4_4", "where_are_you_from");
        hashAudios.Add("1_4_6", "where_are_you_from_v4");
        hashAudios.Add("1_4_8", "where_are_you_from_v5");
        /*
        FALTA UN AUDIO DE NICE TO MEET YOU VER EN QUE VIDEO
        Agregar subtitulos con espacios en texto para expresiones repetidas y sus audios son
        whats_your_name_question - check
        whats_your_name_v2 - check
        whats_your_name_v4 - check
        hello_my_name_is_michael_v2 - check
        hello_my_name_is_michael_v4 - check
        my_name_is_johnny_v2 - check
        my_name_is_johnny_v4 - check
        where_are_you_from_v4 - check
        where_are_you_from_v5 - check
        */
        //Lesson 02
        hashAudios.Add("2_1_1", "hello_michael");
            hashAudios.Add("2_1_2", "hello");
            hashAudios.Add("2_1_3", "this_is_my_friend_jack");
            hashAudios.Add("2_1_4", "jack_this_is_my_cousin_michael");
            hashAudios.Add("2_1_5", "nice_to_meet_you_jack");
            hashAudios.Add("2_1_6", "nice_to_meet_you_too_michael");

            //Lesson 03
            hashAudios.Add("3_1_1", "whats_your_name_l3");
            hashAudios.Add("3_1_2", "my_name_is_michael_smith"); 
            hashAudios.Add("3_1_3", "whats_your_address");
            hashAudios.Add("3_1_4", "my_address_is_fourty_two_wallabe_way_sydney_australia");
            hashAudios.Add("3_1_5", "how_old_are_you");
            hashAudios.Add("3_1_6", "im_thirty-six_years_old");
            hashAudios.Add("3_1_7", "whats_your_phone_number");
            hashAudios.Add("3_1_8", "my_phone_number_is_five_five_five_three_six_nine_three_seven_seven");
            hashAudios.Add("3_1_9", "whats_your_e_mail");
            hashAudios.Add("3_1_10", "my_e_mail_is_michael_smith_com");

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

