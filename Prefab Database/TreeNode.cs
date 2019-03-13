using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.ObjectModel;
using System.Linq;
using System;

namespace Maumer
{
    public class TreeNode<T>
    {
        public readonly T value;
        public List<TreeNode<T>> children { get; private set; }

        public TreeNode()
        {
            value = default(T);
        }

        public TreeNode(T value)
        {
            this.value = value;
        }

        public TreeNode<T> this[int i]
        {
            get { return children[i]; }
        }

        public TreeNode<T> Parent { get; private set; }

        public T Value { get { return value; } }
        
        public TreeNode<T> AddChild(T value)
        {
            var newChild = new TreeNode<T>(value) { Parent = this };
            if (children == null)
                children = new List<TreeNode<T>>();
            children.Add(newChild);
            return newChild;
        }

        public TreeNode<T>[] AddChildren(params T[] values)
        {
            return values.Select(AddChild).ToArray();
        }

        public bool RemoveChild(TreeNode<T> node)
        {
            return children.Remove(node);
        }

        public void Traverse(Action<T> action)
        {
            action(Value);
            foreach (var child in children)
                child.Traverse(action);
        }

        public IEnumerable<T> Flatten()
        {
            return new[] { Value }.Concat(children.SelectMany(x => x.Flatten()));
        }
    }
}
