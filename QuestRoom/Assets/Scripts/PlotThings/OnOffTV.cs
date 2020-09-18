using UnityEngine;

// Скрипт описывает включение/выключение телевизора при нажатии на кнопку и появление последней подсказки
public class OnOffTV : MonoBehaviour
{
    [SerializeField] private GameObject buttonOnOff = null;
    [SerializeField] private GameObject videoPlayer = null;
    [SerializeField] private GameObject hintCode = null;

    public void TVturnOnOff(bool isTVActive)
    {
        videoPlayer.SetActive(!isTVActive);
        buttonOnOff.transform.Translate(0, 0.02f * (isTVActive ? -1 : 1), 0);
    }

    public void ShowHint() => hintCode.SetActive(true);
}
