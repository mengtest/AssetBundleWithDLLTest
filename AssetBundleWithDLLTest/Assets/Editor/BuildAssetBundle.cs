using UnityEditor;
using System.Collections;

public class BuildAssetBundle {
	[MenuItem("Assets/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles_Output");
	}
}
