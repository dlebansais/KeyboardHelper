# KeyboardHelper
Adds support for sequence of gestures and keyboard events to WPF controls that are not TextBox

## MultiKeyGesture

The `MultiKeyGesture` class is a replacement for KeyGesture when a sequence of key is needed, rather than just one key pressed. For example, the sequence `Ctrl+B, T` is used in Visual Studio to toggle a bookmark. There not support for this feature in the KeyGesture class of .NET, but you can do it with `MultiKeyGesture`.

To use this class, proceed as follow:
+ Add the KeyboardHelper assembly to your project.
+ Add the following line in the header of your Xaml file:
`xmlns:kh="clr-namespace:KeyboardHelper.Xaml;assembly=KeyboardHelper"`
+ In the rest of the xaml file, whenever you would write `Gesture="..."`, write `Gesture="{kh:MultiKeyGesture ...}"` instead.

Note that `kh:` is a prefix I chose for this example. You can change it if you want.

Complete example:

``` cs
<UserControl x:Class="Test.MyControl"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:kh="clr-namespace:KeyboardHelper.Xaml;assembly=KeyboardHelper">
    <UserControl.InputBindings>
        <KeyBinding Command="EditingCommands.Delete" Gesture="{kh:MultiKeyGesture Ctrl+D}"/>
        <KeyBinding Command="EditingCommands.ToggleInsert" Gesture="{kh:MultiKeyGesture Ctrl+E, W}"/>
        <KeyBinding Command="EditingCommands.Backspace" Gesture="{kh:MultiKeyGesture Ctrl+F, Ctrl+X}"/>
    </UserControl.InputBindings>
    <Grid/>
</UserControl>
```

## Receiving keyboard events as characters

.NET provides keyboard events such as `KeyDown` and `KeyUp`, but handling them to parse characters the same way a `TextBox` control would is a little tricky.

To help with that, you can create a `KeyboardManager` object, associated to your control, and register a handler for the `CharacterKey` event. This event will be triggered every time a character is obtained from the keyboard:
+ Normally, like when pressing the `a` key.
+ With composed keys, for example by pressing the `^` and `a` keys on a French keyboard, resulting in the `â` character.
+ With unicode character types using the `Alt` key and the numpad. For example, typing `6` and `4` on the numpad while holding the `Alt` key down, then releasing it, will result in the `@` character.

## Receiving move keys as events

You can register commands for all combinations of the `Left`, `Right`, `Up`, `Down`, `PageUp`, `PageDown`, `Home` and `End` keys with `Ctrl` and `Shift`, but it's a lot of events handlers to write and maintain. Instead, you can register a handler for the `MoveKey` event of the `KeyboardManager` class, and process all of them in a central place.
