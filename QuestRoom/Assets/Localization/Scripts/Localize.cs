using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Localization
{
    [RequireComponent(typeof(Text))]
    public class Localize : MonoBehaviour
    {

        private Text text;
        public string localizationKey;

        public void UpdateLocal()
        {
            if (text == null)
                text = GetComponent<Text>();
            
            if (!text) return;
            if(!string.IsNullOrEmpty(localizationKey) && Locale.currentLanguageStrings.ContainsKey(localizationKey))
            {
                text.text = Locale.currentLanguageStrings[localizationKey].Replace(@"\n", "" + '\n');

            }
        }


    }
}