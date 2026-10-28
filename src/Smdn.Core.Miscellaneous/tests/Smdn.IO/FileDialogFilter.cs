// SPDX-FileCopyrightText: 2009 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.IO {
  [TestFixture]
  public class FileDialogFilterTests {
    [Test]
    public void TestCreateFilterStringFromStringArray()
    {
      Assert.AreEqual("Text files (*.txt)|*.txt|Image files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*", FileDialogFilter.CreateFilterString(new string[][] {
        new[] {"Text files", "*.txt"},
        new[] {"Image files", "*.jpg", "*.png", "*.bmp"},
        new[] {"All files", "*.*"},
      }));
    }

    [Test]
    public void TestCreateFilterStringFromDictionary()
    {
      Assert.AreEqual("Text files (*.txt)|*.txt|Image files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*", FileDialogFilter.CreateFilterString(new Dictionary<string, string>() {
        {"Text files", "*.txt"},
        {"Image files", "*.jpg;*.png;*.bmp"},
        {"All files", "*.*"},
      }));
    }

    [Test]
    public void TestCreateFilterStringFromFilter()
    {
      Assert.AreEqual("Text files (*.txt)|*.txt|Image files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*", FileDialogFilter.CreateFilterString(new[] {
        new FileDialogFilter.Filter("Text files", "*.txt"),
        new FileDialogFilter.Filter("Image files", "*.jpg", "*.png", "*.bmp"),
        new FileDialogFilter.Filter("All files", "*.*"),
      }));
    }
  }
}