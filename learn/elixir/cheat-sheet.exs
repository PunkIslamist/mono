### Basic Types ###

## Lists

[1, :hi, "abc", [true, <<12>>]]

[1, 2, 3] ++ ["1", "2", "3"]
# [1, 2, 3, "1", "2", "3"]

[1, 2, 3] -- [2, 3, 4]
# [1]    (not [1, 4])

[1 | [2, 3, 4]]
# [1, 2, 3, 4]

1 in [1, 2, 3]
# true


## Tuples

{1, :"one two three", 2, 3}


## Regex
~r{regexp}
~r{regexp}options

# For the rest, just check `h Regex` tbh :3


## Keyword lists

[ a: 1, b: "one" ] => [ {:a 1}, {:b "one"} ]

[ a: "first key occurence", a: "second key occurence" ]

# Note: brackets can be left off in many contexts if it is the last time
f a, b, c: 1, d: 3 (= f a, b, [{:c 1}, {:d 3}])

{1, a: 2, b:3 } (= {1, [{:a 2, :b 3}]})


## Maps

%{ "A key" => "Some Value", :anotherKey => 123, {:a, :b} => :c }

# Special case: keys are atoms
%{ a: 1, b: 2, c: 3 }

%{ String.downcase("ABCD") => "key from expression"}

aMap = {a: 1, b: 2, "c" => 3}

aMap[a] => 1
aMap["c"] => 3

aMap.a => 1
aMap."c" => Error (only works for atom keys)

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


## Maps

%{:a} = %{a: 1, b: 2}


### Scoping ###

## with blocks
x = with  a       = 1
          b       = a + 2
          [c | d] = [a, b]
          # When using <- instead of =, returns the unmatchable value
          [e]     <- []
    do
      [a, b, c, d]
    end
