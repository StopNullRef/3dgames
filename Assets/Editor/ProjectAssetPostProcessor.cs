using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace Project.Editor
{
    public class ProjectAssetPostProcessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetsPaths)
        {
            StaticDataImporter.Import(importedAssets, deletedAssets, movedAssets, movedFromAssetsPaths);
        }
    }
}
