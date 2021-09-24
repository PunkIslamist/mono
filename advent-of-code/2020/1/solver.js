import { readFileSync } from 'fs';
import { Seq } from 'immutable';
import { EOL } from 'os';

/**
 * 
 * @param {Number[]} numbers 
 * @returns Number
 */
function sum(numbers) { return numbers.reduce((x, y) => x + y) }

const data = readFileSync('expenses.txt', 'utf8')
    .split(EOL)
    .map(x => Number.parseInt(x))
    .sort((a, b) => a - b);

const dat = Seq(data);
const rev = Seq(dat).reverse();
console.log(
    dat.flatMap(x => rev.map(y => [x, y]))
        .filter(xy => sum(xy) < 2020)
        .flatMap(xy => rev.map(z => [...xy, z]))
        .find(xyz => sum(xyz) === 2020)
)
