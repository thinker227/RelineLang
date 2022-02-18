input = ParseInt (ReadLine ())     // Reads input as a string and parses it as an integer
trueFalse = Clamp (input 0 1)    // Clamps the input between 0 and 1
move loopBegin..loopEnd to loopBegin * trueFalse    // Moves the lines between and including loopBegin and loopEnd to line 0 if trueFalse is 0, otherwise they are not moved anywhere

loopBegin:
copy loopEnd - 1 to here    // Copies the line "swap here with loopBegin+1" to this line, moving subsequent lines downward
Write ("1")
swap here with loopBegin + 1    // The line pointer is moved along with the swapped line, causing a loop
loopEnd:

Write ("0")    // This line only executes if 
