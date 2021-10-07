import           Data.List                      ( mapAccumL )
import           Text.Printf                    ( printf )

main = do
    -- Part 1
    let exec name path (x, y) = do
            content <- readFile path
            printf "%s\t%s\t%d\n"
                   name
                   (show (x, y))
                   (countTreeHits content (x, y))
    exec "Example" "inputs/example.txt" (1, 1)
    exec "1"       "inputs/1.txt"       (3, 1)

    -- Part 2
    do
        content <- readFile "inputs/1.txt"
        let treeCounts = map (countTreeHits content)
                             [(1, 1), (3, 1), (5, 1), (7, 1), (1, 2)]
        printf "Product: %d\n" (product treeCounts)

    return ()

countTreeHits s (x, y) = length . filter not $ solve s (x, y)

solve s (x, y) = map isFree $ slope (x, y) $ toLandscape s

toLandscape = map cycle . lines

slope (x, y) landscape = snd $ mapAccumL point 0 sections
  where
    sections = nth y landscape
    point i curr = (i + 1, curr !! (i * x))

nth n = nth' n 0
  where
    nth' _ _ [] = []
    nth' n i (x : xs) =
        if i `mod` n == 0 then x : nth' n (i + 1) xs else nth' n (i + 1) xs

isFree '.' = True
isFree _   = False
