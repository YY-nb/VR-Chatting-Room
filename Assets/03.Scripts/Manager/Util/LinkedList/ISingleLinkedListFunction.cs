using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISingleLinkedListFunction<T>
{
    /// <summary>
    /// 第一个节点
    /// </summary>
    SingleLinkedListNode<T> First { get; }

    /// <summary>
    /// 最后一个节点
    /// </summary>
    SingleLinkedListNode<T> Last { get; }

    /// <summary>
    /// 链表数量
    /// </summary>
    int Count { get; }
    /// <summary>
    /// 链表是否为空
    /// </summary>
    bool IsEmpty { get; }

    /// <summary>
    /// 清空链表
    /// </summary>
    void Clear();

    /// <summary>
    /// 链表中是否包含该数据
    /// </summary>
    /// <param name="value">验证数据项</param>
    /// <returns></returns>
    bool Contains(T value);

    /// <summary>
    /// 添加一个节点到节点起点
    /// </summary>
    /// <param name="value">节点数据</param>
    /// <returns>返回Node节点</returns>
    SingleLinkedListNode<T> AddFirst(T value);

    /// <summary>
    /// 添加一个节点到节点起点
    /// </summary>
    /// <param name="node">需要添加的节点</param>
    /// <returns>添加的并不是参数节点,而是参数数据,返回添加的节点</returns>
    SingleLinkedListNode<T> AddFirst(SingleLinkedListNode<T> node);

    /// <summary>
    /// 添加一个节点到链表最后
    /// </summary>
    /// <param name="value">节点数据</param>
    /// <returns>节点</returns>
    SingleLinkedListNode<T> AddLast(T value);

    ///<summary>
    /// 添加一个节点到链表最后
    /// </summary>
    /// <param name="node">需要添加的节点</param>
    /// <returns>添加的并不是参数节点,而是参数数据,返回添加的节点</returns>
    SingleLinkedListNode<T> AddLast(SingleLinkedListNode<T> node);

    /// <summary>
    /// 插入一个节点到指定的下标出
    /// </summary>
    /// <param name="value">节点数据</param>
    /// <param name="index">插入下标</param>
    /// <returns>新增节点</returns>
    SingleLinkedListNode<T> Insert(T value, int index);

    /// <summary>
    /// 插入一个节点到指定的下标出
    /// </summary>
    /// <param name="node">节点</param>
    /// <param name="index">插入下标</param>
    /// <returns>添加的并不是参数节点,而是参数数据,返回添加的节点</returns>
    SingleLinkedListNode<T> Insert(SingleLinkedListNode<T> node, int index);

    /// <summary>
    /// 删除节点
    /// </summary>
    /// <param name="value">数据</param>
    /// <returns>是否删除成功</returns>
    bool Delete(T value);

    /// <summary>
    /// 删除节点
    /// </summary>
    /// <param name="index">删除的节点下标</param>
    /// <returns>是否删除成功</returns>
    bool DeleteAt(int index);

    /// <summary>
    /// 删除节点
    /// </summary>
    /// <param name="index">删除的节点</param>
    /// <returns>是否删除成功</returns>
    bool Delete(SingleLinkedListNode<T> node);

    /// <summary>
    /// 删除第一个节点
    /// </summary>
    /// <returns>是否删除成功</returns>
    bool DeleteFirst();

    /// <summary>
    /// 删除最后一个节点
    /// </summary>
    /// <returns>是否删除成功</returns>
    bool DeleteLast();

    /// <summary>
    /// 根据数据查找节点
    /// </summary>
    /// <param name="value">数据</param>
    /// <returns>节点</returns>
    SingleLinkedListNode<T> Find(T value);

    /// <summary>
    /// 根据数据查找上一个节点
    /// </summary>
    /// <param name="value">数据</param>
    /// <returns>节点</returns>
    SingleLinkedListNode<T> FindPrevious(T value);

    /// <summary>
    /// 索引器取数据
    /// </summary>
    /// <param name="index">下标</param>
    /// <returns></returns>
    T this[int index] { get; }

    /// <summary>
    /// 根据下标取数据
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    T GetElement(int index);

    /// <summary>
    /// 根据数据获取下标
    /// </summary>
    /// <param name="value">数据</param>
    /// <returns>数据节点下标</returns>
    int IndexOf(T value);
}
