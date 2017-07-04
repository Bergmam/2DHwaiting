using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoad{

    public static void Save(List<Move> movesToSave)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedMoves.mvs");
        MonoBehaviour.print("Saving moves to to: " + Application.persistentDataPath + "/savedMoves.mvs");
        bf.Serialize(file, movesToSave);
        file.Close();
    }

    public static void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/savedMoves.mvs"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            MonoBehaviour.print("Loading moves from: " + Application.persistentDataPath + "/savedMoves.mvs");
            FileStream file = File.Open(Application.persistentDataPath + "/savedMoves.mvs", FileMode.Open);
            AvailableMoves.SetMoves((List<Move>)bf.Deserialize(file));
            file.Close();
        }
    }
}
