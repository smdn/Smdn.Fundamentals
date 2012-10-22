
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:3 -optimize- -debug "-define:NET_2_0;NET_3_5;NET_4_0;DEBUG"
ASSEMBLY = ../build/bin/Debug/netfx4.0/Smdn.Core.Standards.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../build/bin/Debug/netfx4.0/Smdn.dll
BUILD_DIR = ../build/bin/Debug/netfx4.0/

SMDN_CORE_STANDARDS_DLL_MDB_SOURCE=../build/bin/Debug/netfx4.0/Smdn.Core.Standards.dll.mdb
SMDN_CORE_STANDARDS_DLL_MDB=$(BUILD_DIR)/Smdn.Core.Standards.dll.mdb
SMDN_DLL_SOURCE=../build/bin/Debug/netfx4.0/Smdn.dll
SMDN_DLL_MDB_SOURCE=../build/bin/Debug/netfx4.0/Smdn.dll.mdb
SMDN_DLL_MDB=$(BUILD_DIR)/Smdn.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = dmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:3 -optimize+ "-define:NET_2_0;NET_3_5;NET_4_0"
ASSEMBLY = ../build/bin/Release/netfx4.0/Smdn.Core.Standards.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../build/bin/Release/netfx4.0/Smdn.dll
BUILD_DIR = ../build/bin/Release/netfx4.0/

SMDN_CORE_STANDARDS_DLL_MDB=
SMDN_DLL_SOURCE=../build/bin/Release/netfx4.0/Smdn.dll
SMDN_DLL_MDB=

endif

AL=al
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SMDN_CORE_STANDARDS_DLL_MDB) \
	$(SMDN_DLL) \
	$(SMDN_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SMDN_CORE_STANDARDS_NETFX4_0_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	AssemblyInfo.cs \
	Smdn.Formats/FromPercentEncodedTransform.cs \
	Smdn.Formats/FromRFC2152ModifiedBase64Transform.cs \
	Smdn.Formats/FromRFC3501ModifiedBase64Transform.cs \
	Smdn.Formats/ToPercentEncodedTransform.cs \
	Smdn.Formats/ToRFC2152ModifiedBase64Transform.cs \
	Smdn.Formats/ToRFC3501ModifiedBase64Transform.cs \
	Smdn.Formats/CsvReader.cs \
	Smdn.Formats/CsvWriter.cs \
	Smdn.Formats/DateTimeConvert.cs \
	Smdn.IO/LineOrientedStream.cs \
	Smdn.IO/LooseLineOrientedStream.cs \
	Smdn.IO/StrictLineOrientedStream.cs \
	Smdn.Formats/ToPercentEncodedTransformMode.cs \
	Smdn.Formats/EncodingUtils.cs \
	Smdn.Formats/PercentEncoding.cs \
	Smdn.Formats/ModifiedUTF7.cs \
	Smdn.Formats.Mime/ToQuotedPrintableTransform.cs \
	Smdn.Formats.Mime/FromQuotedPrintableTransform.cs \
	Smdn.Formats.Mime/MimeEncoding.cs \
	Smdn.Formats.Mime/MimeEncodingMethod.cs \
	Smdn.Formats.Mime/QuotedPrintableEncoding.cs \
	Smdn.Formats.Mime/ContentTransferEncoding.cs \
	Smdn.Formats.Mime/ContentTransferEncodingMethod.cs \
	Smdn.Formats/HtmlEscape.cs \
	Smdn.Formats/EncodingNotSupportedException.cs \
	Smdn.Formats/EncodingSelectionCallback.cs \
	Smdn.Formats.Mime/MimeUtils.cs \
	Smdn.Formats.Mime/FromQuotedPrintableTransformMode.cs \
	Smdn.Formats.Mime/ToQuotedPrintableTransformMode.cs \
	Smdn.Formats.Mime/MimeEncodedWordConverter.cs \
	Smdn.Formats.UUEncoding/UUDecodingStream.cs \
	Smdn.Formats.UUEncoding/UUDecodingTransform.cs \
	Smdn.Formats.UUEncoding/UUDecoder.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Smdn.Formats \
	Smdn.IO \
	Smdn.Formats.Mime \
	Smdn.Formats.UUEncoding \
	smdn.core.standards-netfx4.0.pc.in 

REFERENCES =  \
	System \
	System.Core

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SMDN_DLL = $(BUILD_DIR)/Smdn.dll
SMDN_CORE_STANDARDS_NETFX4_0_PC = $(BUILD_DIR)/smdn.core.standards-netfx4.0.pc

$(eval $(call emit-deploy-wrapper,SMDN_CORE_STANDARDS_NETFX4_0_PC,smdn.core.standards-netfx4.0.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
