// filename BuildPostProcessor.cs
// put it in a folder Assets/Editor/
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class BuildPostProcessor
{
	[PostProcessBuild]
	public static void ChangeXcodePlist(BuildTarget buildTarget, string path)
	{
		if (buildTarget == BuildTarget.iOS)
		{
			string plistPath	= path + "/Info.plist";
			PlistDocument plist = new PlistDocument();
			plist.ReadFromFile(plistPath);

			PlistElementDict rootDict = plist.root;

			Debug.Log(">> Automation, plist ... <<");

			// example of changing a value:
			// rootDict.SetString("CFBundleVersion", "6.6.6");

			// example of adding a boolean key...
			// < key > ITSAppUsesNonExemptEncryption </ key > < false />
			//rootDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
			rootDict.SetString("NSPhotoLibraryUsageDescription", "${PRODUCT_NAME} photo use");


			File.WriteAllText(plistPath, plist.WriteToString());
		}
	}

	[PostProcessBuildAttribute(1)]
	public static void OnPostProcessBuild(BuildTarget target, string path)
	{

		if (target == BuildTarget.iOS)
		{

			PBXProject project	= new PBXProject();
			string sPath		= PBXProject.GetPBXProjectPath(path);
			project.ReadFromFile(sPath);

			string g = project.GetUnityFrameworkTargetGuid();

			ModifyFrameworksSettings(project, g);

			// modify frameworks and settings as desired
			File.WriteAllText(sPath, project.WriteToString());
		}
	}

	static void ModifyFrameworksSettings(PBXProject project, string g)
	{

		// add hella frameworks

		Debug.Log(">> Automation, Frameworks... <<");

		project.AddFrameworkToProject(g, "blah.framework", false);
		project.AddFrameworkToProject(g, "libz.tbd", false);

		// go insane with build settings

		Debug.Log(">> Automation, Settings... <<");

		project.AddBuildProperty(g,
			"LIBRARY_SEARCH_PATHS",
			"../blahblah/lib");

		project.AddBuildProperty(g,
			"OTHER_LDFLAGS",
			"-lsblah -lbz2");

		// note that, due to some Apple shoddyness, you usually need to turn this off
		// to allow the project to ARCHIVE correctly (ie, when sending to testflight):
		project.AddBuildProperty(g,
			"ENABLE_BITCODE",
			"false");
	}

}