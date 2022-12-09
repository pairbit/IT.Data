using IT.Data.Benchmarks;

var rbe = new RedisBenchmark();

var sum = rbe.HashGet20Fields();
var sum2 = rbe.HashGetAll();

Console.WriteLine("Ok");

BenchmarkDotNet.Running.BenchmarkRunner.Run(typeof(RedisBenchmark));