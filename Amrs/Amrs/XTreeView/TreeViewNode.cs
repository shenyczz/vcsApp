/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: TreeViewNode
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 2015
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Amrs
{
    public class TreeViewNode : TreeViewModelBase
    {
        public TreeViewNode() : this("") { }

        public TreeViewNode(string name)
        {
            this.Name = name;
            this.Nodes = new List<TreeViewNode>();
            this.Icon = "/Images/Refresh.png";
        }

        public TreeViewNode Parent { get; set; }

        public List<TreeViewNode> Nodes { get; private set; }

        public String Icon { get; set; }


        #region Name

        public String Name { get; set; }

        #endregion

        #region IsChecked

        private bool? _isChecked;
        public bool? IsChecked
        {
            get { return _isChecked; }
            set
            {
                SetIsChecked(value, true, true);
            }
        }

        private void SetIsChecked(bool? value, bool checkedChildren, bool checkedParent)
        {
            if (_isChecked == value)
                return;

            _isChecked = value;

            //选中和取消子类
            if (checkedChildren && value.HasValue && Nodes != null)
                Nodes.ForEach(ch => ch.SetIsChecked(value, true, false));

            //选中和取消父类
            if (checkedParent && this.Parent != null)
                this.Parent.CheckParentCheckState();

            //通知更改
            this.SetProperty(x => x.IsChecked);
        }

        /// <summary>
        /// 检查父类是否选中
        /// 如果父类的子类中有一个和第一个子类的状态不一样父类ischecked为null
        /// </summary>
        private void CheckParentCheckState()
        {
            bool? _currentState = this.IsChecked;
            bool? _firstState = null;

            for (int i = 0; i < this.Nodes.Count(); i++)
            {
                bool? childrenState = this.Nodes[i].IsChecked;
                if (i == 0)
                {
                    _firstState = childrenState;
                }
                else if (_firstState != childrenState)
                {
                    _firstState = null;
                }
            }

            if (_firstState != null)
                _currentState = _firstState;

            SetIsChecked(_firstState, false, true);
        }

        #endregion

        #region IsSelected

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                this.SetProperty(x => x.IsSelected);

                if (_isSelected)
                {
                    SelectedTreeItem = this;
                    //System.Windows.MessageBox.Show("选中的是" + SelectedTreeItem.Name);
                }
                else
                {
                    SelectedTreeItem = null;
                }
            }
        }

        #endregion

        #region IsExpanded

        bool _IsExpanded;
        public bool IsExpanded
        {
            get { return _IsExpanded; }
            set
            {
                _IsExpanded = value;
                this.SetProperty(x => x.IsExpanded);
            }
        }

        #endregion

        #region SelectedTreeItem

        public TreeViewNode SelectedTreeItem { get; set; }

        #endregion

        #region 创建树

        public void AddChildren(TreeViewNode children, bool? isChecked)
        {
            this.Nodes.Add(children);
            children.Parent = this;
            children.IsChecked = isChecked;
        }

        #endregion
    }


    /// <summary>
    /// 扩展方法
    /// 避免硬编码问题
    /// </summary>
    public abstract class TreeViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public static class NotifyPropertyBaseEx
    {
        public static void SetProperty<T, U>(this T tvm, Expression<Func<T, U>> expre) where T : TreeViewModelBase, new()
        {
            string _pro = CommonFun.GetPropertyName(expre);
            tvm.RaisePropertyChanged(_pro);
        }
    }

    public abstract class CommonFun
    {
        /// <summary>
        /// 返回属性名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static string GetPropertyName<T, U>(Expression<Func<T, U>> expr)
        {
            string _propertyName = "";
            if (expr.Body is MemberExpression)
            {
                _propertyName = (expr.Body as MemberExpression).Member.Name;
            }
            else if (expr.Body is UnaryExpression)
            {
                _propertyName = ((expr.Body as UnaryExpression).Operand as MemberExpression).Member.Name;
            }
            return _propertyName;
        }
    }

    // 测试
    public class Test_TreeViewModel
    {
        public Test_TreeViewModel()
        {
            MyTrees = new List<TreeViewNode>();
            MyTrees.Add(MyCreateTree());

        }

        public List<TreeViewNode> MyTrees { get; set; }

        /// <summary>
        /// 创建树
        /// </summary>
        /// <returns></returns>
        public TreeViewNode MyCreateTree()
        {
            TreeViewNode tvm = new TreeViewNode("中国");
            tvm.IsExpanded = true;

            #region 北京

            TreeViewNode tvmBJ = new TreeViewNode("北京");
            tvm.AddChildren(tvmBJ, false);
            TreeViewNode _HD = new TreeViewNode("海淀区");
            TreeViewNode _CY = new TreeViewNode("朝阳区");
            TreeViewNode _FT = new TreeViewNode("丰台区");
            TreeViewNode _DC = new TreeViewNode("东城区");

            tvmBJ.AddChildren(_HD, false);
            _HD.AddChildren(new TreeViewNode("某某1"), false);
            _HD.AddChildren(new TreeViewNode("某某2"), true);

            tvmBJ.AddChildren(_CY, false);
            tvmBJ.AddChildren(_FT, false);
            tvmBJ.AddChildren(_DC, false);

            #endregion

            #region 河北
            TreeViewNode _myHB = new TreeViewNode("河北");
            tvm.AddChildren(_myHB, false);
            TreeViewNode _mySJZ = new TreeViewNode("石家庄");
            TreeViewNode _mySD = new TreeViewNode("山东");

            TreeViewNode _myTS = new TreeViewNode("唐山");

            _myHB.AddChildren(_mySJZ, true);
            _myHB.AddChildren(_mySD, false);
            _myHB.AddChildren(_myTS, false);
            #endregion

            return tvm;
        }

    }


}
