/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     Name: LayerViewModel
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 2015
 *  WebSite: 
 *
******************************************************************************/
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amrs.ViewModels
{
    public class LayerViewModel : ToolViewModel
    {
        public void LayerViewModel1()
        {
            Layers = new ObservableCollection<TreeViewNode>();

            //Layers.

            // 1.背景信息
            TreeViewNode tvmBackGround = new TreeViewNode("背景信息");
            {
                //tvmBackGround.AddChildren(new TreeViewNode("图元层"), true);
                //tvmBackGround.AddChildren(new TreeViewNode("GIS层"), false);
                //tvmBackGround.IsExpanded = true;
            }
            //Layers.Add(tvmBackGround);

            // 2.遥感数据
            TreeViewNode tvmRsdata = new TreeViewNode("遥感数据");
            {
                tvmRsdata.AddChildren(new TreeViewNode("rs1"), true);
                tvmRsdata.AddChildren(new TreeViewNode("rs2"), true);
                tvmRsdata.AddChildren(new TreeViewNode("rs3"), false);
                tvmRsdata.IsExpanded = true;
            }
            //Layers.Add(tvmRsdata);

            // 3.图层控制
            TreeViewNode tvmRoot = new TreeViewNode("图层控制");
            tvmRoot.Icon = "/Images/home.png";
            Layers.Add(tvmRoot);

        }

        public ObservableCollection<TreeViewNode> Layers { get; private set; }

        //public List<TreeViewNode> Layers { get; private set; }

        public TreeViewNode MyCreateTree1()
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

        //public TreeViewNode 
    }
}
