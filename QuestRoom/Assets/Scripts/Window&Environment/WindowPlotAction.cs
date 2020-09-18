using System.Collections;
using System.Linq;
using UnityEngine;

// Скрипт описывает то, что происходит, когда игрок разбивает окно: звуки улицы становятся громче, активируется подсказка на следующий сжетный объект и т.д.
public class WindowPlotAction : MonoBehaviour
{
    [SerializeField] private Modes mode;
    [SerializeField] private OnOffTV TVScript;

    //При ударе монтировкой окно выпадает из рамы и у-ле-тай-ет в ней-йе-йе-еба-а-а!...
    IEnumerator ClearWindow()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Окно можно разбить только предметами с тегом "Damage" (конкретно в нашем случае - монтировкой)
        if (other.gameObject.tag == "Damage" && mode.isGasActive)
        {
            //Разбиваем окно
            GetComponent<BreakableWindow>().breakWindow();
            //Запоминаем, что третье задание выполнено
            mode.questTasks[2] = true;
            Destroy(other.gameObject);
            //Убираем осколки стекла
            StartCoroutine(ClearWindow());
            //Делаем звуки улицы громче
            mode.LoudStreetMode();
            //После разбития окна газ выветривается
            mode.GasMode();
            //Считаем, сколько заданий выполнено
            var finishChecking = mode.questTasks.Where(x => x.Equals(true));
            //Есл выполнены все задания, то...
            if (finishChecking.Count() == 3)
            {
                //..включаем на ТВ подсказку с кодом двери
                TVScript.ShowHint();
                //На всякий случай выключаем видео на телевизоре (а вдруг включено)
                TVScript.TVturnOnOff(true);
            }
        }
    }
}
