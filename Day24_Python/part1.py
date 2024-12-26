import sys
print(sys.version)

raw_data = None
with open("input.txt") as file:
    raw_data = file.read()

values = dict()
ops = []


def parse_input():
    for line in raw_data.split("\n\n")[0].split("\n"):
        if line.strip():
            name, value = line.split(": ")
            values[name] = int(value)

    for line in raw_data.split("\n\n")[1].split("\n"):
        if line.strip():
            inp1, gate, inp2 = line.split(" ->")[0].split(" ")
            out = line.split("-> ")[1]
            ops.append((inp1, inp2, gate, out))

    return values, ops


def work() -> int:
    global values
    missing = 0

    for (inp1, inp2, gate, out) in ops:
        if out in values:
            continue
        if inp1 not in values or inp2 not in values:
            missing += 1
            continue

        assert gate in ["AND", "OR", "XOR"], f"undefined gate {gate}"

        match gate:
            case "OR":
                values[out] = values[inp1] | values[inp2]
            case "AND":
                values[out] = values[inp1] & values[inp2]
            case "XOR":
                values[out] = values[inp1] ^ values[inp2]

    return missing


def part1():
    values, ops = parse_input()
    while work():
        pass
    output = 0
    for k, v in values.items():
        if k[0] == "z":
            output |= v << int(k[1:])

    print(output)


if __name__ == "__main__":
    part1()
