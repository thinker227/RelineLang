# Program structure
A Reline program is made up of one or more files ending with the `.rl` extension, each of which is made up of one or more *lines*. A *line* may consist of a *label* and/or a *statement* (see [statements](#Statements)) or none, ending with a new line (`lf`) character or end of file. As such, a line may be completely empty and only contain whitespace. Comments are denoted using `//` after which everything until a new line or end of file is considered a comment.

```
[<label>:] [statement] lf|eof
```

## Execution
When executing a Reline program, the line currently being executed is indicated by the *line pointer*. The line pointer always starts on the first line containing a statement and moves sequentially to the next line containing a statement after executing of the current statement is finished. Lines only containing whitespace or a label are skipped by the line pointer. If a program contains only empty lines or only labels then logically the program will immediately terminate as there is no first line to execute. If a [manipulation statement](#Manipulation-statements) changes the location of the line the line pointer is currently executing, the line pointer is moved along with the line.

# Statements
Each line in a file may contain a single *statement*, each of which has a different effect on the state of the program.

## Manipulation statements
A *manipulation statement* is a statement which in some way alters the arrangement of lines within the program. These statements may rearrange, add or remove lines to/from the program.

### `move`
```
move <source> to <target>
```
A *move* statement moves a specified source range of lines (`<source>`) to a specified destination range of lines (`<target>`). `<source>` has to be an expression evaluating to a range and `<target>` has to be an expression evaluating to an number, and `<target>` cannot be within the range of `<source>`.

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

### `copy`
```
copy <source> to <target>
```

## Assignment (`=`)
```
<variable> = <expression>
```

## Function calls
```
<function> (<arguments>)
```

## Function declarations (`function`)
```
function <function> <range> [(<parameters>)]
```

### `return`
