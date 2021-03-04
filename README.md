# FunctionNameCompare
Quickly update a .flow by comparing two AtlusScriptCompiler Libraries  
Uses [SimpleCommandLine](https://github.com/TGEnigma/SimpleCommandLine) by [TGEnigma](https://github.com/TGEnigma)

I made this to speed up the process of updating a .flow to compile with a newer/older version of an [AtlusScriptCompiler](https://github.com/TGEnigma/AtlusScriptCompiler) library.  
This is useful if you want to replace the library you're using with someone else's between edits.  
If anything other than your function names happens to include the old function name, it will also be renamed, so watch out.
# Usage (Updating .flow)
Run FunctionNameCompare.exe from the command prompt and enter the following:  
``FunctionNameCompare.exe -o "C:\Path\To\OldLibraryFolder" -n "C:\Path\To\NewLibraryFolder" -i "C:\Path\To\YourFlowscript.flow" -p``
## Example 
``FunctionNameCompare.exe -o "C:\Users\ryans\Downloads\AtlusScriptToolchain (4)\Libraries\Persona4Golden" -n "D:\Games\Persona\Tools\P5\AtlusScriptCompiler\Libraries\Persona4Golden\Modules" -i "C:\Users\ryans\Documents\GitHub\Persona-4-Golden-Mod-Menu\field\field.bf.flow" -p``
# Usage (Updating Library)
This program can also overwrite an old library with function names from the new one.  
Run FunctionNameCompare.exe from the command prompt and enter the following:  
``FunctionNameCompare.exe -o "C:\Path\To\OldLibraryFolder" -n "C:\Path\To\NewLibraryFolder" -u -p``
## Example 
``FunctionNameCompare.exe -o "D:\Games\Persona\Tools\P5\AtlusScriptCompiler\Libraries\Persona4" -n "D:\Games\Persona\Tools\P5\AtlusScriptCompiler\Libraries\Persona4Golden" -u -p``
