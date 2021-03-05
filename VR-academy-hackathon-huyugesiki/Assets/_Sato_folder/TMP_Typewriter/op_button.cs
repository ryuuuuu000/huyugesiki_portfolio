using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using KoganeUnityLib;

public class op_button : MonoBehaviour
{
    GameObject tmptext;

    void Start()
    {
        tmptext = GameObject.Find("OPtext");
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    tmptext.GetComponent<TMP_Typewriter>().Pointdown();
    //}

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    tmptext.GetComponent<TMP_Typewriter>().Pointenter();
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    tmptext.GetComponent<TMP_Typewriter>().Pointexit(); IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
    //}


    // Update is called once per frame
    void Update()
    {
        
    }
}
