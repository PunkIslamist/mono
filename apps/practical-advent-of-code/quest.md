Taken from: https://mccue.dev/pages/12-3-22-practical-advent


In all of the examples, there is a piece of information we will call "the vocabulary".

If the context is a string, the vocabulary is that string. If the context is an object, the vocabulary is under the @vocab key. If the context is an array, the vocabulary is a string in the first index of the array.

So in all of these examples the vocabulary is "https://www.w3.org/ns/activitystreams"

Assume one of these shapes of JSON is in a file called `activity.json`. Your job is to extract the vocabulary out of that file and print it.