using System.Collections;
using System.Collections.Generic;
using UnityEngine;

////////////////////////////////////////////////////////////////////////
///   Uses OpenNLP to analyze the input language            ////////////
///   Can change models here to allow multiple languages    ////////////
////////////////////////////////////////////////////////////////////////

public class ParseText {

    private OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger mPosTagger;
    private OpenNLP.Tools.SentenceDetect.MaximumEntropySentenceDetector mSentenceDetector;
    private OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer mTokenizer;
    private string mModelPath = "Assets/Models/";
    public string[] PosTagTokens(string[] tokens)
    {
        if (mPosTagger == null)
        {
            mPosTagger = new OpenNLP.Tools.PosTagger.EnglishMaximumEntropyPosTagger(mModelPath + "EnglishPOS.nbin", mModelPath + @"\Parser\tagdict");
        }
        return mPosTagger.Tag(tokens);
    }

    public string[] TokenizeSentence(string sentence)
    {
        if (mTokenizer == null)
        {
            mTokenizer = new OpenNLP.Tools.Tokenize.EnglishMaximumEntropyTokenizer(mModelPath + "EnglishTok.nbin");
        }
        return mTokenizer.Tokenize(sentence);
    }

    public string[] SplitSentences(string paragraph)
    {
        if (mSentenceDetector == null)
        {
            mSentenceDetector = new OpenNLP.Tools.SentenceDetect.EnglishMaximumEntropySentenceDetector(mModelPath + "EnglishSD.nbin");
        }
        return mSentenceDetector.SentenceDetect(paragraph);
    }
}
