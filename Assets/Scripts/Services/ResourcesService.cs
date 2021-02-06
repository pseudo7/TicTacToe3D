using System;
using System.Collections;
using TicTacToe3D.Shapes;
using UnityEngine;

namespace TicTacToe3D.Services
{
    public class ResourcesService : ServiceBase
    {
        #region Materials

        internal Material LightWoodMat { get; private set; }
        internal Material DarkWoodMat { get; private set; }
        internal Material LightWoodMatFaded { get; private set; }
        internal Material DarkWoodMatFaded { get; private set; }

        private const string MaterialsDir = "Materials";
        private const string LightWoodMaterialDir = "LightWoodMat";
        private const string DarkWoodMaterialDir = "DarkWoodMat";
        private const string LightWoodFadedMaterialDir = "LightWoodFadedMat";
        private const string DarkWoodFadedMaterialDir = "DarkWoodFadedMat";

        #endregion

        internal ShapeBase CrossPrefab { get; private set; }
        internal ShapeBase SpherePrefab { get; private set; }

        private const string PrefabsDir = "Prefabs";
        private const string CrossPrefabDir = "Cross Prefab";
        private const string SpherePrefabDir = "Sphere Prefab";


        protected override void RegisterService()
        {
            Bootstrap.BootstrapInstance.RegisterService(this);
            LoadResources();
        }

        private void LoadResources()
        {
            LoadMaterials();
            LoadPrefabs();
        }

        private void LoadMaterials()
        {
            if (LightWoodMat)
            {
                UnLoadResource(LoadMaterials);
                return;
            }

            LightWoodMat = Resources.Load<Material>($"{MaterialsDir}/{LightWoodMaterialDir}");
            DarkWoodMat = Resources.Load<Material>($"{MaterialsDir}/{DarkWoodMaterialDir}");
            LightWoodMatFaded = Resources.Load<Material>($"{MaterialsDir}/{LightWoodFadedMaterialDir}");
            DarkWoodMatFaded = Resources.Load<Material>($"{MaterialsDir}/{DarkWoodFadedMaterialDir}");
        }

        private void LoadPrefabs()
        {
            if (CrossPrefab)
            {
                UnLoadResource(LoadPrefabs);
                return;
            }

            CrossPrefab = Resources.Load<ShapeBase>($"{PrefabsDir}/{CrossPrefabDir}");
            SpherePrefab = Resources.Load<ShapeBase>($"{PrefabsDir}/{SpherePrefabDir}");
        }

        private void UnLoadResource(Action postUnloadAction = null) =>
            StartCoroutine(UnloadingRoutine(postUnloadAction));

        private static IEnumerator UnloadingRoutine(Action postUnloadAction)
        {
            Debug.Log("Unloading Unused Assets");
            yield return Resources.UnloadUnusedAssets();
            postUnloadAction?.Invoke();
            Debug.Log("Unloading Done");
        }
    }
}