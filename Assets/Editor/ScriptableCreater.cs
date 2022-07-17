using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CMN;
using MagicContext;

public abstract class ScriptableObjectCreater<T> : EditorWindow where T : ScriptableObject
{
    [SerializeField] TextAsset csvFile;
    [SerializeField] string exportFolder;
    [SerializeField] byte startLine = 1;
    [SerializeField] byte nameLine = 0;
    [SerializeField] int[] skipColumns;


    List<string[]> ReadCSVFile(TextAsset csvFile)//CSVファイルを読み取る
    {
        //1行ごとに配列としてCSV読み取り、それをリストに変換する
        List<string> list = File.ReadAllLines(AssetDatabase.GetAssetPath(csvFile)).ToList();

        //1行ごとの文字列を更に、カンマで区切る
        List<string[]> lists = new List<string[]>();
        for (int i = 0; i < list.Count; i++)
            lists.Add(list[i].Split(','));

        return lists;
    }

    public void Create()//データの作成
    {
        //CSVファイルを読み込む
        List<string[]> csvLines = ReadCSVFile(csvFile);

        //読み取った値からScriptableObjectの作成
        for (int i = startLine; i < csvLines.Count; i++)
        {
            //出力パスを決定
            string fileName = $"{csvLines[i][nameLine]}.asset";
            string exportPath = $"{exportFolder}/{fileName}";

            /*既にデータが存在している場合は上書き、
              そうでない場合は新規にScriptableObjectを生成*/
            T obj;
            if (File.Exists(exportPath))
            {
                obj = AssetDatabase.LoadAssetAtPath(exportPath, typeof(T)) as T;
            }
            else
            {
                //新規作成
                obj = CreateInstance<T>();
                AssetDatabase.CreateAsset(obj, exportPath);
            }

            //キューにデータを保存
            Queue<string> data = new Queue<string>();
            int index = 0;
            foreach (var line in csvLines[i])
            {
                bool enable = true;
                foreach (var skip in skipColumns)
                {
                    if (index == skip)
                    {
                        enable = false;
                    }
                }
                if (enable)
                {
                    Debug.Log(line);
                    data.Enqueue(line.ToString());
                }
                index++;
            }

            //データの設定
            SetEachData(obj, data);

            //変更済みのデータであると印をつける
            EditorUtility.SetDirty(obj);
        }
        //保存
        AssetDatabase.SaveAssets();
    }


    //各データを作成する(継承して使用)
    protected abstract void SetEachData(T t, Queue<string> data);

    //エディタウィンドウ上での挙動
    protected void OnGUI()
    {
        ScriptableObject target = this;
        SerializedObject serializedObject = new SerializedObject(target);
        SerializedProperty skipColumns = serializedObject.FindProperty("skipColumns");

        EditorGUILayout.HelpBox("ファイル", MessageType.None);
        csvFile = (TextAsset)EditorGUILayout.ObjectField("CSVファイル", csvFile, typeof(TextAsset), false);
        exportFolder = EditorGUILayout.TextField("出力先フォルダ", exportFolder);
        EditorGUILayout.Space(20);

        EditorGUILayout.HelpBox("ファイル行：1～2…", MessageType.None);
        startLine = (byte)EditorGUILayout.IntField("読み取り開始行番号", startLine);
        EditorGUILayout.Space(20);

        EditorGUILayout.HelpBox("ファイル列：A～B…", MessageType.None);
        nameLine = (byte)EditorGUILayout.IntField("ファイル名とする列番号", nameLine);
        EditorGUILayout.PropertyField(skipColumns, new GUIContent("スキップする列"), true);
        EditorGUILayout.Space(20);

        if (GUILayout.Button("データの設定"))
        {
            Create();
        }
        serializedObject.ApplyModifiedProperties();
    }
}

public class ItemDataCreateWindow : ScriptableObjectCreater<DataVisual>
{
    [MenuItem("Window/Create/DataVisual")]
    protected static void Init()
    {
        ItemDataCreateWindow window = (ItemDataCreateWindow)GetWindow(typeof(ItemDataCreateWindow));
        window.Show();
    }

    protected override void SetEachData(DataVisual data, Queue<string> dataQueue)
    {
        var grade = Utility.GetIntToEnum<MagicGrade>(int.Parse(dataQueue.Dequeue()));
        var atribute = Utility.GetIntToEnum<MagicAttribute>(int.Parse(dataQueue.Dequeue()));
        var ja = dataQueue.Dequeue();
        var en = dataQueue.Dequeue();
        var mp = int.Parse(dataQueue.Dequeue());
        var exp = dataQueue.Dequeue();
        var value = float.Parse(dataQueue.Dequeue());

        data.SetVisualStatement(ja, en, mp, value, exp, grade, atribute);
    }
}