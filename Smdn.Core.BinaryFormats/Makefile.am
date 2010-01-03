
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:3 -optimize- -debug "-define:DEBUG"
ASSEMBLY = ../build/bin/Debug/Smdn.Core.BinaryFormats.dll
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../build/bin/Debug/Smdn.dll
BUILD_DIR = ../build/bin/Debug/

SMDN_DLL_SOURCE=../build/bin/Debug/Smdn.dll
SMDN_DLL_MDB_SOURCE=../build/bin/Debug/Smdn.dll.mdb
SMDN_DLL_MDB=$(BUILD_DIR)/Smdn.dll.mdb
SMDN_CORE_BINARYFORMATS_DLL_MDB_SOURCE=../build/bin/Debug/Smdn.Core.BinaryFormats.dll.mdb
SMDN_CORE_BINARYFORMATS_DLL_MDB=$(BUILD_DIR)/Smdn.Core.BinaryFormats.dll.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:3 -optimize+
ASSEMBLY = ../build/bin/Release/Smdn.Core.BinaryFormats.dll
ASSEMBLY_MDB = 
COMPILE_TARGET = library
PROJECT_REFERENCES =  \
	../build/bin/Release/Smdn.dll
BUILD_DIR = ../build/bin/Release/

SMDN_DLL_SOURCE=../build/bin/Release/Smdn.dll
SMDN_DLL_MDB=
SMDN_CORE_BINARYFORMATS_DLL_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(SMDN_DLL) \
	$(SMDN_DLL_MDB) \
	$(SMDN_CORE_BINARYFORMATS_DLL_MDB)  

LINUX_PKGCONFIG = \
	$(SMDN_CORE_BINARYFORMATS_PC)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

FILES = \
	AssemblyInfo.cs \
	Smdn/UInt24.cs \
	Smdn/UInt48.cs \
	Smdn.IO/BigEndianBinaryReader.cs \
	Smdn.IO/BigEndianBinaryWriter.cs \
	Smdn.IO/BinaryReader.cs \
	Smdn.IO/BinaryReaderImpl.cs \
	Smdn.IO/BinaryWriter.cs \
	Smdn.IO/BinaryWriterImpl.cs \
	Smdn.IO/LittleEndianBinaryReader.cs \
	Smdn.IO/LittleEndianBinaryWriter.cs \
	Smdn/FourCC.cs \
	Smdn.IO/ExtendStream.cs \
	Smdn.IO/ExtendStreamBase.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	Smdn.IO \
	smdn.core.binaryformats.pc.in 

REFERENCES =  \
	System

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_PKGCONFIG) 

include $(top_srcdir)/Makefile.include

SMDN_DLL = $(BUILD_DIR)/Smdn.dll
SMDN_CORE_BINARYFORMATS_PC = $(BUILD_DIR)/smdn.core.binaryformats.pc

$(eval $(call emit-deploy-wrapper,SMDN_CORE_BINARYFORMATS_PC,smdn.core.binaryformats.pc))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY_MDB): $(ASSEMBLY)

$(ASSEMBLY): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
