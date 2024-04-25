using System.Collections.Concurrent;

namespace org.rsp.entity.Helper;

public class MyRedis<TKey, TValue> where TKey : notnull
{
    private static readonly ConcurrentDictionary<TKey, (TValue, Timer)> Store =new ();

    public async ValueTask SetAsync(TKey key, TValue value, TimeSpan? expiryTime)
    {
        Timer timer = null;
        
        //如果不传则设置为永久不过期，那就没必要设定定时器了
        if (expiryTime is null)
        {
            // 添加/更新键值对和定时器
            Store.AddOrUpdate(key, (value, timer), (_, _) => (value, timer));
        }
        else
        {
            timer = new Timer(OnTimerExpired!, key, expiryTime ?? TimeSpan.Zero, TimeSpan.FromMilliseconds(-1));
            // 添加/更新键值对和定时器
            Store.AddOrUpdate(key, (value, timer), (_, tuple) =>
            {
                tuple.Item2.Dispose(); // 先删除旧的定时器
                return (value, timer);
            });
        }
    }
    
    public void Set(TKey key, TValue value)
    {
        Timer timer = null;
        //如果不传则设置为永久不过期，那就没必要设定定时器了
        // 添加/更新键值对和定时器
        Store.AddOrUpdate(key, (value, timer), (_, _) => (value, timer));
    }
    

    public bool TryGet(TKey key, out TValue value)
    {
        if (Store.TryGetValue(key, out var tuple))
        {
            value = tuple.Item1;
            return true;
        }
        
        value = default;
        return false;
    }

    public bool Remove(TKey key)
    {
        if (Store.TryRemove(key, out var tuple))
        {
            tuple.Item2.Dispose(); // 删除定时器
            return true;
        }

        return false;
    }

    private void OnTimerExpired(object state)
    {
        var key = (TKey)state;
        //过期则删除token
        Store.TryRemove(key, out _);
    }
}