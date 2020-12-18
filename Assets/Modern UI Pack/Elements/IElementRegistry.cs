using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IElementRegistry : MonoBehaviour
{

    private ICanvasElement canvasElement;

    //private List<ICanvasElement> canvasElements;

    public void Awake()
    {
        canvasElement = GetComponent<ICanvasElement>();
    }

    public void RegistoryElements()
    {

        if(CanvasUpdateRegistry.IsRebuildingGraphics())
        {
            

        }

        //CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild();

    }

    public void UnregistorElements()
    {



    }

}
