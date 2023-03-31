// SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
#if NET472_OR_GREATER || NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET5_0_OR_GREATER
#define SYSTEM_LINQ_ENUMERABLE_TOHASHSET
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace Smdn.Net;

public static class PortNumberUtils {
  // IANA suggested range for dynamic or private ports
  // ref: https://www.iana.org/assignments/service-names-port-numbers/service-names-port-numbers.xhtml
  public const int MinIanaSystemPort = 0;
  public const int MaxIanaSystemPort = 1023;
  public const int MinIanaUserPort = 1024;
  public const int MaxIanaUserPort = 49151;
  public const int MinIanaDynamicPort = 49152;
  public const int MaxIanaDynamicPort = 65535;

  private static readonly Lazy<bool> isGetActiveTcpListenersAvailable = new(
    valueFactory: () => {
      try {
        IPGlobalProperties.GetIPGlobalProperties().GetActiveTcpListeners();

        return true;
      }
      catch {
        return false;
      }
    },
    isThreadSafe: true
  );

  public static TService CreateServiceWithAvailablePort<TService>(
    Func<int, TService> createService,
    Predicate<Exception> isPortInUseException
  )
    => CreateServiceWithAvailablePort(
      createService: createService,
      exceptPort: null,
      isPortInUseException: isPortInUseException
    );

  public static TService CreateServiceWithAvailablePort<TService>(
    Func<int, TService> createService,
    int exceptPort,
    Predicate<Exception> isPortInUseException
  )
    => CreateServiceWithAvailablePort(
      createService: createService,
      exceptPort: p => p == exceptPort,
      isPortInUseException: isPortInUseException
    );

  public static TService CreateServiceWithAvailablePort<TService>(
    Func<int, TService> createService,
    Predicate<int>? exceptPort,
    Predicate<Exception> isPortInUseException
  )
  {
    if (createService is null)
      throw new ArgumentNullException(nameof(createService));
    if (isPortInUseException is null)
      throw new ArgumentNullException(nameof(isPortInUseException));

    if (isGetActiveTcpListenersAvailable.Value) {
      if (TryFindAvailablePort(exceptPort, out var unusedPort)) {
        try {
          return createService(unusedPort);
        }
        catch (Exception ex) when (isPortInUseException(ex)) {
          // ignore and continue
        }
      }
    }

    foreach (var dynamicPort in EnumerateIanaDynamicPorts(exceptPort)) {
      try {
        return createService(dynamicPort);
      }
      catch (Exception ex) when (isPortInUseException(ex)) {
        continue; // ignore and continue
      }
    }

    throw new InvalidOperationException("could not find any available port");
  }

  public static IEnumerable<int> EnumerateIanaDynamicPorts()
    => EnumerateIanaDynamicPorts(exceptPort: null);

  public static IEnumerable<int> EnumerateIanaDynamicPorts(
    Predicate<int>? exceptPort
  )
  {
    var dynamicPorts = Enumerable.Range(
      start: MinIanaDynamicPort,
      count: MaxIanaDynamicPort - MinIanaDynamicPort + 1
    );

    if (exceptPort is not null)
      dynamicPorts = dynamicPorts.Where(port => !exceptPort(port));

    return dynamicPorts;
  }

  // ref: https://stackoverflow.com/questions/223063/how-can-i-create-an-httplistener-class-on-a-random-port-in-c
  public static bool TryFindAvailablePort(
    out int port
  )
    => TryFindAvailablePort(
      exceptPort: null,
      port: out port
    );

  public static bool TryFindAvailablePort(
    Predicate<int>? exceptPort,
    out int port
  )
  {
    port = default;

    if (!isGetActiveTcpListenersAvailable.Value)
      return false;

    var activeListeners = IPGlobalProperties
      .GetIPGlobalProperties()
      .GetActiveTcpListeners();
#if SYSTEM_LINQ_ENUMERABLE_TOHASHSET
    var listeningPorts = activeListeners
      .Select(static endPoint => endPoint.Port)
      .ToHashSet();
#else
    var listeningPorts = new HashSet<int>(
      activeListeners.Select(static endPoint => endPoint.Port)
    );
#endif

    for (var p = MinIanaDynamicPort; p <= MaxIanaDynamicPort; p++) {
      if (exceptPort is not null && exceptPort(p))
        continue;

      if (!listeningPorts.Contains(p)) {
        port = p;
        return true;
      }
    }

    return false;
  }
}
