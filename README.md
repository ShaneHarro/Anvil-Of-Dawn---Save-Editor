# Anvil of Dawn - Save Editor

A save game editor utility written in C#/.NET for the MS-DOS game Anvil of Dawn.

## Running the editor (EXE file)
Simply navigate to the bin/Debug folder and run "Anvil Of Dawn - Save Editor.exe"

## Known issues
Though the code does include a list of every item and its related ID that I have documented, I have not been able to successfully implement an item editor.
Due to the nature of how Anvil of Dawn's inventory stores items ("free" placement floating points rather than restricted to a solid integer X/Y grid), I have
never been able to figure out how to track what item is where, and then edit it appropriately.