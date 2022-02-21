main = do
    content <- readFile "./inputs/1.txt"
    let split   = words . replace ['-', ':'] ' '
    let entries = map (parse . split) $ lines content
    let valid1  = filter validate1 entries
    let valid2  = filter validate2 entries
    print ("Valid according to rule 1: " ++ show (length valid1))
    print ("Valid according to rule 2: " ++ show (length valid2))
    return ()

replace chars replacement word =
    let replace c = if c `elem` chars then replacement else c
    in  map replace word

parse [low, high, char, pwd] = (readInt low, readInt high, head char, pwd)
parse _                      = (0, 0, 'a', "")

readInt :: String -> Int
readInt = read

validate1 (low, high, char, pwd) =
    let count c n = if c == char then n + 1 else n
        total = foldr count 0 pwd
    in  (total >= low) && (total <= high)

validate2 (low, high, char, pwd) =
    let padded = ' ' : pwd
        hasCharAt i = char == padded !! i
    in  xor (hasCharAt low) (hasCharAt high)

xor True  True  = False
xor True  False = True
xor False True  = True
xor False False = False
