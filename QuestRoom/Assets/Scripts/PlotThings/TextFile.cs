using UnityEngine;
using UnityEngine.UI;

//Скрипт описывает работу с текстовым файлом на сюжетном ноутуке и проверяет соответствие введённого текста ключевому слову
public class TextFile : MonoBehaviour
{
    [SerializeField] private GameObject gameController;
    [SerializeField] private Text answerField;
    [SerializeField] private Modes mode;
    //Отгадка, которую нужно ввести в текстовом файле
    private const string whoIAm = "Bandersnatch";
    private string answer = "";
    private string savedAnswer = "";

    //Функция, которая вводит символ нажатой кнопки в текстовый файл
    string AnswerInput(string letter)
    {
        //Если была нажата клавиша Enter (Return), то переходим на новую строку
        if (letter == "Enter")
        {
            answer += "\r\n";
            return answerField.text.Remove(answerField.text.Length - 1) + "\r\n|";
        }
        //При нажати клавиши Backspace удаляем последнюю букву
        else if (letter == "Backspace" && answer != "")
        {
            answer = answer.Remove(answer.Length - 1);
            return answerField.text.Remove(answerField.text.Length - 2) + "|";
        }
        else
        {
            answer += letter;
            return answerField.text.Remove(answerField.text.Length - 1) + letter + "|";
        }
    }

    void Update()
    {
        //Если режим ноутбука активирован И! если ноутбук включён, то...
        if (mode.isLaptopModeActive && GetComponent<TurnOnAndOpenLaptop>().isLaptotOn)
        {
            //..при нажати клавиши Backspace удаляем последнюю букву
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                answerField.text = AnswerInput("Backspace");
            }
            //..при нажати клавиши Enter (Return) переходим на новую строку
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                answerField.text = AnswerInput("Enter");
            }
            //..при нажати клавиши F5 сохраняем введённый текст (он отобразится при новом запуске текстового файла)
            else if (Input.GetKeyDown(KeyCode.F5))
            {
                savedAnswer = answer;
            }
            //..при нажати клавиши F12 проверяем правильность отгадки
            else if (Input.GetKeyDown(KeyCode.F12))
            {
                //..и если отгадка верна, то выходим из режима сбора предметов и режима ноутбука
                if (answer == whoIAm)
                {
                    //Запоминаем, что первое задание выполнено
                    mode.questTasks[0] = true;
                    mode.GasMode();
                    mode.LaptopMode();
                }

                //..а если нет, то сообщем игроку об этом
                else
                {
                    answerField.text = answerField.text.Remove(answerField.text.Length - 1 - answer.Length) + "WRONG ANSWER!|";
                    answer = "WRONG ANSWER!";
                }
            }
            //При нажатии на TAB компьютер выключается, а режим LaptopMode деактивируется
            else if (Input.GetKeyDown(KeyCode.Tab))
            {                
                answerField.text = answerField.text.Remove(answerField.text.Length - 1 - answer.Length) + savedAnswer + "|";
                answer = savedAnswer;
                mode.LaptopMode();             
            }
            //Ну и при нажатии любой другой клавиши её символ вводится в текстовый файл
            else if (Input.anyKeyDown && answer.Length < 32)
            {
                answerField.text = AnswerInput(Input.inputString);
            }
        }
    }
}
