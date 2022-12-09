using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using StackExchange.Redis;

namespace IT.Data.Benchmarks;

[MemoryDiagnoser]
[MinColumn, MaxColumn]
[Orderer(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
public class RedisBenchmark
{
    private IDatabase _db;

    public RedisBenchmark()
    {
        var conn = ConnectionMultiplexer.Connect("data.stage.sign:6379,defaultDatabase=0,syncTimeout=5000,allowAdmin=False,connectTimeout=5000,ssl=False,abortConnect=False");

        _db = conn.GetDatabase();

        if (!_db.KeyExists("HashGet"))
        {
            var fields = new HashEntry[20];

            for (int i = 0; i < fields.Length; i++)
            {
                var no = i + 1;
                fields[i] = new HashEntry($"field{no}", no);
            }

            _db.HashSet("HashGet", fields);
        }
    }

    [Benchmark]
    public int HashGet20Fields()
    {
        var values = _db.HashGet("HashGet", new RedisValue[] {
            "field1", "field2", "field3", "field4", "field5", "field6", "field7", "field8", "field9", "field10",
            "field11", "field12", "field13", "field14", "field15", "field16", "field17", "field18", "field19", "field20"
        });

        var sum = 0;

        foreach (var value in values)
        {
            var number = (int)value;
            sum += number;
        }

        return sum;
    }

    [Benchmark]
    public int HashGetAll()
    {
        var values = _db.HashGetAll("HashGet");

        var sum = 0;

        foreach (var value in values)
        {
            var number = (int)value.Value;
            sum += number;
        }

        return sum;
    }

    [Benchmark]
    public void Sleep100()
    {
        Thread.Sleep(100);
    }
}