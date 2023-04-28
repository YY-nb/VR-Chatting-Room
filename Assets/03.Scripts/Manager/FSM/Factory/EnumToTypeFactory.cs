using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumToTypeFactory 
{
    //��������ö�ٷ�������������
    public static Type GetType(Enum typeEnum)
    {
        var enumName = typeEnum.GetType().Name;

        //ȥ����׺�õ��ⲿ����
        var outerClassName = "";
        var words = enumName.Split('_');
        outerClassName += words[0];
        for (int i = 1; i < words.Length - 1; i++)
        {
            outerClassName += '_' + words[i];
        }
        string targetClassName = $"{outerClassName}+{enumName}_{Enum.GetName(typeEnum.GetType(), typeEnum)}";  
        Type type = Type.GetType(targetClassName); 

        if (type == null)
            Debug.LogError($"ö������[{enumName}.{typeEnum}]û���ҵ���Ӧ���࣬����");

        return type;
    }
}
