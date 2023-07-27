
/*

	%NAME%
	%TYPE%
	%BODY%
	%INCLUDES%
	%PARENT%
	%ARGS%
	%PATH%
	%DECLS%
	%INDEX%
	%TYPEIN%
	%TYPEOUT%
	%ALIGN%
	%RVALUE%
	
*/


template<FileH>
{
// ************************************************************************************
// * Generated automatically by BinaryLayout generator system, don't change manually! *
// ************************************************************************************
#pragma once
%INCLUDES%
%BODY%

}

template<FileInclude>
{
#include %PATH%

}

template<FileIncludes>
{

%INCLUDES%

}


template<FileCpp>
{
// ************************************************************************************
// * Generated automatically by BinaryLayout generator system, don't change manually! *
// ************************************************************************************
%INCLUDES%
%BODY%

}

template<Namespace>
{
namespace %NAME%
{
	%BODY%
}
}


template<Struct>
{
struct %NAME%%PARENT%
{
	%BODY%
};

}

template<StructDecl>
{
struct %NAME%;
}

template<StructParent>
{
 : public %PARENT%
}

template<Typedef>
{
typedef %TYPE% %NAME%;
}

template<Variable>
{
%TYPE% %NAME%;
}

template<StaticConst>
{
static const %TYPE% %NAME% = %RVALUE%;
}

template<StructMacros>
{
%NAME%(%BODY%)
}


template<Enum>
{
enum %NAME% : %TYPE%
{
	%BODY%
};

}

template<EnumDecl>
{
enum %NAME%;
}

template<EnumItem>
{
%NAME%,
}

template<EnumItemRValue>
{
%NAME% = %RVALUE%,
}

template<BytesFiller>
{
byte __filler%NAME%%INDEX%;
}

template<TypeTemplate>
{
%TYPE%<%ARGS%>
}

template<TypeTemplateArgFirst>
{
%TYPE%
}

template<TypeTemplateArgSecond>
{
, %TYPE%
}

template<Nextline>
{


}

template<Nextline2>
{



}

template<AssignType>
{
AssignType%INDEX%(pinOutObject->%NAME%, inObject.%NAME%, context);
}

template<AssignTypeBase>
{
AssignType((%TYPEOUT% &)pinOutObject.GetRef(), (const %TYPEIN% &)inObject, context);

}

template<AssignTypeVectorHelper>
{
void AssignType%INDEX%(%TYPEOUT% & outObject, const %TYPEIN% & inObject, SerializeContext & context)
{
	uint32 count = (uint32)inObject.size();
	LayoutVectorWriter<%TYPEOUT%> vectorWriter(outObject, context, count, %ALIGN%);
	if ( std::is_arithmetic<typename %TYPEOUT%::Arg>::value )
	{
		if ( count > 0 )
		{
			memcpy( &vectorWriter[0], &inObject[0], count * sizeof( inObject[0] ) );
		}
	}
	else
	{
		for(uint32 i = 0; i < count; i++)
		{
			AssignType%PARENT%(vectorWriter[i], inObject[i], context);
		}
	}

	vectorWriter.Finalize();
}


}


template<AssignTypeArrayHelper>
{
void AssignType%INDEX%(%TYPEOUT% & outObject, const %TYPEIN% & inObject, SerializeContext & context)
{
	uint32 count = (uint32)inObject.size();
	LayoutArrayWriter<%TYPEOUT%> arrayWriter(outObject, context);
	for(uint32 i = 0; i < count; i++)
	{
		AssignType%PARENT%(arrayWriter[i], inObject[i], context);
	}
}


}


template<AssignTypeEnumHelper>
{
void AssignType%INDEX%(%TYPEOUT% & outObject, const %TYPEIN% & inObject, SerializeContext & context)
{
	UNUSED(context);
	switch(inObject)
	{
		%BODY%
	default:
		ASSERT(false, "Invalid enum %TYPEIN% value");
	}
	
}


}


template<AssignTypeEnumHelperCase>
{
case %TYPEIN%:
	outObject = %TYPEOUT%;
	break;
}



template<AssignTypeFunc>
{
void AssignType(%TYPEOUT% & outObject, const %TYPEIN% & inObject, SerializeContext & context)
{
	LayoutObjectPin<%TYPEOUT%> pinOutObject(outObject, context);
	%BODY%
}


}

template<AssignTypeFuncEmpty>
{
void AssignType(%TYPEOUT% & outObject, const %TYPEIN% & inObject, SerializeContext & context)
{
	UNUSED(outObject);
	UNUSED(inObject);
	UNUSED(context);
}


}

template<AssignTypeSerialize>
{

SerializableRegistrator<%TYPEIN%, %TYPEOUT%, %ALIGN%> registrator%NAME%;

uint32 SerializeType(const %TYPEIN% * inObject, SerializeContext & context)
{
	return registrator%NAME%.SerializeType(inObject, context);
}


}

template<AssignTypeSerializeRoot>
{
const %TYPEOUT% * Serialize(const %TYPEIN% * inObject, Allocator & allocator)
{
	SerializeContext context(allocator);
	uint32 checkRootOffset = SerializeType(inObject, context);
	ASSERT(checkRootOffset == 0, "Root offset must be zero!");
	context.SharePass();
	const %TYPEOUT% * outObject = reinterpret_cast<const %TYPEOUT% *>(allocator.GetDataPtr());
	return outObject;
}

}

template<AssignTypeFuncDeclare>
{
void AssignType%INDEX%(%TYPEOUT% & outObject, const %TYPEIN% & inObject, SerializeContext & context);
}

template<AssignTypeSerializeDeclare>
{
uint32 SerializeType(const %TYPEIN% * inObject, SerializeContext & context);
}

template<AssignTypeSerializeDeclareRoot>
{
const %TYPEOUT% * Serialize(const %TYPEIN% * inObject, Allocator & allocator);
}

template<AssignTypeFuncDeclareAppendH>
{


%BODY%


}

template<StaticAssertSize>
{

static_assert(sizeof(%TYPE%) == %INDEX%, "Struct %TYPE% have not expected size");
}

template<StaticAssertOffset>
{

static_assert(offsetof(%TYPE%, %NAME%) == %INDEX%, "Field %NAME% of struct %TYPE% have not expected offset");
}

