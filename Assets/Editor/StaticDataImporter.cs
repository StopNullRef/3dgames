using ProjectW.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Project.Editor
{
    public static class StaticDataImporter
    {
        public static void Import(string[] importedAssets, string[] deletedAssets,
             string[] movedAssets, string[] movedFromAssetsPaths)
        {
            ImportNewOrModified(importedAssets);
            Delete(deletedAssets);
            Move(movedAssets, movedFromAssetsPaths);
        }


        private static void Move(string[] movedAssets, string[] movedFromAssets)
        {
            Delete(movedFromAssets);
            ImportNewOrModified(movedAssets);
        }

        private static void Delete(string[] deletedAssets)
        {
            ExcelToJson(deletedAssets, true);
        }

        private static void ImportNewOrModified(string[] importAssets)
        {
            ExcelToJson(importAssets, false);
        }

        /// <summary>
        /// ���������� �����Ͽ� json���� �������ִ� �Լ�
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="isDeleted"></param>
        private static void ExcelToJson(string[] assets, bool isDeleted)
        {
            // ���� ���ϵ��� ��θ� ��� ����Ʈ
            List<string> staticDataList = new List<string>();

            // �Ű������� ���� ������ �������ϸ����� �ɷ�����
            foreach (var asset in assets)
            {
                if (IsStaticData(asset, isDeleted))
                    staticDataList.Add(asset);
            }

            foreach (var staticData in staticDataList)
            {
                try
                {
                    // ���� �հ�θ� ������ ���� �̸��� Ȯ�����̸����� �����
                    var fileName = staticData.Substring(staticData.LastIndexOf('/') + 1);
                    // Ȯ���ڸ� ������ ���� ���� �����̸��� �����
                    fileName = fileName.Remove(fileName.LastIndexOf('.'));

                    var rootPath = Application.dataPath;
                    // ����� ���� /Assets �κ� �� �����
                    rootPath = rootPath.Remove(rootPath.LastIndexOf('/'));

                    var fullFileName = $"{rootPath}/Assets/StaticData/Excel/{fileName}.xlsx";

                    // ��ȯ�ϴ� ������ ��ü ����
                    var excelToJsonConverter = new ExcelToJsonConvert(fullFileName, $"{rootPath}/{StaticDataPath.SDJson}");

                    //C:/Users/Administrator/Desktop/3dgames/SystemMessage
                    // ��ȯ�� �����ߴ��� üũ
                    if (excelToJsonConverter.SaveJsonFiles() > 0)
                    {
                        AssetDatabase.ImportAsset(StaticDataPath.SDJson + $"{fileName}.json");
                        Debug.Log($"##### StaticData {fileName} reimported");
                    }

                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    Debug.LogErrorFormat("Couldn't convert assets = {0}", staticData);
                }
            }


        }

        /// <summary>
        /// �ش� ����� ������ StaticData���� üũ�ϴ� �Լ�
        /// </summary>
        /// <param name="path">�˻��� ���� ���</param>
        /// <param name="isDeleted">������ ��������</param>
        /// <returns></returns>
        private static bool IsStaticData(string path, bool isDeleted)
        {
            //���� Ȯ���ڰ� ���������� �ƴ϶�� false�� ����
            if (path.EndsWith(".xlsx") == false)
                return false;


            var absoultePath = Application.dataPath + path.Remove(0, "Assets".Length);


            return (isDeleted || File.Exists(absoultePath) && path.StartsWith(StaticDataPath.SDPath));
        }

    }

    public class StaticDataPath
    {
        public const string SDPath = "Assets/StaticData";
        public const string SDJson = "Assets/StaticData/Json";
        public const string SDExcel = "Assets/StaticData/Excel";
    }
}
