namespace Extensions

module Integer =
    let (|Positive|Negative|Zero|) x =
        if x > 0 then Positive
        else if x < 0 then Negative
        else Zero

module List =
    type Diff<'a> =
        { Both: 'a list
          Left: 'a list
          Right: 'a list }

    let private toDiff both left right =
        { Both = both
          Left = left
          Right = right }

    open Integer

    let diff f xs ys =
        let rec loop xs ys both left right =
            match xs, ys with
            | [], [] -> toDiff both left right
            | xs, [] -> toDiff both (left @ xs) right
            | [], ys -> toDiff both left (right @ ys)
            | (x :: xs', y :: ys') ->
                match f x y with
                | Positive -> loop xs ys' both left (right @ [ y ])
                | Negative -> loop xs' ys both (left @ [ x ]) right
                | Zero -> loop xs' ys' (both @ [ x ]) left right

        let sort = List.sortWith f
        let xs' = sort xs
        let ys' = sort ys

        loop xs' ys' [] [] []

    module private Tests =
        let empty = { Both = []; Left = []; Right = [] }
        let f = diff (-)

        let testBothEmpty = f [] [] = empty

        let testLeftEmpty =
            f [] [ 1 ] = { empty with Right = [ 1 ] }

        let testRightEmpty = f [ 1 ] [] = { empty with Left = [ 1 ] }

        let testBothSameElement =
            f [ 1 ] [ 1 ] = { empty with Both = [ 1 ] }

        let testLeftAdditionalElement =
            f [ 1; 2 ] [ 1 ] = { Both = [ 1 ]
                                 Left = [ 2 ]
                                 Right = [] }

        let testRightAdditionalElement =
            f [ 1 ] [ 1; 2 ] = { Both = [ 1 ]
                                 Left = []
                                 Right = [ 2 ] }

        let testBothOnlyDifferentElements =
            f [ 1; 2 ] [ 3; 4 ] = { Both = []
                                    Left = [ 1; 2 ]
                                    Right = [ 3; 4 ] }

        let testBothSomeCommonElements =
            f [ 1; 2; 3 ] [ 2; 3; 4 ] = { Both = [ 2; 3 ]
                                          Left = [ 1 ]
                                          Right = [ 4 ] }

        let testMultipleBlocksOfCommonElements =
            f [ 1; 2; 3; 5; 6; 7 ] [
                2
                3
                4
                7
                8
                9
            ] = { Both = [ 2; 3; 7 ]
                  Left = [ 1; 5; 6 ]
                  Right = [ 4; 8; 9 ] }

        let testUnordered =
            f [ 6; 3; 2; 10 ] [ 3; 10; 12; 15 ] = { Both = [ 3; 10 ]
                                                    Left = [ 2; 6 ]
                                                    Right = [ 12; 15 ] }
