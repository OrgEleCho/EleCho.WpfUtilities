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