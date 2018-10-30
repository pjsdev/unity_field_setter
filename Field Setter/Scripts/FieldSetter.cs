using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using OikosTools;

public class FieldSetter : MonoBehaviour 
{
    public GameObject target;
    public ActionValue actionValue;
    public Component selectedComponent;
    public string selectedProperty = "";
    public string selectedSubProperty = "";

    public void UpdateValue(Vector2 value) 
    {
        PropertyChange.SetValue(selectedComponent, selectedProperty, value, selectedSubProperty);
    }

    public void UpdateValue(Vector3 value) 
    {
        PropertyChange.SetValue(selectedComponent, selectedProperty, value, selectedSubProperty);
    }

    public void UpdateValue(Color value) 
    {
        PropertyChange.SetValue(selectedComponent, selectedProperty, value, selectedSubProperty);
    }

    public void UpdateValue(float value) 
    {
        PropertyChange.SetValue(selectedComponent, selectedProperty, value, selectedSubProperty);
    }

    public void UpdateValue(int value) 
    {
        PropertyChange.SetValue(selectedComponent, selectedProperty, value, selectedSubProperty);
    }

    public void UpdateValue(string value) 
    {
        PropertyChange.SetValue(selectedComponent, selectedProperty, value, selectedSubProperty);
    }

    public void UpdateValue(object value) 
    {
        PropertyChange.SetValue(selectedComponent, selectedProperty, value, selectedSubProperty);
    }

    public void UpdateValueToDefault()
    {
        object value = PropertyChange.GetValue(selectedComponent, selectedProperty, selectedSubProperty);

        if (value.GetType() == typeof(float))   PropertyChange.SetValue(selectedComponent, selectedProperty, actionValue.GetFloat(), selectedSubProperty);
        if (value.GetType() == typeof(int))     PropertyChange.SetValue(selectedComponent, selectedProperty, actionValue.GetInt(), selectedSubProperty);
        if (value.GetType() == typeof(Color))   PropertyChange.SetValue(selectedComponent, selectedProperty, actionValue.GetColor(), selectedSubProperty);
        if (value.GetType() == typeof(Vector2)) PropertyChange.SetValue(selectedComponent, selectedProperty, actionValue.GetVector2(), selectedSubProperty);
        if (value.GetType() == typeof(Vector3)) PropertyChange.SetValue(selectedComponent, selectedProperty, actionValue.GetVector3(), selectedSubProperty);
        if (value.GetType() == typeof(string))  PropertyChange.SetValue(selectedComponent, selectedProperty, actionValue.GetString(), selectedSubProperty);
    }

    void Reset()
    {
        target = gameObject;
    }

    // imported from: https://github.com/fernandoramallo/oikospiel-tools
    [System.Serializable]
	public class ActionValue {
		[SerializeField()]
		private float v_float = 0;
		[SerializeField()]
		private int v_int = 0;
		[SerializeField()]
		private string v_string = "";
		[SerializeField()]
		private Color v_color = Color.white;//{ new colorKeys[2]{Color.black,Color.white} };
		[SerializeField()]
		private Gradient v_gradient = new Gradient();//{ new colorKeys[2]{Color.black,Color.white} };
		[SerializeField()]
		private Vector2 v_v2 = Vector2.zero;
		[SerializeField()]
		private Vector3 v_v3 = Vector3.zero;
		[SerializeField()]
		private bool v_bool = false;

		public object Get(System.Type type) {
			object o = null;
			switch(type.ToString()) {
			case "System.Single":
				o = v_float;
			break;
			case "System.Int32":
				o = v_int;
				break;
			case "System.String":
				o = v_string;
				break;
			case "UnityEngine.Color":
				o = v_color;
				break;
			case "UnityEngine.Gradient":
				o = v_gradient;
				break;
			case "UnityEngine.Vector2":
				o = v_v2;
				break;
			case "UnityEngine.Vector3":
				o = v_v3;
				break;
			case "System.Boolean":
				o = v_bool;
				break;
			default:
				Debug.LogWarning("Type " + type.ToString() + " is unsupported");
				break;
			}

			return o;
		}

		public void Set(object Value) {
			if (Value == null)
				return;
			try { 
				switch(Value.GetType().ToString()) {
				case "System.Single":
					v_float = (float)Value;
					break;
				case "System.Int32":
					v_int = (int)Value;
					break;
				case "System.String":
					v_string = (string)Value;
					break;
				case "UnityEngine.Color":
					v_color = (Color)Value;
					break;
				case "UnityEngine.Gradient":
					v_gradient = (Gradient)Value;
					break;
				case "UnityEngine.Vector2":
					v_v2 = (Vector2)Value;
					break;
				case "UnityEngine.Vector3":
					v_v3 = (Vector3)Value;
					break;
				case "System.Boolean":
					v_bool = (bool)Value;
					break;
				}
			} catch (System.Exception e) {
				Debug.Log("Error setting a value of type " + Value.GetType() + ", " + e.ToString());
			}
		}

		public float GetFloat() { return v_float; }
		public int GetInt() { return v_int; }
		public string GetString() { return v_string; }
		public Color GetColor() { return v_color; }
		public Gradient GetGradient() { return v_gradient; }
		public Vector2 GetVector2() { return v_v2; }
		public Vector3 GetVector3() { return v_v3; }
		public bool GetBool() { return v_bool; }

		public static object Lerp(System.Type type, ActionValue av1, ActionValue av2, float lerp) {
			if (av1 == null || av2 == null)
				return null;
			//lerp = Mathf.Clamp01(lerp);
			if (type == typeof(float)) {
				return Tools.GetMinMaxValue(av1.GetFloat(), av2.GetFloat(), lerp);
			} else if (type == typeof(int)) {
				return Mathf.RoundToInt(Tools.GetMinMaxValue(av1.GetInt(), av2.GetInt(), lerp));
			} else if (type == typeof(Color)) {
				return Color.Lerp(av1.GetColor(), av2.GetColor(),lerp);
			} else if (type == typeof(Vector2)) {
				return Vector2.Lerp(av1.GetVector2(), av2.GetVector2(), lerp);
			} else if (type == typeof(Vector3)) {
				return Vector3.Lerp(av1.GetVector3(), av2.GetVector3(), lerp);
			} else if (type == typeof(bool)) {
				return lerp < 0.5f ? av1.GetBool() : av2.GetBool();
			}
			return null;
		}

		public static object Lerp(object from, object to, float lerp) {
			if (from == null || to == null)
				return null;
			lerp = Mathf.Clamp01(lerp);
			System.Type type = from.GetType();
			if (type == typeof(float)) {
				return Tools.GetMinMaxValue((float)from, (float)to, lerp);
			} else if (type == typeof(int)) {
				return Mathf.RoundToInt(Tools.GetMinMaxValue((int)from, (int)to, lerp));
			} else if (type == typeof(Color)) {
				return Color.Lerp((Color)from, (Color)to,lerp);
			} else if (type == typeof(Vector2)) {
				return Vector2.Lerp((Vector2)from, (Vector2)to, lerp);
			} else if (type == typeof(Vector3)) {
				return Vector3.Lerp((Vector3)from, (Vector3)to, lerp);
			} else if (type == typeof(bool)) {
				return lerp < 0.5f ? (bool)from : (bool)to;
			}
			return null;
		}
	}
}
