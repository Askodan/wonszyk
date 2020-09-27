using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Texts
{
    join, meal, hit, shot
}
public class TextBank : MonoBehaviour
{
    static Dictionary<Texts, string> female = new Dictionary<Texts, string>(){
        { Texts.join, "dołączyła" },
        { Texts.meal, "zjadła" },
        { Texts.hit, "uderzyła" },
        { Texts.shot, "strzeliła" },
        };
    static Dictionary<Texts, string> male = new Dictionary<Texts, string>(){
        { Texts.join, "dołączył" },
        { Texts.meal, "zjadł" },
        { Texts.hit, "uderzył" },
        { Texts.shot, "strzelił" },
        };
    static Dictionary<Texts, string> other = new Dictionary<Texts, string>(){
        { Texts.join, "dołączyło" },
        { Texts.meal, "zjadło" },
        { Texts.hit, "uderzyło" },
        { Texts.shot, "strzeliło" },
        };

    static Dictionary<Gender, Dictionary<Texts, string> > texts = new Dictionary<Gender, Dictionary<Texts, string> >()
    {
        {Gender.female, female },
        {Gender.male, male },
        {Gender.other, other }
    };

    public static string Say(string prefix, Texts text, Gender gender, string suffix)
    {
        return prefix + texts[gender][text] + suffix;
    }
}
