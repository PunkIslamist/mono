import { readFileSync } from 'fs';
import { Seq } from 'immutable';
import { EOL } from 'os';

const add = (x, y) => x + y
const sum = (numbers) => numbers.reduce(add)

const data = readFileSync('expenses.txt', 'utf8')
    .split(EOL)
    .map(x => Number.parseInt(x))
    .sort((a, b) => a - b);

const descending = Seq(data);
const ascending = Seq(descending).reverse();
console.log(
    descending.flatMap(x => ascending.map(y => [x, y]))
        .takeWhile(xy => sum(xy) < 2020)
        .flatMap(xy => ascending.map(z => [...xy, z]))
        .find(xyz => sum(xyz) === 2020)
)
