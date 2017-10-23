/******************************************************************************
 * 
 * Announce: Designed by ShenYongchen(shenyczz@163.com),ZhengZhou,HeNan.
 *           Copyright (C) shenyc. All rights reserved.
 *
 *     RgnName: UIEventToCommand
 *  Version: 
 * Function: 
 * 
 *   Author: 申永辰.郑州 (shenyczz@163.com)
 * DateTime: 2010 - 2014
 *  WebSite: 
 *
    <ListBox x:RgnName="listBoxHdwDayFiles"
                SelectionMode="Single"
                ItemsSource="{Binding Mans}"
                local:UIEventToCommand.Event="MouseUp"
                local:UIEventToCommand.Command="{Binding TestCommand123}"
            ></ListBox>
 * 
******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mescp
{
    /// <summary>
    /// 将UI事件转化Command实现
    /// 直接注入到控件中使用
    /// 支持一次绑定多个事件到一个操作上
    /// 操作为“事件名称1,事件名称2,...事件名称3”
    /// 对于多事件绑定操作，在ViewModel层可以通过事件参数类型，判断当前执行的操作
    /// </summary>
    internal static class UIEventToCommand
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        public static readonly DependencyProperty EventProperty =
            DependencyProperty.RegisterAttached("Event",
            typeof(String),
            typeof(UIEventToCommand),
            new PropertyMetadata(OnEventChanged));

        /// <summary>
        /// 操作属性
        /// </summary>
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(UIEventToCommand),
            new PropertyMetadata(null));

        private static List<String> _EventList = new List<String>();

        /// <summary>
        /// 获取事件名称属性
        /// </summary>
        /// <param name="obj">控件对象</param>
        /// <returns></returns>
        public static String GetEvent(DependencyObject obj)
        {
            return (String)obj.GetValue(EventProperty);
        }
        /// <summary>
        /// 设置事件属性
        /// </summary>
        /// <param name="obj">控件对象</param>
        /// <param name="value">事件名称</param>
        public static void SetEvent(DependencyObject obj, String value)
        {
            obj.SetValue(EventProperty, value);
        }
        /// <summary>
        /// 获取操作对象
        /// </summary>
        /// <param name="obj">控件对象</param>
        /// <returns></returns>
        public static ICommand GetCommand(DependencyObject obj)
        {
            //获取操作对象
            object com = obj.GetValue(CommandProperty);
            //如果为空则返回空
            if (com == null)
                return null;

            return (ICommand)com;
        }
        /// <summary>
        /// 设置操作对象
        /// </summary>
        /// <param name="obj">控件对象</param>
        /// <param name="value">操作值</param>
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        /// <summary>
        /// 事件名称回调用事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnEventChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            //获取控件类型对象
            Type senderType = sender.GetType();

            if (!e.NewValue.ToString().Contains(","))
            {
                //反射得到控件添加操作的事件信息对象
                EventInfo eventInfo = senderType.GetEvent(e.NewValue.ToString());
                //为事件添加托管
                eventInfo.AddEventHandler(sender, GenerateDelegate(eventInfo));
            }
            else
            {
                foreach (var item in e.NewValue.ToString().Split(','))
                {
                    if (string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item))
                        continue;
                    _EventList.Add(item);

                    //反射得到控件添加操作的事件信息对象
                    EventInfo eventInfo = senderType.GetEvent(item.ToString());
                    //为事件添加托管
                    eventInfo.AddEventHandler(sender, GenerateDelegate(eventInfo));
                }
            }
        }

        /// <summary>
        /// 生成代理
        /// </summary>
        /// <param name="eventInfo">事件信息对象</param>
        /// <returns>返回创建的代理</returns>
        private static Delegate GenerateDelegate(EventInfo eventInfo)
        {
            //创建空代理
            Delegate result = null;
            //获取事件信息对象中的Invoke方法信息
            MethodInfo methodInfo = eventInfo.EventHandlerType.GetMethod("Invoke");
            //获取参数信息集合
            ParameterInfo[] parameters = methodInfo.GetParameters();
            //判断参数是否为2个
            //默认情况下所有控件的事件都为2个
            if (parameters.Length == 2)
            {
                //获取当前类的类型
                Type currentType = typeof(UIEventToCommand);
                //获取第2个参数的类型
                //默认情况下控件的事件参数第一个都为object类型
                Type argType = parameters[1].ParameterType;
                //获取调用事件方法信息对象
                MethodInfo eventRaisedMethod = currentType.GetMethod("OnEventRaised", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(argType);
                //设置代理
                result = Delegate.CreateDelegate(eventInfo.EventHandlerType, eventRaisedMethod, true);
            }
            return result;
        }
        /// <summary>
        /// 调用事件
        /// </summary>
        /// <typeparam name="T">事件参数类型</typeparam>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private static void OnEventRaised<T>(object sender, T arg) where T : EventArgs
        {
            //获取控件对象
            DependencyObject dependencyObject = sender as DependencyObject;

            if (dependencyObject != null)
            {
                //设置操作对象
                ICommand command = GetCommand(dependencyObject);
                if (command == null)
                    return;

                //判断是否可以执行
                if (command.CanExecute(sender))
                {
                    //调用操作
                    command.Execute(sender);
                }
                //if (command.CanExecute((sender as Control).DataContext))
                //{
                //    //调用操作
                //    command.Execute((sender as Control).DataContext);
                //}
            }
        }
    }
}
