using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLink : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.InputField link;

    public void OnClick()
    {
        PlayerPrefs.SetString("source", link.text);
        SceneManager.LoadScene("test");
    }
}
