using System;
using System.Windows;

namespace EleCho.WpfUtilities.ConditionControls
{
    public sealed class MatchCase : DependencyObject
    {
        public object? Value
        {
            get { return (object?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public object? Content
        {
            get { return (object?)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public int IntValue
        {
            get => Value is int value ? value : throw new InvalidOperationException("Value is not System.Int32");
            set => Value = value;
        }

        public float SingleValue
        {
            get => Value is float value ? value : throw new InvalidOperationException("Value is not System.Single");
            set => Value = value;
        }

        public double DoubleValue
        {
            get => Value is double value ? value : throw new InvalidOperationException("Value is not System.Double");
            set => Value = value;
        }

        public bool BooleanValue
        {
            get => Value is bool value ? value : throw new InvalidOperationException("Value is not System.Boolean");
            set => Value = value;
        }

        public MatchControl? Owner { get; internal set; }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(object), typeof(MatchCase), new PropertyMetadata(null, UpdateOwner));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(MatchCase), new PropertyMetadata(null, UpdateOwner));

        internal static void UpdateOwner(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not MatchCase switchCase)
                return;

            if (switchCase.Owner is MatchControl switchControl)
                MatchControl.UpdateMatchControl(switchControl, default);
        }
    }
}
