using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    public static Watcher instance;
    [SerializeField] private bool watch;
    [SerializeField] private WatcherConfig config;
    [SerializeField] private float updateInterval = 1f;
    private GUIStyle watcherStyle;
    private List<FieldInfo> fields;
    private List<PropertyInfo> properties;
    private float ellapsedTime = 0;
    void Awake()
    {
        instance = this;
        if (config == null) config = Resources.Load<WatcherConfig>("Config/default");
        watcherStyle = new GUIStyle();
        watcherStyle.font = config.font;
        watcherStyle.fontSize = 14;
        watcherStyle.normal.textColor = config.commandColor;
        // watcherStyle.
        var assembly = System.AppDomain.CurrentDomain.Load("Assembly-CSharp");
        fields = assembly
            .GetTypes()
            .SelectMany(x => x.GetFields())
            .Where(y => y.GetCustomAttributes(true).OfType<WatchAttribute>().Any()).ToList();
        properties = assembly
            .GetTypes()
            .SelectMany(x => x.GetProperties())
            .Where(y => y.GetCustomAttributes(true).OfType<WatchAttribute>().Any()).ToList();
    }

    void OnGUI()
    {
        ellapsedTime += 1;
        if (!watch) return;
        // foreach (FieldInfo fieldInfo in fields)
        for (int j = 0; j < fields.Count; j++)
        {
            var fieldInfo = fields[j];
            var fieldType = fieldInfo.DeclaringType;
            var fieldObjects = FindObjectsOfType(fieldType) as MonoBehaviour[];
            if (fieldObjects != null)
            {
                for (int i = 0; i < fieldObjects.Length; i++)
                {
                    var fobject = fieldObjects[i];
                    string fName = fieldInfo.Name;
                    string fvalue = fieldInfo.GetValue(fobject).ToString();
                    var pos = Camera.main.WorldToScreenPoint(fobject.transform.position);
                    Rect rect = new Rect(pos.x, -pos.y + Screen.height + j * 20, 150, 20);
                    DrawQuad(rect, config.backColor);
                    GUI.Label(rect, " " + fName + " : " + fvalue, watcherStyle);
                }
            }
        }
    }

    void DrawQuad(Rect position, Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.Box(position, GUIContent.none);
    }
}
