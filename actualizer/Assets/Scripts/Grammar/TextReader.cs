using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TextReader : MonoBehaviour {

    public InputField textInput;
    ParseText textParser = new ParseText();
    public GameObject Nouns;
    private NounManager nounManager;
    private string singular;
    private string currentTokenValue;
    private string currentTagValue;
    public int pluralCount = 2; // TODO: scan text for numbers related to plural values and replace default value with value parsed from sentence

    void Start ()
    {
        nounManager = Nouns.GetComponent<NounManager>();
    }
	
	public void AnalyzeText() // Tag part of speech and send sanitized data to the noun manager
    {
        // TODO: Integrate relations with story
        if (nounManager.nouns.Count > 0)
        {
            nounManager.nouns.Clear();
            nounManager.adjectiveDictionary.Clear();
            nounManager.noun_names.Clear();
            foreach (Transform rt in nounManager.defaultNextTo)
            {
                rt.gameObject.SetActive(false);
                Destroy(rt.gameObject);
            }               
            foreach (Transform rt in nounManager.onAboveBelow)
            {
                rt.gameObject.SetActive(false);
                Destroy(rt.gameObject);
            }   
        }

        StringBuilder debugOutput = new StringBuilder();
        string[] sentences = textParser.SplitSentences(textInput.text);
        foreach (string sentence in sentences)
        {
            string[] tokens = textParser.TokenizeSentence(sentence); // Tagged values (words)
            string[] tags = textParser.PosTagTokens(tokens);  // label the POS of each word
            string posTaggedSentence = string.Empty;
            List<string> adjectives = new List<string>();

            for (int currentTag = 0; currentTag < tags.Length; currentTag++)
            {
                currentTagValue = tags[currentTag];
                currentTokenValue = tokens[currentTag];
                
                // TODO: handle prepositions, and spawn to different parents
                if (currentTagValue == "JJ") adjectives.Add(currentTokenValue); // Add adjectives until we reach a noun
                if (currentTagValue == "NN")
                {
                    nounManager.addNoun(tokens[currentTag], 1);
                    if (adjectives.Count > 0)
                    {
                        nounManager.addAdjectives(currentTokenValue, adjectives); // Key is noun name, value is list of strings describing noun
                        adjectives.Clear();
                    }
                }                   
                else if (currentTagValue == "NNS")
                {
                    if (!char.IsUpper(currentTokenValue[0]))
                    {
                        singular = currentTokenValue.TrimEnd('s');
                        nounManager.addNoun(singular, pluralCount); // If plural spawn multiple
                    }
                    else nounManager.addNoun(currentTokenValue, 1);    // If ends in s but starts capital, assume proper noun and spawn one
                    if (adjectives.Count > 0)
                    {
                        nounManager.addAdjectives(currentTokenValue, adjectives);
                        adjectives.Clear();
                    }
                }
                posTaggedSentence += tokens[currentTag] + @"/" + currentTagValue + " ";
            }
            debugOutput.Append(posTaggedSentence); // Clean debug output
            debugOutput.Append("\r\n");
            debugOutput.Append("\r\n\r\n");
        }
        Debug.Log(debugOutput.ToString());
        nounManager.spawnNouns();
    }
}
