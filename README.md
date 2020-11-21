# FunctionNameCompare
Quickly update a .flow by comparing two AtlusScriptCompiler Libraries  
Uses [SimpleCommandLine](https://github.com/TGEnigma/SimpleCommandLine) by [TGEnigma](https://github.com/TGEnigma)

I made this to speed up the process of updating a .flow to compile with a newer/older version of an [AtlusScriptCompiler](https://github.com/TGEnigma/AtlusScriptCompiler) library.  
This is useful if you want to replace the library you're using with someone else's between edits.  
If anything other than your function names happens to include the old function name, it will also be renamed, so watch out.
# Usage
Run FunctionNameCompare.exe from the command prompt and enter the following:  
``FunctionNameCompare.exe -n "C:\Path\To\NewLibraryFolder" -o "C:\Path\To\OldLibraryFolder" -i "C:\Path\To\YourFlowscript.flow" ``
Example:  
``FunctionNameCompare.exe -n "D:\Games\Persona\Compiler Libraries\TGE\Persona4Golden" -o "D:\Games\Persona\Tools\P5\AtlusScriptCompiler\Libraries\Persona4Golden\Modules" -i "C:\Users\ryans\Documents\GitHub\Persona-4-Golden-Mod-Menu\test\ModMenu.flow"``
