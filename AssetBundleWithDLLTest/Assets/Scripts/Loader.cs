using System;
using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

	// Create a symbolic link by first
	// ex) /Users/Shared/AssetResources/ -> /AssetBundleWithDLLTest/AssetBundleWithDLLTest/Assets/AssetBundles_Output/
	string BundleURL = "file:///Users/Shared/AssetResources/packingassets.unity3d";
	string AssetName = "NowPrintingCube";
	int version = 1;

	void Start() {
		StartCoroutine (DownloadAndCache ());
	}

	IEnumerator DownloadOnly () {
		Debug.Log ("DownloadOnly mode");
		using (WWW www = new WWW (BundleURL)) {
			yield return www;
			if (www.error != null) {
				throw new Exception ("WWW download error:" + www.error);
			}
			LoadAsset (www.assetBundle, AssetName);
			www.assetBundle.Unload (false);
		}
	}

	IEnumerator DownloadAndCache () {
		Debug.Log ("DownloadAndCache mode");
		while (!Caching.ready) {
			yield return null;
		}
		using (WWW www = WWW.LoadFromCacheOrDownload (BundleURL, version)) {
			yield return www;
			if (www.error != null) {
				throw new Exception ("WWW download error:" + www.error + " AssetVersion:" + version);
			}
			LoadAsset (www.assetBundle, AssetName);
			www.assetBundle.Unload (false);
		}
	}

	private void LoadAsset(AssetBundle bundle, string asset_name) {
		if (asset_name == "") {
			Instantiate (bundle.mainAsset);
		} else {
			Instantiate (bundle.LoadAsset (asset_name));
		}
	}
	
	void Update () {
	
	}
}
