using CliFx;

await new CliApplicationBuilder().AddCommandsFromThisAssembly().Build()
    .RunAsync(new[] { "decompile", "MyClass.class" });