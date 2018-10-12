using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System;

public class NounManager : MonoBehaviour {

    public Transform defaultNextTo;
    public Transform onAboveBelow;
    private Noun newNoun;
    public List<string> noun_names = new List<string>();
    public List<GameObject> nouns;
    // TODO: Should adjectiveDictionary just be in the noun class itself?
    // Note keeping out of noun class allows lookup later in sentence potentially (with 2nd instance of noun)
    public Dictionary<string, List<string>> adjectiveDictionary = new Dictionary<string, List<string>>();

    // TODO: scan text for numbers related to plural values and replace default value with value parsed from sentence
    public float pluralCount = 2;

    private IEnumerator downloadCoroutine;
    public static NounManager instance;
    private NounManager(){}

    public void Start()
    {
        instance = this;
    }

    public void addNoun(string name, int number)
    {
        for(int i = 0; i < number; i++) noun_names.Add(name);
    }

    public void addAdjectives(string nounName, List<string> adjectives)
    {
        adjectiveDictionary.Add(nounName, adjectives);
    }

    public void spawnNouns()
    {
        foreach (string noun_name in noun_names)
        {
            newNoun = new Noun(noun_name, defaultNextTo);
            nouns.Add(newNoun.NounObject);
        }   
    }

    public void downloadSprite(string URL, Image img)
    {
        downloadCoroutine = getJSON(URL, img);
        StartCoroutine(downloadCoroutine);
    }

    public IEnumerator getJSON(string url, Image img)
    {
        WWW www = new WWW(url);
        yield return www;
        PixabayJSON json = JsonConvert.DeserializeObject<PixabayJSON>(www.text);
        // TODO based on tags, sort through and pick best image rather than the first
        StartCoroutine(getImage(json.hits[0].largeImageURL, img));
    }

    // TODO adjust texture so not blurry
    public IEnumerator getImage(string url, Image img)
    {
        WWW www = new WWW(url);
        yield return www;
        try
        {
            img.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
            img.preserveAspect = true;
        }
        catch(Exception e)
        {
            Debug.LogError("Sprite creation failed for url :" + url);
            Debug.LogError(e.ToString());
        }
            
    }
}
