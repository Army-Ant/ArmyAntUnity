﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArmyAnt.ViewUtil.Components
{

    /// <summary>
    /// 用于简单的滚动列表的 Component,
    /// 采用 Unity 自带的 ScrollView 即可, 但是 content 需要添加一种 LayoutGroup :
    /// VerticalLayoutGroup, HorizontalLayoutGroup, 或者 GridLayoutGroup
    /// </summary>
    public class ListView : UnityEngine.UI.ScrollRect, IEnumerable<RectTransform>
    {
        public class DefaultItemNotSetException : System.Exception
        {
            public DefaultItemNotSetException() : base() { }
            public DefaultItemNotSetException(string message) : base(message) { }
        }

        /// <summary>
        /// 设定 Default Item, 类似于 cocos2d 的 ListView
        /// </summary>
        public RectTransform DefaultElement
        {
            get
            {
                return defaultElement;
            }
            set
            {
                defaultElement = value;
                if (value != null && content.GetComponent<UnityEngine.UI.GridLayoutGroup>() != null)
                {
                    content.GetComponent<UnityEngine.UI.GridLayoutGroup>().cellSize = defaultElement.rect.size;
                }
            }
        }

        /// <summary>
        /// 设定 content 的 padding 
        /// </summary>
        public RectOffset Padding
        {
            get { return content.GetComponent<UnityEngine.UI.LayoutGroup>().padding; }
            set { content.GetComponent<UnityEngine.UI.LayoutGroup>().padding = value; }
        }

        /// <summary>
        /// 获取指定索引位置的子项. 索引超出时按列表索引超出处理
        /// </summary>
        /// <param name="index">子项索引</param>
        /// <returns></returns>
        public RectTransform this[int index]
        {
            get
            {
                return children[index];
            }
            private set
            {
                if (ItemCount > index)
                {
                    throw new System.IndexOutOfRangeException();
                }
                else if (ItemCount == index)
                {
                    children.Add(value);
                    value.SetParent(content);
                }
                else
                {
                    children[index].SetParent(null);
                    children[index] = value;
                    value.SetParent(content);
                    value.SetSiblingIndex(index);
                }
            }
        }

        public List<RectTransform>.Enumerator GetEnumerator()
        {
            return children.GetEnumerator();
        }

        /// <summary>
        /// 子项总数量
        /// </summary>
        public int ItemCount => children.Count;

        /// <summary>
        /// 在表末尾添加一个 default item
        /// </summary>
        /// <returns> 成功添加的item </returns>
        public RectTransform PushbackDefaultItem(float localScale = 1.0f)
        {
            return InsertDefaultItem(children.Count, localScale);
        }

        public T PushbackDefaultItem<T>(float localScale = 1.0f) where T:Component {
            return PushbackDefaultItem(localScale).GetComponent<T>();
        }

        /// <summary>
        /// 在指定索引处插入 default item
        /// </summary>
        /// <param name="index">要插入的索引处</param>
        /// <returns>返回插入的 item </returns>
        public virtual RectTransform InsertDefaultItem(int index, float localScale = 1.0f)
        {
            if (defaultElement == null)
            {
                throw new DefaultItemNotSetException();
            }
            var ret = Instantiate(defaultElement.gameObject).GetComponent<RectTransform>();
            ret.SetParent(content);
            ret.SetSiblingIndex(index);
            ret.localScale = Vector3.one * localScale;
            var pos = ret.localPosition;
            pos.z = defaultElement.localPosition.z;
            ret.localPosition = pos;
            children.Insert(index, ret);
            return ret;
        }

        /// <summary>
        /// 删除指定索引处的item, 后面的依次补上
        /// </summary>
        /// <param name="index"> 要删除的 item 的索引 </param>
        /// <returns></returns>
        public virtual RectTransform DeleteItem(int index)
        {
            var ret = children[index];
            ret.SetParent(null);
            children.RemoveAt(index);
            return ret;
        }

        /// <summary>
        /// 清空所有 item
        /// </summary>
        public virtual void Clear()
        {
            foreach (var i in children)
            {
                i.SetParent(null);
                Destroy(i.gameObject);
            }
            children.Clear();
        }

        /// <summary>
        /// 使列表滚动到指定项目处
        /// </summary>
        /// <param name="index">Index.</param>
        public void ScrollToItem(int index)
        {
            ScrollToItem(children[index]);
        }

        /// <summary>
        /// 使列表滚动到指定项目处
        /// </summary>
        /// <param name="item">item.</param>
        public void ScrollToItem(RectTransform item)
        {
            var pos = content.localPosition;
            if (horizontal) {
                pos.x = -item.localPosition.x  - GetComponent<RectTransform>().rect.size.x / 2;
                if (pos.x < 0) { pos.x = 0; }
                if (pos.x > content.rect.size.x) { pos.x = content.rect.size.x; }
            }
            if (vertical) {
                pos.y = -item.localPosition.y  - GetComponent<RectTransform>().rect.size.y / 2;
                if (pos.y < 0) { pos.y = 0; }
                if (pos.y > content.rect.size.y) { pos.y = content.rect.size.y; }
            }
            content.localPosition = pos;
        }

        /// <summary>
        /// 将指定索引处的 item 替换成新的 default item
        /// </summary>
        /// <param name="index"> 要替换的 item 的索引 </param>
        /// <returns></returns>
        public virtual RectTransform ReplaceItemToDefault(int index)
        {
            if (defaultElement == null)
            {
                throw new DefaultItemNotSetException();
            }
            var ret = children[index];
            ret.SetParent(null);
            var newItem = Instantiate(defaultElement.gameObject).GetComponent<RectTransform>();
            newItem.SetParent(content);
            newItem.SetSiblingIndex(index);
            this[index] = newItem;
            return ret;
        }

        /// <summary>
        /// 寻找指定项在列表中的索引位置, 可用于项本身回调时定位
        /// </summary>
        /// <returns>找到的项的位置, 未找到返回-1</returns>
        /// <param name="item">要找的 RectTransform 项</param>
        public int GetItemIndex(RectTransform item)
        {
            if (item == null || children == null || children.Count == 0)
                return -1;
            for (var i = 0; i < children.Count; ++i)
            {
                if (children[i] == item)
                {
                    return i;
                }
            }
            return -1;
        }

        public int GetItemIndex(System.Func<RectTransform, bool> func)
        {
            if (func == null || children == null || children.Count == 0)
                return -1;
            for (var i = 0; i < children.Count; ++i)
            {
                if (func(children[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        // Use this for initialization
        protected override void Start()
        {
            base.Start();
            if (defaultElement != null && content.GetComponent<UnityEngine.UI.GridLayoutGroup>() != null)
            {
                content.GetComponent<UnityEngine.UI.GridLayoutGroup>().cellSize = defaultElement.rect.size;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<RectTransform> IEnumerable<RectTransform>.GetEnumerator()
        {
            return GetEnumerator();
        }

        [Tooltip("要添加的默认项目")]
        [Space(4)]
        public RectTransform defaultElement;
        private readonly List<RectTransform> children = new List<RectTransform>();
    }

}