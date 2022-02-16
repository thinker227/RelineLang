// Keywords

move swap copy to with return
here start end

// Should not be highlighted
movehere returnfunction swapwith



// Strings

"this is a string"
"string containing \"\"\"escaped\"\"\" characters\n"

"string should not be
highlighted on several
lines"



// Digits

0 1 2 3 4 5 6 7 8 9
0123456789



// Function identifiers

function
function Foo
function _Foo_bar
function 	Foo0123456789
function @Foo@Bar // Only @Foo should be highlighted
functionFoo // Should not be highlighted



// Function ivocations

Foo()
_Foo_bar ()
Foo0123456789 	()
// Only @Bar should be highlighted
@Foo@Bar ()

// Should not be highlighted
P
()



// Labels

foo:
_foo_bar :
foo0123456789 	:
// Only @bar should be highlighted
@foo@bar:

// Should not be highlighted
m
:
