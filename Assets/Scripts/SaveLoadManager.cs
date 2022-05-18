using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Inventory _inventory;

    [SerializeField] private GameRuler _gameRuler;
    [SerializeField] private Daytime _worldTime;
    [SerializeField] private StoryManager _storyManager;

    [Header("Changeable environment")]
    [SerializeField] private GameObject _treeAdvPrefab;
    [SerializeField] private GameObject _logPrefab;

    [SerializeField] private List<GameObject> _dataPiston;

    private void Update()
    {
        //save
        if(Input.GetKeyUp(KeyCode.Keypad7))
        {
            Save();
        }

        //load
        if (Input.GetKeyUp(KeyCode.Keypad9))
        {
            StartCoroutine(LoadStateScene());
        }
    }

    public void Save()
    {
        Debug.LogWarning($"! SAVE: {_player.transform.position} & Inventory");
        BinarySavingSystem.SavePlayer(_player.transform);
        BinarySavingSystem.SaveInventory(_inventory.InventoryItems);

        BinarySavingSystem.SaveGameInformation(new GameInfoData(_worldTime.TimeProgress, (float)_gameRuler.CurrentHealth, _gameRuler.CurrentMadness, _storyManager.HistoryProgress));

        // время (0..1)
        // жизнь
        // психологическое здоровье
        // ID истории (загрузка: список со ссылками на тригеры, вызов тригеров)

        BinarySavingSystem.UniversalSave(FindObjectsWithTag("TreeAdv"), "TreeAdv");
        BinarySavingSystem.UniversalSave(FindObjectsWithTag("Log"), "Log");

    }

    public void Load()
    {
        #region Player

        var playerData = BinarySavingSystem.LoadPlayer();

        if (playerData != null)
        {
            Debug.LogWarning($"! LOAD: {playerData.Position[0]} {playerData.Position[1]} {playerData.Position[2]} | {playerData.Rotation[0]} {playerData.Rotation[1]} {playerData.Rotation[2]} {playerData.Rotation[3]}");

            if(_player.TryGetComponent<TPPlayerMovement>(out TPPlayerMovement moveController))
            {
                moveController.enabled = false;

                _player.transform.position = new Vector3(playerData.Position[0], playerData.Position[1], playerData.Position[2]);
                _player.transform.rotation = new Quaternion(playerData.Rotation[0], playerData.Rotation[1], playerData.Rotation[2], playerData.Rotation[3]);

                moveController.enabled = true;
            }
            else
            {
                Debug.LogError("TPPlayerMovement does not exist!");
            }
        }
        else
        {
            Debug.LogError("Save data is null!");
        }

        #endregion

        #region Inventory

        var inventoryData = BinarySavingSystem.LoadInventory();

        if(inventoryData != null)
        {
            foreach (var value in inventoryData)
            {
                foreach (var inventoryItems in _inventory.InventoryItems)
                {
                    if (value.UID == inventoryItems.UID)
                    {
                        inventoryItems.Amount = value.Amount;
                        inventoryItems.UIItem.SetAmount(value.Amount);
                    }
                }
            }
        }

        #endregion

        #region UniversalZone
        UniversalLoadObject(_treeAdvPrefab, BinarySavingSystem.UniversalLoad("TreeAdv"), "TreeAdv");
        UniversalLoadObject(_logPrefab, BinarySavingSystem.UniversalLoad("Log"), "Log");

        #endregion

        #region GameInformation
        var gameInfoData = BinarySavingSystem.LoadGameInformation();

        if (gameInfoData != null)
        {
            Debug.Log($"Load GameInfo (hp, mdness, time, story id): {gameInfoData.Health},  {gameInfoData.Madness},  {gameInfoData.Time},  {gameInfoData.Story_ID}");

            _gameRuler.CurrentHealth = (int)gameInfoData.Health;
            _gameRuler.CurrentMadness = gameInfoData.Madness;
            _worldTime.TimeProgress = gameInfoData.Time;

            var piston = _dataPiston[gameInfoData.Story_ID];

            if (piston == null)
            {
                Debug.LogWarning($"GameInfoData is null | Story ID = {gameInfoData.Story_ID}");
                //return;
            }


            if(piston.TryGetComponent<ExternalDataPiston>(out ExternalDataPiston externalDataPiston))
            {
                externalDataPiston.Event_Complete.Invoke();
            }
            else if(piston.TryGetComponent<AccumulativeInfoPiston>(out AccumulativeInfoPiston accumulativeInfoPiston))
            {
                accumulativeInfoPiston.Event_Complete.Invoke();
            }
            else if(piston.TryGetComponent<ExternalDataPistonWithButton>(out ExternalDataPistonWithButton externalDataPistonWithButton))
            {
                externalDataPistonWithButton.Event_Complete.Invoke();
            }

            
        }
        else
        {
            Debug.LogError("Save data is null!");
        }

        #endregion


    }

    private void UniversalLoadObject(GameObject prefab, PositionRotationData[] positionAndRotationData, string TagFromDelete)
    {
        foreach (var item in GameObject.FindGameObjectsWithTag(TagFromDelete))
        {
            Destroy(item);
        }

        if (positionAndRotationData != null)
        {
            foreach (var item in positionAndRotationData)
            {
                Instantiate(prefab, new Vector3(item.Position[0], item.Position[1], item.Position[2]), new Quaternion(item.Rotation[0], item.Rotation[1], item.Rotation[2], item.Rotation[3]));
            }
        }
        else
        {
            Debug.LogError($"Error loading prefab {prefab.name}");
        }
    }

    private List<Transform> FindObjectsWithTag(string tag)
    {
        List<Transform> transforms = new List<Transform>();

        foreach (var item in GameObject.FindGameObjectsWithTag(tag))
        {
            transforms.Add(item.transform);
        }

        return transforms;
    }

    private IEnumerator LoadStateScene()
    {
        var waitFading = true;
        Fader.instance.FadeIn(() => waitFading = false);

        while (waitFading)
            yield return null;



        Load();



        waitFading = true;
        Fader.instance.FadeOut(() => waitFading = false);

        while (waitFading)
            yield return null;
    }

}

[System.Serializable]
public class PositionRotationData
{
    public float[] Position = new float[3];
    public float[] Rotation = new float[4];

    public PositionRotationData(Transform value)
    {
        Position[0] = value.position.x;
        Position[1] = value.position.y;
        Position[2] = value.position.z;

        Rotation[0] = value.rotation.x;
        Rotation[1] = value.rotation.y;
        Rotation[2] = value.rotation.z;
        Rotation[3] = value.rotation.w;
    }
}

[System.Serializable]
public class GameInfoData
{
    public float Time;
    public float Health;
    public float Madness;
    public int Story_ID;

    public GameInfoData(float time, float health, float madness, int story_ID)
    {
        Time = time;
        Health = health;
        Madness = madness;
        Story_ID = story_ID;
    }
}

public static class BinarySavingSystem
{
    public static void SavePlayer(Transform value)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.b";
        FileStream stream = new FileStream(path, FileMode.Create);

        PositionRotationData data = new PositionRotationData(value);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PositionRotationData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PositionRotationData data = formatter.Deserialize(stream) as PositionRotationData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }


    public static void SaveInventory(List<InventoryItem> values)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/inventory.b";
        FileStream stream = new FileStream(path, FileMode.Create);

        InventoryItem[] data = values.ToArray();

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static InventoryItem[] LoadInventory()
    {
        string path = Application.persistentDataPath + "/inventory.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            InventoryItem[] data = formatter.Deserialize(stream) as InventoryItem[];
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void UniversalSave(List<Transform> values, string filename)
    {
        if(values != null)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + $"/{filename}.b";
            FileStream stream = new FileStream(path, FileMode.Create);

            //PositionRotationData[] data = new PositionRotationData[values.Count];


            List<PositionRotationData> data = new List<PositionRotationData>();

            foreach (var item in values)
            {
                data.Add(new PositionRotationData(item));
            }


            //for (int i = 0; i < values.Count; i++)
            //{
            //    data[i].Position[0] = values[i].position.x;
            //    data[i].Position[1] = values[i].position.y;
            //    data[i].Position[2] = values[i].position.z;

            //    data[i].Rotation[0] = values[i].rotation.x;
            //    data[i].Rotation[1] = values[i].rotation.y;
            //    data[i].Rotation[2] = values[i].rotation.z;
            //    data[i].Rotation[3] = values[i].rotation.w;
            //}

            formatter.Serialize(stream, data.ToArray());
            stream.Close();
        }
        else
        {
            Debug.LogError($"Values is null. [{filename}]");
        }


        
    }

    public static PositionRotationData[] UniversalLoad(string filename)
    {
        string path = Application.persistentDataPath + $"/{filename}.b";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PositionRotationData[] data = formatter.Deserialize(stream) as PositionRotationData[];
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveGameInformation(GameInfoData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/gameInformation.b";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameInfoData LoadGameInformation()
    {
        string path = Application.persistentDataPath + "/gameInformation.b";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameInfoData data = formatter.Deserialize(stream) as GameInfoData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

}
