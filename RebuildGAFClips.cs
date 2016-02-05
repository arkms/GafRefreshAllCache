using UnityEngine;
using System.Collections;
using GAF.Core;
using UnityEditor;
using System.IO;
using GAFEditorInternal.Assets;
using GAFInternal.Assets;
using GAF.Assets;

//This scripts work ty to Mark Fennell https://gafmedia.com/forum/viewtopic.php?f=5&t=427

public class RebuildGAFClips
{
    [MenuItem("Arj2D/GAF/Rebuild GAF Clips")]
    static void RebuildGAFClipsMenu()
    {
        //I Dont Think this is necesary... but this is the code for complete force the reset Cache folder

        // Delete the GAF/Resources/Cache folder
        //FileUtil.DeleteFileOrDirectory("Assets/GAF/Resources/Cache");
        // Create New Empty Folder
        //AssetDatabase.CreateFolder("Assets/GAF/Resources/", "Cache");

        // Find All Prefabs
        string fileExtension = ".asset";
        DirectoryInfo directory = new DirectoryInfo(Application.dataPath);
        FileInfo[] goFileInfo = directory.GetFiles("*" + fileExtension, SearchOption.AllDirectories);

        //Prepare vars
        int i = 0; int goFileInfoLength = goFileInfo.Length;
        FileInfo tempGoFileInfo; string tempFilePath;
        Object tempGO;

        for (; i < goFileInfoLength; i++)
        {
            EditorUtility.DisplayProgressBar("Rebuilding GAF Clip", "Working", (float)i / (float)goFileInfoLength);

            //is prefab?
            tempGoFileInfo = goFileInfo[i];
            if (tempGoFileInfo == null)
                continue;

            //get Prefab
            tempFilePath = tempGoFileInfo.FullName;
            tempFilePath = tempFilePath.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            tempGO = AssetDatabase.LoadAssetAtPath(tempFilePath, typeof(Object)) as Object;

            try
            {
                GAFAnimationAsset GafClip = (tempGO as GAFAnimationAsset);
   
                if (GafClip == null) continue;                

                GAFResourceManagerInternal.instance.createResources<GAFTexturesResourceInternal>(GafClip);
            }
            catch
            {
                continue;
            }
        }

        if (goFileInfoLength > 0)
            EditorUtility.ClearProgressBar();

        Debug.Log("Done!");
    }
}