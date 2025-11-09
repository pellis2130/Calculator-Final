# Software Design Document
**Author:** Princess Ellis  
**Course:** 202510E OL SDC220L AO Object-Oriented Programming Using C# Lab C. Fry  
**Instructor:** Dr. Chris Fry  
**Date:** November 2025  

## Classes
- **Program** – entry point.  
- **UserInterface** – menu control.  
- **Calculator** – arithmetic methods.  
- **MemoryManager** – handles single and collection memory.  
- **InputHelper** – validation.

## Data Structures
|Purpose|Type|
|-------|----|
|Arithmetic|`double`|
|Single Memory|`double?`|
|Collection|`List<int>` (max 10)|

## Error Handling
Catches `DivideByZeroException`, invalid input loops, and general exceptions.

## Flow
