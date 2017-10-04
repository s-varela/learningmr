using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.AnswersLogic
{
    public class ProcessAnswer
    {
        public ProcessAnswer(){}
        
        public bool evaluateAnswer(String userAnswer, DialogType dialogType)
        {
            ArrayList answers = dialogType.Answers;
            String userAnswerAux = userAnswer;

            if (userAnswer != null && userAnswer != "") 
			{
                
                foreach (string answer in answers)
                {
                    //Debug.Log("["+ this.GetType().Name+"."+System.Reflection.MethodBase.GetCurrentMethod()+"][ answer: " + answer +"]");
                    String answerAux = answer;

                    if (userAnswerAux.ToLower().Contains(answerAux.ToLower()))
                    {
                        return true;
                    }
                }
				
			}
			return false;
        }
    }

    
}
