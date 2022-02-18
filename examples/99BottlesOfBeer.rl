bottles = 99    // Sets bottles to 99

loopBegin:
copy loopEnd+1 to here     // Copies the line after the label loopEnd to the current line, setting up a loop

// Write the lyrics
str = String (bottles)    // Converts bottles to a string
Write (str < " bottles of beer on the wall, " < str < " bottles of beer")
Write ("Take one down and pass it around, ")

move loopEnd+1 to loopEnd+2 * (Clamp (bottles - 1 0 1))    // Moves the two lines after the label loopEnd to line 0 if bottles is equal to or less than 1, otherwise the line is not moved anywhere
loopEnd:
Write (str < " bottles of beer on the wall")
swap here with loopBegin+1    // Swaps the line after the label loopBegin with the current line, causing a loop (see line 4)

Write ("No more bottles of beer on the wall")
