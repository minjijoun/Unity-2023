using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //UI�� ��Ʈ�� �Ұ��̶� �߰�
using System;           // Arrary ���� ����� ����ϱ����� �߰� 

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private SpeakerUI[] speakers;                       //��ȭ�� �����ϴ� ĳ���͵��� UI�迭
    [SerializeField]
    private DialogData[] dialogs;                       //���� �б��� ��� ��� �迭
    [SerializeField]
    private bool DialogInit = true;                     //init �˻� FLAG
    [SerializeField]
    private bool dialogsDB = false;                     //DB�����͸� ���� FLAG

    public int currentDialogIndex = -1;
    public int currentSpeakerIndex = 0;
    public float typingSpeed = 0.1f;
    private bool isTypingEffect = false;


    private void SetActiveObjects(SpeakerUI speaker, bool visible)
    {
        speaker.imageDialog.gameObject.SetActive(visible);
        speaker.textName.gameObject.SetActive(visible);
        speaker.textDialogue.gameObject.SetActive(visible);

        // ȭ��ǥ�� ��簡 ����Ǿ��� ���� Ȱ��ȭ�ϱ� ������ �׻� False
        speaker.objectArrow.SetActive(false);

        //ĳ���� ���İ� ����
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
        {   //1���� ȣ�� 
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
    public Image imgCharacter;                  //ĳ���� �̹���
    public Image imageDialog;                   // ��ȭâ Image UI
    public Text textName;                       // ���� ������� ĳ���� �̸� ���
    public Text textDialogue;                   // ĳ���� ���ñ� 
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
