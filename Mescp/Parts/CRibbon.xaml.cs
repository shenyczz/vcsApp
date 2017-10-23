using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mescp.Parts
{
    /// <summary>
    /// Ribbon.xaml 的交互逻辑
    /// </summary>
    public partial class CRibbon// : Page
    {
        public CRibbon()
        {
            InitializeComponent();
        }
    }
}


/*
        
        <ribbon:Ribbon Grid.Row="0" x:RgnName="ribbon" Background="GreenYellow">

            <!--Ribbon 标签-->
            <ribbon:RibbonTab x:RgnName="RibbonTab1" Header="Ribbon Demo">

                <ribbon:RibbonGroup x:RgnName="Group1" Header="Button">
                    <ribbon:RibbonButton x:RgnName="Button1"
                                         Label="大图标按钮"
                                         LargeImageSource="Assets\LargeIcon.png"/>
                    <ribbon:RibbonButton x:RgnName="Button2"
                                         Label="小图标按钮"
                                         SmallImageSource="Assets\SmallIcon.png"/>
                    <ribbon:RibbonButton x:RgnName="Button3"
                                         Label="小图标按钮"
                                         SmallImageSource="Assets\SmallIcon.png"/>
                    <ribbon:RibbonButton x:RgnName="Button4"
                                         Label="小图标按钮"
                                         SmallImageSource="Assets\SmallIcon.png"/>
                </ribbon:RibbonGroup>

                <ribbon:RibbonGroup x:RgnName="Group2" Header="ToggleButton">
                    <ribbon:RibbonToggleButton Label="123"/>
                    <ribbon:RibbonToggleButton Label="987"/>
                </ribbon:RibbonGroup>

                <ribbon:RibbonGroup x:RgnName="Group3" Header="CheckBox">
                    <ribbon:RibbonCheckBox Label="RibbonCheckBox" SmallImageSource="Assets\SmallIcon.png"/>
                    <ribbon:RibbonCheckBox Label="RibbonCheckBox"/>
                </ribbon:RibbonGroup>

                <ribbon:RibbonGroup x:RgnName="Group4" Header="RadioButton">
                    <ribbon:RibbonRadioButton Label="Radio00"/>
                    <ribbon:RibbonRadioButton Label="Radio01"/>
                    <ribbon:RibbonRadioButton Label="Radio02"/>
                </ribbon:RibbonGroup>

                <ribbon:RibbonGroup x:RgnName="Group5" Header="TextBox">
                    <ribbon:RibbonTextBox Label="123" Text="456"/>
                </ribbon:RibbonGroup>

                <ribbon:RibbonGroup x:RgnName="Group6" Header="">
                    <ribbon:RibbonComboBox Label="123">
                        <ribbon:RibbonButton Label="1111"/>
                    </ribbon:RibbonComboBox>
                </ribbon:RibbonGroup>

                <ribbon:RibbonGroup x:RgnName="Group7" Header="">
                    
                </ribbon:RibbonGroup>

            </ribbon:RibbonTab>
            <ribbon:RibbonTab x:RgnName="RibbonTab2" Header="RibbonTab2">
            </ribbon:RibbonTab>

        </ribbon:Ribbon>

 */