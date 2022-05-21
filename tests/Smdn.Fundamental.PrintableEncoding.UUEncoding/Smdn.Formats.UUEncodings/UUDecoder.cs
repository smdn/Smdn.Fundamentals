// SPDX-FileCopyrightText: 2012 smdn <smdn@smdn.jp>
// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

using Smdn.IO;

namespace Smdn.Formats.UUEncodings {
  [TestFixture]
  public class UUDecoderTests {
    private static readonly byte[] testimg_png = new byte[] {
      0x89, 0x50, 0x4e, 0x47, 0x0d, 0x0a, 0x1a, 0x0a, 0x00, 0x00, 0x00, 0x0d, 0x49, 0x48, 0x44, 0x52,
      0x00, 0x00, 0x00, 0x20, 0x00, 0x00, 0x00, 0x20, 0x08, 0x06, 0x00, 0x00, 0x00, 0x73, 0x7a, 0x7a,
      0xf4, 0x00, 0x00, 0x00, 0x04, 0x67, 0x41, 0x4d, 0x41, 0x00, 0x00, 0xb1, 0x8f, 0x0b, 0xfc, 0x61,
      0x05, 0x00, 0x00, 0x00, 0x47, 0x49, 0x44, 0x41, 0x54, 0x58, 0x47, 0xed, 0xd6, 0xb1, 0x0d, 0x00,
      0x20, 0x08, 0x04, 0x40, 0xdc, 0x7f, 0x68, 0x44, 0xdd, 0x00, 0x0a, 0x9b, 0x23, 0xf9, 0x52, 0xfd,
      0x5c, 0x83, 0x2b, 0x22, 0xb2, 0xd2, 0x9e, 0xcc, 0xd1, 0xf1, 0xfb, 0xee, 0xb9, 0xa1, 0x9d, 0x2a,
      0x30, 0x1a, 0x05, 0x08, 0x10, 0x20, 0x40, 0x80, 0x00, 0x81, 0x18, 0xed, 0xd2, 0xf7, 0x19, 0x98,
      0x45, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0xe0, 0xb7, 0xc0, 0x06, 0x97, 0xf5, 0xaa, 0xd5,
      0x6d, 0xcc, 0x16, 0xf5, 0x00, 0x00, 0x00, 0x00, 0x49, 0x45, 0x4e, 0x44, 0xae, 0x42, 0x60, 0x82,
    };

    [Test]
    public void TestExtractFilesWithIterator()
    {
      var input = @"
****************
extra text block
****************

begin 644 testimg.png
MB5!.1PT*&@H    -24A$4@   ""     @"" 8   !S>GKT    !&=!34$  +&/
M""_QA!0   $=)1$%46$?MUK$- "" (!$#<?VA$W0 *FR/Y4OU<@RLBLM*>S-'Q
M^^ZYH9TJ,!H%""! @0(  @1CMTO<9F$4! @0($""! X+? !I?UJM5MS!;U    
) $E%3D2N0F""""
 
end

****************
extra text block
****************

begin 644 testimg2.png
MB5!.1PT*&@H````-24A$4@```""`````@""`8```!S>GKT````!&=!34$``+&/
M""_QA!0```$=)1$%46$?MUK$-`""`(!$#<?VA$W0`*FR/Y4OU<@RLBLM*>S-'Q
M^^ZYH9TJ,!H%""!`@0(``@1CMTO<9F$4!`@0($""!`X+?`!I?UJM5MS!;U````
)`$E%3D2N0F""""
`
end

****************
extra text block
****************
begin 644 cat.txt
#0V%T
`
end
****************
extra text block
****************

".Replace("\r\n", "\n").Replace("\n", "\r\n");

      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(input))) {
        var files = UUDecoder.ExtractFiles(stream).ToArray();

        Assert.AreEqual(3, files.Length, "extracted file count");

        var file = files[0];

        Assert.AreEqual("testimg.png", file.FileName, "extracted file name #1");
        Assert.AreEqual(Convert.ToUInt32("0644", 8), file.Permissions, "extracted file permissions #1");
        Assert.AreEqual(testimg_png, file.Stream.ReadToEnd(), "extracted file content #1");

        file.Dispose();

        Assert.Throws<ObjectDisposedException>(() => Assert.IsNotNull(file.Stream));
        Assert.DoesNotThrow(() => file.Dispose(), "dispose again #1");

        file = files[1];

        Assert.AreEqual("testimg2.png", file.FileName, "extracted file name #2");
        Assert.AreEqual(Convert.ToUInt32("0644", 8), file.Permissions, "extracted file permissions #2");
        Assert.AreEqual(testimg_png, file.Stream.ReadToEnd(), "extracted file content #2");

        file = files[2];

        Assert.AreEqual("cat.txt", file.FileName, "extracted file name #3");
        Assert.AreEqual(Convert.ToUInt32("0644", 8), file.Permissions, "extracted file permissions #3");
        Assert.AreEqual(new byte[] {0x43, 0x61, 0x74} /* 'C' 'a' 't' */,
                        file.Stream.ReadToEnd(), "extracted file content #3");

        Assert.DoesNotThrow(() => stream.ReadByte(), "read base stream");
      }
    }

    [Test]
    public void TestExtractFilesWithAction()
    {
      var input = @"
****************
extra text block
****************

begin 644 testimg.png
MB5!.1PT*&@H    -24A$4@   ""     @"" 8   !S>GKT    !&=!34$  +&/
M""_QA!0   $=)1$%46$?MUK$- "" (!$#<?VA$W0 *FR/Y4OU<@RLBLM*>S-'Q
M^^ZYH9TJ,!H%""! @0(  @1CMTO<9F$4! @0($""! X+? !I?UJM5MS!;U    
) $E%3D2N0F""""
 
end

****************
extra text block
****************

begin 644 testimg2.png
MB5!.1PT*&@H````-24A$4@```""`````@""`8```!S>GKT````!&=!34$``+&/
M""_QA!0```$=)1$%46$?MUK$-`""`(!$#<?VA$W0`*FR/Y4OU<@RLBLM*>S-'Q
M^^ZYH9TJ,!H%""!`@0(``@1CMTO<9F$4!`@0($""!`X+?`!I?UJM5MS!;U````
)`$E%3D2N0F""""
`
end

****************
extra text block
****************
begin 644 cat.txt
#0V%T
`
end
****************
extra text block
****************

".Replace("\r\n", "\n").Replace("\n", "\r\n");

      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(input))) {
        var fileCount = 0;

        UUDecoder.ExtractFiles(stream, delegate(UUDecoder.FileEntry file) {
          if (fileCount == 0) {
            Assert.AreEqual("testimg.png", file.FileName, "extracted file name #1");
            Assert.AreEqual(Convert.ToUInt32("0644", 8), file.Permissions, "extracted file permissions #1");
            Assert.AreEqual(testimg_png, file.Stream.ReadToEnd(), "extracted file content #1");
          }
          else if (fileCount == 1) {
            Assert.AreEqual("testimg2.png", file.FileName, "extracted file name #2");
            Assert.AreEqual(Convert.ToUInt32("0644", 8), file.Permissions, "extracted file permissions #2");
            Assert.AreEqual(testimg_png, file.Stream.ReadToEnd(), "extracted file content #2");
          }
          else if (fileCount == 2) {
            Assert.AreEqual("cat.txt", file.FileName, "extracted file name #3");
            Assert.AreEqual(Convert.ToUInt32("0644", 8), file.Permissions, "extracted file permissions #3");
            Assert.AreEqual(new byte[] {0x43, 0x61, 0x74} /* 'C' 'a' 't' */,
                            file.Stream.ReadToEnd(), "extracted file content #3");
          }
          else {
            Assert.Fail("unexpected file entry");
          }

          fileCount++;

          file.Dispose();

          Assert.Throws<ObjectDisposedException>(() => Assert.IsNotNull(file.Stream));
          Assert.DoesNotThrow(() => file.Dispose(), "dispose again");
        });

        Assert.AreEqual(3, fileCount, "extracted file count");

        Assert.DoesNotThrow(() => stream.ReadByte(), "read base stream");
      }
    }

    [Test]
    public void TestExtractFiles_EmptyFileName()
    {
      const string filename = "";
      var input = @$"
begin 644 {filename}
#0V%T
`
end
".Replace("\r\n", "\n").Replace("\n", "\r\n");

      using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(input))) {
        var fileCount = 0;

        UUDecoder.ExtractFiles(stream, delegate(UUDecoder.FileEntry file) {
          if (fileCount == 0) {
            Assert.AreEqual(string.Empty, file.FileName, "extracted file name #1");
            Assert.AreEqual(Convert.ToUInt32("0644", 8), file.Permissions, "extracted file permissions #1");
            Assert.AreEqual(new byte[] {0x43, 0x61, 0x74} /* 'C' 'a' 't' */, file.Stream.ReadToEnd(), "extracted file content #1");

            Assert.Throws<InvalidOperationException>(() => file.Save());
          }
          else {
            Assert.Fail("unexpected file entry");
          }

          fileCount++;

          file.Dispose();

          Assert.Throws<ObjectDisposedException>(() => Assert.IsNotNull(file.Stream));
          Assert.DoesNotThrow(() => file.Dispose(), "dispose again");
        });

        Assert.AreEqual(1, fileCount, "extracted file count");

        Assert.DoesNotThrow(() => stream.ReadByte(), "read base stream");
      }
    }
  }
}

