// Chapter 14 - Nested updates

const jinx = {
    name: 'Powder',
    nick: 'Jinx',
    sister: {
        name: 'Violet',
        nick: 'Vi'
    },
    enemies: [
        {
            name: 'Caitlyn',
            nick: 'Cupcake'
        },
        {
            name: 'Violet',
            nick: 'Vi'
        },
    ]
}

const pop = xs => ({ head: xs[0], tail: xs.slice(1) })

const update = (thing, accessor, f) => {
    let copy = Array.isArray(thing)
        ? [...thing]
        : { ...thing };
    copy[accessor] = f(copy[accessor]);
    return copy;
}

const deepUpdate = (obj, props, modify) => {
    if (props.length === 0) {
        return modify(obj);
    } else {
        const { head, tail } = pop(props);

        return update(obj, head, propValue =>
            deepUpdate(propValue, tail, modify));
    }
}

console.log(update(jinx, 'name', _ => 'Lynx'));
console.log(deepUpdate(jinx, ['name'], n => `Changed ${n}`));
console.log(deepUpdate(jinx, ['sister', 'name'], n => `Changed ${n}`));
console.log(deepUpdate(jinx, ['enemies'], (arr => arr.filter(x => x.name != 'Caitlyn'))));
console.log(deepUpdate(jinx, ['enemies', 1, 'name'], n => `Changed ${n}`));

// Ensure we never mutated
console.log(jinx);
