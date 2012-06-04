
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -unsafe -warn:3 -optimize- -debug "-define:NET_2_0;NET_3_5;NET_4_0;DEBUG"
ASSEMBLY = ../build/bin/Debug/netfx4.0/Smdn.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = ../build/bin/Debug/netfx4.0/

SMDN_DLL_MDB_SOURCE=../build/bin/Debug/netfx4.0/Smdn.dll.mdb
SMDN_DLL_MDB=$(BUILD_DIR)/Smdn.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -unsafe -warn:3 -optimize+ "-define:NET_2_0;NET_3_5;NET_4_0"
ASSEMBLY = ../build/bin/Release/netfx4.0/Smdn.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES = 
BUILD_DIR = ../build/bin/Release/netfx4.0/

SMDN_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SMDN_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SMDN_NETFX4_0_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	AssemblyInfo.cs \
	System.Runtime.CompilerServices/ExtensionAttribute.cs \
	Smdn.Mathematics/Fraction.cs \
	Smdn.Formats/Chars.cs \
	Smdn.Formats/Octets.cs \
	Smdn/Runtime.cs \
	Smdn.IO/PathUtils.cs \
	Smdn/ArrayExtensions.cs \
	Smdn/StringExtensions.cs \
	Mono.Simd/SimdRuntime.cs \
	Smdn/RuntimeEnvironment.cs \
	Smdn/EnumUtils.cs \
	Smdn/Action.cs \
	Smdn/Func.cs \
	Smdn.Mathematics/Radian.cs \
	Smdn.Mathematics/Polar.cs \
	Smdn/MimeType.cs \
	Smdn.IO/DirectoryInfoExtensions.cs \
	Smdn.IO/DirectoryUtils.cs \
	Smdn.Collections/IEnumerableExtensions.cs \
	Smdn.Threading/Parallel.cs \
	Smdn.Media/mmsystem.h.cs \
	Smdn/Platform.cs \
	Smdn/Endianness.cs \
	Smdn/Shell.cs \
	Smdn/Uuid.cs \
	Smdn/UuidVersion.cs \
	Smdn/Urn.cs \
	Smdn/Lazy.cs \
	Smdn.IO/StreamExtensions.cs \
	Smdn.Collections/ReadOnlyDictionary.cs \
	Smdn.Collections/IDictionaryExtensions.cs \
	Smdn/UnixTimeStamp.cs \
	Smdn/MathUtils.cs \
	Smdn.Formats/Csv.cs \
	Smdn.Formats/Hexadecimals.cs \
	Smdn/ConvertUtils.cs \
	Smdn/ByteString.cs \
	Smdn/ByteStringBuilder.cs \
	Smdn.IO/ChunkedMemoryStream.cs \
	Smdn.IO/PartialStream.cs \
	Smdn.IO/CachedStreamBase.cs \
	Smdn.IO/NonPersistentCachedStream.cs \
	Smdn.IO/PersistentCachedStream.cs \
	Smdn/Tuple.cs \
	Smdn.IO/TextReaderExtensions.cs \
	Smdn.Formats/Base64.cs \
	Smdn.Security.Cryptography/ICryptoTransformExtensions.cs \
	Smdn.Collections/Enumerable.cs \
	Smdn/AggregateException.cs \
	Smdn/BinaryConvert.cs \
	Smdn/ExceptionUtils.cs \
	Smdn.Collections/HashSet.cs \
	Smdn.Collections/ISet.cs \
	Smdn/UriUtils.cs \
	Smdn.Collections/KeyValuePair.cs 

DATA_FILES = 

RESOURCES = \
	resources/exceptions-ja.txt,Smdn.resources.exceptions-ja.txt 

EXTRAS = \
	Smdn.Formats \
	Mono.Simd \
	Smdn.Collections \
	Smdn.Threading \
	Smdn \
	resources \
	smdn-netfx4.0.pc.in 

REFERENCES =  \
	System \
	System.Core

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SMDN_NETFX4_0_PC = $(BUILD_DIR)/smdn-netfx4.0.pc

$(eval $(call emit-deploy-wrapper,SMDN_NETFX4_0_PC,smdn-netfx4.0.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
