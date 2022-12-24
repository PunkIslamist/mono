### Basic Types ###

## Lists

[1, :hi, "abc", [true, <<12>>]]

# Concat
[1, 2, 3] ++ ["1", "2", "3"]

# Append
[1 | [2, 3, 4]]

## Tuples

{1, :"one two three", 2, 3}

## Regex
~r{regexp}
~r{regexp}options

# For the rest, just check `h Regex` tbh :3

### Matching ###
# General note: the value of a successful match expression
# is the value that was matched, aka the right hand side

## Basic

a = 1
# a = 1

1 = a
# works

2 = a
# MatchError

## Lists

[a, b, 3] = [1, 2, 3]
# a = 1, b = 2

[[a, b], c] = [[1, 2], 3]
# a = 1, b = 2, c = 3

[a | b] = [1, 2]
# a = 1, b = 2

[a | b] = [1, 2, 3]
# a = 1, b = [2, 3]

[a, b | c] = [1, 2, 3]
# a = 1, b = 2, c = 3

[a, b | c] = [1, 2, 3, 4]
# a = 1, b = 2, c = [3, 4]

[a, a] = [1, 1]
# a = 1

[a, a] = [1, 2]
# MatchError

a = 2
[a, ^a] = [1, 2]
# Works

a = 2
[^a, a] = [1, 2]
# MatchError

## Tuples

{:ok, a} = {:ok, "some nice values"}
# a = "some nice value"

{a, b, {:name, name}} = {1, {:a, :b}, {:name, "Amy}}
# a = 1, b = {:a, :b}, name = "Amy"
