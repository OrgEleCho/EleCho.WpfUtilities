using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace EleCho.WpfUtilities.ConditionControls
{
    public class IfNotControl : ContentControl
    {
        static IfNotControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IfNotControl), new FrameworkPropertyMetadata(typeof(IfNotControl)));
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
            DependencyProperty.Register(nameof(Condition), typeof(bool), typeof(IfNotControl), new PropertyMetadata(true, IfNotControlUpdate));

        public static readonly DependencyProperty ThenProperty =
            DependencyProperty.Register(nameof(Then), typeof(object), typeof(IfNotControl), new PropertyMetadata(null, IfNotControlUpdate));

        public static readonly DependencyProperty ElseProperty =
            DependencyProperty.Register(nameof(Else), typeof(object), typeof(IfNotControl), new PropertyMetadata(null, IfNotControlUpdate));

        public static readonly DependencyProperty CurrentContentProperty =
            DependencyProperty.Register(nameof(CurrentContent), typeof(object), typeof(IfControl), new PropertyMetadata(null));


        private static void IfNotControlUpdate(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not IfNotControl conditionControl)
                return;

            if (!conditionControl.Condition)
            {
                if (conditionControl.Then is object content)
                    conditionControl.Content = content;
                else if (ReferenceEquals(conditionControl.Content, conditionControl.Else))
                {
                    conditionControl.Content = null;
                }
            }
            else
            {
                if (conditionControl.Else is object content)
                    conditionControl.Content = content;
                else if (ReferenceEquals(conditionControl.Content, conditionControl.Content))
                {
                    conditionControl.Content = null;
                }
            }
        }
    }
}
