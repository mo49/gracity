using UnityEngine;
using System.Collections;

public static class TagUtility {

	public static string getParentTagName(string name) {
		int pos = name.IndexOf("/");

		if (0 < pos) {
			return name.Substring(0, pos);
		} else {
			return name;
		}
	}

	public static string getParentTagName(GameObject gameObject) {
		string name = gameObject.tag;
		int pos = name.IndexOf("/");

		if (0 < pos) {
			return name.Substring(0, pos);
		} else {
			return name;
		}
	}

	public static string getChildTagName(string name) {
		int pos = name.IndexOf("/");

		if (0 < pos) {
			return name.Substring(pos + 1);
		} else {
			return name;
		}
	}

	public static string getChildTagName(GameObject gameObject) {
		string name = gameObject.tag;
		int pos = name.IndexOf("/");

		if (0 < pos) {
			return name.Substring(pos + 1);
		} else {
			return name;
		}
	}
}