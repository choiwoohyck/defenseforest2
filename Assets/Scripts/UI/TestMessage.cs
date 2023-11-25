using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doublsb.Dialog;
using UnityEditor.Rendering;
using System.Linq;
using System;

public class TestMessage : MonoBehaviour
{
    public DialogManager DialogManager;
    public GameObject Printer;
    public GameObject Character;
    public GameObject SkipButton;


    public GameObject[] Example;


     private void Start()
    {
        ShowText();
    }

    private void Awake()
    {
    

        //dialogTexts.Add(new DialogData("You can also change the character's sprite /emote:Sad/like this, /click//emote:Happy/Smile.", "Li",  () => Show_Example(2)));

        //dialogTexts.Add(new DialogData("If you need an emphasis effect, /wait:0.5/wait... /click/or click command.", "Li", () => Show_Example(3)));

        //dialogTexts.Add(new DialogData("Text can be /speed:down/slow... /speed:init//speed:up/or fast.", "Li", () => Show_Example(4)));

        //dialogTexts.Add(new DialogData("You don't even need to click on the window like this.../speed:0.1/ tada!/close/", "Li", () => Show_Example(5)));

        //dialogTexts.Add(new DialogData("/speed:0.1/AND YOU CAN'T SKIP THIS SENTENCE.", "Li", () => Show_Example(6), false));

        //dialogTexts.Add(new DialogData("And here we go, the haha sound! /click//sound:haha/haha.", "Li", null, false));

        //dialogTexts.Add(new DialogData("That's it! Please check the documents. Good luck to you.", "Sa"));

    }

    private void Show_Example(int index)
    {
        Example[index].SetActive(true);
    }

    public void ShowText(bool isFrist =true)
    {
        var dialogTexts = new List<DialogData>();

        if(isFrist)
        {
            dialogTexts.Add(new DialogData("안녕 난 마법의 돌 정령이야. 난 지금 힘을 잃어서 숲을 보호하기 힘든 상황이야. 너의 도움이 꼭 필요해.", "MagicStone"));
            dialogTexts.Add(new DialogData("알았어.", "Player"));
        }

        dialogTexts.Add(new DialogData("먼저 조작키를 알려줄게 잘 들어야해.", "MagicStone"));
        dialogTexts.Add(new DialogData("기본적으로 방향키로 움직일 수 있고 Tab키를 누르면 WASD로 이동키를 바꿀 수 있어.", "MagicStone"));
        dialogTexts.Add(new DialogData("SpaceBar를 누르면 마법을 사용할 수 있어.", "MagicStone"));
        dialogTexts.Add(new DialogData("또 Shift를 누르면 텔레포트를 사용할 수 있어.", "MagicStone"));
        dialogTexts.Add(new DialogData("텔레포트 하는 도중에는 무적판정이 생겨.", "MagicStone"));
        dialogTexts.Add(new DialogData("하단 밑에 Build UI를 통해 정령들에게 도움을 받을 수 있어.", "MagicStone"));
        dialogTexts.Add(new DialogData("몬스터가 이동하는 길에 정령을 설치하게 되면 능력이 변화되고 체력이 다할때까지 적의 이동을 막아줘.", "MagicStone"));
        dialogTexts.Add(new DialogData("설치하기 전에 마우스 우클릭을 누르면 설치를 취소할 수 있어.", "MagicStone"));
        dialogTexts.Add(new DialogData("하지만 정령에게 도움을 받으려면 에너지가 필요해! 에너지는 적들을 해치우면 얻을 수 있어.", "MagicStone"));
        dialogTexts.Add(new DialogData("준비가 되면 하단에 Battle 버튼을 눌러. 그러면 전투가 시작돼.", "MagicStone"));
        dialogTexts.Add(new DialogData("너의 체력과 나의 체력이 다하면 숲을 보호할 수 없게 되니까 주의해야 해!", "MagicStone"));
        dialogTexts.Add(new DialogData("참고로 왼쪽 상단에 보이는 빨간색 바가 너의 체력이고 밑에 파란색 바가 내 체력이야!", "MagicStone"));



        var repeatQuestion = new DialogData("다시 들을래?", "MagicStone");

        repeatQuestion.SelectList.Add("Repeat", "응.");
        repeatQuestion.SelectList.Add("NoRepeat", "아니.");

        repeatQuestion.Callback = () => Check_Correct();
        dialogTexts.Add(repeatQuestion);

        DialogManager.Show(dialogTexts);

    }

    private void Check_Correct()
    {
        if (DialogManager.Result == "Repeat")
        {
            ShowText(false);
        }
        else if (DialogManager.Result == "NoRepeat")
        {
            var dialogTexts = new List<DialogData>();
            dialogTexts.Add(new DialogData("내 힘이 점점 다하고 있어 숲을 지켜줘!!", "MagicStone"));
            GameManager.instance.isTutorialFinished = true;
            SkipButton.SetActive(false);
            DialogManager.Show(dialogTexts);
        }
       
    }

    public void ClickSkipButton()
    {

        Printer.SetActive(false);
        Character.SetActive(false);
        SkipButton.SetActive(false);
        GameManager.instance.isTutorialFinished = true;
        DialogManager.RemoveText();
        DialogManager.Hide();
        AudioManager.instance.PlayOnShotSFX(2);

    }

    public void OnSkipButton()
    {
        SkipButton.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
    }

    public void ExitSkipButton()
    {
        SkipButton.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

}
