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
        /// 엑셀파일을 감지하여 json파일 생성해주는 함수
        /// </summary>
        /// <param name="assets"></param>
        /// <param name="isDeleted"></param>
        private static void ExcelToJson(string[] assets, bool isDeleted)
        {
            // 엑셀 파일들의 경로를 담는 리스트
            List<string> staticDataList = new List<string>();

            // 매개변수로 받은 에셋을 엑셀파일만으로 걸러낸다
            foreach (var asset in assets)
            {
                if (IsStaticData(asset, isDeleted))
                    staticDataList.Add(asset);
            }

            foreach (var staticData in staticDataList)
            {
                try
                {
                    // 파일 앞경로를 날려서 파일 이름과 확장자이름까지 남긴다
                    var fileName = staticData.Substring(staticData.LastIndexOf('/') + 1);
                    // 확장자명 까지도 지워 순수 파일이름만 남긴다
                    fileName = fileName.Remove(fileName.LastIndexOf('.'));

                    var rootPath = Application.dataPath;
                    // 상대경로 끝에 /Assets 부분 만 지운다
                    rootPath = rootPath.Remove(rootPath.LastIndexOf('/'));

                    var fullFileName = $"{rootPath}/Assets/StaticData/Excel/{fileName}.xlsx";

                    // 변환하는 컨버터 객체 생성
                    var excelToJsonConverter = new ExcelToJsonConvert(fullFileName, $"{rootPath}/{StaticDataPath.SDJson}");

                    //C:/Users/Administrator/Desktop/3dgames/SystemMessage
                    // 변환에 성공했는지 체크
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
        /// 해당 경로의 파일이 StaticData인지 체크하는 함수
        /// </summary>
        /// <param name="path">검사할 파일 경로</param>
        /// <param name="isDeleted">삭제할 파일인지</param>
        /// <returns></returns>
        private static bool IsStaticData(string path, bool isDeleted)
        {
            //파일 확장자가 엑셀파일이 아니라면 false를 리턴
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
