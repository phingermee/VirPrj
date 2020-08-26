using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Localization
{

    public static class Locale
    {
        static string currentLanguage;
        public static Dictionary<string, string> currentLanguageStrings = new Dictionary<string, string>();
        static TextAsset currentLocalizationText;


        public static string CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                if (value != null && value.Trim() != string.Empty)
                {
                    currentLanguage = value;
                    currentLocalizationText = Resources.Load(currentLanguage, typeof(TextAsset)) as TextAsset;

                    if (currentLocalizationText == null)
                    {
                        currentLanguage = Application.systemLanguage.ToString();
                        currentLocalizationText = Resources.Load(currentLanguage, typeof(TextAsset)) as TextAsset;
                        Debug.LogError("Lng not supported, system's set");
                    }
                    if (currentLocalizationText != null)
                    {
                        string[] lines = currentLocalizationText.text.Split(new string[] { "\r\n", "\n\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
                        currentLanguageStrings.Clear();
                        for(int i = 0; i < lines.Length; i++)
                        {
                            string[] pairs = lines[i].Split(new char[] { '%' }, 2);
                            if (pairs.Length == 2)
                            {
                                currentLanguageStrings.Add(pairs[0].Trim(), pairs[1].Trim());
                            }
                        }
                    }
                }
            }
        }
    }

}
