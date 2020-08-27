using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    //Создём объект аниматора, через который будем получать доступ к анимационным состояниям котика
    Animator anim;
    //Создаём публичную переменную, посредством которой мы сможем "отключать" котика из других скриптов
    public bool isMovingPossible;
    //Создаём список целей, к которым направляемся котик
    List<Transform> targets;
    //Цель котика
    Transform target;
    //Объекты и свойства класса, которые помогают переопределить оператор ++ для смены целей
    public CatBehavior objCat;
    public int Index { get; set; }
    public CatBehavior(int ind)
    {
        Index = ind;
    }
    //Переопределяем оператор ++, чтобы номер анимационного состояния не выходил за пределы списка
    public static CatBehavior operator ++(CatBehavior obj)
    {
        obj.Index = obj.Index < 2 ? ++obj.Index : 0;
        return obj;
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        isMovingPossible = true;
        //Находим все цели котика и загоняем их в список
        Transform seif = GameObject.FindGameObjectWithTag("AmmoBox").GetComponent<Transform>();
        Transform TV = GameObject.FindGameObjectWithTag("TV").GetComponent<Transform>();
        Transform Door = GameObject.FindGameObjectWithTag("Finish").GetComponent<Transform>();
        targets = new List<Transform> { seif, Door, TV };
        Index = 0;
        objCat = new CatBehavior(Index);
        //Выбираем первую цель
        target = targets[objCat.Index];
        //Выпускаем котика погулять
        OnMove(target);
        
    }
    //Если котик достигает цели, он выбирает следующую цель из списка
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == target.tag)
        {
            StartCoroutine(ChooseDirection());
        }
    }

    //Можно было обойтись без корутина, но мне хотелось, чтобы котик "подумал" пару секунд во вемя выбора новой цели))
    IEnumerator ChooseDirection()
    {
        //Останавливаем котика
        GetComponent<NavMeshAgent>().isStopped = true;
        //..анимацию тоже ставим статичную
        anim.SetInteger("Start", SetAnim(Animations.Stay));
        //Даём котику время "подумать"
        yield return new WaitForSeconds(1.8f);
        //Разрешаем движение
        GetComponent<NavMeshAgent>().isStopped = false;
        //Выбираем следующую цель в списке
        objCat++;
        target = targets[objCat.Index];
        //Запускаем котика
        OnMove(target);
    }

    //Эта функция помогает выбирать подходящую анимацию котика (чтобы не использовать обезличенные цифры)
    int SetAnim(Animations animationVariant)
    {
        switch (animationVariant)
        {
            case Animations.Go:
                return 1;
            case Animations.Jump:
                return 2;
            case Animations.Eat:
                return 3;
        }
        return 0;
    }

    //Движение котика
    private void OnMove(Transform t)
    {
        if (isMovingPossible)
        {
            anim.SetInteger("Start", SetAnim(Animations.Go));
            GetComponent<NavMeshAgent>().destination = t.position;
        }
    }
}
