// Copyright (c) 2026 SynesthesiaDev <synesthesiadev@proton.me>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using Microsoft.AspNetCore.Components;

namespace Mocha.Components;

public abstract class HasContext : ComponentBase, IDisposable
{
    [Inject] protected MochaContext Context { get; init; } = null!;

    private IDisposable? subscriber;

    protected override void OnInitialized()
    {
        subscriber = Context.Subscribe(_ =>
        {
            OnContextChanged();
            InvokeAsync(StateHasChanged);
        });
    }

    protected virtual void OnContextChanged() { }

    public virtual void Dispose() => subscriber?.Dispose();
}
