// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;
using Smdn.OperatingSystem;

namespace Smdn;

[TestFixture()]
public class RuntimeTests {
  [Test]
  public void OutputRuntimeEnvironment()
  {
    TestContext.Out.WriteLine("[Runtime environment information]");
    TestContext.Out.WriteLine($"{nameof(RuntimeInformation)}.{nameof(RuntimeInformation.FrameworkDescription)}: {RuntimeInformation.FrameworkDescription}");

#if SYSTEM_ASSEMBLY_GETREFERENCEDASSEMBLIES
    TestContext.Out.WriteLine(
      "ReferencedAssemblies(entry assembly): {0}",
      string.Join(
        ", ",
        Assembly
          .GetEntryAssembly()
          ?.GetReferencedAssemblies()
          ?.Select(static a => a.Name) ?? Enumerable.Empty<string>()
      )
    );
    TestContext.Out.WriteLine(
      "ReferencedAssemblies(Runtime class): {0}",
      string.Join(
        ", ",
        typeof(Runtime)
          .GetTypeInfo()
          .Assembly
          .GetReferencedAssemblies()
          .Select(static a => a.Name)
      )
    );
#endif

#if SYSTEM_ENVIRONMENT_VERSION
    TestContext.Out.WriteLine($"{nameof(Environment)}.{nameof(Environment.Version)}: {Environment.Version}");
#endif

    TestContext.Out.WriteLine($"{nameof(Runtime.RuntimeEnvironment)}: {Runtime.RuntimeEnvironment}");
    TestContext.Out.WriteLine($"{nameof(Runtime.Name)}: {Runtime.Name}");
    TestContext.Out.WriteLine($"{nameof(Runtime.Version)}: {Runtime.Version}");
    TestContext.Out.WriteLine($"{nameof(Runtime.VersionString)}: {Runtime.VersionString}");

    Assert.Inconclusive("see output");
  }

  [Test]
  public void TestIsRunningOnRuntime()
  {
    switch (Runtime.RuntimeEnvironment) {
      case RuntimeEnvironment.NetFx:
        Assert.That(Runtime.IsRunningOnNetFx, Is.True, nameof(Runtime.IsRunningOnNetFx));
        Assert.That(Runtime.IsRunningOnNetCore, Is.False, nameof(Runtime.IsRunningOnNetCore));
        Assert.That(Runtime.IsRunningOnMono, Is.False, nameof(Runtime.IsRunningOnMono));
        Assert.That(Runtime.IsRunningOnDotNet5OrOver, Is.False, nameof(Runtime.IsRunningOnDotNet5OrOver));
        break;

      case RuntimeEnvironment.NetCore:
        Assert.That(Runtime.IsRunningOnNetCore, Is.True, nameof(Runtime.IsRunningOnNetCore));
        Assert.That(Runtime.IsRunningOnNetFx, Is.False, nameof(Runtime.IsRunningOnNetFx));
        Assert.That(Runtime.IsRunningOnMono, Is.False, nameof(Runtime.IsRunningOnMono));

        if (RuntimeInformation.FrameworkDescription.Contains(".NET Core"))
          Assert.That(Runtime.IsRunningOnDotNet5OrOver, Is.False, nameof(Runtime.IsRunningOnDotNet5OrOver));
        else
          Assert.That(Runtime.IsRunningOnDotNet5OrOver, Is.True, nameof(Runtime.IsRunningOnDotNet5OrOver));
        break;

      case RuntimeEnvironment.Mono:
        Assert.That(Runtime.IsRunningOnMono, Is.True, nameof(Runtime.IsRunningOnMono));
        Assert.That(Runtime.IsRunningOnNetFx, Is.False, nameof(Runtime.IsRunningOnNetFx));
        Assert.That(Runtime.IsRunningOnNetCore, Is.False, nameof(Runtime.IsRunningOnNetCore));
        Assert.That(Runtime.IsRunningOnDotNet5OrOver, Is.False, nameof(Runtime.IsRunningOnDotNet5OrOver));
        break;

      default:
        Assert.That(Runtime.IsRunningOnMono, Is.False, nameof(Runtime.IsRunningOnMono));
        Assert.That(Runtime.IsRunningOnNetFx, Is.False, nameof(Runtime.IsRunningOnNetFx));
        Assert.That(Runtime.IsRunningOnNetCore, Is.False, nameof(Runtime.IsRunningOnNetCore));
        Assert.That(Runtime.IsRunningOnDotNet5OrOver, Is.False, nameof(Runtime.IsRunningOnDotNet5OrOver));
        break;
    }
  }

  [Test]
  public void TestVersionString()
  {
    // returns non-null value always
    Assert.That(Runtime.VersionString, Is.Not.Null);

    var version = Runtime.VersionString.ToLowerInvariant();

    switch (Runtime.RuntimeEnvironment) {
      case RuntimeEnvironment.Mono:
        Assert.That(version, Does.Contain("mono"));
        break;
      case RuntimeEnvironment.NetFx:
        Assert.That(version, Does.Contain(".net framework"));
        break;
      case RuntimeEnvironment.NetCore:
        Assert.That(version, Does.Contain(".net")); // .NET Core, .NET
        break;
      default:
        TestContext.Out.WriteLine($"[Test: {nameof(TestVersionString)}]");
        TestContext.Out.WriteLine($"{nameof(Runtime.VersionString)}: {Runtime.VersionString}");
        Assert.Inconclusive("see output");
        break;
    }
  }

  [Test]
  public void TestName()
  {
    // returns non-null value always
    Assert.That(Runtime.Name, Is.Not.Null);

    var name = Runtime.Name.ToLowerInvariant();

    switch (Runtime.RuntimeEnvironment) {
      case RuntimeEnvironment.Mono:
        Assert.That(name, Does.Contain("mono"));
        break;
      case RuntimeEnvironment.NetFx:
        Assert.That(name, Does.Contain(".net framework"));
        break;
      case RuntimeEnvironment.NetCore:
        if (Runtime.Version is null) {
          TestContext.Out.WriteLine($"[Test: {nameof(TestName)}]");
          TestContext.Out.WriteLine($"{nameof(Runtime.Name)}: {Runtime.Name}");

          Assert.Inconclusive("see output");
        }
        else {
          if (5 <= Runtime.Version.Major)
            Assert.That(name, Does.Contain(".net"));
          else
            Assert.That(name, Does.Contain(".net core"));
        }
        break;
      default:
#if NETFRAMEWORK
        Assert.That(name, Does.Contain("compatible"));
#else
        Assert.That(name, Does.Contain(".net"));
#endif
        break;
    }
  }

  [Test]
  public void TestVersion()
  {
#if SYSTEM_ENVIRONMENT_VERSION
    Assert.That(Runtime.Version, Is.Not.Null);
#else
    TestContext.Out.WriteLine($"[Test: {nameof(TestVersion)}]");
    TestContext.Out.WriteLine($"{nameof(Runtime.Version)}: {Runtime.Version}");

    Assert.Inconclusive("see output");

    return;
#endif

    var version = Runtime.Version!;

    Assert.That(version.Major, Is.Not.EqualTo(0), nameof(version.Major));
    Assert.That(version.Minor, Is.Not.EqualTo(-1), nameof(version.Minor));

    switch (Runtime.RuntimeEnvironment) {
      case RuntimeEnvironment.Mono:
        Assert.That(Runtime.VersionString, Does.Contain(version.ToString()), "Mono verion string");
        break;

      case RuntimeEnvironment.NetFx:
        Assert.That(version.Major, Is.LessThan(5), ".NET Framework major verion must be less than 5");
        Assert.That(Runtime.VersionString, Does.Contain(version.ToString()), ".NET Framework verion string");
        break;

      case RuntimeEnvironment.NetCore:
        if (!Runtime.Name.Contains(".NET Core"))
          Assert.That(version.Major, Is.GreaterThanOrEqualTo(5), ".NET major verion must be greater than or equal to 5");
#if SYSTEM_ENVIRONMENT_VERSION
        Assert.That(version, Is.EqualTo(Environment.Version), "CoreCLR version must be equal to Environment.Version");
#endif
        Assert.That(Runtime.VersionString, Does.Contain(version.ToString()), "CoreCLR version string");
        break;

      default:
        TestContext.Out.WriteLine($"[Test: {nameof(TestVersion)}]");
        TestContext.Out.WriteLine($"{nameof(Runtime.Version)}: {Runtime.Version}");

        Assert.Inconclusive("see output");
        break;
    }
  }

  [Test]
  public void SupportsIanaTimeZoneName()
  {
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      SupportsIanaTimeZoneName_Windows();
    else
      SupportsIanaTimeZoneName_NonWindowsOS();
  }

  private void SupportsIanaTimeZoneName_Windows()
  {
    if (Runtime.IsRunningOnNetFx)
      // .NET Framework
      Assert.That(Runtime.SupportsIanaTimeZoneName, Is.False, ".NET Framework does not support IANA time zone name");
    else if (Runtime.IsRunningOnDotNet5OrOver)
      // .NET >= 5.0
      Assert.Inconclusive(".NET on Windows supports IANA time zone name, but is configurable");
    else
      // .NET Core
      Assert.That(Runtime.SupportsIanaTimeZoneName, Is.False, ".NET Core on Windows does not support IANA time zone name");
  }

  private void SupportsIanaTimeZoneName_NonWindowsOS()
    => Assert.That(Runtime.SupportsIanaTimeZoneName, Is.True, "Mono or .NET on non-windows OS supports IANA time zone name");

  private static TResult ExecutePrintRuntimeInformation<TResult>(
    string[] args,
    string[] buildAdditionalProperties,
    IReadOnlyDictionary<string, string> environmentVariables,
    Func<string, TResult> getResult
  )
  {
    int exitCode;
    string stdout, stderr;

    static IReadOnlyDictionary<string, string> GetEnvironmentVariablesForDotnetCommand(
      IReadOnlyDictionary<string, string> additionalEnvironmentVariables
    )
    {
      var environmentVariables = new Dictionary<string, string>() {
        ["MSBUILDTERMINALLOGGER"] = "off", // make sure to disable terminal logger
        ["NO_COLOR"] = "NO_COLOR", // disable emitting ANSI color escape codes
      };

      if (additionalEnvironmentVariables is not null) {
        foreach (var pair in additionalEnvironmentVariables) {
          environmentVariables[pair.Key] = pair.Value;
        }
      }

      return environmentVariables;
    }

    /*
     * execute 'dotnet clean'
     */
    var cleanPrintRuntimeInformationCommandLineArgs = new List<string>() {
      "clean", // dotnet clean
      PrintRuntimeInformationProps.ProjectPath,
      "--framework",
      PrintRuntimeInformationProps.TargetFrameworkMoniker,
    };

    exitCode = Shell.Execute(
      command: "dotnet",
      arguments: cleanPrintRuntimeInformationCommandLineArgs,
      environmentVariables: GetEnvironmentVariablesForDotnetCommand(null),
      out stdout,
      out stderr
    );

    if (exitCode != 0)
      throw new InvalidOperationException($"clean failed: (stdout: {stdout}, stderr: {stderr})");

    /*
     * execute 'dotnet build'
     */
    var buildPrintRuntimeInformationCommandLineArgs = new List<string>() {
      "build", // dotnet build
      PrintRuntimeInformationProps.ProjectPath,
      "--framework",
      PrintRuntimeInformationProps.TargetFrameworkMoniker,
    };

    buildPrintRuntimeInformationCommandLineArgs.AddRange(
      (buildAdditionalProperties ?? Enumerable.Empty<string>()).Select(static p => "--property:" + p)
    );

    exitCode = Shell.Execute(
      command: "dotnet",
      arguments: buildPrintRuntimeInformationCommandLineArgs,
      environmentVariables: GetEnvironmentVariablesForDotnetCommand(null),
      out stdout,
      out stderr
    );

    if (exitCode != 0)
      throw new InvalidOperationException($"build failed: (stdout: {stdout}, stderr: {stderr})");

    /*
     * execute 'dotnet run' (no build)
     */
    var runPrintRuntimeInformationCommandLineArgs = new List<string>() {
      "run", // dotnet run
      "--project",
      PrintRuntimeInformationProps.ProjectPath,
      "--nologo",
      "--no-build",
      "--framework",
      PrintRuntimeInformationProps.TargetFrameworkMoniker,
      "--",
    };

    runPrintRuntimeInformationCommandLineArgs.AddRange(args);

    exitCode = Shell.Execute(
      command: "dotnet",
      arguments: runPrintRuntimeInformationCommandLineArgs,
      environmentVariables: GetEnvironmentVariablesForDotnetCommand(environmentVariables),
      out stdout,
      out stderr
    );

    if (exitCode != 0)
      throw new InvalidOperationException($"run failed: (stdout: '{stdout}', stderr: '{stderr}')");

    try {
      return getResult(stdout);
    }
    catch (Exception ex) {
      throw new InvalidOperationException($"could not get result: (stdout: '{stdout}', stderr: '{stderr}')", ex);
    }
  }

  [Test]
  public void SupportsIanaTimeZoneName_UseNls_RuntimeConfiguration()
  {
    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
      Assert.Ignore("This test case is intended only for Windows.");
      return;
    }

    if (Runtime.IsRunningOnNetFx && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"))) {
      // test fails with following error: "The active test run was aborted. Reason: Test host process crashed"
      Assert.Ignore("disables unstable test case due to running environment (GitHub Actions Windows runner + .NET Framework)");
      return;
    }

    var processSupportsIanaTimeZoneName = ExecutePrintRuntimeInformation(
      args: new[] { nameof(Runtime.SupportsIanaTimeZoneName) },
      buildAdditionalProperties: new[] { "RuntimeConfigurationSystemGlobalizationUseNls=true" },
      environmentVariables: null,
      getResult: result => bool.Parse(result.TrimEnd())
    );

    Assert.That(processSupportsIanaTimeZoneName, Is.False);
  }

  [TestCase("true")]
  [TestCase("1")]
  public void SupportsIanaTimeZoneName_UseNls_EnvironmentVariable(string value)
  {
    if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
      Assert.Ignore("This test case is intended only for Windows.");
      return;
    }

    if (Runtime.IsRunningOnNetFx && !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("GITHUB_ACTIONS"))) {
      // test fails with following error: "The active test run was aborted. Reason: Test host process crashed"
      Assert.Ignore("disables unstable test case due to running environment (GitHub Actions Windows runner + .NET Framework)");
      return;
    }

    var processSupportsIanaTimeZoneName = ExecutePrintRuntimeInformation(
      args: new[] { nameof(Runtime.SupportsIanaTimeZoneName) },
      buildAdditionalProperties: null,
      environmentVariables: new Dictionary<string, string>() { ["DOTNET_SYSTEM_GLOBALIZATION_USENLS"] = value },
      getResult: result => bool.Parse(result.TrimEnd())
    );

    Assert.That(processSupportsIanaTimeZoneName, Is.False);
  }
}
