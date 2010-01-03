
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:3 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../build/bin/Debug/Smdn.Core.Standards.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../build/bin/Debug/Smdn.dll
BUILD_DIR = ../build/bin/Debug/

SMDN_DLL_SOURCE=../build/bin/Debug/Smdn.dll
SMDN_DLL_MDB_SOURCE=../build/bin/Debug/Smdn.dll.mdb
SMDN_DLL_MDB=$(BUILD_DIR)/Smdn.dll.mdb
SMDN_CORE_STANDARDS_DLL_MDB_SOURCE=../build/bin/Debug/Smdn.Core.Standards.dll.mdb
SMDN_CORE_STANDARDS_DLL_MDB=$(BUILD_DIR)/Smdn.Core.Standards.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:3 -optimize+
ASSEMBLY = ../build/bin/Release/Smdn.Core.Standards.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../build/bin/Release/Smdn.dll
BUILD_DIR = ../build/bin/Release/

SMDN_DLL_SOURCE=../build/bin/Release/Smdn.dll
SMDN_DLL_MDB=
SMDN_CORE_STANDARDS_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SMDN_DLL) \
	$(SMDN_DLL_MDB) \
	$(SMDN_CORE_STANDARDS_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SMDN_CORE_STANDARDS_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	AssemblyInfo.cs \
	Smdn.Formats/FromPercentEncodedTransform.cs \
	Smdn.Formats/FromQuotedPrintableTransform.cs \
	Smdn.Formats/FromRFC2152ModifiedBase64Transform.cs \
	Smdn.Formats/FromRFC3501ModifiedBase64Transform.cs \
	Smdn.Formats/MimeEncoding.cs \
	Smdn.Formats/TextConvert.cs \
	Smdn.Formats/ToPercentEncodedTransform.cs \
	Smdn.Formats/ToQuotedPrintableTransform.cs \
	Smdn.Formats/ToRFC2152ModifiedBase64Transform.cs \
	Smdn.Formats/ToRFC3501ModifiedBase64Transform.cs \
	Smdn.Formats/CsvReader.cs \
	Smdn.Formats/CsvWriter.cs \
	Smdn.Formats/DateTimeConvert.cs \
	Smdn.IO/LineOrientedStream.cs \
	Smdn.IO/LooseLineOrientedStream.cs \
	Smdn.IO/StrictLineOrientedStream.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Smdn.Formats \
	Smdn.IO \
	smdn.core.standards.pc.in 

REFERENCES =  \
	System

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SMDN_DLL = $(BUILD_DIR)/Smdn.dll
SMDN_CORE_STANDARDS_PC = $(BUILD_DIR)/smdn.core.standards.pc

$(eval $(call emit-deploy-wrapper,SMDN_CORE_STANDARDS_PC,smdn.core.standards.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
