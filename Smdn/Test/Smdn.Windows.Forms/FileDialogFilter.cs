using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Smdn.Windows.Forms {
  [TestFixture]
  public class FileDialogFilterTest {
    [Test]
    public void CreateFilterStringFromStringArray()
    {
      Assert.AreEqual("Text files (*.txt)|*.txt|Image files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*", FileDialogFilter.CreateFilterString(new string[][] {
        new[] {"Text files", "*.txt"},
        new[] {"Image files", "*.jpg", "*.png", "*.bmp"},
        new[] {"All files", "*.*"},
      }));
    }

    [Test]
    public void CreateFilterStringFromDictionary()
    {
      Assert.AreEqual("Text files (*.txt)|*.txt|Image files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*", FileDialogFilter.CreateFilterString(new Dictionary<string, string>() {
        {"Text files", "*.txt"},
        {"Image files", "*.jpg;*.png;*.bmp"},
        {"All files", "*.*"},
      }));
    }

    [Test]
    public void CreateFilterStringFromFilter()
    {
      Assert.AreEqual("Text files (*.txt)|*.txt|Image files (*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*", FileDialogFilter.CreateFilterString(new[] {
        new FileDialogFilter.Filter("Text files", "*.txt"),
        new FileDialogFilter.Filter("Image files", "*.jpg", "*.png", "*.bmp"),
        new FileDialogFilter.Filter("All files", "*.*"),
      }));
    }
  }
}