Write ("Left operand: ")
left = ParseInt (ReadLine ())    // Reads input as a string and parses it as an integer
Write ("Operation: ")
operation = ReadLine ()
op = Ascii (StringIndex (operation 0))    // Gets the ASCII code for the first character of the operation
Write ("Right operand: ")
right = ParseInt (ReadLine ())    // Reads input as a string and parses it as an integer

move op to here + 1    // Moves the line with the line number of the ASCII code of the operator to the next line
Write (result)
swap here with end    // Swaps the current line with the last line of the program, immediately terminating it

























result = left % right    // % has ASCII code 37




result = left * right    // * has ASCII code 42
result = left + right    // + has ASCII code 43

result = left - right    // - has ASCII code 45

result = left / right    // / has ASCII code 47

// end