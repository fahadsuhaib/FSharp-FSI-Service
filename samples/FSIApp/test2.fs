namespace Test

module UseMath =
    
    let doSum() =
        let x = MathT.sum 10 10
        printfn "%d" x