using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialTextTrigger : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _UIText;
    // Start is called before the first frame update
    void Start()
    {
        _UIText.gameObject.SetActive(false);   
    }

    private void OnTriggerEnter(Collider other)
    {
        _UIText.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        _UIText.gameObject.SetActive(false);
    }
}
