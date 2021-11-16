using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Wash.Utilities;

public class CheckList : MonoBehaviour
{
    [SerializeField]
    private Transform eventsContainer;
    [SerializeField]
    private Transform checkListItemContainer;
    [SerializeField]
    private GameObject checkListItemPrefab;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private float transitionTime = 1f;

    private List<CheckListItem> checkList = new List<CheckListItem>();
    private int currentCheckListItem = 0;


    private void Start()
    {
        int checkListIndex = 0;
        foreach (Transform washEvent in eventsContainer)
        {
            PlayerEvent playerEvent = washEvent.GetComponent<PlayerEvent>();

            if (playerEvent != null && playerEvent.gameObject.activeSelf && playerEvent.GetShouldBeChecklistEvent())
            {
                GameObject checklistItemObj = Instantiate(checkListItemPrefab, checkListItemContainer);
                CheckListItem checkListItem = checklistItemObj.GetComponent<CheckListItem>();
                checkListItem.SetText("Wash " + playerEvent.GetEventName());
                checkListItem.RegisterEvent(playerEvent, checkListIndex);
                checkListIndex++;
                checkList.Add(checkListItem);
            }
        }

        HighlightItem(currentCheckListItem);
    }

    public void CheckOffItem()
    {
        UnHighlightItem(currentCheckListItem);
        CheckListItem currentItem = checkList[currentCheckListItem];
        currentCheckListItem++;
        currentItem.SetToggle(true);

        if (currentCheckListItem < checkList.Count)
        {
            HighlightItem(currentCheckListItem);
            ScrollToItem(currentCheckListItem);
        }
    }



    public void ScrollToItem(int checkListItemIndex)
    {
        float normalizedPosition = 1 - ((1f / checkList.Count) * (checkListItemIndex+ 1));
        scrollRect.DOVerticalNormalizedPos(normalizedPosition, transitionTime, false);
        //Debug.Log(normalizedPosition);
    }

    public void HighlightItem(int index)
    {
        Debug.Log("Highligting " + index);
        CheckListItem currentItem = checkList[index];
        currentItem.GetComponent<Image>().DOFade(255, 1f);
    }

    public void ChooseItem(int index)
    {
        UnHighlightItem(currentCheckListItem);
        currentCheckListItem = index;
        HighlightItem(index);
        ScrollToItem(index);
    }

    public void UnHighlightItems()
    {
        for (int i=0; i < checkList.Count; i++)
        {
            UnHighlightItem(i);
        }
    }

    public void UnHighlightItem(int index)
    {
        CheckListItem currentItem = checkList[index];
        currentItem.GetComponent<Image>().SetAlpha(0f);
        currentItem.GetComponent<Image>().DOKill();
    }

}
