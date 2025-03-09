namespace blazor.utils;

public class Debouncer : IDisposable
{
    private readonly object _syncLock = new();
    private Action? _action;
    private int _delay;
    private CancellationTokenSource? _cts;
    private bool _isDisposed;

    public void Debounce(Action action, int delay = 250)
    {
        if (action is null || _isDisposed) { return; }

        _action = action;
        _delay = delay;

        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        Task.Run(() => DebounceAsync(_cts.Token), _cts.Token);
    }

    private async Task DebounceAsync(CancellationToken token)
    {
        try
        {
            await Task.Delay(_delay, token).ConfigureAwait(false);

            lock (_syncLock)
            {
                if (!token.IsCancellationRequested)
                {
                    _action?.Invoke();
                    _action = null;
                }
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

    public void Dispose()
    {
        lock (_syncLock)
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _cts?.Cancel();
                _cts?.Dispose();
                _action = null;
            }
        }
    }
}