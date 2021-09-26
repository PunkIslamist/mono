import { readFileSync } from 'fs';
import { Seq } from 'immutable';
import { EOL } from 'os';

const add = (x, y) => x + y
const sum = (numbers) => numbers.reduce(add)

const data = readFileSync('expenses.txt', 'utf8')
    .split(EOL)
    .map(x => Number.parseInt(x))
    .sort((a, b) => a - b);

const ascending = Seq(data);
const descending = Seq(ascending).reverse();
console.log(
    descending.flatMap(x =>
        ascending.map(y => [x, y])
            .takeUntil(xy => sum(xy) > 2020))
        .flatMap(xy =>
            ascending.map(z => [...xy, z])
                .takeUntil(xy => sum(xy) > 2020))
        .find(xyz => sum(xyz) === 2020)
)
