using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomVoiceGenerate : MonoBehaviour
{
    [SerializeField] int[] _number;
    [SerializeField] float _minPlayTime = 1.5f;
    [SerializeField] float _maxPlayTime = 2.5f;

    private int _totalNumber;

    void Start()
    {
        _totalNumber = _number.Length;
        //StartCoroutine(PlayVoice());
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            int chosenNumner = _number[Random.Range(0, _totalNumber)];
            Debug.Log(chosenNumner);
        }
    }

    private IEnumerator PlayVoice()
    {
        for (int i = 1; i <=_totalNumber; i++)
        {
            yield return new WaitForSeconds(Random.Range(_minPlayTime, _maxPlayTime));
            Debug.Log(_number[Random.Range(0, _totalNumber)]);
            
            
        }
        
    }
}
