﻿using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
    using System.IO;
#endif

namespace LevelSelector
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 3)]
    public class LevelData : ScriptableObject
    {

        public int level;
        [Header("Dominoes")]
        [Range(1, 5)] public int tokensInLevelMultiplier =1;
        [HideInInspector] public int dominoesAmount;
        [SerializeField] public TokenData[] tokenDatas;

        //[Header("LevelGoals")]
        //[Range(1, 400)] public int time = 200;
        //[Range(1, 200)] public int goal = 5;
        //[Range(1, 10)] public int streak = 3;
        //public float streakWaitTime = 15f;
        //public static UnityAction assetsChanged;

#if UNITY_EDITOR

        public static string tokenDatasPath =  Path.Combine("Assets", "Terraforming", "TriDominoTiles", "Scripts", "Data", "DominoToken", "Level");
        public static string levelDatasPath = Path.Combine("Assets", "Terraforming","LevelSelector","Scripts","LevelData");

        public TokenData[] GetTokenDatas()
        {
            tokenDatas = DuplicateArray(LoadAssets<TokenData>(tokenDatasPath+level), tokensInLevelMultiplier);
            dominoesAmount = tokenDatas.Length;
            return tokenDatas;
        }

        //Method to duplicate every of the elements of an array N times
        public T[] DuplicateArray<T>(T[] array, int times)
        {
            List<T> duplicatedArray = new List<T>();

            for (int i = 0; i < times; i++)
            {
                duplicatedArray.AddRange(array);
            }

            return duplicatedArray.ToArray();
        }

        [MenuItem("DevTools/UpdateAllLevelsData",false, 10)]
        public static void UpdateAllLevelsData()
        {

            LevelData[] levelDatas = LoadAssets<LevelData>(levelDatasPath);

            foreach (LevelData levelData in levelDatas)
            {
                levelData.GetTokenDatas();
                EditorUtility.SetDirty(levelData);
            }
        }

        private static T[] LoadAssets<T>(string folderPath) where T : Object
        {
            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folderPath });
            List<T> loadedAssets = new List<T>();

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

                if (asset != null)
                {
                    loadedAssets.Add(asset);
                    //Debug.Log($"Loaded asset of type {typeof(T).Name}: {asset.name}");
                }
            }
            return loadedAssets.ToArray();
        }
#endif
    }
}