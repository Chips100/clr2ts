# clr2ts
[![Build Status](https://travis-ci.com/Chips100/clr2ts.svg?branch=master)](https://travis-ci.com/Chips100/clr2ts)
![GitHub](https://img.shields.io/github/license/Chips100/clr2ts.svg)
![Nuget (with prereleases)](https://img.shields.io/nuget/vpre/clr2ts.svg)

Transpiler that takes CLR assemblies and outputs corresponding typescript definitions.

## Note: Under development
Please note that this tool is still in an early phase of development and missing many important features. There is already a version available on nuget.org that can be installed and used for very simple examples. Future changes should only affect the configuration file, so that you can update to a better version later and just change that file accordingly.

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

### Configuration
A configuration file may look like this:

```
{
    "input": { 
        "assemblyFiles": [
            "./Path/To/Your/Assembly.dll"
        ],
        "typeFilters": [
            // Define multiple filters for OR-semantics.
            {
                // Multiple values in a single filter for AND-semantics.
                "hasAttribute": ["AttributeName"],
                "subTypeOf": "BaseType",
                "namespace": "Namespace.SubNamespace"
            }
        ]
    },
    "output": {
        "bundledFile": "./Path/To/Your/TypeScriptCodeBase/bundle.ts",
        "files": {
            "directory": "./Path/To/Your/TypeScriptCodeBase/directory",
            "mimicNamespacesWithSubdirectories": true
        }
    }
}
```

## Future plans
You can find some of the ideas for future releases in the github projects of this repository as well as in the draft for a more complex configuration file located under *docs* in the source code. Those plans include better translation features (like support of generics and collection types or converting to camelCase naming conventions) and better handling of clr2ts (logging, custom type maps or type filters).