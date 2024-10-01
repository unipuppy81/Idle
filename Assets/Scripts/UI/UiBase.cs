using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiBase : MonoBehaviour
{
    #region Fields

    private readonly Dictionary<Type, Dictionary<string, UnityEngine.Object>> objects = new();

    #endregion

    #region Init
    private void Start()
    {
        Init();
    }

    protected virtual void Init() { }

    #endregion

    #region Properties

    /// <summary>
    /// �θ� ������Ʈ���� �ڽ� ������Ʈ���� T Ÿ�� ������Ʈ ã�Ƽ� ���ε�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    protected void SetUI<T>() where T : UnityEngine.Object => Binding<T>(gameObject);

    /// <summary>
    /// Ư�� �̸��� TŸ�� UI ������Ʈ�� ��ųʸ����� ã�� ��ȯ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="componentName"></param>
    /// <returns></returns>
    protected T GetUI<T>(string componentName) where T : UnityEngine.Object => GetComponent<T>(componentName);

    #endregion

    #region Binding

    /// <summary>
    /// UnityEngine.Object Ÿ���� ������Ʈ���� �θ� ������Ʈ�� �ڽĵ� �߿��� ã�Ƽ� ��ųʸ��� ����
    /// </summary>
    /// <typeparam name="T">������Ʈ</typeparam>
    public void Binding<T>(GameObject parent) where T : UnityEngine.Object
    {
        T[] objects = parent.GetComponentsInChildren<T>(true);

        // �ߺ��� �̸��� ���� ������Ʈ���� �ϳ��� Ű�� ����
        // �� �׷쿡�� ù ��°�� �����ϴ� ������Ʈ�� �����Ͽ� ��ųʸ��� ����
        Dictionary<string, UnityEngine.Object> objectDict = objects
            .GroupBy(comp => comp.name)
            .ToDictionary(group => group.Key, group => group.First() as UnityEngine.Object);

        this.objects[typeof(T)] = objectDict;
        AssignComponentsDirectChild<T>(parent);
    }

    /// <summary>
    /// parent ������ ������Ʈ�� �̸��� ��ġ�ϴ� ������Ʈ�� ���� ���, 
    /// �ش� �ڽ��� ã�Ƽ� _objects ��ųʸ��� �ִ� ������Ʈ���� �Ҵ�
    /// </summary>
    /// <typeparam name="T">������Ʈ</typeparam>
    private void AssignComponentsDirectChild<T>(GameObject parent) where T : UnityEngine.Object
    {
        if (!objects.TryGetValue(typeof(T), out var _objects)) return;

        // �� ������Ʈ�� ���� �ݺ�
        foreach (var key in _objects.Keys.ToList())
        {
            // �̹� �Ҵ�� ��� ��ŵ
            if (_objects[key] != null) continue;

            // GameObject Ÿ������ Ȯ�� ��, ������ FindComponent �޼��� ȣ��
            UnityEngine.Object component = typeof(T) == typeof(GameObject)
                ? FindComponentDirectChild<GameObject>(parent, key)
                : FindComponentDirectChild<T>(parent, key);

            // ã�� ������Ʈ�� null�� �ƴ϶�� �Ҵ��ϰ�, �׷��� �ʴٸ� ���� �α� ���
            if (component != null)
            {
                _objects[key] = component;
            }
            else
            {
                Debug.Log($"Binding failed for Object : {key}");
            }
        }
    }

    /// <summary>
    /// ���� �ڽĵ� �߿��� �̸��� Ư���� ���ǰ� ��ġ�ϴ� ������Ʈ ��ȯ
    /// </summary>
    /// <typeparam name="T">������Ʈ</typeparam>
    /// <param name="name">�־��� �̸��� ��ġ�ϴ� ù° �ڽ� �̸�</param>
    private T FindComponentDirectChild<T>(GameObject parent, string name) where T : UnityEngine.Object
    {
        return parent.transform
            .Cast<Transform>()
            .FirstOrDefault(child => child.name == name)
            ?.GetComponent<T>();
    }

    /// <summary>
    /// �Լ��� ����� ��ųʸ����� Ư�� Ÿ�԰� �̸��� �ش��ϴ� ������Ʈ�� �������� ����
    /// </summary>
    /// <typeparam name="T">������Ʈ</typeparam>
    public T GetComponent<T>(string componentName) where T : UnityEngine.Object
    {
        if (objects.TryGetValue(typeof(T), out var components) && components.TryGetValue(componentName, out var component))
        {
            return component as T;
        }

        return null;
    }

    #endregion

    #region Action Binding

    /// <summary>
    /// ��ư�� �Լ� �����ϱ�
    /// </summary>
    protected Button SetButtonEvent(string buttonName, UIEventType uIEventType, Action<PointerEventData> action)
    {
        Button button = GetUI<Button>(buttonName);
        button.gameObject.SetEvent(uIEventType, action);
        return button;
    }

    #endregion
}
