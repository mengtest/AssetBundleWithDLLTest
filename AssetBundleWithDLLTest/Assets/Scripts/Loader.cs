using System;
using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	// Create a symbolic link by first
	// ex) /Users/Shared/AssetResources/ -> /AssetBundleWithDLLTest/AssetBundleWithDLLTest/Assets/AssetBundles_Output/
	string BundleURL = "file:///Users/Shared/AssetResources/packingassets.unity3d";
	string AssetName = "NowPrintingCube";

	IEnumerator Start () {
		using (WWW www = new WWW (BundleURL)) {
			yield return www;
			if (www.error != null) {
				throw new Exception ("WWW download error:" + www.error);
			}
			AssetBundle bundle = www.assetBundle;
			if (AssetName == "") {
				Instantiate (bundle.mainAsset);
			} else {
				Instantiate (bundle.LoadAsset (AssetName));
			}
			bundle.Unload (false);
		}
	}
	
	void Update () {
	
	}
}
