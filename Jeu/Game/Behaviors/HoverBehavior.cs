namespace Game.Behaviors;

public static class HoverBehavior
{
    public static readonly BindableProperty CursorHoverTypeProperty =
        BindableProperty.CreateAttached(
            "CursorHoverType",
            typeof(string),
            typeof(HoverBehavior),
            null,
            propertyChanged: OnCursorHoverTypeChanged);

    public static string GetCursorHoverType(BindableObject obj)
    {
        return (string)obj.GetValue(CursorHoverTypeProperty);
    }

    public static void SetCursorHoverType(BindableObject obj, string value)
    {
        obj.SetValue(CursorHoverTypeProperty, value);
    }

    private static void OnCursorHoverTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not View view || newValue is null)
            return;

        var hoverCursorType = (string)newValue;
        var pointerGesture = new PointerGestureRecognizer();

        pointerGesture.PointerEntered += (s, e) => {
#if WINDOWS
            if (view.Handler?.PlatformView is Microsoft.UI.Xaml.UIElement nativeView)
            {
                SetCursor(nativeView, hoverCursorType);
            }
#endif
        };

        pointerGesture.PointerExited += (s, e) => {
#if WINDOWS
            if (view.Handler?.PlatformView is Microsoft.UI.Xaml.UIElement nativeView)
            {
                SetCursor(nativeView, "Arrow");
            }
#endif
        };

        view.GestureRecognizers.Add(pointerGesture);
    }

#if WINDOWS
    private static void SetCursor(Microsoft.UI.Xaml.UIElement nativeView, string cursorShapeName)
    {
        try
        {
            var cursorShape = (Microsoft.UI.Input.InputSystemCursorShape)Enum.Parse(
                typeof(Microsoft.UI.Input.InputSystemCursorShape), 
                cursorShapeName, 
                ignoreCase: true);

            var cursorProperty = typeof(Microsoft.UI.Xaml.UIElement).GetProperty("ProtectedCursor",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            if (cursorProperty != null && cursorProperty.CanWrite)
            {
                var cursor = Microsoft.UI.Input.InputSystemCursor.Create(cursorShape);
                cursorProperty.SetValue(nativeView, cursor);
            }
        }
        catch
        {
        }
    }
#endif
}
