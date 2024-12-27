from collections import defaultdict


def main():
    # Read inputs from the file 'input.txt'
    grid = parse_input('input.txt')
    values, operations = parse(grid)

    print('Part 1:', part1(values, operations))
    print('Part 2:', part2(values, operations))


def parse_input(filename):
    with open(filename) as inf:
        lines = inf.readlines()
    return [line.strip() for line in lines]


def is_input(operand):
    return operand[0] in 'xy'


def part2(values, operations):
    use_map = defaultdict(list)
    for op in operations:
        use_map[op[0]].append(op)
        use_map[op[2]].append(op)

    swapped = set()
    for operation in operations:
        left, op, right, result = operation
        # Special-case the first and last bits
        if result == 'z45' or left == 'x00':
            continue

        if op == 'XOR':
            if is_input(left):
                if not is_input(right):
                    swapped.add(result)
                    print(operation, 'only 1 is an input')
                if result[0] == 'z' and result != 'z00':
                    swapped.add(result)
                    print(operation, 'output is a z when using an input')
                usage = use_map[result]
                using_ops = [o[1] for o in usage]
                if result != 'z00' and sorted(using_ops) != ['AND', 'XOR']:
                    swapped.add(result)
                    print(operation, 'wrong output ops', usage)
            else:
                if result[0] != 'z':
                    swapped.add(result)
                    print(operation, 'output was not a z')

        elif op == 'AND':
            if is_input(left):
                if not is_input(right):
                    swapped.add(result)
                    print(operation, 'only 1 is an input')
            usage = use_map[result]
            if [o[1] for o in usage] != ['OR']:
                swapped.add(result)
                print(operation, 'wrong usage', usage)

        elif op == 'OR':
            if is_input(left) or is_input(right):
                swapped.add(result)
                print(operation, 'used an input')
            usage = use_map[result]
            using_ops = [o[1] for o in usage]
            if sorted(using_ops) != ['AND', 'XOR']:
                swapped.add(result)
                print(operation, 'wrong usage', usage)

        else:
            print(operation, 'unknown op')

    return ','.join(sorted(swapped))


def part1(values, operations):
    results = apply_operations(values, operations)
    return sum_zvalues(results)


def apply_operations(values, operations):
    mem = dict(**values)

    while True:
        did_operation = False

        for op in operations:
            if op[3] in mem:
                # already done
                continue
            if op[0] not in mem or op[2] not in mem:
                # dependent values not done yet
                continue

            mem[op[3]] = do_op(mem[op[0]], op[1], mem[op[2]])
            did_operation = True

        if not did_operation:
            break

    return mem


def do_op(left, op, right):
    if op == 'AND':
        return left and right
    if op == 'OR':
        return left or right
    if op == 'XOR':
        return left ^ right
    raise Exception()


def sum_zvalues(results):
    z_keys = sorted([k for k in results if k.startswith('z')])[::-1]
    result = 0
    for k in z_keys:
        result <<= 1
        result += results[k]
    return result


def parse(lines):
    values = {}
    operations = []
    for line in lines:
        if ':' in line:
            name, rest = line.split(':')
            value = int(rest.strip())
            values[name] = value
        elif '->' in line:
            parts = line.split(' ')
            op = (parts[0], parts[1], parts[2], parts[4])
            operations.append(op)
    return (values, operations)


if __name__ == '__main__':
    main()
