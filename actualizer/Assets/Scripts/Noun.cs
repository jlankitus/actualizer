using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Noun : MonoBehaviour
{
    public GameObject NounObject;
    RectTransform position;
    Image img;

    public string URL = "https://pixabay.com/api/?key=10251035-a05d4a04b2f830d06ff821d22&q=";
    public string URL_Options = "&image_type=photo&pretty=true";

    public Noun(string noun_name, Transform parent)
    {
        // TODO: Add text label
        NounObject = new GameObject(noun_name);
        position = NounObject.AddComponent<RectTransform>();
        img = NounObject.AddComponent<Image>();
        img.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
        NounObject.transform.SetParent(parent);
        assembleURL();
    }

    public void assembleURL()
    {
        URL += NounObject.name;
        // TODO fix bugs with adjectives being included properly in search
        /*
        foreach (string adjective in NounManager.instance.adjectiveDictionary[NounObject.name])
            URL += '+' + adjective;
        */
        // List<string> adjectives = NounManager
        URL += URL_Options;
        NounManager.instance.downloadSprite(URL, img);
    }
}
