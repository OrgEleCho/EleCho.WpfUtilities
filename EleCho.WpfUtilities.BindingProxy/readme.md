# EleCho.WpfUtilities.BindingProxy

Types:

- EleCho.WpfUtilities.BindingProxy


## Usage

Using namespace:

```xml
<Window
    xmlns:elechowpf="clrnamespace:EleCho.WpfUtilities">
   ...
</Window>
```


Add BindingProxy to static resources:

```xml
<Window.Resources>
    <BindingProxy x:Key="Proxy" Data="{Binding}" />
</Window.Resources>
```

Now you can use your data context as a static resource.

```xml
<ContextMenu>
    <MenuItem Header="Test" 
              Command="{Binding Source={StaticResource Proxy}, Path=Data.MyCommand}"/>
</ContextMenu>
```