# Program structure
A Reline program is contained within a file, ending with the `.rl` extension, which is made up of one or more *lines*. A *line* may consist of a *label* (see [labels](#Labels)) and/or a *statement* (see [statements](#Statements)) or none, ending with a new line (`lf`) character or end of file. As such, a line may be completely empty and only contain whitespace. Comments are denoted using `//` after which everything until a new line or end of file is considered a comment.

```
[<label>:] [statement] lf|eof
```

## Execution
When executing a Reline program, the line currently being executed is indicated by the *line pointer*. The line pointer always starts on the first line containing a statement and moves sequentially to the next line containing a statement after execution of the current statement is finished. Lines only containing whitespace or a label are skipped by the line pointer. If a program contains only empty lines or only labels then logically the program will immediately terminate as there is no first line to execute. If a [manipulation statement](#Manipulation-statements) changes the location of the line the line pointer is currently executing, the line pointer is moved along with the line.

# Statements
Each line in a file may contain a single *statement*, each of which has a different effect on the state of the program.

## Manipulation statements
A *manipulation statement* is a statement which in some way alters the arrangement of lines within the program. These statements may rearrange, add or remove lines to/from the program.

### `move`
```
move <source> to <target>
```
A *move* statement moves a specified source range of lines (`<source>`) to a specified destination line (`<target>`). `<source>` has to be an expression evaluating to a range and `<target>` has to be an expression evaluating to a number, and `<target>` cannot be within the range of `<source>`.

In the following example, lines 2 and 3 are moved to line 6. Line numbers added for clarity.
```
1.  move 2..4 to 6
2.  write ("owo")   // source
3.                  // source
4.  write ("uwu")
5.  
6.  write ("^w^")   // target
```
In the process of moving, lines 2 and 3 have been copied to memory and removed. The line that was previous specified by `<target>` as line 6 is now line 4.
```
1.  move 2..4 to 6
2.  write ("uwu")
3.  
4.  write ("^w^")   // target
```
Lines 2 and 3 are copied from memory and inserted at line 4.
```
1.  move 2..4 to 6
2.  write ("uwu")
3.  
4.  write ("owo")   // target
5.  
6.  write ("^w^")
```

### `swap`
```
swap <source> with <target>
```
A *swap* statement swaps a specified source range of lines (`<source>`) with a specified destination range of lines (`<target>`). Both `<source>` and `<target>` have to evaluate to a range, be of the same length and not overlap.

In the following example, lines 2 and 3 are swapped with each other. Line numbers added for clarity.
```
1.  swap 2 with 3
2.  Write ("owo")   // source
3.  Write ("uwu")   // target
```
Both line 2 and 3 are copied to memory and inserted at the others location.
```
1.  swap 2 with 3
2.  Write ("uwu")   // source
3.  Write ("owo")   // target
```

### `copy`
```
copy <source> to <target>
```
A *copy* statement copies a specified source range of lines (`<source>`) to a specified desination line (`<target>`). `<source>` has to evaluate to a range and `<target>` has to evaluate to a number, and `<target>` cannot be within the range of `<source>`.

In the following example, lines 2 and 3 are copied to line 4. Line numbers added for clarity.
```
1.  copy 2..3 to 4
2.  Write ("owo")   // source
3.  Write ("uwu")   // source
4.                  // target
```
Lines 2 and 3 are copied to memory and inserted at line 4.
```
1.  copy 2..3 to 4
2.  Write ("owo")   // source
3.  Write ("uwu")   // source
4.  Write ("owo")   // target
5.  Write ("uwu")
6.  
```

## Assignment (`=`)
```
<variable> = <expression>
```
An *assignment* statement assigns the value of an expression (`<expression>`) to a variable (`<variable>`) (see [Variables](#Variables)).

In the following example, the variable `foo` is assigned the value of `2`, is written to stdout, is incremented by `5` and is again written to stdout. Line numbers added for clarity.
```
1.  foo = 2
2.  Write (foo)
3.  foo = foo + 5
4.  Write (foo)
```

## Function calls
```
<function> (<arguments>)
```

## Function declarations (`function`)
```
function <function> <range> [<parameters>]
```
A *function declaration* declares a function which can be referenced throughout the rest of the program. The declaration includes a name (`<function>`), the line range of the function (`<range>`) and optionally a list of parameters (`[<parameters>]`) made up of zero or more identifiers (see [Parameters](#Parameters)). `<range>` has to evaluate to a constant range.

In the following example, the function `WritePerson` is declared between the lines 2 and 4 taking the parameters `name`, `age` and `gender`. The function is then called with the values `"Jay"`, `99` and `"male"`. Line numbers added for clarity.
```
1.  function WritePerson 2..4 (name age gender)
2.  Write ("Name: " < name)
3.  Write ("Age: " < String (age))
4.  Write ("Gender: " < gender)
5.  
6.  WritePerson ("Jay" 99 "male")
```

### `return`
```
return <expression>
```
A *return* statement returns the value of an expression (`<expression>`) from a function and terminates execution of the function immediately. Return statements can only be used within the range of a function.

In the following example, the function `Add` is declared on line 2 taking the parameters `a` and `b` and returning the value of `a + b`. The function is then called with the values `3` and `4`. Line numbers added for clarity.
```
1.  function Add 2..2 (a b)
2.  return a + b
3.  
4.  Write (Add (3, 4))
```

# Labels
```
<label>:
```
A *label* acts as an alias for a line number. A label can be referenced using the label's identifier and will at runtime be evaluated to the current line number of the line of the label. If the line of a label is moved using a `move` or `copy` statement then the label is moved along with the line. If the line of a label is copied using a `copy` statement then the line is copied but not the label as duplicate labels would be unresolvable.

# Variables
Variables are weakly and dynamically typed as well as implicitly declared. A variable is declared if it is assigned at any point in the program, no specific keyword or syntax is required for variable declaration.

Here, `foo` is implicitly declared, first given the value of `2` then the value of `"owo"`.
```
foo = 2
foo = "owo"
```
A variable can be referenced by simply referencing its identifier.
```
bar = 69
Write (bar)   // Outputs "69"
```
As a consequence of the existence of manipulation statements, variables may be referenced before they are ever assigned. In this situation, the variable will hold the default value of `?` which is treated as a regular value, though which will cause a runtime error if any operator is applied to it.
```
Write (baz)   // Outputs "?"
baz = 420
```
Subsequently, variables may be assigned to themselves without any other assignment. In this situation, the variable will be declared and assigned to its own unassigned value.
```
boo = boo
Write (boo)   // Outputs "?"
```

# Functions
A *function* (more accurately, *user-defined function*, see [native functions](#Native-functions)) is a range of lines contained under an identifier which may take in parameters and/or return a value.

During compilation, functions are lowered into manipulation statements and variable assignments. As such, functions do not exist at runtime.

## Parameters

## Native functions
*Native functions* are functions which are defined by the runtime as opposed to compiled source code and are treated separately from user-defined functions by the compiler. These functions include the standard library, such as `Write`, `ReadLine` and `Clamp`. Native functions may not be referenced using a function pointer expression (`*Function`) as they are not defined within source code.
