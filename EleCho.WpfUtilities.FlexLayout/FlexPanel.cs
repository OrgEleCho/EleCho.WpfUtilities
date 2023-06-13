using System;
using System.Buffers;
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

namespace EleCho.WpfUtilities.FlexLayout
{
    public class FlexPanel : Panel
    {
        static FlexPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FlexPanel), new FrameworkPropertyMetadata(typeof(FlexPanel)));
        }

        #region Properties

        public FlexDirection Direction
        {
            get { return (FlexDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }

        public FlexWrap Wrap
        {
            get { return (FlexWrap)GetValue(WrapProperty); }
            set { SetValue(WrapProperty, value); }
        }

        public FlexMainAlignment MainAlignment
        {
            get { return (FlexMainAlignment)GetValue(MainAlignmentProperty); }
            set { SetValue(MainAlignmentProperty, value); }
        }

        public FlexCrossAlignment CrossAlignment
        {
            get { return (FlexCrossAlignment)GetValue(CrossAlignmentProperty); }
            set { SetValue(CrossAlignmentProperty, value); }
        }

        public FlexItemAlignment ItemsAlignment
        {
            get { return (FlexItemAlignment)GetValue(ItemsAlignmentProperty); }
            set { SetValue(ItemsAlignmentProperty, value); }
        }





        // Using a DependencyProperty as the backing store for MainAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MainAlignmentProperty =
            DependencyProperty.Register("MainAlignment", typeof(FlexMainAlignment), typeof(FlexPanel),
                new FrameworkPropertyMetadata(FlexMainAlignment.Start, FrameworkPropertyMetadataOptions.AffectsArrange));

        // Using a DependencyProperty as the backing store for CrossAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CrossAlignmentProperty =
            DependencyProperty.Register("CrossAlignment", typeof(FlexCrossAlignment), typeof(FlexPanel),
                new FrameworkPropertyMetadata(FlexCrossAlignment.Start, FrameworkPropertyMetadataOptions.AffectsArrange));

        // Using a DependencyProperty as the backing store for ItemsAlignment.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsAlignmentProperty =
            DependencyProperty.Register("ItemsAlignment", typeof(FlexItemAlignment), typeof(FlexPanel),
                new FrameworkPropertyMetadata(FlexItemAlignment.Start, FrameworkPropertyMetadataOptions.AffectsArrange));

        // Using a DependencyProperty as the backing store for Direction.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DirectionProperty =
            DependencyProperty.Register("Direction", typeof(FlexDirection), typeof(FlexPanel),
                new FrameworkPropertyMetadata(FlexDirection.Row, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        // Using a DependencyProperty as the backing store for Wrap.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WrapProperty =
            DependencyProperty.Register("Wrap", typeof(FlexWrap), typeof(FlexPanel),
                new FrameworkPropertyMetadata(FlexWrap.NoWrap, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));




        public static double? GetGrow(DependencyObject obj)
        {
            return (double?)obj.GetValue(GrowProperty);
        }

        public static void SetGrow(DependencyObject obj, double? value)
        {
            obj.SetValue(GrowProperty, value);
        }

        public static double? GetShrink(DependencyObject obj)
        {
            return (double?)obj.GetValue(ShrinkProperty);
        }

        public static void SetShrink(DependencyObject obj, double? value)
        {
            obj.SetValue(ShrinkProperty, value);
        }

        // Using a DependencyProperty as the backing store for Grow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GrowProperty =
            DependencyProperty.RegisterAttached("Grow", typeof(double?), typeof(FlexPanel),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentArrange));

        // Using a DependencyProperty as the backing store for Shrink.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShrinkProperty =
            DependencyProperty.RegisterAttached("Shrink", typeof(double?), typeof(FlexPanel),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsParentArrange));



        public double UniformGrow
        {
            get { return (double)GetValue(UniformGrowProperty); }
            set { SetValue(UniformGrowProperty, value); }
        }

        public double UniformShrink
        {
            get { return (double)GetValue(UniformShrinkProperty); }
            set { SetValue(UniformShrinkProperty, value); }
        }

        // Using a DependencyProperty as the backing store for UniformGrow.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UniformGrowProperty =
            DependencyProperty.Register("UniformGrow", typeof(double), typeof(FlexPanel),
                new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        // Using a DependencyProperty as the backing store for UniformShrink.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty UniformShrinkProperty =
            DependencyProperty.Register("UniformShrink", typeof(double), typeof(FlexPanel),
                new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsArrange));




        #endregion


        #region MeasureOverride

        Size MeasureOverrideOfRowDirection(Size availableSize)
        {
            Size panelDesiredSize = new Size();

            if (Wrap == FlexWrap.NoWrap)
            {
                foreach (UIElement child in InternalChildren)
                {
                    child.Measure(availableSize);

                    Size childDesiredSize = child.DesiredSize;
                    panelDesiredSize = new Size()
                    {
                        Width = panelDesiredSize.Width + childDesiredSize.Width,
                        Height = Math.Max(panelDesiredSize.Height, childDesiredSize.Height)
                    };
                }

                if (panelDesiredSize.Width > availableSize.Width)
                    panelDesiredSize.Width = availableSize.Width;

                return panelDesiredSize;
            }
            else
            {
                double aggregateHeight = 0;

                foreach (UIElement child in InternalChildren)
                {
                    child.Measure(availableSize);

                    Size childDesiredSize = child.DesiredSize;

                    if (panelDesiredSize.Width + childDesiredSize.Width > availableSize.Width)
                    {
                        aggregateHeight += panelDesiredSize.Height;
                        panelDesiredSize.Width = 0;
                        panelDesiredSize.Height = 0;
                    }

                    panelDesiredSize = new Size()
                    {
                        Width = panelDesiredSize.Width + childDesiredSize.Width,
                        Height = Math.Max(panelDesiredSize.Height, childDesiredSize.Height)
                    };
                }

                panelDesiredSize.Height += aggregateHeight;

                return panelDesiredSize;
            }
        }

        Size MeasureOverrideOfColumnDirection(Size availableSize)
        {
            Size panelDesiredSize = new Size();

            if (Wrap == FlexWrap.NoWrap)
            {
                foreach (UIElement child in InternalChildren)
                {
                    child.Measure(availableSize);

                    Size childDesiredSize = child.DesiredSize;
                    panelDesiredSize = new Size()
                    {
                        Width = Math.Max(panelDesiredSize.Width, childDesiredSize.Width),
                        Height = panelDesiredSize.Height + childDesiredSize.Height
                    };
                }

                if (panelDesiredSize.Height > availableSize.Height)
                    panelDesiredSize.Height = availableSize.Height;

                return panelDesiredSize;
            }
            else
            {
                double aggregateWidth = 0;

                foreach (UIElement child in InternalChildren)
                {
                    child.Measure(availableSize);

                    Size childDesiredSize = child.DesiredSize;

                    if (panelDesiredSize.Height + childDesiredSize.Height > availableSize.Height)
                    {
                        aggregateWidth += panelDesiredSize.Width;
                        panelDesiredSize.Width = 0;
                        panelDesiredSize.Height = 0;
                    }

                    panelDesiredSize = new Size()
                    {
                        Width = Math.Max(panelDesiredSize.Width, childDesiredSize.Width),
                        Height = panelDesiredSize.Height + childDesiredSize.Height
                    };
                }

                panelDesiredSize.Width += aggregateWidth;

                return panelDesiredSize;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return Direction switch
            {
                FlexDirection.Row or FlexDirection.RowReverse => MeasureOverrideOfRowDirection(availableSize),
                FlexDirection.Column or FlexDirection.ColumnReverse => MeasureOverrideOfColumnDirection(availableSize),
                _ => throw new InvalidOperationException("Invalid Direction")
            };
        }

        #endregion


        #region ArrangeOverride

        private record ChildrenLine
        {
            public ChildrenLine(UIElement[] elements, Size[] elementOffsets, Size[] elementSizes, double selfOffset, double selfSize)
            {
                Elements = elements;
                ElementOffsets = elementOffsets;
                ElementSizes = elementSizes;
                SelfOffset = selfOffset;
                SelfSize = selfSize;
            }

            public UIElement[] Elements { get; }
            public Size[] ElementOffsets { get; }
            public Size[] ElementSizes { get; }
            public double SelfOffset { get; set; }
            public double SelfSize { get; set; }
        }

        /// <summary>
        /// 横向的将元素分割成不同行
        /// </summary>
        /// <param name="children"></param>
        /// <param name="mainAxisPanelSize"></param>
        /// <returns></returns>
        ChildrenLine[] SplitChildrenWithRowDirection(UIElementCollection children, double mainAxisPanelSize)
        {
            Size currentLineSize = new Size();

            if (Wrap == FlexWrap.NoWrap)
            {
                UIElement[] lineChildren = new UIElement[children.Count];
                Size[] lineChildrenOffsets = new Size[children.Count];
                Size[] lineChildrenSizes = new Size[children.Count];

                for (int i = 0; i < children.Count; i++)
                {
                    var child = children[i];
                    var childDesiredSize = child.DesiredSize;

                    currentLineSize = new Size()
                    {
                        Width = currentLineSize.Width + childDesiredSize.Width,
                        Height = Math.Max(currentLineSize.Height, childDesiredSize.Height)
                    };

                    lineChildren[i] = child;
                    lineChildrenSizes[i] = childDesiredSize;
                }

                if (Direction == FlexDirection.RowReverse)
                {
                    Array.Reverse(lineChildren);
                    Array.Reverse(lineChildrenOffsets);
                    Array.Reverse(lineChildrenSizes);
                }

                return new ChildrenLine[]
                {
                    new ChildrenLine(lineChildren, lineChildrenOffsets, lineChildrenSizes, 0, currentLineSize.Height)
                };
            }
            else
            {
                bool isWrapReverse = Wrap == FlexWrap.WrapReverse;

                List<UIElement> currentLineChildren = new List<UIElement>();
                List<Size> currentLineChildrenSizes = new List<Size>();
                List<ChildrenLine> result = new List<ChildrenLine>();
                for (int i = 0; i < children.Count; i++)
                {
                    var child = children[i];
                    var childDesiredSize = child.DesiredSize;

                    // 长度上限, 该换行了
                    if (currentLineSize.Width + childDesiredSize.Width > mainAxisPanelSize && currentLineChildren.Count != 0)
                    {
                        if (Direction == FlexDirection.RowReverse)
                        {
                            currentLineChildren.Reverse();
                            currentLineChildrenSizes.Reverse();
                        }

                        result.Add(
                            new ChildrenLine(
                                currentLineChildren.ToArray(),
                                new Size[currentLineChildren.Count],
                                currentLineChildrenSizes.ToArray(),
                                0,
                                currentLineSize.Height));

                        // reset
                        currentLineChildren.Clear();
                        currentLineChildrenSizes.Clear();
                        currentLineSize = new Size();
                    }

                    // 增长大小
                    currentLineSize = new Size()
                    {
                        Width = currentLineSize.Width + childDesiredSize.Width,
                        Height = Math.Max(currentLineSize.Height, childDesiredSize.Height)
                    };

                    currentLineChildren.Add(child);
                    currentLineChildrenSizes.Add(childDesiredSize);
                }

                if (currentLineChildren.Count != 0)
                {
                    if (Direction == FlexDirection.RowReverse)
                    {
                        currentLineChildren.Reverse();
                        currentLineChildrenSizes.Reverse();
                    }

                    result.Add(
                        new ChildrenLine(
                            currentLineChildren.ToArray(),
                            new Size[currentLineChildren.Count],
                            currentLineChildrenSizes.ToArray(),
                            0,
                            currentLineSize.Height));
                }


                if (isWrapReverse)
                    result.Reverse();

                return result.ToArray();
            }
        }

        /// <summary>
        /// 纵向的将元素分割成不同行
        /// </summary>
        /// <param name="children"></param>
        /// <returns></returns>
        ChildrenLine[] SplitChildrenWithColumnDirection(UIElementCollection children, double mainAxisPanelSize)
        {
            Size currentLineSize = new Size();

            if (Wrap == FlexWrap.NoWrap)
            {
                UIElement[] lineChildren = new UIElement[children.Count];
                Size[] lineChildrenOffsets = new Size[children.Count];
                Size[] lineChildrenSizes = new Size[children.Count];

                for (int i = 0; i < children.Count; i++)
                {
                    var child = children[i];
                    var childDesiredSize = child.DesiredSize;

                    currentLineSize = new Size()
                    {
                        Width = Math.Max(currentLineSize.Width, childDesiredSize.Width),
                        Height = currentLineSize.Height + childDesiredSize.Height
                    };

                    lineChildren[i] = child;
                    lineChildrenSizes[i] = childDesiredSize;
                }

                return new ChildrenLine[]
                {
                    new ChildrenLine(lineChildren, lineChildrenOffsets, lineChildrenSizes, 0, currentLineSize.Width)
                };
            }
            else
            {
                bool isWrapReverse = Wrap == FlexWrap.WrapReverse;

                List<UIElement> currentLineChildren = new List<UIElement>();
                List<Size> currentLineChildrenSizes = new List<Size>();
                List<ChildrenLine> result = new List<ChildrenLine>();
                for (int i = 0; i < children.Count; i++)
                {
                    var child = children[i];
                    var childDesiredSize = child.DesiredSize;

                    // 长度上限, 该换行了
                    if (currentLineSize.Height + childDesiredSize.Height > mainAxisPanelSize && currentLineChildren.Count != 0)
                    {
                        result.Add(
                            new ChildrenLine(
                                currentLineChildren.ToArray(),
                                new Size[currentLineChildren.Count],
                                currentLineChildrenSizes.ToArray(),
                                0,
                                currentLineSize.Width));

                        // reset
                        currentLineChildren.Clear();
                        currentLineChildrenSizes.Clear();
                        currentLineSize = new Size();
                    }

                    // 增长大小
                    currentLineSize = new Size()
                    {
                        Width = Math.Max(currentLineSize.Width, childDesiredSize.Width),
                        Height = currentLineSize.Height + childDesiredSize.Height,
                    };

                    currentLineChildren.Add(child);
                    currentLineChildrenSizes.Add(childDesiredSize);
                }

                if (currentLineChildren.Count != 0)
                {
                    result.Add(
                        new ChildrenLine(
                            currentLineChildren.ToArray(),
                            new Size[currentLineChildren.Count],
                            currentLineChildrenSizes.ToArray(),
                            0,
                            currentLineSize.Width));
                }


                if (isWrapReverse)
                    result.Reverse();

                return result.ToArray();
            }
        }

        void ApplyGrowWithRowDirection(ChildrenLine childrenLine, double lineSize)
        {
            double remainingSize = lineSize - childrenLine.ElementSizes.Sum(size => size.Width);

            if (remainingSize < 0)
                return;

            double weightTotal = childrenLine.Elements.Sum(ele => GetGrow(ele) ?? UniformGrow);

            if (weightTotal == 0)
                return;

            for (int i = 0; i < childrenLine.ElementSizes.Length; i++)
            {
                UIElement currentElement = childrenLine.Elements[i];
                Size currentElementSize = childrenLine.ElementSizes[i];
                childrenLine.ElementSizes[i] = currentElementSize with
                {
                    Width = currentElementSize.Width + (GetGrow(currentElement) ?? UniformGrow) / weightTotal * remainingSize
                };
            }
        }

        void ApplyGrowWithColumnDirection(ChildrenLine childrenLine, double lineSize)
        {
            double remainingSize = lineSize - childrenLine.ElementSizes.Sum(size => size.Height);

            if (remainingSize < 0)
                return;

            double weightTotal = childrenLine.Elements.Sum(ele => GetGrow(ele) ?? UniformGrow);

            if (weightTotal == 0)
                return;

            for (int i = 0; i < childrenLine.ElementSizes.Length; i++)
            {
                UIElement currentElement = childrenLine.Elements[i];
                Size currentElementSize = childrenLine.ElementSizes[i];
                childrenLine.ElementSizes[i] = currentElementSize with
                {
                    Height = currentElementSize.Height + (GetGrow(currentElement) ?? UniformGrow) / weightTotal * remainingSize
                };
            }
        }

        void ApplyShrinkWithRowDirection(ChildrenLine childrenLine, double lineSize)
        {
            double sizeToShrink = childrenLine.ElementSizes.Sum(size => size.Width) - lineSize;

            if (sizeToShrink < 0)
                return;

            double weightTotal = Enumerable.Range(0, childrenLine.Elements.Length).Sum(index => (GetGrow(childrenLine.Elements[index]) ?? UniformGrow) * childrenLine.ElementSizes[index].Width);

            if (weightTotal == 0)
                return;

            for (int i = 0; i < childrenLine.ElementSizes.Length; i++)
            {
                UIElement currentElement = childrenLine.Elements[i];
                Size currentElementSize = childrenLine.ElementSizes[i];
                childrenLine.ElementSizes[i] = currentElementSize with
                {
                    Width = currentElementSize.Width - (GetGrow(currentElement) ?? UniformGrow) * currentElementSize.Width / weightTotal * sizeToShrink
                };
            }
        }

        void ApplyShrinkWithColumnDirection(ChildrenLine childrenLine, double lineSize)
        {
            double sizeToShrink = childrenLine.ElementSizes.Sum(size => size.Height) - lineSize;

            if (sizeToShrink < 0)
                return;

            double weightTotal = Enumerable.Range(0, childrenLine.Elements.Length).Sum(index => (GetGrow(childrenLine.Elements[index]) ?? UniformGrow) * childrenLine.ElementSizes[index].Height);

            if (weightTotal == 0)
                return;

            for (int i = 0; i < childrenLine.ElementSizes.Length; i++)
            {
                UIElement currentElement = childrenLine.Elements[i];
                Size currentElementSize = childrenLine.ElementSizes[i];
                childrenLine.ElementSizes[i] = currentElementSize with
                {
                    Height = currentElementSize.Height - (GetGrow(currentElement) ?? UniformGrow) * currentElementSize.Height / weightTotal * sizeToShrink
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childrenLine"></param>
        /// <param name="mainAxisPanelSize"></param>
        /// <returns>在主轴上的位置 (偏移量)</returns>
        void ApplyMainAlignmentWithRowDirection(ChildrenLine childrenLine, double mainAxisPanelSize)
        {
            double current = 0;
            for (int i = 0; i < childrenLine.ElementSizes.Length; i++)
            {
                childrenLine.ElementOffsets[i].Width = current;
                current += childrenLine.ElementSizes[i].Width;
            }

            if (MainAlignment == FlexMainAlignment.Start)
                return;

            double remainingSize = mainAxisPanelSize - childrenLine.ElementSizes.Sum(size => size.Width);

            if (MainAlignment == FlexMainAlignment.Center)
            {
                for (int i = 0; i < childrenLine.ElementOffsets.Length; i++)
                    childrenLine.ElementOffsets[i].Width += remainingSize / 2;
            }
            else if (MainAlignment == FlexMainAlignment.End)
            {
                for (int i = 0; i < childrenLine.ElementOffsets.Length; i++)
                    childrenLine.ElementOffsets[i].Width += remainingSize;
            }
            else if (MainAlignment == FlexMainAlignment.SpaceBetween)
            {
                if (remainingSize < 0)
                    return;

                double every = remainingSize / (childrenLine.ElementSizes.Length - 1);
                for (int i = 0; i < childrenLine.ElementOffsets.Length; i++)
                    childrenLine.ElementOffsets[i].Width += every * i;
            }
            else if (MainAlignment == FlexMainAlignment.SpaceAround)
            {
                if (remainingSize < 0)
                    return;

                double every = remainingSize / childrenLine.ElementSizes.Length;
                for (int i = 0; i < childrenLine.ElementOffsets.Length; i++)
                    childrenLine.ElementOffsets[i].Width += every * i + every / 2;
            }
            else
            {
                if (remainingSize < 0)
                    return;

                double every = remainingSize / (childrenLine.ElementSizes.Length + 1);
                for (int i = 0; i < childrenLine.ElementOffsets.Length; i++)
                    childrenLine.ElementOffsets[i].Width += every * i + every;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="childrenLine"></param>
        /// <param name="mainAxisPanelSize"></param>
        /// <returns>在主轴上的位置 (偏移量)</returns>
        void ApplyMainAlignmentWithColumnDirection(ChildrenLine childrenLine, double mainAxisPanelSize)
        {
            double current = 0;
            for (int i = 0; i < childrenLine.ElementSizes.Length; i++)
            {
                childrenLine.ElementOffsets[i].Height = current;
                current += childrenLine.ElementSizes[i].Height;
            }

            if (MainAlignment == FlexMainAlignment.Start)
                return;

            double remainingSize = mainAxisPanelSize - childrenLine.ElementSizes.Sum(size => size.Height);

            if (MainAlignment == FlexMainAlignment.Center)
            {
                for (int i = 0; i < childrenLine.ElementOffsets.Length; i++)
                    childrenLine.ElementOffsets[i].Height += remainingSize / 2;
            }
            else if (MainAlignment == FlexMainAlignment.End)
            {
                for (int i = 0; i < childrenLine.ElementOffsets.Length; i++)
                    childrenLine.ElementOffsets[i].Height += remainingSize;
            }
            else if (MainAlignment == FlexMainAlignment.SpaceBetween)
            {
                if (remainingSize < 0)
                    return;

                double every = remainingSize / (childrenLine.ElementSizes.Length - 1);
                for (int i = 0; i < childrenLine.ElementOffsets.Length; i++)
                    childrenLine.ElementOffsets[i].Height += every * i;
            }
            else if (MainAlignment == FlexMainAlignment.SpaceAround)
            {
                if (remainingSize < 0)
                    return;

                double every = remainingSize / childrenLine.ElementSizes.Length;
                for (int i = 0; i < childrenLine.ElementOffsets.Length; i++)
                    childrenLine.ElementOffsets[i].Height += every * i + every / 2;
            }
            else
            {
                if (remainingSize < 0)
                    return;

                double every = remainingSize / (childrenLine.ElementSizes.Length + 1);
                for (int i = 0; i < childrenLine.ElementOffsets.Length; i++)
                    childrenLine.ElementOffsets[i].Height += every * i + every;
            }
        }

        void ApplyCrossAlignment(ChildrenLine[] childrenLines, double crossAxisPanelSize)
        {
            double current = 0;
            for (int i = 0; i < childrenLines.Length; i++)
            {
                childrenLines[i].SelfOffset = current;
                current += childrenLines[i].SelfSize;
            }

            if (CrossAlignment == FlexCrossAlignment.Start)
                return;

            double remainingSize = crossAxisPanelSize - childrenLines.Sum(line => line.SelfSize);

            if (CrossAlignment == FlexCrossAlignment.Center)
            {
                for (int i = 0; i < childrenLines.Length; i++)
                    childrenLines[i].SelfOffset += remainingSize / 2;
            }
            else if (CrossAlignment == FlexCrossAlignment.End)
            {
                for (int i = 0; i < childrenLines.Length; i++)
                    childrenLines[i].SelfOffset += remainingSize;
            }
            else if (CrossAlignment == FlexCrossAlignment.SpaceBetween)
            {
                if (remainingSize < 0)
                    return;

                double every = remainingSize / (childrenLines.Length - 1);
                for (int i = 0; i < childrenLines.Length; i++)
                    childrenLines[i].SelfOffset += every * i;
            }
            else if (CrossAlignment == FlexCrossAlignment.SpaceAround)
            {
                if (remainingSize < 0)
                    return;

                double every = remainingSize / childrenLines.Length;
                for (int i = 0; i < childrenLines.Length; i++)
                    childrenLines[i].SelfOffset += every * i + every / 2;
            }
            else if (CrossAlignment == FlexCrossAlignment.SpaceEvently)
            {
                if (remainingSize < 0)
                    return;

                double every = remainingSize / (childrenLines.Length + 1);
                for (int i = 0; i < childrenLines.Length; i++)
                    childrenLines[i].SelfOffset += every * i + every;
            }
            else
            {
                double totalWeight = childrenLines.Sum(line => line.SelfSize);
                double lineOffsetCurrent = 0;
                for (int i = 0; i < childrenLines.Length; i++)
                {
                    double growSize = remainingSize * (childrenLines[i].SelfSize / totalWeight);

                    childrenLines[i].SelfOffset += lineOffsetCurrent;
                    childrenLines[i].SelfSize += growSize;
                    lineOffsetCurrent += growSize;
                }
            }
        }

        void ApplyItemsAlignmentWithRowDirection(ChildrenLine childrenLine)
        {
            if (ItemsAlignment == FlexItemAlignment.Start)
                return;

            if (ItemsAlignment == FlexItemAlignment.Center)
            {
                for (int i = 0; i < childrenLine.Elements.Length; i++)
                {
                    double remainingSpace = childrenLine.SelfSize - childrenLine.ElementSizes[i].Height;
                    childrenLine.ElementOffsets[i].Height += remainingSpace / 2;
                }
            }
            else if (ItemsAlignment == FlexItemAlignment.End)
            {
                for (int i = 0; i < childrenLine.Elements.Length; i++)
                {
                    double remainingSpace = childrenLine.SelfSize - childrenLine.ElementSizes[i].Height;
                    childrenLine.ElementOffsets[i].Height += remainingSpace;
                }
            }
            else
            {
                for (int i = 0; i < childrenLine.Elements.Length; i++)
                {
                    childrenLine.ElementSizes[i].Height = childrenLine.SelfSize;
                }
            }
        }

        void ApplyItemsAlignmentWithColumnDirection(ChildrenLine childrenLine)
        {
            if (ItemsAlignment == FlexItemAlignment.Start)
                return;

            if (ItemsAlignment == FlexItemAlignment.Center)
            {
                for (int i = 0; i < childrenLine.Elements.Length; i++)
                {
                    double remainingSpace = childrenLine.SelfSize - childrenLine.ElementSizes[i].Width;
                    childrenLine.ElementOffsets[i].Width += remainingSpace / 2;
                }
            }
            else if (ItemsAlignment == FlexItemAlignment.End)
            {
                for (int i = 0; i < childrenLine.Elements.Length; i++)
                {
                    double remainingSpace = childrenLine.SelfSize - childrenLine.ElementSizes[i].Width;
                    childrenLine.ElementOffsets[i].Width += remainingSpace;
                }
            }
            else
            {
                for (int i = 0; i < childrenLine.Elements.Length; i++)
                {
                    childrenLine.ElementSizes[i].Width = childrenLine.SelfSize;
                }
            }
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Direction is FlexDirection.Row or FlexDirection.RowReverse)
            {
                // 分割
                ChildrenLine[] lines = SplitChildrenWithRowDirection(InternalChildren, finalSize.Width);

                // 对每一行进行 grow, shrink, 以及主轴对齐计算
                for (int i = 0; i < lines.Length; i++)
                {
                    ApplyGrowWithRowDirection(lines[i], finalSize.Width);
                    ApplyShrinkWithRowDirection(lines[i], finalSize.Width);
                    ApplyMainAlignmentWithRowDirection(lines[i], finalSize.Width);
                }

                // 交叉轴对齐计算
                ApplyCrossAlignment(lines, finalSize.Height);

                // Items 对齐计算
                for (int i = 0; i < lines.Length; i++)
                    ApplyItemsAlignmentWithRowDirection(lines[i]);

                for (int i = 0; i < lines.Length; i++)
                {
                    ChildrenLine currentLine = lines[i];

                    for (int j = 0; j < currentLine.Elements.Length; j++)
                    {
                        currentLine.Elements[j].Arrange(
                            new Rect()
                            {
                                X = currentLine.ElementOffsets[j].Width,
                                Y = currentLine.SelfOffset + currentLine.ElementOffsets[j].Height,
                                Size = currentLine.ElementSizes[j]
                            });
                    }
                }
            }
            else
            {
                ChildrenLine[] lines = SplitChildrenWithColumnDirection(InternalChildren, finalSize.Height);

                for (int i = 0; i < lines.Length; i++)
                {
                    ApplyGrowWithColumnDirection(lines[i], finalSize.Height);
                    ApplyShrinkWithColumnDirection(lines[i], finalSize.Height);
                    ApplyMainAlignmentWithColumnDirection(lines[i], finalSize.Height);
                }

                ApplyCrossAlignment(lines, finalSize.Width);

                for (int i = 0; i < lines.Length; i++)
                    ApplyItemsAlignmentWithColumnDirection(lines[i]);

                for (int i = 0; i < lines.Length; i++)
                {
                    ChildrenLine currentLine = lines[i];

                    for (int j = 0; j < currentLine.Elements.Length; j++)
                    {
                        currentLine.Elements[j].Arrange(
                            new Rect()
                            {
                                X = currentLine.SelfOffset + currentLine.ElementOffsets[j].Width,
                                Y = currentLine.ElementOffsets[j].Height,
                                Size = currentLine.ElementSizes[j]
                            });
                    }
                }
            }

            return finalSize;
        }

        #endregion
    }
}
