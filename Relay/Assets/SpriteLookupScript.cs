using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookupScript : MonoBehaviour {

	private Object[] spriteNames;
	private Dictionary<string, Sprite[]> lookup;
	// Use this for initialization
	void Awake () {
		spriteNames = Resources.LoadAll<Texture2D>("Sprites/");
		lookup = new Dictionary<string, Sprite[]>();
		foreach(Object t in spriteNames) {
			string s = t.ToString().Split('(')[0].Trim();
			lookup.Add(s, Resources.LoadAll<Sprite>("Sprites/" + s));
		}
	}

	public Sprite GetSprite(string name, int index) {
		if (lookup.ContainsKey(name)){
			return lookup[name][index];
		}
		return null;
	}

	public Sprite[] GetSpriteSheet(string name) {
		if (lookup.ContainsKey(name)){
			return lookup[name];
		}
		return null;
	}
}
