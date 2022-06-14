#if ENABLE_BLENDER_FBX_IMPORT_SETTINGS

using UnityEditor;

/// <summary>
/// Custom import settings for FBX exported from Blender
/// </summary>
public class BlenderFBXImportSettings : AssetPostprocessor
{
    private void OnPreprocessModel()
    {
        ModelImporter importer = assetImporter as ModelImporter;
        importer.materialImportMode = ModelImporterMaterialImportMode.None;
        importer.bakeAxisConversion = true;
        importer.generateSecondaryUV = true;
    }
}

#endif