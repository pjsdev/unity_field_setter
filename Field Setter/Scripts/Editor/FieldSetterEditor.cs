using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using OikosTools;

[CustomEditor(typeof(FieldSetter))]
public class FieldSetterEditor : Editor 
{
    FieldSetter script;

    void OnEnable()
    {
        script = (FieldSetter) target;
    }

    public override void OnInspectorGUI()
    {
        // the script specifies its target game object (not always its gameObject)
        script.target = (GameObject)EditorGUILayout.ObjectField(script.target, typeof(GameObject), true);

        GameObject theTarget = script.target;
        if (script.target == null) 
        {
            EditorGUILayout.HelpBox("No Target Object defined, using itself.", MessageType.Info);
            theTarget = script.gameObject;
        }

        // show our default value field at the top
        if (script.selectedComponent != null && script.selectedProperty.Length > 0) 
        {
            object value = PropertyChange.GetValue(script.selectedComponent, script.selectedProperty, script.selectedSubProperty);

            if (value != null) 
            {
                if (value.GetType() == typeof(float))   script.actionValue.Set(EditorGUILayout.FloatField("Default", script.actionValue.GetFloat()));
                if (value.GetType() == typeof(int))     script.actionValue.Set(EditorGUILayout.IntField("Default", script.actionValue.GetInt()));
                if (value.GetType() == typeof(Color))   script.actionValue.Set(EditorGUILayout.ColorField("Default", script.actionValue.GetColor()));
                if (value.GetType() == typeof(Vector2)) script.actionValue.Set(EditorGUILayout.Vector2Field("Default", script.actionValue.GetVector2()));
                if (value.GetType() == typeof(Vector3)) script.actionValue.Set(EditorGUILayout.Vector3Field("Default", script.actionValue.GetVector3()));
                if (value.GetType() == typeof(string))  script.actionValue.Set(EditorGUILayout.TextField("Default", script.actionValue.GetString()));
            }
        }

        // get the object's components
        Component[] components = theTarget.GetComponents<Component>();

        // get the index of the currently selected component
        int componentIndex = 0;
        if (components.Length > 0 && script.selectedComponent != null && System.Array.IndexOf(components, script.selectedComponent) >= 0)
            componentIndex = System.Array.IndexOf(components, script.selectedComponent);

        // make a list of names to display
        string[] componentsLabels = new string[components.Length];
        for (int i = 0; i < components.Length; i++) 
        {
            componentsLabels[i] = i + ": " + components[i].GetType().ToString();
        }

        componentIndex = EditorGUILayout.Popup("Component", componentIndex, componentsLabels);
        if (componentIndex >= 0) 
        {
            script.selectedComponent = components[componentIndex];
        }
        else 
        {
            Debug.LogWarning("Can't find the component anymore, Target Object changed or the component was removed", this);
        }
        
        // properties
        Component c = script.selectedComponent;
        string[] properties = PropertyChange.GetProperties(c).ToArray();
        string[] propertiesLabels = new string[properties.Length];
        if (properties.Length > 0) 
        {
            for (int i = 0; i < properties.Length; i++) 
            {
                propertiesLabels[i] = string.Format("{0} ({1})", properties[i], PropertyChange.GetValue(c, properties[i]).GetType().Name);
            }

            int propertyIndex = EditorGUILayout.Popup("  Property", System.Array.IndexOf(properties, script.selectedProperty), propertiesLabels);
            if (propertyIndex >=0) 
            {
                script.selectedProperty = properties[propertyIndex];
            }
            else 
            {
                //if (script.selectedProperty.Length > 0)
                    //Debug.LogWarning("Can't find the property " + script.selectedProperty + " anymore", this);
                script.selectedProperty = "";
            }
            
            if (script.selectedProperty.Length > 0) 
            {
                // subproperties
                object property = PropertyChange.GetValue(script.selectedComponent, script.selectedProperty);
                List<string> subproperties = PropertyChange.GetProperties(property).Where(x => x.ToString() != "Item").ToList();
                List<string> subpropertiesLabels = new List<string>();

                if (subproperties.Count > 0) 
                {
                    subproperties.Insert(0,"");
                    subpropertiesLabels.Add("None");
                    for (int i = 1; i < subproperties.Count; i++) {
                        subpropertiesLabels.Add(subproperties[i].ToString());
                    }

                    int subpropertyIndex = EditorGUILayout.Popup("    Subproperty", subproperties.IndexOf(script.selectedSubProperty), subpropertiesLabels.ToArray());
                    if (subpropertyIndex >=0)
                        script.selectedSubProperty = subproperties[subpropertyIndex];
                }
                else 
                {
                    script.selectedSubProperty = "";
                }
            } 
            else 
            {
                script.selectedSubProperty = "";
            }
        }
        
        if (script.selectedComponent == null) 
        {
            EditorGUILayout.HelpBox("Choose a component", MessageType.Error);
        } 
        else if (script.selectedProperty.Length == 0)
        {
            EditorGUILayout.HelpBox("Choose a property", MessageType.Error);
        }
    }
}
