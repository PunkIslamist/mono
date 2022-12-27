IO.puts("Greetings!")

# Exercise Functions-1

list_concat = fn a, b -> a ++ b end

sum = fn x, y, z -> x + y + z end

pair_tuple_to_list = fn {a, b} -> [a, b] end

# Exercise Functions-2 & -3
# Using module and named functions as I cannot send to repl from file :(

defmodule Functions_2 do
  def fizz_buzz(0, 0, _), do: "FizzBuzz"
  def fizz_buzz(0, _, _), do: "Fizz"
  def fizz_buzz(_, 0, _), do: "Buzz"
  def fizz_buzz(_, _, n), do: n
  def fizz_buzz(x), do: fizz_buzz(rem(x, 3), rem(x, 5), x)
end

# Exercise Functions-4
prefix = fn x -> fn y -> "#{x} #{y}" end end

# Exercise Functions-5
Enum.map([1, 2, 3, 4], &(&1 + 2))
Enum.map([1, 2, 3, 4], &inspect/1)

# Exercise Functions-6
defmodule Functions_6 do
  def sum(1), do: 1
  def sum(n), do: n + sum(n - 1)

  def gcd(x, 0), do: x
  def gcd(x, y), do: gcd(y, rem(x, y))
end

# Exercise ModulesAndFunctions-6
defmodule Chop do
  def guess(goal, from..to), do: guess(goal, div(from + to, 2), from..to)

  def guess(goal, guess, _) when goal == guess, do: guess

  def guess(goal, guess, from..to) when goal > guess do
    mid = div(from + to, 2)
    guess = div(mid + to, 2)
    IO.puts("Is it #{guess}?")
    guess(goal, guess, mid..to)
  end

  def guess(goal, guess, from..to) when goal < guess do
    mid = div(from + to, 2)
    guess = div(from + mid, 2)
    IO.puts("Is it #{guess}?")
    guess(goal, guess, from..mid)
  end

  # def guess(n, from..to) when div(from + to, 2) > n do
  #  IO.puts("Is it #{div(from + to, 2)}")
  #  guess(n, from..div(from + to, 2))
  # end

  # def guess(n, from..to) when div(from + to, 2) < n do
  #  IO.puts("Is it #{div(from + to, 2)}")
  #  guess(n, div(from + to, 2)..to)
  # end
end
