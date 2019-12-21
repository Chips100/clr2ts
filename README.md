# clr2ts
[![Build Status](https://travis-ci.com/Chips100/clr2ts.svg?branch=master)](https://travis-ci.com/Chips100/clr2ts)
[![Build status](https://ci.appveyor.com/api/projects/status/242dl9xb86ua6220?svg=true)](https://ci.appveyor.com/project/Chips100/clr2ts)
![GitHub](https://img.shields.io/github/license/Chips100/clr2ts.svg)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/clr2ts.svg)

Transpiler that takes CLR assemblies and outputs corresponding typescript definitions.

## Installation
Get clr2ts by installing it as a dotnet tool from nuget.org.

```
dotnet tool install -g clr2ts
```

More information about installing dotnet tools can be found [here](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-tool-install).

## Usage
Run clr2ts by calling the dotnet tool with the configuration file you would like to use.

```
clr2ts your.config.json
```

> If you call clr2ts without specifying a configuration file, it will automatically look for one in the current working directory and its parent directories using the search pattern `*clr2ts.json`. If you use relative paths in your configuration, they are always relative to the configuration file.

## Configuration
The following example shows a complete configuration file that demonstrates simple settings. More complex scenarios are described in detail below.

```
{
    "input": {
        /* Required: At least one assembly that should be translated. */
        "assemblyFiles": [
            "./Path/To/Your/Assembly.dll"
        ]
    },
    "logging": {
        /* Write log messages to the console. */
        "console": true,
        /* Write log messages to a file. */
        "file": "./clr2ts.log"
    },
    "transpilation": {
        /* Specify if property names should be converted to camelCase (TypeScript convention). */
        "camelCase": true,

        /* Specify if base class hierarchies should be removed and instead all properties repeated in the subtype.*/
        "flattenBaseTypes": true
    },
    "output": {
        /* Write all TypeScript types into a single file.*/
        "bundledFile": "./Path/To/Your/File.ts",

        /* Write TypeScript types to individual files in a directory. */
        "files": {
            "directory": "./Path/To/Your/TypeScriptCodeBase/directory",

            /* Specify if a directory structure should be created following the namespaces of the assembly (otherwise all types are put into the directory). */
            "mimicNamespacesWithSubdirectories": false
        }
    }
}
```

### Type filters
In most cases, clr2ts should not translate all types in the source assembly, but rather a specific subset. You can define type filters to control which types should be translated. clr2ts will still translate dependencies of the matching types, even if they do not match the filter criteria. If you do not want to create translations of specific dependencies you can use custom type maps (see below) to map those to the `any` type, for example.

```
"input": {
    /* Define multiple filters for OR-semantics. */
    "typeFilters": [
        /* Multiple values in a single filter for AND-semantics. */
        {
            /* Filters types that have all of the specified attributes applied to them. The "Attribute"-suffix can be omitted. */
            "hasAttribute": ["AttributeName"],

            /* Filters types that are a subtype of all of the specified types. */
            "subTypeOf": ["BaseType", "SomeInterface"],

            /* Filters types that are defined in the specified namespace (StartsWith-semantic). */
            "namespace": "Namespace.SubNamespace"
        }
    ]
}
```

### Custom Type Maps
Custom type maps are used to specify types that should be mapped to existing TypeScript definitions and not be translated by clr2ts. 

```
"transpilation": {
    "customTypeMaps": [
        /* Importing a type definition from the surrounding code base. */
        {
            "type": "Namespace.TypeName",
            "name": "CustomType",
            "source": "../../CustomType"
        },
        /* Importing a type definition from a library. */
        {
            "type": "Namespace.TypeName",
            "name": "LibraryComponent",
            "source": "@lib/library"
        },
        /* Referencing a type that does not need to be imported, like "any". */
        {
            "type": "System.Type",
            "name": "any"
        }
    ]
},
```

### Decorators
You can configure clr2ts to apply decorators to the translated TypeScript definitions. Decorators can be applied to classes and properties. You can specify the parameters that should be passed to the decorator in the format of simple template strings, which can include information from the current context (providing information about the current type or property, its attributes, the assembly, and more).

```
"transpilation": {
    "classDecorators": [
        {
            /* Condition specifying where the decorator should be applied, see type filters. */
            "if": {
                "namespace": "Clr2Ts.EndToEnd.Targets.SubTyping"
            },
            /* Name of the decorator that should be applied. */
            "decorator": "typeName",
            /* Parameters that should be passed to the decorator. */
            "parameters": ["{Type.FullName} + ', ' + {AssemblyName.Name}"],
            /* Optional import of the decorator definition. */
            "import": "../../typeName.decorator"
        }
    ],
    "propertyDecorators": [
        {
            "if": {
                /* Type allows you to specify a full type filter (see above) for the property's declaring type. */
                "type": { "namespace": "Clr2Ts.EndToEnd.Targets.SubTyping" }
            },
            "decorator": "propertyName",
            "parameters": ["{Type.FullName} + '.' + {Property.Name}"],
            "import": "../../propertyName.decorator"
        }
    ]
},
```

The example above demonstrates that decorators do not have to match attributes in the source assembly, but can be applied by arbitrary conditions (type filter mechanism). This still allows you to map decorators from attributes if you want to:

```
/* Decorator configuration that maps the DisplayAttribute from the source assembly to the display decorator. */
{
    "if": { "hasAttribute": [ "Display" ] },
    "decorator": "display",
    "parameters": [ "{Attributes.Display.Name}" ],
    "import": "@decorators/display"
}
```

### Enum attribute maps
As of now, TypeScript does not support decorators for enum members. If you need information from attributes applied to enums in the source assembly, clr2ts supports a workaround in the form of simple maps stored in the TypeScript enum itself. For example, the following configuration instructs clr2ts to generate a map for the `DisplayAttribute.Name` property like shown below:  

```
"transpilation": {
    "enumAttributeMaps": {
        "_displayNames": "{Attributes.DisplayAttribute.Name}",
        "_displayDescriptions": "{Attributes.DisplayAttribute.Description}"
    }
},
```

**Enum definition (C# example):**
```
public enum ExampleEnum
{
    [Display(Name = "DisplayName1")]
    Value1,

    [Display(Name = "DisplayName2")]
    Value2,

    [Display(Name = "DisplayName3")]
    Value3
}
```

**TypeScript translation:**

```
export enum ExampleEnum {
	Value1 = 0,
	Value2 = 1,
	Value3 = 2
}

(<any>ExampleEnum)._displayNames = {
	[ExampleEnum.Value1]: "DisplayName1",
	[ExampleEnum.Value2]: "DisplayName2",
	[ExampleEnum.Value3]: "DisplayName3"
};
```