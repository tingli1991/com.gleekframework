## Project

> om.GleekFramework.CommonSdk

## Overview

This is a basic development toolkit, which is mainly used to secondary expand and package some commonly used basic methods in the project, which helps everyone to be more efficient, more standardized, and achieve uniform code style in the development process, etc.

> Since more than 90% of the classes and methods in this project are written in the form of expansion, no sample code is provided here, and it can be called in the way of calling the expansion method uniformly. For specific expansion methods, please refer to [Directory Introduction](#directory-introduction).

## Directory introduction

```text
Com.GleekFramework.CommonSdk/
├── Attributes/                           -> Custom Attribute Directory
│   ├── ActionAttribute.cs                -> Action Attribute (for marking consumer service method names)
│   ├── CommentAttribute.cs               -> Table Remark Attribute (for defining table and field comments)
│   ├── IndexAttribute.cs                 -> Index Attribute (for defining table indexes)
│   ├── NotMapAttribute.cs                -> Indicates that the attribute or class should be excluded from the mapping
│   ├── PrecisionAttribute.cs             -> Precision Attribute (for indicating the precision of a table field)
├── Enums/                                -> Enum Directory
│   ├── VersionCompare.cs                 -> Version Comparison Enum
├── Extensions/                           -> Extension Directory
│   ├── ActivatorExtensions.cs            -> Instance Activation Extensions (Reflection)
│   ├── AssemblyExtensions.cs             -> Extensions for Type (Reflection)
│   ├── AttributeExtensions.cs            -> Attribute Extensions
│   ├── Base64Extensions.cs               -> Base64 Extension Class
│   ├── ClassAttributeExtensions.cs       -> Class Attribute Extensions (Reflection)
│   ├── ConnectionExtensions.cs           -> Connection String Extensions
│   ├── ConvertExtensions.cs              -> Data Conversion Methods
│   ├── DataMapperExtensions.cs           -> Data Model Mapping Extension Class (similar to AutoMap, but with better performance due to its implementation based on expression trees)
│   ├── DateTimeExtensions.cs             -> DateTime Extension Class (for date and time conversions)
│   ├── DictionaryExtensions.cs           -> Dictionary Type Extensions (mainly for retrieving values, such as property values and attribute values)
│   ├── EnumerableExtensions.cs           -> Basic Extensions for Collections (e.g., retrieving values, adding, traversing, in-memory pagination, etc.)
│   ├── EnumExtensions.cs                 -> Enum Extension Class (for retrieving enum attributes, such as descriptions, method names [ActionAttribute], etc.)
│   ├── HeaderExtensions.cs               -> Header Information Extension Class (mainly for message headers and HTTP request headers)
│   ├── JsonFormatExtensions.cs           -> JSON Formatting Extension Class (for formatting, compression, etc.)
│   ├── LambdaExtensions.cs               -> Lambda Expression Extensions (for combining lambda expressions, and concatenating AND and OR conditions)
│   ├── MatchExtensions.cs                -> Match Extension Class (for example: extracting database names)
│   ├── Md5Extensions.cs                  -> MD5 Encryption Extension Class
│   ├── ObjectExtensions.cs               -> Object Extension Class (for example: getting and setting property values)
│   ├── PropertyExtensions.cs             -> Property Reflection Extension Class (Reflection, mainly for optimizing and extending PropertyInfo, with better performance than the original)
│   ├── ProtobufExtensions.cs             -> Protobuf Serialization Extension Class
│   ├── QueryableExtensions.cs            -> Query Extension Class (mainly for extending IQueryable<T> objects to enhance code quality, performance, and coding style)
│   ├── RadomExtensions.cs                -> Random Number Basic Extension (to reduce the probability of repetition when concurrency is high)
│   ├── SerializeExtensions.cs            -> Serialization Extension Class
│   ├── StringExtensions.cs               -> Basic String Extension Class (for removing tail strings, starting strings, etc.)
│   ├── VersionExtensions.cs              -> Version Number Extension (mainly for version comparison)
├── Hostings/                             -> Host Extension Directory
│   ├── CommonHostExtensions.cs           -> Custom Host Extension Class (for example: register host startup completion events, host stopping events, and host stop completion events)
│   ├── CommonHostingExtensions.cs        -> Hosting Extension Class (to enable semaphore switches, which are used to control the number of Task threads, mainly for semaphore control on the consumer side)
├── Mappers/                              -> Object Mapping Directory
│   ├── DataMapper.cs                     -> Data Model Mapping Extension Class `DataMapperExtensions` specific implementation (similar to AutoMap)
├── Providers/                            -> Implementation Directory for Extension Classes and Methods (not recommended for direct use, the implementations below are usually found in the Extensions directory)
│   ├── ActivatorProvider.cs              -> Instance Activation Implementation Class
│   ├── AssemblyProvider.cs               -> Assembly Implementation Class
│   ├── AssemblyTypeProvider.cs           -> Assembly Object Type Implementation Class
│   ├── ClassAttributeProvider.cs         -> Class Attribute Implementation Class
│   ├── DocumentProvider.cs               -> Documentation Implementation Class for Assemblies
│   ├── FieldAttributeProvider.cs         -> Field Custom Attribute Implementation Class
│   ├── FieldProvider.cs                  -> Field Implementation Class
│   ├── LibraryProvider.cs                -> Compilation Library Implementation Class
│   ├── PropertyAttributeProvider.cs      -> Property Custom Attribute Implementation Class
│   ├── PropertyProvider.cs               -> Reflection Property Implementation Class
│   ├── SemaphoreProvider.cs              -> Semaphore Extension Implementation Class
└── VerifyExtensions/                     -> Verification Extension Directory (for example: non-null verification)
    ├── EmptyVerifyExtensions.cs          -> Non-empty Verification Extension Class
    ├── IdCardVerifyExtensions.cs         -> ID Card Number Verification
    ├── JsonVerifyExtensions.cs           -> JSON Validation Extension Class
    ├── VerifyExtensions.cs               -> Basic Data Verification Extension Class (for example: checking whether it's a string type, a basic type, checking the format of an IP address, whether it's a boolean value, etc.)
```
