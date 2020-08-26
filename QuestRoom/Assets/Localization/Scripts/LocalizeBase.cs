using JetBrains.Annotations;
using Localization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Localization
{
    public abstract class LocalizeBase : MonoBehaviour
    {

        #region Public Methods

        public static string GetLocaliedString(string key)
        {
            if (Locale.currentLanguageStrings.ContainsKey(key))
            {
                return Locale.currentLanguageStrings[key];
            }
            else
            {
                return string.Empty;
            }
        }

        public static void SetCurrentLanguage(SystemLanguage language)
        {
            Locale.CurrentLanguage = language.ToString();
            Localize[] allTexts = Resources.FindObjectsOfTypeAll(typeof(Localize)) as Localize[];
            for (int i = 0; i < allTexts.Length; i++)
            {
                allTexts[i].UpdateLocal();
            }
        }

        #endregion Public Methods


    }

}
