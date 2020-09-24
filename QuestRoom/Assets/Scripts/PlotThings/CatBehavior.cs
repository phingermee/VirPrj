using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
   Скрипт описывает поведение котика, в том числе сюжетную функцию: когда игрок заказчивает работу с ноутбуком,
   котик указывает на следующий сюжетный предмет (по сути, просто изменяется цель для NavMeshAgent)
 */
public class CatBehavior : MonoBehaviour
{
    //Создаём список анимационных состояний котика (каждой цифре соответствует анимация в Animator Parameters)
    enum Animation
    {
        Stay = 0,
        Go = 1,
        Jump = 2,
        Eat = 3
    }
    
    //Создаём публичную переменную, посредством которой мы сможем "отключать" котика из других скриптов
    public bool isMovingPossible;

    //Ограничиваем число целей для котика (их всего три)
    private int targetIndex = 0;
    public int TargetIndex
    {
        get => targetIndex;
        set => targetIndex = value < 3 ? value : 0;
    }

    //Создём объект аниматора, через который будем получать доступ к анимационным состояниям котика
    private Animator anim;
    //Создаём список целей, к которым направляемся котик
    [SerializeField]  private List<Transform> targetsList;
    //Цель котика
    private Transform target;
    private Vector3 startPosition;
    private NavMeshAgent destinatonTarget;

    private void Start()
    {
        destinatonTarget = GetComponent<NavMeshAgent>();
        startPosition = transform.position;
        anim = GetComponent<Animator>();
        isMovingPossible = true;

        //Выбираем первую цель
        target = targetsList[TargetIndex];
        //Выпускаем котика погулять
        OnMove(target);
    }

    //Движение котика
    private void OnMove(Transform t)
    {
        anim.SetInteger("Start", (int)Animation.Go);
        destinatonTarget.destination = t.position;
    }

    //В этом корутине котик запрыгивает на ящик (и спрыгивает потом обратно), чтобы указать игроку на спасительную монтировку
    private IEnumerator JumpAndShow()
    {
        if (!isMovingPossible)
        {
            anim.SetInteger("Start", (int)Animation.Jump);
            yield return new WaitForSeconds(1.5f);
            anim.SetInteger("Start", (int)Animation.Eat);
            destinatonTarget.isStopped = true;
        }
        else if (isMovingPossible)
        {
            OnMove(targetsList[TargetIndex]);
            yield return new WaitForSeconds(2.5f);
            destinatonTarget.isStopped = true;
            transform.position = startPosition;
            anim.SetInteger("Start", (int)Animation.Eat);
        }
    }

    //Можно было обойтись без корутина, но мне хотелось, чтобы котик "подумал" пару секунд во вемя выбора новой цели))
    private IEnumerator ChooseDirection()
    {
        //Останавливаем котика
        destinatonTarget.isStopped = true;
        //..анимацию тоже ставим статичную
        anim.SetInteger("Start", (int)Animation.Stay);
        //Даём котику время "подумать"
        yield return new WaitForSeconds(1.2f);

        // Если котика остановили, чтобы включить сюжетное поведение, то нацеливаем его на сейф
        TargetIndex = isMovingPossible ? ++TargetIndex : 0;
        //Выбираем следующую цель по списку
        target = targetsList[TargetIndex];
        //Разрешаем движение
        destinatonTarget.isStopped = false;
        //Запускаем котика
        OnMove(target);
    }

    //Если котик достигает цели, он выбирает следующую цель из списка
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == target.tag)
        {
            if (!isMovingPossible && other.gameObject.tag == "AmmoBox")
            {
                StartCoroutine(JumpAndShow());
            }
            else
                StartCoroutine(ChooseDirection());
        }
    }

    //Когда игрок наконец-то открывает сейф, котик спрыгивает на землю, чтоб не мешать (переключатель isMovingPossible показывает, какую часть функции JumpAndShow() стоит задействовать - спрыгивание или запрыгивание)
    public void CatBack()
    {
        isMovingPossible = true;
        StartCoroutine(JumpAndShow());
    }
}
