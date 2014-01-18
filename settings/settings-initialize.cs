/*	Reads saved settings data from file and applies to client
 *	If file doesn't exist (or data is corrupt), enables default values
 *
 *	To-Do:
 *	- Make an option default resolution auto-detect
 *	- Make volume options
 *	- Make better exception handling
 */

using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

[System.Serializable]
public class DefaultSettings {
	public enum TextureQuality { low, medium, high }
	public enum ShadowQuality { low, medium, high }
	public enum AntiAliasing { disabled, x2, x4, x8 }

	public int screenResolutionWidth = 800;
	public int screenResolutionHeight = 600;
	public bool isFullscreen = false;
	public TextureQuality textureQuality = TextureQuality.high;
	public ShadowQuality shadowQuality = ShadowQuality.high;
	public AntiAliasing antiAliasing = AntiAliasing.x2;
	public bool anisotropicFiltering = true;
	public bool verticalSync = false;
	public float musicVolume = 75f;
	public float gameVolume = 75f;
}

[System.Serializable]
public class LoadedSettings {
	public enum TextureQuality { low, medium, high }
	public enum ShadowQuality { low, medium, high }
	public enum AntiAliasing { disabled, x2, x4, x8 }

	public int screenResolutionWidth = 800;
	public int screenResolutionHeight = 600;
	public bool isFullscreen = false;
	public TextureQuality textureQuality = TextureQuality.high;
	public ShadowQuality shadowQuality = ShadowQuality.high;
	public AntiAliasing antiAliasing = AntiAliasing.x2;
	public bool anisotropicFiltering = true;
	public bool verticalSync = false;
	public float musicVolume = 75f;
	public float gameVolume = 75f;
}

public class SettingsInit : MonoBehaviour {

	public string settingsFilename = "settings.ini";
	public DefaultSettings defaultSettings;
	public LoadedSettings loadedSettings;

	private int failedBuildCount = 0;

	public void Start() {

		// Don't play with files in editor
		if (!Application.isEditor) {
			if (File.Exists(settingsFilename)) {
				if (!LoadSettings()) {
					Rebuild();
				}

				if (!ApplySettings()) {
					Rebuild();
				}
			} else {
				Rebuild();
			}
		} else {
			print("Game starting from editor. Skipping settings initialization.");
		}
	}

	/*	Applies settings from settings file */
	public bool ApplySettings() {
		try {
			// Set resolution and screen mode
			Screen.SetResolution(loadedSettings.screenResolutionWidth, loadedSettings.screenResolutionHeight, loadedSettings.isFullscreen);
			
			// Set texture quality
			switch (loadedSettings.textureQuality) {
				case LoadedSettings.TextureQuality.low:
					QualitySettings.masterTextureLimit = 2;
					break;
				case LoadedSettings.TextureQuality.medium:
					QualitySettings.masterTextureLimit = 1;
					break;
				case LoadedSettings.TextureQuality.high:
					QualitySettings.masterTextureLimit = 0;
					break;
				default:
					break;
			}
			
			// Set shadows quality
			switch (loadedSettings.shadowQuality) {
				case LoadedSettings.ShadowQuality.low:
					QualitySettings.shadowDistance = 20;
					QualitySettings.shadowCascades = 0;
					break;
				case LoadedSettings.ShadowQuality.medium:
					QualitySettings.shadowDistance = 70;
					QualitySettings.shadowCascades = 2;
					break;
				case LoadedSettings.ShadowQuality.high:
					QualitySettings.shadowDistance = 150;
					QualitySettings.shadowCascades = 4;
					break;
				default:
					break;
			}

			// Set anti aliasing
			switch (loadedSettings.antiAliasing) {
				case LoadedSettings.AntiAliasing.disabled:
					QualitySettings.antiAliasing = 0;
					break;
				case LoadedSettings.AntiAliasing.x2:
					QualitySettings.antiAliasing = 2;
					break;
				case LoadedSettings.AntiAliasing.x4:
					QualitySettings.antiAliasing = 4;
					break;
				case LoadedSettings.AntiAliasing.x8:
					QualitySettings.antiAliasing = 8;
					break;
				default:
					break;
			}

			// Set anisotropic filtering
			if (loadedSettings.anisotropicFiltering) {
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
			} else {
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
			}

			// Set vertical sync
			if (loadedSettings.verticalSync) {
				QualitySettings.vSyncCount = 1;
			} else {
				QualitySettings.vSyncCount = 0;
			}


			return true;
		} catch (System.Exception e) {
			print("[Exception]: "+e.Message);
			return false;
		}
	}

	/*	Loads settings from file and sets them to LoadedSettings class variables */
	public bool LoadSettings() {
		try {
			string line;
			string line_variable;
			string line_value;
			StreamReader fileReader = new StreamReader(settingsFilename, Encoding.Default);

			// Loop through settings file line by line
			using (fileReader) {
				do {
					line = fileReader.ReadLine();

					if (line != null) {
						line_variable = line.Split('=')[0];
						line_value = line.Split('=')[1];
						
						// Update LoadedSettings variables by values found in file
						switch (line_variable) {
							case "screenResolutionWidth":
								loadedSettings.screenResolutionWidth = int.Parse(line_value);
								break;
							case "screenResolutionHeight":
								loadedSettings.screenResolutionHeight = int.Parse(line_value);
								break;
							case "isFullscreen":
								loadedSettings.isFullscreen = bool.Parse(line_value);
								break;
							case "textureQuality":
								switch (line_value) {
									case "low":
										loadedSettings.textureQuality = LoadedSettings.TextureQuality.low;
										break;
									case "medium":
										loadedSettings.textureQuality = LoadedSettings.TextureQuality.medium;
										break;
									case "high":
										loadedSettings.textureQuality = LoadedSettings.TextureQuality.high;
										break;
									default:
										break;
								}
								break;
							case "shadowQuality":
								switch (line_value) {
									case "low":
										loadedSettings.shadowQuality = LoadedSettings.ShadowQuality.low;
										break;
									case "medium":
										loadedSettings.shadowQuality = LoadedSettings.ShadowQuality.medium;
										break;
									case "high":
										loadedSettings.shadowQuality = LoadedSettings.ShadowQuality.high;
										break;
									default:
										break;
								}
								break;
							case "antiAliasing":
								switch (line_value) {
									case "disabled":
										loadedSettings.antiAliasing = LoadedSettings.AntiAliasing.disabled;
										break;
									case "x2":
										loadedSettings.antiAliasing = LoadedSettings.AntiAliasing.x2;
										break;
									case "x4":
										loadedSettings.antiAliasing = LoadedSettings.AntiAliasing.x4;
										break;
									case "x8":
										loadedSettings.antiAliasing = LoadedSettings.AntiAliasing.x8;
										break;
									default:
										break;
								}
								break;
							case "anisotropicFiltering":
								loadedSettings.anisotropicFiltering = bool.Parse(line_value);
								break;
							case "verticalSync":
								loadedSettings.verticalSync = bool.Parse(line_value);
								break;
							case "musicVolume":
								loadedSettings.musicVolume = float.Parse(line_value);
								break;
							case "gameVolume":
								loadedSettings.gameVolume = float.Parse(line_value);
								break;
							default:
								break;
						}
					}
				} while (line != null);

				fileReader.Close();
			}

			return true;
		} catch (System.Exception e) {
			print("[Exception]: "+e.Message);
			return false;
		}
	}

	/* 	Re-creates settings file with default values and applies it */
	public bool ResetToDefaults() {
		try {
			// Clearing old file
			System.IO.File.WriteAllText(settingsFilename, "");

			// Writing default values
			System.IO.File.AppendAllText(settingsFilename, "screenResolutionWidth="+defaultSettings.screenResolutionWidth+"\r\n");
			System.IO.File.AppendAllText(settingsFilename, "screenResolutionHeight="+defaultSettings.screenResolutionHeight+"\r\n");
			System.IO.File.AppendAllText(settingsFilename, "isFullscreen="+defaultSettings.isFullscreen+"\r\n");
			System.IO.File.AppendAllText(settingsFilename, "textureQuality="+defaultSettings.textureQuality+"\r\n");
			System.IO.File.AppendAllText(settingsFilename, "shadowQuality="+defaultSettings.shadowQuality+"\r\n");
			System.IO.File.AppendAllText(settingsFilename, "antiAliasing="+defaultSettings.antiAliasing+"\r\n");
			System.IO.File.AppendAllText(settingsFilename, "anisotropicFiltering="+defaultSettings.anisotropicFiltering+"\r\n");
			System.IO.File.AppendAllText(settingsFilename, "verticalSync="+defaultSettings.verticalSync+"\r\n");
			System.IO.File.AppendAllText(settingsFilename, "musicVolume="+defaultSettings.musicVolume+"\r\n");
			System.IO.File.AppendAllText(settingsFilename, "gameVolume="+defaultSettings.gameVolume+"\r\n");
			
			return true;
		} catch (System.Exception e) {
			print("[Exception]: "+e.Message);
			return false;
		}
	}

	/* Rebuilds settings file, writes defaults, applies settings
	 *	@params: none
	 */
	public void Rebuild() {
		failedBuildCount++;

		if (failedBuildCount > 3) {
			print("[FATAL] Cannot initialize settings.");
			Application.Quit();
		} else {
			
			if (!ResetToDefaults()) {
				Rebuild();
			}

			if (!LoadSettings()) {
				Rebuild();
			}

			if(!ApplySettings()) {
				Rebuild();
			}
		}

	}
}
