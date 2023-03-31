// SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

using NUnit.Framework;

namespace Smdn.Net;

[TestFixture]
public class PortNumberUtilsTests {
  private static readonly Lazy<bool> IsGetActiveTcpListenersAvailable = new(
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

  [Test]
  public void CreateServiceWithAvailablePort()
  {
    Assert.That(
      PortNumberUtils.CreateServiceWithAvailablePort(
        createService: static port => port,
        isPortInUseException: static ex => false
      ),
      Is.InRange(PortNumberUtils.MinIanaDynamicPort, PortNumberUtils.MaxIanaDynamicPort)
    );
  }

  [Test]
  public void CreateServiceWithAvailablePort_ArgumentNull_CreateService()
    => Assert.Throws<ArgumentNullException>(
      () => PortNumberUtils.CreateServiceWithAvailablePort<int>(
        createService: null!,
        isPortInUseException: static ex => ex is NotSupportedException
      )
    );

  [Test]
  public void CreateServiceWithAvailablePort_ArgumentNull_IsPortInUseException()
    => Assert.Throws<ArgumentNullException>(
      () => PortNumberUtils.CreateServiceWithAvailablePort(
        createService: static port => port,
        isPortInUseException: null!
      )
    );

  [Test]
  public void CreateServiceWithAvailablePort_UnexpectedExceptionThrown()
  {
    const int min = 50000;
    const int max = 50005;

    static bool ExceptPort(int port)
      => !(min <= port && port <= max);

    Assert.Throws<NotImplementedException>(
      () => PortNumberUtils.CreateServiceWithAvailablePort<int>(
        createService: static port => throw new NotImplementedException("not implemented creating service"),
        exceptPort: ExceptPort,
        isPortInUseException: static ex => ex is NotSupportedException
      )
    );
  }

  [Test]
  public void CreateServiceWithAvailablePort_PortInUseExceptionThrown()
  {
    const int min = 50000;
    const int max = 50005;

    static bool ExceptPort(int port)
      => !(min <= port && port <= max);

    int servicePort = default;

    Assert.DoesNotThrow(() => {
      servicePort = PortNumberUtils.CreateServiceWithAvailablePort(
        createService: static port => port == max
          ? port
          : throw new NotSupportedException($"cannot create service with port {port}"),
        exceptPort: ExceptPort,
        isPortInUseException: static ex => ex is NotSupportedException
      );
    });

    Assert.AreEqual(max, servicePort);
  }

  [Test]
  public void CreateServiceWithAvailablePort_NoAvailablePort()
  {
    Assert.Throws<InvalidOperationException>(
      () => PortNumberUtils.CreateServiceWithAvailablePort(
        createService: static port => port,
        exceptPort: static port => true /* except ALL ports */,
        isPortInUseException: static ex => false
      )
    );
  }

  [Test]
  public void CreateServiceWithAvailablePort_AllPortInUse()
  {
    const int min = 50000;
    const int max = 50002;

    static bool ExceptPort(int port)
      => !(min <= port && port <= max);

    static Socket CreateListener(int port)
    {
      var endPoint = new IPEndPoint(
        address: IPAddress.Any,
        port
      );

      Socket? listener = null;

      try {
        listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, false);

        listener.Bind(endPoint);
        listener.Listen(backlog: 1);

        return listener;
      }
      catch {
        listener?.Dispose();
        throw;
      }
    }

    var listeners = new Dictionary<int, Socket>();

    try {
      // make ports in use
      for (var attempt = 0; attempt < 3; attempt++) {
        var failed = false;

        for (var port = min; port <= max; port++) {
          if (listeners.ContainsKey(port))
            continue;

          try {
            listeners[port] = CreateListener(port);
          }
          catch (SocketException) {
            // ignore
            failed = true;
          }
        }

        if (failed) {
          System.Threading.Thread.Sleep(1000);
          continue;
        }

        break;
      }

      Assert.Throws<InvalidOperationException>(
        () => PortNumberUtils.CreateServiceWithAvailablePort(
          createService: CreateListener,
          exceptPort: ExceptPort,
          isPortInUseException: static ex => ex is SocketException
        )
      );
    }
    finally {
      foreach (var listener in listeners.Values) {
        listener.Dispose();
      }
    }
  }

  [Test]
  public void EnumerateIanaDynamicPorts()
    => Assert.That(
      PortNumberUtils.EnumerateIanaDynamicPorts(),
      Is.All.InRange(PortNumberUtils.MinIanaDynamicPort, PortNumberUtils.MaxIanaDynamicPort)
    );

  [Test]
  public void EnumerateIanaDynamicPorts_ExceptPort()
  {
    const int min = 50000;
    const int max = 50020;

    static bool ExceptPort(int port)
      => !(min <= port && port <= max);

    Assert.That(
      PortNumberUtils.EnumerateIanaDynamicPorts(exceptPort: ExceptPort),
      Is.All.InRange(min, max)
    );
  }

  [Test]
  public void TryFindAvailablePort()
  {
    if (!IsGetActiveTcpListenersAvailable.Value) {
      Assert.IsFalse(PortNumberUtils.TryFindAvailablePort(out _));
      return;
    }

    Assert.IsTrue(PortNumberUtils.TryFindAvailablePort(out var port));
    Assert.That(port, Is.InRange(PortNumberUtils.MinIanaDynamicPort, PortNumberUtils.MaxIanaDynamicPort));
  }

  [Test]
  public void TryFindAvailablePort_ExceptPort()
  {
    const int min = 50000;
    const int max = 50020;

    static bool ExceptPort(int port)
      => !(min <= port && port <= max);

    if (!IsGetActiveTcpListenersAvailable.Value) {
      Assert.IsFalse(PortNumberUtils.TryFindAvailablePort(exceptPort: ExceptPort, out _));
      return;
    }

    Assert.IsTrue(PortNumberUtils.TryFindAvailablePort(exceptPort: ExceptPort, out var port));
    Assert.That(port, Is.InRange(min, max));
  }
}
