namespace Microsoft.FSharp.Core

    //-------------------------------------------------------------------------
    // Basic type abbreviations

    type obj = System.Object
    type exn = System.Exception
    type nativeint = System.IntPtr
    type unativeint = System.UIntPtr
    type string = System.String
    type float32 = System.Single
    type float = System.Double
    type single = System.Single
    type double = System.Double
    type sbyte = System.SByte
    type byte = System.Byte
    type int8 = System.SByte
    type uint8 = System.Byte
    type int16 = System.Int16
    type uint16 = System.UInt16
    type int32 = System.Int32
    type uint32 = System.UInt32
    type int64 = System.Int64
    type uint64 = System.UInt64
    type char = System.Char
    type bool = System.Boolean
    type decimal = System.Decimal
    type int = int32

    type ``[]``<'T> = (# "!0[]" #)
    type ``[,]``<'T> = (# "!0[0 ...,0 ...]" #)
    type ``[,,]``<'T> = (# "!0[0 ...,0 ...,0 ...]" #)
    type ``[,,,]``<'T> = (# "!0[0 ...,0 ...,0 ...,0 ...]" #)

    type array<'T> = 'T[]

    type byref<'T> = (# "!0&" #)

    type nativeptr<'T when 'T : unmanaged> = (# "native int" #)
    type ilsigptr<'T> = (# "!0*" #)

