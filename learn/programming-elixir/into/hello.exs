IO.puts("Greetings!")

# Exercise Functions-1

list_concat = fn a, b -> a ++ b end

sum = fn x, y, z -> x + y + z end

pair_tuple_to_list = fn {a, b} -> [a, b] end

# Exercise Functions-2
# Using module and named functions as I cannot send to repl from file :(

defmodule Functions_2 do
  def fizz_buzz(0, 0, _), do: "FizzBuzz"
  def fizz_buzz(0, _, _), do: "Fizz"
  def fizz_buzz(_, 0, _), do: "Buzz"
  def fizz_buzz(_, _, n), do: n
  def fizz_buzz(x), do: fizz_buzz(rem(x, 3), rem(x, 5), x)
end
