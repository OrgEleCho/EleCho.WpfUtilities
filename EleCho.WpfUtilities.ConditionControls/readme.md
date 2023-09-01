# EleCho.WpfUtilities.ConditionControls

Types:

- EleCho.WpfUtilities.ConditionControls.IfControl
- EleCho.WpfUtilities.ConditionControls.IfNotControl
- EleCho.WpfUtilities.ConditionControls.MatchControl

## Usage

Using namespace:

```xml
<Window
    xmlns:condition="clrnamespace:EleCho.WpfUtilities.ConditionControls">
    ...
</Window>
```

`IfControl` can display different content depending on whether the condition is true or false.

```xml
<CheckBox Content="Test" Name="checkbox"/>
<condition:IfControl Condition="{Binding ElementName=checkbox,Path=IsChecked}">
    <condition:IfControl.Then>
        <TextBlock Text="QWQ"/>
    </condition:IfControl.Then>
</condition:IfControl>
```

> 'IfNotControl' just reversed the result of 'IfControl'

'MatchControl' can match different values and display different contents.

```xml
<Slider Width="120" Name="slider" SmallChange="1" Value="1" LargeChange="2"/>
<TextBlock Text="{Binding ElementName=slider,Path=Value}" />
<condition:MatchControl Value="{Binding ElementName=slider,Path=Value}">
    <condition:MatchCase DoubleValue="1.0" Content="Hello"/>
    <condition:MatchCase DoubleValue="2.0" Content="World"/>
</condition:MatchControl>
```