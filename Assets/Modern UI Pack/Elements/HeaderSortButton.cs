using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeaderSortButton : MonoBehaviour
{

    private Button _button;
    public Button button
    {
        get
        {
            return _button;
        }
        set
        {
            _button = value;
        }
    }

    private Animator animator;

    private int selectionCount = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        _button.onClick.AddListener(OnSelect);
    }

    public void OnSelect()
    {
        selectionCount++;

        animator.SetInteger("ArrowState", (selectionCount % 2) + 1);
    }

    public void OnDeselect()
    {
        selectionCount = 0;

        animator.SetInteger("ArrowState", 0);
        //arrow should dissapear
    }
}
