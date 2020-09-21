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
    enum Animations
    {
        Stay = 0,
        Go = 1,
        Jump = 2,
        Eat = 3
    }
    
    //Создаём публичную переменную, посредством которой мы сможем "отключать" котика из других скриптов
    public bool isMovingPossible;

    public int TargetIndex
    {
        get
        {
            return targetIndex;
        }
        set
        {
            targetIndex = value < 2 ? ++targetIndex : 0;
        }
    }
    private int targetIndex = 0;

    [SerializeField] private Transform seif;
    [SerializeField] private Transform TV;
    [SerializeField] private Transform door;

    //Создём объект аниматора, через который будем получать доступ к анимационным состояниям котика
    private Animator anim;
    //Создаём список целей, к которым направляемся котик
    private List<Transform> targets;
    //Цель котика
    private Transform target;

    private void Start()
    {
        anim = GetComponent<Animator>();
        isMovingPossible = true;

        //Находим все цели котика и загоняем их в список
        targets = new List<Transform> { seif, door, TV };

        //Выбираем первую цель
        target = targets[TargetIndex];
        //Выпускаем котика погулять
        OnMove(target);
    }

    //В этом корутине котик запрыгивает на ящик (и спрыгивает потом обратно), чтобы указать игроку на спасительную монтировку
    private IEnumerator JumpAndShow()
    {
        float y = 0.06f;

        if (!isMovingPossible)
        {
            anim.SetInteger("Start", (int)Animations.Jump);
            yield return new WaitForSeconds(1.5f);
            anim.SetInteger("Start", (int)Animations.Eat);
            for (int i = 0; i < 20; i++)
            {
                transform.Translate(0, y, 0.025f);
                y -= 0.004f;
                yield return new WaitForFixedUpdate();
            }
            GetComponent<NavMeshAgent>().isStopped = true;
        }
        else if (isMovingPossible)
        {
            anim.SetInteger("Start", (int)Animations.Jump);
            yield return new WaitForSeconds(1.5f);
            transform.position = new Vector3(0, 3, -3);
            anim.SetInteger("Start", (int)Animations.Eat);
        }
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

    //Можно было обойтись без корутина, но мне хотелось, чтобы котик "подумал" пару секунд во вемя выбора новой цели))
    private IEnumerator ChooseDirection()
    {
        //Останавливаем котика
        GetComponent<NavMeshAgent>().isStopped = true;
        //..анимацию тоже ставим статичную
        anim.SetInteger("Start", (int)Animations.Stay);
        //Даём котику время "подумать"
        yield return new WaitForSeconds(1.8f);

        // Если котика остановили, чтобы включить сюжетное поведение, то нацеливаем его на сейф
        TargetIndex = isMovingPossible ? TargetIndex : 0;
        //Разрешаем движение
        GetComponent<NavMeshAgent>().isStopped = false;
        //Выбираем цель
        target = targets[TargetIndex];
        //Запускаем котика
        OnMove(target);
    }

    //Движение котика
    private void OnMove(Transform t)
    {
            anim.SetInteger("Start", (int)Animations.Go);
            GetComponent<NavMeshAgent>().destination = t.position;
    }

    //Когда игрок наконец-то открывает сейф, котик спрыгивает на землю, чтоб не мешать (переключатель isMovingPossible показывает, какуб часть функции JumpAndShow() стоит задействовать - спрыгивание или запрыгивание)
    public void CatBack()
    {
        isMovingPossible = true;
        StartCoroutine(JumpAndShow());
    }
}
