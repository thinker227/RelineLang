bottles = 99    // Sets bottles to 99

loopBegin:
copy loopEnd+1 to loopBegin+1    // Copies the line after the label loopEnd to the line after the label loopBegin, setting up a loop

// Writes the lyrics
bottlesString = String (bottles)    // Turns the value of bottles into a string
Write (" bottles of beer on the wall, ")
Write (bottlesString)
Write (" bottles of beer\n"Take one down and pass it around, ")
Write (bottlesString)
Write (" bottles of beer on the wall")

move loopEnd+1 to loopEnd+1 * Clamp (bottles-1 0 1)    // Moves the line after the label loopEnd to line 0 (line 1) if bottles is equal to or less than 1, otherwise the line is moved nowhere

loopEnd:
swap loopBegin+1 with loopEnd+1    // Swaps the line after the label loopBegin with the line after the label loopBegin, causing a loop (see line 4)

Write ("1 bottle of beer on the wall, 1 bottle of beer\nTake one down and pass it around, no more bottles of beer on the wall")
