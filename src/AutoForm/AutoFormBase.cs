using Microsoft.AspNetCore.Components;

namespace AutoForm;

public abstract class AutoFormBase<T> : ComponentBase, IDisposable
{
    //protected T Clone { get; private set; } = null!;

    public bool Loading { get; set; }

    [Parameter]
    [EditorRequired]
    public T Model { get; set; } = default!;

    public AutoFields<T>? AutoFields { get; set; }

    [Parameter]
    public AutoFieldOptions? Options { get; set; }

    [Parameter]
    public Action? OnValidSubmit { get; set; }

    [Parameter]
    public Action? OnInvalidSubmit { get; set; }

    public void Dispose()
    {
        //Model = Clone;
    }

    public async Task CallValidSubmit()
    {
        OnValidSubmit?.Invoke();
    }
}