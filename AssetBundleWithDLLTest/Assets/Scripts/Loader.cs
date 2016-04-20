using System;
using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class Loader : MonoBehaviour {

	// Create a symbolic link by first
	// ex) /Users/Shared/AssetResources/ -> /AssetBundleWithDLLTest/AssetBundleWithDLLTest/Assets/AssetBundles_Output/
	string BundleURL = "file:///Users/Shared/AssetResources/packingassets.unity3d";
	string AssetName = "ROMtst.bytes";
	int version = 1;

	void Start() {
		StartCoroutine (DownloadOnly ());
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
		Regex rgx = new Regex ("^ROM.*bytes$");

		if (asset_name == "") {
			Instantiate (bundle.mainAsset);
		} else if (rgx.IsMatch(asset_name)) {
			// Load code from bytes file (DLL)
			TextAsset txt = bundle.LoadAsset (asset_name, typeof(TextAsset)) as TextAsset;
			System.Reflection.Assembly assembly = System.Reflection.Assembly.Load (txt.bytes);
			Type type = assembly.GetType ("Hoge");
			GameObject obj = new GameObject ();
			obj.AddComponent (type);
		} else {
			// Load other assets
			Instantiate (bundle.LoadAsset (asset_name));
		}
	}
	
	void Update () {
	
	}
}
