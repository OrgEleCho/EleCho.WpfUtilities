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

namespace EleCho.WpfUtilities.ConditionControls
{
    public class IfControl : Control
    {
        static IfControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IfControl), new FrameworkPropertyMetadata(typeof(IfControl)));
        }

        public bool Condition
        {
            get { return (bool)GetValue(ConditionProperty); }
            set { SetValue(ConditionProperty, value); }
        }

        public object? Then
        {
            get { return (object?)GetValue(ThenProperty); }
            set { SetValue(ThenProperty, value); }
        }

        public object? Else
        {
            get { return (object?)GetValue(ElseProperty); }
            set { SetValue(ElseProperty, value); }
        }

        public object? CurrentContent
        {
            get { return (object?)GetValue(CurrentContentProperty); }
            protected set { SetValue(CurrentContentProperty, value); }
        }




        public static readonly DependencyProperty ConditionProperty =
            DependencyProperty.Register(nameof(Condition), typeof(bool), typeof(IfControl), new PropertyMetadata(true, IfControlUpdate));

        public static readonly DependencyProperty ThenProperty =
            DependencyProperty.Register(nameof(Then), typeof(object), typeof(IfControl), new PropertyMetadata(null, IfControlUpdate));

        public static readonly DependencyProperty ElseProperty =
            DependencyProperty.Register(nameof(Else), typeof(object), typeof(IfControl), new PropertyMetadata(null, IfControlUpdate));

        public static readonly DependencyProperty CurrentContentProperty =
            DependencyProperty.Register(nameof(CurrentContent), typeof(object), typeof(IfControl), new PropertyMetadata(null));

        private static void IfControlUpdate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not IfControl conditionControl)
                return;

            if (conditionControl.Condition)
            {
                if (conditionControl.Then is object content)
                    conditionControl.CurrentContent = content;
                else if (ReferenceEquals(conditionControl.CurrentContent, conditionControl.Else))
                {
                    conditionControl.CurrentContent = null;
                }
            }
            else
            {
                if (conditionControl.Else is object content)
                    conditionControl.CurrentContent = content;
                else if (ReferenceEquals(conditionControl.CurrentContent, conditionControl.CurrentContent))
                {
                    conditionControl.CurrentContent = null;
                }
            }
        }
    }
}
