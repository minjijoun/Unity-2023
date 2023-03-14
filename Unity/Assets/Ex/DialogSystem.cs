using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //UI를 컨트롤 할것이라서 추가
using System;           // Arrary 수정 기능을 사용하기위해 추가 

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private SpeakerUI[] speakers;                       //대화에 참여하는 캐릭터들의 UI배열
    [SerializeField]
    private DialogData[] dialogs;                       //현재 분기의 대사 목록 배열
    [SerializeField]
    private bool DialogInit = true;                     //init 검사 FLAG
    [SerializeField]
    private bool dialogsDB = false;                     //DB데이터를 읽음 FLAG

    public int currentDialogIndex = -1;
    public int currentSpeakerIndex = 0;
    public float typingSpeed = 0.1f;
    private bool isTypingEffect = false;


    private void SetActiveObjects(SpeakerUI speaker, bool visible)
    {
        speaker.imageDialog.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialogue.gameObject.SetActive(visible);

        // 화살표는 대사가 종료되었을 때만 활성화하기 때문에 항상 False
        speaker.objectArrow.SetActive(false);

        //캐릭터 알파값 변경
        Color color = speaker.imgCharacter.color;
        if (visible)
        {
            color.a = 1;
        }
        else
        {
            color.a = 0.2f;
        }
        speaker.imgCharacter.color = color;

    }

    private void SetAllClose()
    {
        for (int i = 0; i < speakers.Length; ++i)
        {
            SetActiveObjects(speakers[i], false);
        }
    }

    private void SetNextDialog(int currentIndex)
    {
        SetAllClose();
        currentDialogIndex = currentIndex;
        currentSpeakerIndex = dialogs[currentDialogIndex].speakerUIindex;
        SetActiveObjects(speakers[currentSpeakerIndex], true);

        speakers[currentSpeakerIndex].textName.text = dialogs[currentDialogIndex].name;
        StartCoroutine("OnTypingText");
    }

    private void Awake()
    {
      SetAllClose();
    }

    public bool UpdateDialog(int currentIndex, bool InitType)
    {
        if (DialogInit == true && InitType == true)
        {   //1번만 호출 
            SetAllClose();
            SetNextDialog(currentIndex); 
            DialogInit = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isTypingEffect == true)
            {
                isTypingEffect = false;
                StopCoroutine("OnTypingText");
                speakers[currentSpeakerIndex].textDialogue.text = dialogs[currentDialogIndex].dialogue;

                speakers[currentSpeakerIndex].objectArrow.SetActive(true);
                return false;
            }

            if (dialogs[currentDialogIndex].nextindex != -100)
            {
                SetNextDialog(dialogs[currentDialogIndex].nextindex);
            }
            else
            {
                SetAllClose();
                DialogInit = true;
                return true;
            }
        }

        return false;
    }

    private IEnumerator OnTypingText()
    {
        int index = 0;
        isTypingEffect = true;
        
        if (dialogs[currentDialogIndex].characterPath != "None")
        {
            speakers[currentSpeakerIndex].imgCharacter =
                (Image)Resources.Load(dialogs[currentDialogIndex].characterPath);
        }

        while (index < dialogs[currentDialogIndex].dialogue.Length +1)
        {
            speakers[currentSpeakerIndex].textDialogue.text =
                dialogs[currentDialogIndex].dialogue.Substring(0, index);
            index++;
            yield return new WaitForSeconds(typingSpeed);

            isTypingEffect = false;

            speakers[currentSpeakerIndex].objectArrow.SetActive(true);
        }
    }
}

[System.Serializable]
public struct SpeakerUI
{
    public Image imgCharacter;                  //캐릭터 이미지
    public Image imageDialog;                   // 대화창 Image UI
    public Text textName;                       // 현재 대사중인 캐릭터 이름 출력
    public Text textDialogue;                   // 캐릭터 뭐시기 
    public GameObject objectArrow;
}

[System.Serializable]
public struct DialogData
{
    public int index;
    public int speakerUIindex;
    public string name;
    public string dialogue;
    public string characterPath;
    public int tweenType;
    public int nextindex;
}
