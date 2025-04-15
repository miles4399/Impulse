using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelCompleteText : MonoBehaviour
{
    [SerializeField] private RectTransform _text1;
    [SerializeField] private RectTransform _text2;
    [SerializeField] private float _moveTime = 3f;
    [SerializeField] private float _scaleTime = 0.3f;
    [SerializeField] private float _scaleSizde = 1.5f;

    private Vector3 _text1Start;
    private Vector3 _text1End;
    private Vector3 _text2Start;
    private Vector3 _text2End;

    private void Awake()
    {
        _text1End = _text1.anchoredPosition3D;
        float x = _text1.anchoredPosition3D.x + 1200f;
        float y = _text1.anchoredPosition3D.y;
        float z = _text1.anchoredPosition3D.z;
        _text1Start = new Vector3(x, y, z);
        _text2End = _text2.anchoredPosition3D;
        float x2 = _text2.anchoredPosition3D.x + 1100f;
        float y2 = _text2.anchoredPosition3D.y;
        float z2 = _text2.anchoredPosition3D.z;
        _text2Start = new Vector3(x2, y2, z2);

        _text1.anchoredPosition3D = _text1Start;
        _text2.anchoredPosition3D = _text2Start;
    }

    private void Start()
    {
        StartCoroutine(MoveTextOne());
    }

    private IEnumerator MoveTextOne()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _moveTime)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / _moveTime;

            Vector3 movePos = Vector3.Lerp(_text1Start, _text1End, percentageComplete);

            _text1.anchoredPosition3D = movePos;
            

            yield return null;
        }

        yield return new WaitForSeconds(0.3f);
        StartCoroutine(MoveTextTwo());
    }

    private IEnumerator MoveTextTwo()
    {
        float elapsedTime = 0f;
        while (elapsedTime < _moveTime)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / _moveTime;

            Vector3 movePos = Vector3.Lerp(_text2Start, _text2End, percentageComplete);

            _text2.anchoredPosition3D = movePos;

            yield return null;
        }

        //StartCoroutine(ScaleText());
    }

    //private IEnumerator ScaleText()
    //{
    //    float elapsedTime = 0f;
    //    while (elapsedTime < _scaleTime)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float percentageComplete = elapsedTime / _scaleTime;
    //        Vector3 text1Scale = _text1.localScale;
    //        Vector3 text1ScaleEnd =  new Vector3(_scaleSizde, _scaleSizde, _scaleSizde);

    //        Vector3 scale = Vector3.Lerp(text1Scale, text1ScaleEnd, percentageComplete);

    //        _text1.localScale = scale;

    //        yield return null;
    //    }

    //}


}
