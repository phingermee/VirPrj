using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad : MonoBehaviour
{
    public FPSInput playerPos;

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        GameData game = new GameData();
        game.posSaveX = playerPos.transform.position.x;
        game.posSaveY = playerPos.transform.position.y;
        game.posSaveZ = playerPos.transform.position.z;
        FileStream file = File.Create(Application.persistentDataPath + "/saves.qr");
        bf.Serialize(file, game);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/saves.qr"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saves.qr", FileMode.Open);
            GameData game = new GameData();
            game = (GameData)bf.Deserialize(file);
            print(game.posSaveX);
            playerPos.SetPosition(new Vector3(game.posSaveX, game.posSaveY, game.posSaveZ));
            file.Close();
        }
    }

    private void Start()
    {
        if (LoaderCheck.isGameLoad)
        {
            Load();
        }
    }

    public void Reset()
    {
        if (File.Exists(Application.persistentDataPath + "/saves.qr"))
        {
            File.Delete(Application.persistentDataPath + "/saves.qr");
        }
    }
}

[System.Serializable]
public class GameData
{
    public float posSaveX, posSaveY, posSaveZ;
}