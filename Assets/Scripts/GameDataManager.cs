using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameDataManager : MonoBehaviour
    {
        public static GameDataManager Instance { get; private set; }

        public GameData GameData;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadGameData();
        }

        public void SaveGameData()
        {
            var json = JsonUtility.ToJson(GameData);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }

        public void LoadGameData()
        {
            var path = Application.persistentDataPath + "/savefile.json";
            if (!File.Exists(path)) return;
            var json = File.ReadAllText(path);
            GameData = JsonUtility.FromJson<GameData>(json);
        }
    }
}