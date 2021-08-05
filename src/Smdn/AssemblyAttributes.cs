// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT

using System.Runtime.CompilerServices;

[assembly: System.CLSCompliant(true)]

/*
 * Smdn.Fundamentals.Exceptions
 */
[assembly: TypeForwardedTo(typeof(Smdn.ExceptionUtils))]

/*
 * Smdn.Fundamentals.UInt24n
 */
[assembly: TypeForwardedTo(typeof(Smdn.UInt24))]
[assembly: TypeForwardedTo(typeof(Smdn.UInt48))]
