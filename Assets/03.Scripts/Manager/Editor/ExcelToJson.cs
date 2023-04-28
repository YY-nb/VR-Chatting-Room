
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Data;
using Excel;
using System.IO;
using LitJson;
using System.Text;
using System.Text.RegularExpressions;
using System;
using System.CodeDom;
using System.Reflection;
using System.CodeDom.Compiler;


public class ExcelToJson : EditorWindow
{

    List<string> ExcelPath = new List<string>();
    string JsonPath;
    string CSharpPath;
    string JsonName;
    List<string> dataType = new List<string>();
    List<string> dataName = new List<string>();
    List<object[]> ExcelDateList = new List<object[]>();

    [UnityEditor.MenuItem("Tools/MyTool/ExcelToJson")]
    static void ExceltoJson()
    {
        ExcelToJson toJson = (ExcelToJson)EditorWindow.GetWindow(typeof(ExcelToJson), true, "ExcelToJson");
        toJson.Show();
    }

    private void OnGUI()
    {
        Color oldColor = GUI.backgroundColor;

        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("转换excel文件"))
        {
            GetAllExcelPath();
        }
        GUI.backgroundColor = oldColor;

        //Color color = new Color(201, 232, 255);
        //GUI.backgroundColor = Color.yellow;
        //if (GUILayout.Button("ExcelToJson"))
        //{
        //    CreatJsonFile();
        //}
        //GUI.backgroundColor = oldColor;
        //
        //GUI.backgroundColor = Color.gray;
        //if (GUILayout.Button("CreatCSharp"))
        //{
        //    CreatCSharp();
        //}
        //GUI.backgroundColor = oldColor;

    }
    
    #region Excel文件处理
    void GetAllExcelPath()
    {
        //Excel文件夹路径
        string assetPath = UnityEngine.Application.dataPath + "/05.Data/Excel";
        //获取Excel文件夹中的Excel文件
        string[] files = Directory.GetFiles(assetPath, "*.xlsx");
        for (int i = 0; i < files.Length; i++)
        {
            ExcelPath.Add(files[i]);
            Debug.Log(ExcelPath[i]);
            ReadExcel(files[i].Replace("\\", "/"));
        }
    }

    /// <summary>
    /// 读取Excel
    /// </summary>
    /// <param name="path">excel路径</param>
    /// <param name="columnNum">列</param>
    /// <param name="rowNum">行</param>
    void ReadExcel(string path)
    {
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet data = excelReader.AsDataSet();
        dataName.Clear();
        dataType.Clear();
        ExcelDateList.Clear();
        // 读取Excel的所有页签
        for (int i = 0; i < data.Tables.Count; i++)
        {
            DataRowCollection dataRow = data.Tables[i].Rows;            // 每行
            DataColumnCollection dataColumn = data.Tables[i].Columns;   // 每列

            string tableName = data.Tables[i].TableName;
            JsonPath = UnityEngine.Application.dataPath + "/05.Data/Json/"; 
            JsonName = tableName + ".json";
            JsonPath = JsonPath + "/" + JsonName; Debug.Log("Json路径："+JsonPath);
            CSharpPath = UnityEngine.Application.dataPath + "/03.Scripts/Data/Entity/" + tableName + ".cs";

            for (int rowNum = 0; rowNum < data.Tables[i].Rows.Count; rowNum++)
            {
                object[] table = new object[data.Tables[i].Columns.Count];
                for (int columnNum = 0; columnNum < data.Tables[i].Columns.Count; columnNum++)
                {
                    if (rowNum == 0)  // 第一行的值：数据类型
                    { 
                        dataType.Add(data.Tables[i].Rows[0][columnNum].ToString()); 
                    }
                    else if (rowNum == 1)  // 第二行的值：数据名
                    {
                        dataName.Add(data.Tables[i].Rows[1][columnNum].ToString());
                    }
                    else
                    {
                        //Debug.Log(data.Tables[i].Rows[rowNum][columnNum].ToString() + "\n");
                        table[columnNum] = data.Tables[i].Rows[rowNum][columnNum].ToString();
                        /*
                        Debug.Log(data.Tables[i].Rows[rowNum][columnNum]);
                        switch (dataType[columnNum])
                        {
                            case "float":
                                table[columnNum] = (double) data.Tables[i].Rows[rowNum][columnNum];
                                break;
                            case "int":
                                table[columnNum] = (int)data.Tables[i].Rows[rowNum][columnNum];
                                break;
                            case "string":
                                table[columnNum] = data.Tables[i].Rows[rowNum][columnNum].ToString();
                                break;
                        }
                        */
                    }

                }
                if (rowNum > 1)
                {
                    //将一行数据存入list
                    ExcelDateList.Add(table);
                }
            }

            CreatJsonFile();

            CreatCSharp(tableName);
        }
    }

    #endregion

    #region Excel转json
    void CreatJsonFile()
    {
        if (File.Exists(JsonPath))
        {
            File.Delete(JsonPath);
        }
        
        JsonData jsonDatas = new JsonData();
        jsonDatas.SetJsonType(LitJson.JsonType.Array);

        for (int i = 0; i < ExcelDateList.Count; i++)
        {
            JsonData jsonData = new JsonData();
            for (int j = 0; j < dataName.Count; j++)
            {
                jsonData[dataName[j]] = ExcelDateList[i][j].ToString();
                //Debug.Log("第二轮输出:\n");
                //Debug.Log(ExcelDateList[i][j].ToString() + "\n");
            }
            jsonDatas.Add(jsonData);
        }
        string json = jsonDatas.ToJson();

        //防止中文乱码
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        StreamWriter writer = new StreamWriter(JsonPath, false, Encoding.GetEncoding("UTF-8"));
        writer.WriteLine(reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); }));

        writer.Flush();
        writer.Close();

        System.Diagnostics.Process.Start("explorer.exe", JsonPath.Replace("/", "\\"));
    }
    #endregion
    
    #region 创建C#代码
    void CreatCSharp(string name)
    {
        if (File.Exists(CSharpPath))
        {
            File.Delete(CSharpPath);
        }
        //CodeTypeDeclaration 代码类型声明类
        CodeTypeDeclaration CSharpClass = new CodeTypeDeclaration(name);
        CSharpClass.IsClass = true;
        CSharpClass.TypeAttributes = TypeAttributes.Public;
        // 设置成员的自定义属性
        //CodeAttributeDeclaration代码属性声明
        //CodeTypeReference代码类型引用类
        //System.Serializable 给脚本打上[System.Serializable()]标签，将 成员变量 在Inspector中显示
        //CSharpClass.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference("System.Serializable")));
        for (int i = 0; i < dataName.Count; i++)
        {
            // 创建字段
            //CodeMemberField 代码成员字段类 => (Type, string name)
            CodeMemberField member = new CodeMemberField(GetTypeForExcel(dataName[i], dataType[i]), dataName[i]);
            member.Attributes = MemberAttributes.Public;
            CSharpClass.Members.Add(member);
        }

        // 获取C#语言的实例
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        //代码生成器选项类
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        //设置支撑的样式
        options.BracingStyle = "C";
        //在成员之间插入空行
        options.BlankLinesBetweenMembers = true;

        StreamWriter writer = new StreamWriter(CSharpPath, false, Encoding.GetEncoding("UTF-8"));
        //生成最终代码
        provider.GenerateCodeFromType(CSharpClass, writer, options);

        writer.Flush();
        writer.Close();

        System.Diagnostics.Process.Start("explorer.exe", CSharpPath.Replace("/", "\\"));
    }

    Type GetTypeForExcel(string Name, string Type)
    {
        if (Type == "int")
            return typeof(Int32);
        if (Type == "float")
            return typeof(Single);  //float关键字是System.Single的别名
        if (Type == "double")
            return typeof(Double);

        return typeof(String);
    }
    #endregion
}
