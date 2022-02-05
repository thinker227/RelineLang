in = ReadLine ()    // Reads input as a string
parsed = Clamp (ParseInt (in) 0 1)    // Parses input as an int and clamps it between 0 and 1
move here+2..here+4 to (here+2)*parsed    // Moves the following three lines (not counting whitespace) to position 0 or nowhere

copy here+2 to here-1    // Copies "swap 4 with 7" to the previous line
Write ("1")
swap 4 with 7    // Swap moves the line pointer with it, causing a loop

Write ("0")    // Assuming the previous three lines were moved to position 0 (not counting whitespace), otherwise this would never execute
