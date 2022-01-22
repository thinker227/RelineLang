function Fib 3..9 (i)    // Function declaring Fib as a function between lines 3 and 9 taking a single parameter i

move l to l + Clamp (i 0 2)    // Moves the label l forward between 0 and 2 lines
swap here with l    // Swap moves the instruction pointer with it, causing execution to jump to l

l:
return 0    // Will only execute if i is equal to 0
return 1    // Will only execute if i is equal to 1
return Fib (i-1) + Fib (i-2)    // Will only execute if i is greater than or equal to 2
