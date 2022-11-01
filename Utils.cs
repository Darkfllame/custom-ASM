using System;
using System.Collections.Generic;

public class IntUtils
{
    public static int? FromString(string str)
    {   
        //ascii : 48-57 : 0-9
        int? n = 0;
        for (int i = 0; i < str.Length ; i++)
        {
            int j = str.Length - i - 1;
            char c = str[j];
            if(c >= 48 && 57 >= c)
            {
                n += (int)((c-48)*Math.Pow(10, i));
            }else if(c != '-')
            {
                n = null;
                break;
            }
        }

        if(str[0]=='-' && n != null) n *= -1;

        return n;
    }
}
public class StringUtils
{
    public static List<string> Separate(string text, char sep)
    {
        List<string> words = new List<string>();

        string word = "";
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (c == sep && word.Length > 0)
            {
                words.Add(word);
                word = "";
            }else if(c != sep)
            {
                word += c;
            }
        }
        words.Add(word);

        return words;
    }
}