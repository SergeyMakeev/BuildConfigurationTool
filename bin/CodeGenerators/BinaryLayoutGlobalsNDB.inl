//=============================================================================================================================
//
//ƒополнительный глобальный файл специализирующий BinaryLayout дл€ ресурсной системы NDb.
//
//=============================================================================================================================


#include "BinaryLayoutGlobals.inl"


$LayoutGlobalReplace(renameNamespace = ::NDbDeveloper; generateName = NDBRelease);

$LayoutDeclareNative(typein = DBPtr<T>; typeout = layout_ptr; kind = pointer; size = 32);
$LayoutDeclareNative(typein = ResourceString; kind = usertype; size = 32);


namespace NDb
{
	$LayoutDeclareNative(typein = vector<T>; typeout = layout_vector; kind = vector; size = 64; typealing = 32);
}

$LayoutAttribute(generateName = NDBRelease)
namespace NDb
{
	$LayoutAttribute(Assign = External; DisablePack)
	$LayoutStructSerializable()
	$LayoutStructMacros(name = NDB_OBJECT; generateName = NDB_OBJECT_RELEASE; group = Unique; transport)
	$LayoutStructMacros(name = NDB_ABSTRACT; generateName = ABSTRACT_RELEASE; group = Unique; transport)
	struct CResource
	{
		uint16 id;
	};

	$LayoutAttribute(Assign = External; DisablePack)
	$LayoutStructMacros(name = NDB_STRUCT; generateName = STRUCT_RELEASE; group = Unique; transport)
	struct AggregateBase
	{
		uint16 id;
	};

	namespace Inlining
	{
		$LayoutAttribute(Assign = External; DisablePack)
		$LayoutStructSerializable()
		$LayoutStructMacros(name = NDB_OBJECT; generateName = OBJECT_RELEASE; group = Unique; transport)
		struct Base
		{
			uint16 id;
		};
	
	}
}


$LayoutDeclareNative(typein = GlobalString; kind = usertype; size = 32);
$LayoutDeclareNative(typein = StringId; kind = usertype; size = 32);


namespace Client
{
	$LayoutAttribute(generateName = NDBRelease)
	namespace File
	{
		$LayoutAttribute(Assign = External; DisablePack)
		struct FilePath
		{
			uint32 packIndex;
			uint32 fileIndex;
		};

		$LayoutAttribute(Assign = External; DisablePack)
		struct TextFilePath : public FilePath
		{
		};
	}
}
