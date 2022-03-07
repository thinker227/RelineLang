namespace Reline.Compilation;

/// <summary>
/// The opcode of a native function.
/// </summary>
public enum NativeFunction {
    /// <summary>
    /// The function is unknown or could not be found.
    /// </summary>
    Unknown = 0,

    // IO (001 to 099)
    Write = 001,
    ReadLine = 002,

    // Conversions (101 to 199)
    String = 101,
    ParseInt = 102,

    // Math (201 to 299)
    Clamp = 201,
    Min = 202,
    Max = 203,

    // String operations (301 to 399)
    StringIndex = 301,
    Ascii = 302,
}
