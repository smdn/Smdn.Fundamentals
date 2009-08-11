
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -unsafe -warn:3 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../build/bin/Debug/Smdn.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = ../build/bin/Debug/

SMDN_DLL_MDB_SOURCE=../build/bin/Debug/Smdn.dll.mdb
SMDN_DLL_MDB=$(BUILD_DIR)/Smdn.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -unsafe -warn:3 -optimize+
ASSEMBLY = ../build/bin/Release/Smdn.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = ../build/bin/Release/

SMDN_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SMDN_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SMDN_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	AssemblyInfo.cs \
	System.Runtime.CompilerServices/ExtensionAttribute.cs \
	Smdn.IO/BigEndianBinaryReader.cs \
	Smdn.IO/BigEndianBinaryWriter.cs \
	Smdn.Mathematics/Fraction.cs \
	Smdn.IO/BinaryReader.cs \
	Smdn.IO/LittleEndianBinaryReader.cs \
	Smdn.IO/BinaryWriter.cs \
	Smdn.IO/LittleEndianBinaryWriter.cs \
	Smdn.Formats/TextConvert.cs \
	Smdn.Formats/Chars.cs \
	Smdn.Formats/Octets.cs \
	Smdn.Mathematics/Operations.cs \
	Smdn/UInt48.cs \
	Smdn/Runtime.cs \
	Smdn.Formats/MimeEncoding.cs \
	Smdn.IO/PathUtils.cs \
	Smdn.Formats/FromQuotedPrintableTransform.cs \
	Smdn.Formats/ToQuotedPrintableTransform.cs \
	Smdn.Formats/FromRFC3501ModifiedBase64Transform.cs \
	Smdn.Formats/ToRFC3501ModifiedBase64Transform.cs \
	Smdn.Formats/ToRFC2152ModifiedBase64Transform.cs \
	Smdn.Formats/FromRFC2152ModifiedBase64Transform.cs \
	Smdn.IO/LineOrientedStream.cs \
	Smdn.IO/StrictLineOrientedStream.cs \
	Smdn.IO/LooseLineOrientedStream.cs \
	Smdn/ByteString.cs \
	Smdn/ByteStringBuilder.cs \
	Smdn.Formats/DateTimeConvert.cs \
	Smdn/ArrayExtensions.cs \
	Smdn/StringExtensions.cs \
	Smdn.Mathematics/Matrix3D.cs \
	Smdn.Mathematics/Matrix4D.cs \
	Smdn.Mathematics/Vector3D.cs \
	Smdn.Mathematics/Vector4D.cs \
	Smdn.Interop/mmsystem.h.cs \
	Smdn.Interop/HeapMemoryBuffer.cs \
	Smdn.IO/PartialStream.cs \
	Smdn.Media/PcmStreamPlayer.cs \
	Smdn.Interop/CoTaskMemoryBuffer.cs \
	Smdn.Interop/GlobalMemoryBuffer.cs \
	Smdn.Interop/UnmanagedMemoryBuffer.cs \
	Smdn.Interop/kernel32.cs \
	Smdn.Interop/DynamicLibrary.cs \
	Smdn.Interop/DynamicLinkLibrary.cs \
	Smdn.Interop/SharedLibrary.cs \
	Smdn.Interop/libdl.cs \
	Smdn.Interop/FunctionNotFoundException.cs \
	Smdn/FourCC.cs \
	Smdn.Imaging/LockedBitmap.cs \
	Smdn/UInt24.cs \
	Smdn.IO/BinaryReaderImpl.cs \
	Smdn.IO/BinaryWriterImpl.cs \
	Mono.Simd/SimdRuntime.cs \
	Smdn.Interop/Shell.cs \
	Smdn.Interop/Platform.cs \
	Smdn/RuntimeEnvironment.cs \
	Smdn/ConsoleUtils.cs \
	Smdn.IO/ExtendStream.cs \
	Smdn.IO/ExtendStreamBase.cs \
	Smdn.IO/InterruptStream.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Smdn.Formats \
	Smdn.Interop \
	Smdn.Media \
	Smdn.Imaging \
	Mono.Simd \
	smdn.pc.in 

REFERENCES =  \
	System \
	System.Drawing

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SMDN_PC = $(BUILD_DIR)/smdn.pc

$(eval $(call emit-deploy-wrapper,SMDN_PC,smdn.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
