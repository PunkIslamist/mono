{-
resolver: lts-18.9
-}
import           Data.List

main = do
    content <- readFile "../input/expenses.txt"
    let nums = map readInt $ lines content
    print $ solve 2020 $ sort nums
    return ()

readInt :: String -> Int
readInt = read

solve target numbers = solve' target $ sort numbers
solve' target numbers = head
    [ x : yz
    | yz <-
        [ [y, z]
        | z <- reverse numbers
        , y <- takeWhile (\n -> n + z < target) numbers
        ]
    , x <- takeWhile (\n -> sum (n : yz) <= target) numbers
    , sum (x : yz) == target
    ]
