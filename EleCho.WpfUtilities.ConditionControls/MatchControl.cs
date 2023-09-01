using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EleCho.WpfUtilities.ConditionControls
{
    [ContentProperty(nameof(MatchControl.Cases))]
    public partial class MatchControl : Control
    {
        static MatchControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MatchControl), new FrameworkPropertyMetadata(typeof(MatchControl)));
        }

        public MatchControl()
        {
            Cases = new MatchCaseCollection(this);
        }

        public object? Value
        {
            get { return (object?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public object? CurrentContent
        {
            get { return (object?)GetValue(CurrentContentProperty); }
            protected set { SetValue(CurrentContentProperty, value); }
        }

        public MatchCaseCollection Cases { get; }



        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(object), typeof(MatchControl), new PropertyMetadata(null, UpdateMatchControl));

        public static readonly DependencyProperty CurrentContentProperty =
            DependencyProperty.Register(nameof(CurrentContent), typeof(object), typeof(MatchControl), 
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));


        internal static void UpdateMatchControl(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not MatchControl switchControl)
                return;

            if (switchControl.Cases.MatchedCase is MatchCase switchCase)
                switchControl.CurrentContent = switchCase.Content;
            else
                switchControl.CurrentContent = null;
        }
    }
}
