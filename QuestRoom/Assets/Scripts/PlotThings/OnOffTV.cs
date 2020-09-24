using UnityEngine;

// Скрипт описывает включение/выключение телевизора при нажатии на кнопку и появление последней подсказки
public class OnOffTV : MonoBehaviour
{
    [SerializeField] private Modes mode;
    [SerializeField] private GameObject videoPlayer = null;
    [SerializeField] private GameObject hintCode = null;

    public void TVturnOnOff(bool isTVActive)
    {
        mode.TVButtonAnim.Play("TVButtonAnim");
        videoPlayer.SetActive(!isTVActive);        
    }

    public void ShowHint() => hintCode.SetActive(true);
}
