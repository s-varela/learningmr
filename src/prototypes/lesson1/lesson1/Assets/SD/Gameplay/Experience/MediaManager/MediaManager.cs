using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.UI;
using System.Diagnostics;
using Assets.AnswersLogic;
using System.IO;
using System.Text.RegularExpressions;
using System;
using Assets.Interaction;

public class MediaManager : MonoBehaviour
{

    private VRExperience experience = null;

    [SerializeField] private MediaPlayerCtrl media;
    [SerializeField] private AudioSource audioLeft;
    [SerializeField] private AudioSource audioRight;
    [SerializeField] private MediaManagerData data;
    [SerializeField] private VRGameMenu menu;

    [SerializeField] private LoadPanel loadPanel;
    [SerializeField] GameObject panelInfo;
    [SerializeField] GameObject panelSub;
    [SerializeField] GameObject panelInput;
    [SerializeField] GameObject panelQuestion;
    [SerializeField] GameObject panelAnswer;
    //[SerializeField] GameObject panelHintButton;
    [SerializeField] GameObject panelHintText;
    //[SerializeField] GameObject hintButton;
   // [SerializeField] GameObject skipButton;
    [SerializeField] GameObject sphere;
    [SerializeField] GameObject keyboard;
    [SerializeField] Text keyboardInp;
    [SerializeField] TextMesh Sub;
    [SerializeField] TextMesh userAnswer;
    [SerializeField] TextMesh givenHint;
    [SerializeField] GameObject gifTick;
    [SerializeField] GameObject gifCross;
    [SerializeField] private TextMesh normalText;
    [SerializeField] NavigationPanel navigationPanel;
    [SerializeField] GameObject mediaDialogMenuPanel;
    VRDialogMenu mediaDialogMenu;

	[SerializeField] TextMesh textRespuesta;
	[SerializeField] GameObject PanelTextLAR;
	[SerializeField] GameObject GuiaPanelInfo;
	[SerializeField] GameObject GuiaPanelInteraccion;
	[SerializeField] GameObject BtnGuia;
	[SerializeField] GameObject PanelResume;
	[SerializeField] GameObject PanelNavigation;
	[SerializeField] GameObject PanelFinalLesson;

    [SerializeField] GameObject panelSkipHint;

  [SerializeField] Material play;

    private const string TECLADO_RESPUESTA_VACIA = "Por favor, ingrese una respuesta.";
    private SubtitleReader subReader;
    private AudioManager audioManager;
    private ProcessAnswer processAnswer;
    private string[] arrayText;
    private Color originalColor;
    private AudioSource sfx;
    private Stopwatch counterVideo;
    private Stopwatch counterAudio;
    private Stopwatch counterDelay;
    private bool changeSub;
    private bool showUserInput;
    private bool listen;
    private bool finish;
    private bool isInPanelInfoMode = false;
    private bool isButtonGuiaOn = false;
    private bool pasarGuia = true;

    private bool answerOK = false;
    private bool menuPause = false;
    private bool emptyKeyboardAnswer = false;
    private int indiceAudio;
    private DialogType dialogType;
    private int i = -1;
    private bool skip = false;
    private ArrayList textForRepeat;
    private ArrayList audioCodesForRepeat;
    private string selectedString;
    private int currentPage;
    private const int windows = 5;
    private bool wait;
    private bool firstTimePanelInfo = true;
    private bool firstTimePanelInteraccion = true;
    private UserQualificationType userQualification;

    private bool isLessonFinish;

    void Start()
    {
        selectedString = "";
        currentPage = 0;
        audioLeft = new AudioSource();
        experience = VRExperience.Instance;
        dialogType = new DialogType();
        changeSub = false;
        experience.ResetIndice();
        originalColor = Sub.color;
        textForRepeat = new ArrayList();
        userQualification = new UserQualificationType();
        audioCodesForRepeat = new ArrayList();
        //Inicializar variables
        InitializeVariables();

        if (audioLeft != null)
        {
            audioLeft.volume = experience.GetConfigurationValue<float>(data.videoVolumeConfigValue);
            audioLeft.Play();
        }

        if (audioRight != null)
        {
            audioRight.volume = experience.GetConfigurationValue<float>(data.videoVolumeConfigValue);
            audioRight.Play();
        }

        if (audioRight != null)
        {
            audioRight.volume = experience.GetConfigurationValue<float>(data.videoVolumeConfigValue);
            audioRight.Play();
        }

        mediaDialogMenu = mediaDialogMenuPanel.GetComponent<VRDialogMenu>();
        mediaDialogMenu.OnAcceptClick += DialogAcceptHandle;

        ManagerVideo();
    }



    private void DialogAcceptHandle()
    {

        userQualification.SkipCount += 1;
        UnityEngine.Debug.Log("[MediaManager][DialogAcceptHandle] SkipCount:" + userQualification.SkipCount);

        skip = true;
        menuPause = false;
        wait = false;
        showUserInput = false;
        listen = false;
        mediaDialogMenuPanel.SetActive(false);
    }

    void InitializeVariables()
	{
		changeSub = false;
		answerOK = false;
		showUserInput = false;
		listen = false;
		panelInfo.SetActive(false);
		panelSub.SetActive(true);
		panelInput.SetActive(false);
		sphere.SetActive(false);
		keyboard.SetActive(false);
		panelAnswer.SetActive(false);
		panelQuestion.SetActive(false);
        //panelHintButton.SetActive(false);
        panelHintText.SetActive(false);
        panelSkipHint.SetActive(false);

        PanelTextLAR.SetActive (false);
		BtnGuia.SetActive (false);
		PanelResume.SetActive (false);
		PanelNavigation.SetActive (true);
		PanelFinalLesson.SetActive (false);
		Sub.text="";
		normalText.text = "";
		userAnswer.text = "";
		givenHint.text = "";
		indiceAudio = 0;
        textForRepeat = new ArrayList();
        audioCodesForRepeat = new ArrayList();
        wait = false;
        currentPage = 0;
        isLessonFinish = false;
   }

	public void InitializeVariables2()
	{
		changeSub = false;
		answerOK = false;
		showUserInput = false;
		listen = false;
		panelInfo.SetActive(false);
		panelSub.SetActive(true);
		panelInput.SetActive(false);
		sphere.SetActive(false);
		keyboard.SetActive(false);
		panelAnswer.SetActive(false);
		panelQuestion.SetActive(false);
        //panelHintButton.SetActive(false);
        panelHintText.SetActive(false);

        panelSkipHint.SetActive(false);

        PanelTextLAR.SetActive (false);
		BtnGuia.SetActive (false);
		PanelResume.SetActive (false);
		PanelNavigation.SetActive (true);
		PanelFinalLesson.SetActive (false);
		Sub.text="";
		normalText.text = "";
		userAnswer.text = "";

		givenHint.text = "";
		indiceAudio = 0;
//		textForRepeat = new ArrayList();
		wait = false;
		currentPage = 0;
	}

    void Awake()
    {
        if (media == null)
        {
            textForRepeat = new ArrayList();
            audioCodesForRepeat = new ArrayList();
            experience = VRExperience.Instance;
            counterVideo = new Stopwatch();
            counterVideo.Start();
            counterAudio = new Stopwatch();
            counterDelay = new Stopwatch();
            subReader = new SubtitleReader();
            audioManager = new AudioManager();
            processAnswer = new ProcessAnswer();
            dialogType = new DialogType();
            media = FindObjectOfType<MediaPlayerCtrl>();
            if (media == null)
                throw new UnityException("No Media Player Ctrl object in scene");
            listen = false;
            showUserInput = false;
            finish = false;
            indiceAudio = 0;
            dialogType = new DialogType();
            textForRepeat = new ArrayList();
            userQualification = new UserQualificationType();
       
        }
    }


    // Update is called once per frame
    void Update()
    {
        try
        {   //not paused nor waiting

            if (!menuPause && !wait)
            {   //enters here if user must answer direct questions
                if (IsDialogMode())
                {
                    long seconds = counterVideo.ElapsedMilliseconds;
                    // search if duration is in last subtitle second (in miliseconds)
                    dialogType = subReader.ReadSubtitleLine(seconds);
                    // it's not the last subtitle
                    if (dialogType != null)
                    {
                        //if enters here the video is still running
                        if (!dialogType.Text.Equals("") && dialogType.Text != normalText.text)
                        {
                            if (panelInput.activeSelf)
                            {
                                DisableInterationMenu();
                            }
                            normalText.text = dialogType.Text;
                            //saves text to repeat in excercise in arraylist
                            textForRepeat.Add(dialogType);
                            answerOK = false;
                            Sub.color = originalColor;
                            gifTick.SetActive(false);
                        }

                        if (dialogType.Listen)
                        {
                            ConfigListenMode();

                        }
                        else if (dialogType.RequiredInput && !answerOK)
                        {
                            ConfigInputMode();

                        }
                        else if (dialogType.Finish)
                        {
                            ConfigDialogFinishMode();
                        }
                    }
                }
                else if (IsListenMode())// Modo Listen, muestro panel frontal y reproduzco audios
                {
                    isInPanelInfoMode = true;

                    if (ElapsedAudioTime(5000))
                    {
                        try
                        {
                            // if (experience.getIndice() == 1 && experience.getLessonID() == 1)
                            // {
                            if (pasarGuia)
                            {
                                //no hago nada, espero al click del usuario
                                // para mostrar el ejercicio de repeticion
                            }
                            else {
                                RepeatDialog(); //Se reproducen los dialogos
                            }                                                
                          //  }
                            
                        }
                        catch (System.Exception ex)
                        {
                            //TODO logger
                            UnityEngine.Debug.Log("[MediaManager][update] " + ex.Message);
                        }
                    }
                }
                else if (IsInputMode())
                {
                    isInPanelInfoMode = false;
                    PauseMedia();
                    EnableInterationMenu();
                }
                else if (IsFinishMode())
                {
                    UnityEngine.Debug.Log("[MediaManager][FinishLesson]. Inicio");
                    isInPanelInfoMode = false;
                    PanelResume.SetActive(true);
                    PanelFinalLesson.SetActive(true);
                    PanelNavigation.SetActive(false);
                    panelSub.SetActive(false);
                    LoadPanelResumeTexts();
                    LoadPanelResumeScore();
                    
                    //Guardo los resultados en la VrExperience y lo guardo
                    experience.UserQualification = userQualification;
                    isLessonFinish = true;
                    UnityEngine.Debug.Log("[MediaManager][FinishLesson]. Fin");

                }
                else if (IsSkipMode())
                {
                    isInPanelInfoMode = false;
                    DisableInterationMenu();
                    ResumeMedia();
                    answerOK = true;
                    skip = false;
                    givenHint.text = "";
                }
            }
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log("[MediaManager][update] Excepcion" + ex.Message);
        }

    }

    //	private void SetMaterialPlay()
    //	{
    //		for (int i = 1; i <= 5; i++) {
    //			GameObject ri = GameObject.Find ("UI_Replay"+i);
    //			ri.GetComponent<Renderer> ().material = play;
    //		}
    //	}

    private bool IsSkipMode()
    {
        return skip && ElapsedTime(1000);
    }

    private bool IsFinishMode()
    {
        return dialogType.Finish && ElapsedTime(2000) && !isLessonFinish;
    }

    private bool IsInputMode()
    {
        return showUserInput && ElapsedTime(300) && !keyboard.activeSelf;
    }

    private bool IsListenMode()
    {
        return listen && ElapsedTime(2000);
    }

    private bool IsDialogMode()
    {
        return !listen && !showUserInput && !finish && !skip;
    }

    private void ConfigDialogMode()
    {
        showUserInput = false;
        listen = false;
        menuPause = false;
        wait = false;
        finish = false;
        skip = false;
        StartDelayTime();
    }


    private void ConfigDialogFinishMode()
    {
        finish = true;
        listen = false;
        counterDelay.Reset();
        counterDelay.Start();
    }

    private void ConfigInputMode()
    {
        i = -1;
        showUserInput = true;
        listen = false;
        menuPause = false;
        wait = false;
        skip = false;
        counterDelay.Reset();
        counterDelay.Start();
    }

    private void ActivePanelTextLAR()
    {
        listen = true;
        PauseMedia();
        sphere.SetActive(true);
        panelSub.SetActive(false);
        PanelTextLAR.SetActive(true);
    }


    private void ConfigListenMode()
    {
        /**Se oculta panel de informacion y se cargan los textos**/
        indiceAudio = 0;
        listen = true;
        PauseMedia();
        panelInfo.SetActive(true);
        sphere.SetActive(true);
        panelSub.SetActive(false);
        textRespuesta.text = "";
        PanelTextLAR.SetActive(true);

        Vector3 apagar = new Vector3(0.00001f, 0.00001f, 0.00001f);
        panelInfo.transform.localScale = apagar;

        if (currentPage == 0)
        {
            GameObject BtnAnterior = GameObject.Find("UI_BtnAnterior");
            BtnAnterior.transform.localScale = apagar;
        }
        DisablePanelInteration();

        TextInfoFill();
        counterAudio.Start();
        counterDelay.Reset();
        counterDelay.Start();
    }

    private void RepeatDialog()
    {
        Vector3 panelInfoScale = new Vector3(3f, 2f, 0.008f);
        panelInfo.transform.localScale = panelInfoScale;
        PanelTextLAR.SetActive(false);
        int textRepeat = textForRepeat.Count - currentPage * windows;

        if (indiceAudio < windows && indiceAudio < textRepeat)
        {
            TextMesh textObject = GameObject.Find("TextInfo" + (indiceAudio + 1)).GetComponent<TextMesh>();
            textObject.color = Color.yellow;

            //obtengo el audio segun codigo
            if (currentPage+1 == 1)
            {
                DialogType audioDiag = (DialogType)textForRepeat[indiceAudio];
                string audioCodeToRepeat = audioDiag.audioId;
                PlayAudio(audioCodeToRepeat);
            }
            else if(currentPage + 1 == 2) {
                DialogType audioDiag = (DialogType)textForRepeat[indiceAudio + 5];
                string audioCodeToRepeat = audioDiag.audioId;
                PlayAudio(audioCodeToRepeat);
            }
            else if (currentPage + 1 == 3)
            {
                DialogType audioDiag = (DialogType)textForRepeat[indiceAudio + 10];
                string audioCodeToRepeat = audioDiag.audioId;
                PlayAudio(audioCodeToRepeat);
            }

            if (indiceAudio > 0)
            {
                TextMesh textObjectBefore = GameObject.Find("TextInfo" + indiceAudio).GetComponent<TextMesh>();
                textObjectBefore.color = Color.white;
            }

            counterAudio.Stop();
            counterAudio.Reset();
            counterAudio.Start();
            indiceAudio++;
        }
        else //Se reprodujeron todos los audios, espero 
        {
            GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.yellow;

            TextMesh textObjectBefore = GameObject.Find("TextInfo" + indiceAudio).GetComponent<TextMesh>();
            textObjectBefore.color = Color.white;

            //activar botones interaccion
            EnablePanelInteration();

            indiceAudio = 0; //reseteo el contador
            counterAudio.Stop();
            counterAudio.Reset();
            counterAudio.Start();
            listen = false;
            wait = true;
            isInPanelInfoMode = true;

        }
    }

    private bool ElapsedTime(int seconds)
    {
        return counterDelay.ElapsedMilliseconds > seconds;
    }

    private bool ElapsedAudioTime(int seconds)
    {
        return counterAudio.ElapsedMilliseconds > seconds;
    }

    private void StartDelayTime()
    {
        counterDelay.Reset();
        counterDelay.Start();
    }

    public void EnableInterationMenu()
    {
        sphere.SetActive(true);
        panelSub.SetActive(false);
        panelInput.SetActive(true);
        panelAnswer.SetActive(true);

        panelQuestion.SetActive(true);

        TextMesh textObject = GameObject.Find("QuestionText").GetComponent<TextMesh>();
        textObject.text = dialogType.Text; ;

        //panelHintButton.SetActive(true);
        panelHintText.SetActive(true);
        panelSkipHint.SetActive(true);

        BtnGuia.SetActive(true);

        if (firstTimePanelInteraccion)
        {
            ButtonGuiaOn();
            firstTimePanelInteraccion = false;
        }
    }

    public void DisableInterationMenu()
    {
        listen = false;
        showUserInput = false;
        sphere.SetActive(false);
        panelInput.SetActive(false);
        panelSub.SetActive(true);
        panelAnswer.SetActive(false);
        panelQuestion.SetActive(false);
        //panelHintButton.SetActive(false);
        panelHintText.SetActive(false);
        panelSkipHint.SetActive(false);
        userAnswer.text = "";
        givenHint.text = "";
        Sub.text = "";
        BtnGuia.SetActive(false);
    }

    public void KeyboardExitButton()
    {

        string answer = keyboardInp.text;

        if (isInPanelInfoMode)
        {

            KeyboardExitRepeatPanel();

        }
        else
        {

            keyboardInp.text = "";
            keyboard.SetActive(false);
            emptyKeyboardAnswer = false;


            if (!emptyKeyboardAnswer)
            {
                emptyKeyboardAnswer = false;
                panelSub.SetActive(false);
                panelInput.SetActive(true);
                panelAnswer.SetActive(true);
                panelQuestion.SetActive(true);
                //panelHintButton.SetActive(true);
                //hintButton.SetActive(true);
                //skipButton.SetActive(true);
                panelHintText.SetActive(true);
                panelSkipHint.SetActive(true);
                gifCross.SetActive(false);
                gifTick.SetActive(false);
            }

        }
    }

    public void KeyboardOKButton()
    {

        string answer = keyboardInp.text;
        answer = answer.Trim();
        answer = Regex.Replace(answer, @"\s+", " ");

        if (isInPanelInfoMode)
        {

            KeyboardOKRepeatPanel();

        }
        else
        {

            if (answer.Equals("") || answer.Equals(TECLADO_RESPUESTA_VACIA))
            {
                emptyKeyboardAnswer = true;
            }
            else
            {
                emptyKeyboardAnswer = false;
            }

            if (!emptyKeyboardAnswer)
            {
                emptyKeyboardAnswer = false;
                ValidateAnswer(answer);
                panelSub.SetActive(false);
                panelInput.SetActive(true);
                panelAnswer.SetActive(true);
                panelQuestion.SetActive(true);
                //panelHintButton.SetActive(true);
                panelHintText.SetActive(true);
                //hintButton.SetActive(true);
                //skipButton.SetActive(true);
                panelSkipHint.SetActive(true);
            }
        }
    }

    public void KeyboardExitRepeatPanel()
    {
        keyboardInp.text = "";
        panelInfo.SetActive(true);
        keyboard.SetActive(false);
    }

    public void KeyboardOKRepeatPanel()
    {

        panelInfo.SetActive(true);
        keyboard.SetActive(false);
        string answer = keyboardInp.text;
        answer = answer.Replace(".", string.Empty);

        if (answer.Equals("") || answer.Equals(TECLADO_RESPUESTA_VACIA))
        {
            emptyKeyboardAnswer = true;
        }
        else
        {
            emptyKeyboardAnswer = false;
        }

        if (!emptyKeyboardAnswer)
        {
            emptyKeyboardAnswer = false;
            ValidateAnswerRepeatPanel(answer);

        }
    }

    public void ValidateAnswerRepeatPanel(string answer)
    {
        try
        {
            PanelInfoText selectedText = PanelInfoText.Instance;
            int numberTextSelected = selectedText.WhichTextSelected();
            string selectedString = GameObject.Find("TextInfo" + numberTextSelected).GetComponent<TextMesh>().text;

            keyboard.SetActive(false);

            answer = answer.Replace("!", string.Empty);
            answer = answer.Replace(".", string.Empty);
            answer = answer.Trim();
            answer = Regex.Replace(answer, @"\s+", " ");

            selectedString = selectedString.Replace("!", string.Empty);
            selectedString = selectedString.Replace(".", string.Empty);

            bool evaluatedAnswer = answer.ToLower().Contains(selectedString.ToLower());

            if (evaluatedAnswer)
            {
                listen = false;
                showUserInput = false;
                answerOK = true;
                GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo2").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo3").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo4").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo5").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo" + numberTextSelected).GetComponent<TextMesh>().color = Color.green;
                PlayAudio("correct");
            }
            else
            {
                GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo2").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo3").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo4").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo5").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo" + numberTextSelected).GetComponent<TextMesh>().color = Color.red;
                PlayAudio("incorrect");
            }
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log("[MediaManager][ValidateAnswerRepeatPanel] Excepcion: " + ex.Message);
        }
    }

    public void ValidateAnswerRepeatPanelVoice(string answer)
    {
        try
        {
            PanelInfoText selectedText = PanelInfoText.Instance;
            int numberTextSelected = selectedText.WhichTextSelected();
            string selectedString = GameObject.Find("TextInfo" + numberTextSelected).GetComponent<TextMesh>().text;

            answer = answer.Trim();
            answer = Regex.Replace(answer, @"\s+", " ");

            selectedString = selectedString.Replace("!", string.Empty);
            selectedString = selectedString.Replace(".", string.Empty);
            selectedString = selectedString.Replace(",", string.Empty);
            selectedString = selectedString.Replace("?", string.Empty);

            bool evaluatedAnswer = answer.ToLower().Contains(selectedString.ToLower());

            if (evaluatedAnswer)
            {
                listen = false;
                showUserInput = false;
                answerOK = true;
                GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo2").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo3").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo4").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo5").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo" + numberTextSelected).GetComponent<TextMesh>().color = Color.green;
                PlayAudio("correct");
            }
            else
            {
                GameObject.Find("TextInfo1").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo2").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo3").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo4").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo5").GetComponent<TextMesh>().color = Color.white;
                GameObject.Find("TextInfo" + numberTextSelected).GetComponent<TextMesh>().color = Color.red;
                PlayAudio("incorrect");
            }
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log("[MediaManager][ValidateAnswerRepeatPanelVoice] Excepcion: " + ex.Message);
        }
    }


    private void PauseMedia()
    {
        media.Pause();
        counterVideo.Stop();
    }

    private void ResumeMedia()
    {
        media.Play();
        counterVideo.Start();
    }

    [System.Serializable]
    public struct MediaManagerData
    {
        [SerializeField] public string audioAssetKey;
        [SerializeField] public string audioVolumeConfigValue;
        [SerializeField] public string videoAssetKey;
        [SerializeField] public string videoVolumeConfigValue;
    }

	public void FinishLesson()
    {
        try
        {
            experience.BackToMainMenu();

        }catch (Exception ex)
        {
            UnityEngine.Debug.Log("[MediaManager][FinishLesson] Excepcion: " + ex.Message);
        }
    }


    private void FinishLessonPart()
    {
        try
        {
            listen = false;
            counterVideo.Reset();
            counterVideo.Stop();
            counterVideo.Start();
            panelInfo.SetActive(false);
            sphere.SetActive(false);
            panelSub.SetActive(true);
            textForRepeat.Clear();
            audioCodesForRepeat.Clear();
            ManagerVideo();
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log("[MediaManager][FinishLessonPart] Excepcion: " + ex.Message);
        }
    }

    private void ManagerVideo()
    {
        try
        {
            if (experience == null)
            {
                experience = VRExperience.Instance;
            }
            string videoName = experience.NextVideo();

            counterVideo.Reset();
            if (!videoName.Equals("End"))
            {
				if(!videoName.Equals("Error"))
				{
					navigationPanel.materialOriginal();
					navigationPanel.colorPart();
					navigationPanel.OcultarPart();
                    InitializeVariables();
                    pasarGuia = true;
                    media.UnLoad();

                    subReader.RestFileReader(videoName, experience.ResourcesPath + experience.MatedataPath);
                    media.Load("file://" + experience.ResourcesPath + experience.VideosPath + videoName);
                    media.Play();
                    counterVideo.Start();
                }
            }
            else
            {
                FinishLesson();
            }
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log("[MediaManager][ManagerVideo] Excepcion: " + ex.Message);
        }

    }

    private void PlayAudio(string soundName)
    {
        try
        {
            sfx = gameObject.AddComponent<AudioSource>();
            if (soundName != null && soundName != "")
            {
                    string pathSounds = audioManager.getAudioPathName(soundName);
                    sfx.clip = Resources.Load<AudioClip>(pathSounds) as AudioClip;
                    //sfx.clip = (AudioClip)audioManager.HashAudios[soundName];
                    sfx.loop = false;
                    sfx.volume = 1.0f;
                    sfx.ignoreListenerPause = true;
                    sfx.enabled = false;
                    sfx.enabled = true;
                    sfx.Play();

                    //SetMaterialPlay();
            }
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log("[MediaManager][PlayAudio] Excepcion: " + ex.Message);
        }
    }

    private void Wait(float waitTime)
    {
        float time = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - time <= waitTime) ;
    }

    public void ActiveObject(GameObject gameObj)
    {
        ActiveMeshRenderer(gameObj, true);
    }

    public void DesactiveObject(GameObject gameObj)
    {
        ActiveMeshRenderer(gameObj, false);
    }

    private void ActiveMeshRenderer(GameObject gameObj, bool v)
    {
        MeshRenderer mesh = gameObj.GetComponent<MeshRenderer>();
        mesh.enabled = v;
    }

    public void GiveHint()
    {
        SetInactiveButtonGuia();
        userQualification.HelpCount +=1;
        UnityEngine.Debug.Log("[MediaManager][GiveHint] HelpCount:" + userQualification.HelpCount);

        givenHint.text += " " + dialogType.Answers[0].ToString().Split(' ')[++i];
    }

    public void ExecuteSkip()
    {
        SetInactiveButtonGuia();
        mediaDialogMenuPanel.SetActive(true);
    }


    public void SelectVideo(int indice)
    {
        try
        {
            if (experience == null)
            {
                experience = VRExperience.Instance;
            }
            indice--;
            string videoName = experience.SelectVideo(indice);
            pasarGuia = true;
            counterVideo.Reset();

            if (!videoName.Equals("End"))
            {
                if (!videoName.Equals("Error"))
                {

                    navigationPanel.materialOriginal();
                    Sub.text = "";
                    normalText.text = "";
                    panelInfo.SetActive(false);
                    panelInput.SetActive(false);

                    sphere.SetActive(false);
                    InitializeVariables();
                    navigationPanel.colorPart();
                    navigationPanel.OcultarPart();

                    subReader.RestFileReader(videoName, experience.ResourcesPath + experience.MatedataPath);
                    media.Load("file://" + experience.ResourcesPath + experience.VideosPath + videoName);
                    media.Play();
                    counterVideo.Start();
                }
            }
            else
            {
                FinishLesson();
            }
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log("[MediaManager][SelectVideo] Excepcion: " + ex.Message);
        }
    }

    public void DisplayWarningMessage(string message)
    {
        userAnswer.text = message;
        userAnswer.color = Color.red;
    }

    public void ValidateAnswer(string answer)
    {
        try
        {
            keyboard.SetActive(false);
            //delete leading and final white-spaces 
            answer = answer.Trim();
            //substitute multiple spaces from within words for one space
            answer = Regex.Replace(answer, @"\s+", " ");

            bool evaluatedAnswer = processAnswer.evaluateAnswer(answer, this.dialogType);

            if (evaluatedAnswer)
            {
                answerOK = true;
                sphere.SetActive(false);
                userAnswer.text = answer;
                givenHint.text = "";
                userAnswer.color = Color.green;
                gifTick.SetActive(true);
                gifCross.SetActive(false);
            
                ConfigDialogMode();
                ResumeMedia();
                userQualification.SuccessCount +=1;
                UnityEngine.Debug.Log("[MediaManager][ValidateAnswer] SuccessCount:" + userQualification.SuccessCount);
            }
            else
            {
                userAnswer.text = answer;
                userAnswer.color = Color.red;
                panelInput.SetActive(true);
                gifCross.SetActive(true);
                userQualification.RepeatCount +=1;
                UnityEngine.Debug.Log("[MediaManager][ValidateAnswer] RepeatCount:" + userQualification.RepeatCount);

            }
        }
        catch (System.Exception ex)
        {
            //TODO logger
            UnityEngine.Debug.Log("[MediaManager][ValidateAnswer] Excepcion: " + ex.Message);
        }
    }

    public void SelectTextInfo(int TextInt)
    {
        //paint all texts white
        for (int i = 1; i <= 5; i++)
        {
            GameObject.Find("TextInfo" + i).GetComponent<TextMesh>().color = Color.white;
        }
        //assign selected material to selected Text and paint selected text in yellow
        string selectedString = GameObject.Find("TextInfo" + TextInt).GetComponent<TextMesh>().text;
        GameObject.Find("TextInfo" + TextInt).GetComponent<TextMesh>().color = Color.yellow;
    }

    //metodo repetir audio panel de resumen

    public void repeatAudio(int selectedString) {

        if (currentPage + 1 == 1)
        {
            DialogType audioDiag = (DialogType)textForRepeat[selectedString];
            string audioCodeToRepeat = audioDiag.audioId;
            PlayAudio(audioCodeToRepeat);
        }
        else if (currentPage + 1 == 2)
        {
            DialogType audioDiag = (DialogType)textForRepeat[selectedString + 5];
            string audioCodeToRepeat = audioDiag.audioId;
            PlayAudio(audioCodeToRepeat);
        }
        else if (currentPage + 1 == 3)
        {
            DialogType audioDiag = (DialogType)textForRepeat[selectedString + 10];
            string audioCodeToRepeat = audioDiag.audioId;
            PlayAudio(audioCodeToRepeat);
        }
    }

    //metodo proxima pagina panel de resumen
    public void NextPage()
    {

        currentPage += 1;
        if (currentPage * windows < textForRepeat.Count)
        {
            Vector3 prender = new Vector3(0.4f, 0.8f, 1f);
            GameObject BtnAnterior = GameObject.Find("UI_BtnAnterior");
            BtnAnterior.transform.localScale = prender;
            ConfigListenMode();
            wait = false;
        }
        else
        {
            FinishLessonPart();
        }

    }

    //metodo pagina anterior panel de resumen
    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage -= 1;
            ConfigListenMode();
            wait = false;
        }
    }


    //metodo rellenar panel de resumen al final de cada video
    public void TextInfoFill()
    {
        //primero activa todo los radioButton y los panelInfo
        Vector3 prender = new Vector3(1f, 0.133f, 1f);
        for (int i = 1; i <= windows; i++)
        {
            GameObject.Find("PanelInfo" + i).transform.localScale = prender;
        }

        int indice = 0;
        for (int i = 0; i < windows; i++)
        {
            TextMesh textObject = GameObject.Find("TextInfo" + (i + 1)).GetComponent<TextMesh>();
            indice = currentPage * windows + i;

            if (indice < textForRepeat.Count)
            {
                DialogType diag = (DialogType)textForRepeat[indice];
                string text = diag.Text;
                textObject.text = text;
                textObject.color = Color.white;
            }
            else
            {
                textObject.text = "";
            }
        }
        int textRepeat = textForRepeat.Count - currentPage * windows;
        Vector3 apagar = new Vector3(0.00001f, 0.00001f, 0.00001f);
        for (int i = textRepeat + 1; i <= windows; i++)
        {
            GameObject.Find("PanelInfo" + i).transform.localScale = apagar;
        }
    }

    public void DisablePanelInteration()
    {
        Vector3 apagar = new Vector3(0.00001f, 0.00001f, 0.00001f);
        GameObject InteraccionBasePanelInfo = GameObject.Find("UI_InteraccionBasePanelInfo");
        InteraccionBasePanelInfo.transform.localScale = apagar;

        GameObject Paginado = GameObject.Find("UI_Paginado");
        Paginado.transform.localScale = apagar;

        GameObject Replay = GameObject.Find("UI_ReplayPanel");
        Replay.transform.localScale = apagar;
    }

    public void EnablePanelInteration()
    {
        Vector3 interaccionScale = new Vector3(0.02f, 0.09f, 0.075f);
        Vector3 paginadoScale = new Vector3(0.03f, 0.018f, 0.075f);
        Vector3 replayScale = new Vector3(0.01f, 0.09f, 1f);

        GameObject InteraccionBasePanelInfo = GameObject.Find("UI_InteraccionBasePanelInfo");
        InteraccionBasePanelInfo.transform.localScale = interaccionScale;
        GameObject Paginado = GameObject.Find("UI_Paginado");
        Paginado.transform.localScale = paginadoScale;
        GameObject Replay = GameObject.Find("UI_ReplayPanel");
        Replay.transform.localScale = replayScale;

        BtnGuia.SetActive(true);

        if (firstTimePanelInfo)
        {
            ButtonGuiaOn();
            firstTimePanelInfo = false;
        }

        //Primero los prendo a todos
        Vector3 prender = new Vector3(0.7f, 0.12f, 1f);
        for (int i = 1; i <= windows; i++)
        {
            GameObject.Find("UI_Replay" + i).transform.localScale = prender;
        }

        //Luego apago las que no corresponden
        int textRepeat = textForRepeat.Count - currentPage * windows;
        Vector3 apagar = new Vector3(0.00001f, 0.00001f, 0.00001f);
        for (int i = textRepeat + 1; i <= windows; i++)
        {
            GameObject.Find("UI_Replay" + i).transform.localScale = apagar;
        }
    }

    public void ButtonGuia()
    {
        if (!isButtonGuiaOn)
        {
            ButtonGuiaOn();
        }
        else
        {
            ButtonGuiaOff();
        }
    }

    private void ButtonGuiaOn()
    {
        if (panelInfo.activeSelf)
        {
            GuiaPanelInfo.SetActive(true);
            LoadPanelInfoTexts();
            isButtonGuiaOn = true;
        }
        else if (panelInput.activeSelf)
        {
            GuiaPanelInteraccion.SetActive(true);
            isButtonGuiaOn = true;
            LoadPanelInteractionTexts();
        }
    }

    private void LoadPanelInteractionTexts()
    {
        GameObject.Find("TextGuiaBtnTeclado").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaBtnTeclado").Replace("\\n", "\n");
        GameObject.Find("TextGuiaBtnStart").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaBtnStart").Replace("\\n", "\n");
        GameObject.Find("TextGuiaQuestionPanel").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaQuestionPanel").Replace("\\n", "\n");
        GameObject.Find("TextGuiaAnswerPanel").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaAnswerPanel").Replace("\\n", "\n");
        GameObject.Find("TextGuiaHintText").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaHintText").Replace("\\n", "\n");
        GameObject.Find("TextGuiaHintButton").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaHintButton").Replace("\\n", "\n");
        GameObject.Find("TextGuiaSkipButton").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaSkipButton").Replace("\\n", "\n");
    }

    private void LoadPanelInfoTexts()
    {
        // GameObject.Find("TextGuiaRadioButton").GetComponent<TextMesh>().text = experience.GetGameObjectText("TextGuiaRadioButton").Replace("\\n", "\n");
        GameObject.Find("TextGuiaPanelInfo").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaPanelInfo").Replace("\\n", "\n");
        GameObject.Find("TextGuiaBtnPlay").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaBtnPlay").Replace("\\n", "\n");
        GameObject.Find("TextGuiaBtnTecladoInfo").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaBtnTecladoInfo").Replace("\\n", "\n");
        GameObject.Find("TextGuiaBtnStartInfo").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaBtnStartInfo").Replace("\\n", "\n");
        GameObject.Find("TextGuiaPanelRespuesta").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaPanelRespuesta").Replace("\\n", "\n");
        GameObject.Find("TextGuiaPaginado").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaPaginado").Replace("\\n", "\n");
        GameObject.Find("TextGuiaBtnGuia").GetComponent<TextMesh>().text = VRExperience.Instance.GetGameObjectText("TextGuiaBtnGuia").Replace("\\n", "\n");
    }

    private void ButtonGuiaOff()
	{
		if (panelInfo.activeSelf) {
			GuiaPanelInfo.SetActive(false);
			isButtonGuiaOn = false;
		} else if (panelInput.activeSelf) {
			GuiaPanelInteraccion.SetActive(false);
			isButtonGuiaOn = false;
		}
	}

	public void SetInactiveButtonGuia()
	{
		if (isButtonGuiaOn) {
			GuiaPanelInfo.SetActive(false);
			GuiaPanelInteraccion.SetActive(false);
			isButtonGuiaOn = false;
		}
	}

    public void pasarPanelGuia() {
        pasarGuia = false;
    }


    private void LoadPanelResumeTexts()
	{
		GameObject.Find("TextTituloResumen").GetComponent<TextMesh>().text = experience.GetGameObjectText("TextTituloResumen").Replace("\\n", "\n");
		GameObject.Find("TextMensaje").GetComponent<TextMesh>().text = experience.GetGameObjectText("TextMensaje").Replace("\\n", "\n");
		GameObject.Find("TextCorrectas").GetComponent<TextMesh>().text = experience.GetGameObjectText("TextCorrectas").Replace("\\n", "\n");
		GameObject.Find("TextReintentos").GetComponent<TextMesh>().text = experience.GetGameObjectText("TextReintentos").Replace("\\n", "\n");
		GameObject.Find("TextAyuda").GetComponent<TextMesh>().text = experience.GetGameObjectText("TextAyuda").Replace("\\n", "\n");
		GameObject.Find("TextSalteadas").GetComponent<TextMesh>().text = experience.GetGameObjectText("TextSalteadas").Replace("\\n", "\n");
		GameObject.Find("UI_BtnMenuText").GetComponent<TextMesh>().text = experience.GetGameObjectText("UI_BtnMenuText").Replace("\\n", "\n");
		//GameObject.Find("UI_BtnRepetirText").GetComponent<TextMesh>().text = experience.GetGameObjectText("UI_BtnRepetirText").Replace("\\n", "\n");
	}

	private void LoadPanelResumeScore()
	{
		GameObject.Find("TextCorrectas").GetComponent<TextMesh>().text += userQualification.SuccessCount.ToString();
		GameObject.Find("TextReintentos").GetComponent<TextMesh>().text += userQualification.RepeatCount.ToString();
		GameObject.Find("TextAyuda").GetComponent<TextMesh>().text += userQualification.HelpCount.ToString();
		GameObject.Find("TextSalteadas").GetComponent<TextMesh>().text += userQualification.SkipCount.ToString();
	}
}