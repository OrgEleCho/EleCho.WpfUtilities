# EleCho.WpfUtilities.FlexLayout

Implementation of Flex Layout in WPF

## Usage

1. Add namespace reference.

```xml
<Window
  xmlns:flex="clr-namespace:EleCho.WpfUtilities.FlexLayout;assembly=EleCho.WpfUtilities.FlexLayout">
</Window>
```

2. Add `FlexPanel` to your xaml

```xml
<flex:FlexPanel Direction="Row" MainAlignment="SpaceBetween" CrossAlignment="Start" ItemsAlignment="Start" Wrap="Wrap">
    <Rectangle Fill="Pink" Width="200" Height="200"/>
    <Rectangle Fill="Purple" Width="150" Height="150"/>
    <Rectangle Fill="BlueViolet" Width="200" Height="200"/>
    <Rectangle Fill="Fuchsia" Width="150" Height="150"/>
    <Rectangle Fill="BlueViolet" Width="200" Height="200"/>
</flex:FlexPanel>
```

## Reference

| Property | Description | CSS Property |
| --- | --- | --- |
| Direction | The layout direction of the FlexPanel | flex-direction |
| Wrap | Whether or not to wrap items onto multiple lines | flex-wrap |
| MainAlignment | The alignment of content along the main axis | justify-content |
| CrossAlignment | The alignment of content along the cross axis | align-content |
| ItemsAlignment | The alignment of individual items along the cross axis | align-items |
| SelfAlignment | The alignment of an individual item along the cross axis | align-self |
| UniformGrow | The default grow value for items in the container | grow |
| UniformShrink | The default shrink value for items in the container | shrink |
| Grow | An additional property for specifying an item's individual grow value | grow |
| Shrink | An additional property for specifying an item's individual shrink value | shrink |