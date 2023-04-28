
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
        if (GUILayout.Button("ת��excel�ļ�"))
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
    
    #region Excel�ļ�����
    void GetAllExcelPath()
    {
        //Excel�ļ���·��
        string assetPath = UnityEngine.Application.dataPath + "/05.Data/Excel";
        //��ȡExcel�ļ����е�Excel�ļ�
        string[] files = Directory.GetFiles(assetPath, "*.xlsx");
        for (int i = 0; i < files.Length; i++)
        {
            ExcelPath.Add(files[i]);
            Debug.Log(ExcelPath[i]);
            ReadExcel(files[i].Replace("\\", "/"));
        }
    }

    /// <summary>
    /// ��ȡExcel
    /// </summary>
    /// <param name="path">excel·��</param>
    /// <param name="columnNum">��</param>
    /// <param name="rowNum">��</param>
    void ReadExcel(string path)
    {
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet data = excelReader.AsDataSet();
        dataName.Clear();
        dataType.Clear();
        ExcelDateList.Clear();
        // ��ȡExcel������ҳǩ
        for (int i = 0; i < data.Tables.Count; i++)
        {
            DataRowCollection dataRow = data.Tables[i].Rows;            // ÿ��
            DataColumnCollection dataColumn = data.Tables[i].Columns;   // ÿ��

            string tableName = data.Tables[i].TableName;
            JsonPath = UnityEngine.Application.dataPath + "/05.Data/Json/"; 
            JsonName = tableName + ".json";
            JsonPath = JsonPath + "/" + JsonName; Debug.Log("Json·����"+JsonPath);
            CSharpPath = UnityEngine.Application.dataPath + "/03.Scripts/Data/Entity/" + tableName + ".cs";

            for (int rowNum = 0; rowNum < data.Tables[i].Rows.Count; rowNum++)
            {
                object[] table = new object[data.Tables[i].Columns.Count];
                for (int columnNum = 0; columnNum < data.Tables[i].Columns.Count; columnNum++)
                {
                    if (rowNum == 0)  // ��һ�е�ֵ����������
                    { 
                        dataType.Add(data.Tables[i].Rows[0][columnNum].ToString()); 
                    }
                    else if (rowNum == 1)  // �ڶ��е�ֵ��������
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
                    //��һ�����ݴ���list
                    ExcelDateList.Add(table);
                }
            }

            CreatJsonFile();

            CreatCSharp(tableName);
        }
    }

    #endregion

    #region Excelתjson
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
                //Debug.Log("�ڶ������:\n");
                //Debug.Log(ExcelDateList[i][j].ToString() + "\n");
            }
            jsonDatas.Add(jsonData);
        }
        string json = jsonDatas.ToJson();

        //��ֹ��������
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        StreamWriter writer = new StreamWriter(JsonPath, false, Encoding.GetEncoding("UTF-8"));
        writer.WriteLine(reg.Replace(json, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); }));

        writer.Flush();
        writer.Close();

        System.Diagnostics.Process.Start("explorer.exe", JsonPath.Replace("/", "\\"));
    }
    #endregion
    
    #region ����C#����
    void CreatCSharp(string name)
    {
        if (File.Exists(CSharpPath))
        {
            File.Delete(CSharpPath);
        }
        //CodeTypeDeclaration ��������������
        CodeTypeDeclaration CSharpClass = new CodeTypeDeclaration(name);
        CSharpClass.IsClass = true;
        CSharpClass.TypeAttributes = TypeAttributes.Public;
        // ���ó�Ա���Զ�������
        //CodeAttributeDeclaration������������
        //CodeTypeReference��������������
        //System.Serializable ���ű�����[System.Serializable()]��ǩ���� ��Ա���� ��Inspector����ʾ
        //CSharpClass.CustomAttributes.Add(new CodeAttributeDeclaration(new CodeTypeReference("System.Serializable")));
        for (int i = 0; i < dataName.Count; i++)
        {
            // �����ֶ�
            //CodeMemberField �����Ա�ֶ��� => (Type, string name)
            CodeMemberField member = new CodeMemberField(GetTypeForExcel(dataName[i], dataType[i]), dataName[i]);
            member.Attributes = MemberAttributes.Public;
            CSharpClass.Members.Add(member);
        }

        // ��ȡC#���Ե�ʵ��
        CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
        //����������ѡ����
        CodeGeneratorOptions options = new CodeGeneratorOptions();
        //����֧�ŵ���ʽ
        options.BracingStyle = "C";
        //�ڳ�Ա֮��������
        options.BlankLinesBetweenMembers = true;

        StreamWriter writer = new StreamWriter(CSharpPath, false, Encoding.GetEncoding("UTF-8"));
        //�������մ���
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
            return typeof(Single);  //float�ؼ�����System.Single�ı���
        if (Type == "double")
            return typeof(Double);

        return typeof(String);
    }
    #endregion
}
