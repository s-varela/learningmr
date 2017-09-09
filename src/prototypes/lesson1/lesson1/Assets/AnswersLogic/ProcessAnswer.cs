using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.AnswersLogic
{
    public class ProcessAnswer
    {
        public ProcessAnswer(){}
        
        public bool evaluateAnswer(String userAnswer, DialogType dialogType)
        {
            ArrayList answers = dialogType.Answers;

			if (userAnswer != null && userAnswer != "") 
			{
                
                foreach (string answer in answers)
                {
                    Debug.Log("["+ this.GetType().Name+"."+System.Reflection.MethodBase.GetCurrentMethod()+"][ answer: " + answer +"]");

                    if (userAnswer.ToLower().Contains(answer.ToLower()))
                    {
                        return true;
                    }
                }
				
			}
			return false;
        }
    }

    
}
