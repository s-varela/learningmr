using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.AnswersLogic
{
    public class ProcessAnswer
    {
        public ProcessAnswer(){}
        
        public bool evaluateAnswer(String userAnswer)
        {
			string answer1 = "my name is";
			string answer2_1 = "i'm from";
			string answer2_2 = "i am from";

			if (userAnswer != null && userAnswer != "") 
			{
				if (userAnswer.ToLower ().Contains (answer1) || userAnswer.ToLower ().Contains (answer2_1) || userAnswer.ToLower ().Contains (answer2_2)) 
				{
					return true;
				}
			}
			return false;
        }
    }

    
}
